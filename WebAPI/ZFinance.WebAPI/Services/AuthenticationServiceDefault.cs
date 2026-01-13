using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ZDatabase.Exceptions;
using ZDatabase.Interfaces;
using ZDatabase.Services.Interfaces;
using ZFinance.Core.Entities.Security;
using ZFinance.Core.ExtensionMethods;
using ZFinance.Core.Repositories.Security.Interfaces;
using ZFinance.Core.Services.Interfaces;
using ZFinance.WebAPI.Exceptions;
using ZFinance.WebAPI.Models.Authentication;
using ZFinance.WebAPI.Services.Interfaces;

namespace ZFinance.WebAPI.Services
{
    /// <inheritdoc />
    public partial class AuthenticationServiceDefault : IAuthenticationService
    {
        #region Variables
        private readonly IConfiguration config;
        private readonly ICurrentUserProvider<long> currentUserProvider;
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        private readonly IInformationProvider informationProvider;
        private readonly IMapper mapper;
        private readonly IRefreshTokensRepository refreshTokensRepository;
        private readonly IUsersRepository usersRepository;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationServiceDefault"/> class.
        /// </summary>
        /// <param name="config">The <see cref="IConfiguration"/> instance.</param>
        /// <param name="currentUserProvider">The <see cref="ICurrentUserProvider{TUsersKey}"/> instance.</param>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        /// <param name="informationProvider">The <see cref="IInformationProvider"/> instance.</param>
        /// <param name="mapper">The <see cref="IMapper"/> instance.</param>
        /// <param name="refreshTokensRepository">The <see cref="IRefreshTokensRepository"/> instance.</param>
        /// <param name="usersRepository">The <see cref="IUsersRepository"/> instance.</param>
        public AuthenticationServiceDefault(
            IConfiguration config,
            ICurrentUserProvider<long> currentUserProvider,
            IDbContext dbContext,
            IExceptionHandler exceptionHandler,
            IInformationProvider informationProvider,
            IMapper mapper,
            IRefreshTokensRepository refreshTokensRepository,
            IUsersRepository usersRepository)
        {
            this.config = config;
            this.currentUserProvider = currentUserProvider;
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
            this.informationProvider = informationProvider;
            this.mapper = mapper;
            this.refreshTokensRepository = refreshTokensRepository;
            this.usersRepository = usersRepository;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task<IEnumerable<string?>> GetActionsAsync()
        {
            if (currentUserProvider.CurrentUserID is not long currentUserID || currentUserID <= 0)
            {
                throw new EntityNotFoundException<Users>(0);
            }

            try
            {
                return (await informationProvider.ListAllActionsAsync(currentUserID)).Select(x => x.Code);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in service when getting user actions.");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<AuthenticationResponseModel> RefreshTokenAsync(AuthenticationRefreshTokenModel refreshTokenModel)
        {
            if (string.IsNullOrEmpty(refreshTokenModel.Username) || string.IsNullOrEmpty(refreshTokenModel.RefreshToken))
            {
                throw new RefreshTokenNotFoundException();
            }

            try
            {
                RefreshTokens? refreshToken = await refreshTokensRepository.FindRefreshTokenByUserEmailAndTokenAsync(refreshTokenModel.Username, refreshTokenModel.RefreshToken);
                if (refreshToken is null)
                {
                    throw new RefreshTokenNotFoundException();
                }
                else if (!refreshToken.IsActive || refreshToken.User == null)
                {
                    throw new InvalidRefreshTokenException();
                }

                using IDbContextTransaction transaction = dbContext.Database.BeginTransaction();
                try
                {
                    refreshTokensRepository.RevokeRefreshToken(refreshToken);

                    refreshToken = CreateRefreshToken(refreshToken.User);
                    await refreshTokensRepository.InsertRefreshTokenAsync(refreshToken);

                    await dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();

                    AuthenticationResponseModel authenticationModel = mapper.Map<AuthenticationResponseModel>(refreshToken.User);
                    authenticationModel.Token = CreateJwtToken(refreshToken.User!);
                    authenticationModel.RefreshToken = refreshToken.Token;
                    authenticationModel.RefreshTokenExpiration = refreshToken.Expiration;

                    return authenticationModel;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in service when refreshing the token model.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(refreshTokenModel), refreshTokenModel },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<AuthenticationResponseModel> SignInAsync(AuthenticationSignInModel signInModel)
        {
            try
            {
                if (await usersRepository.FindUserByEmailAsync(signInModel.Username ?? string.Empty) is not Users user
                    || !user.IsActive
                    || string.IsNullOrEmpty(signInModel.Password))
                {
                    throw new InvalidAuthenticationException();
                }

                // TODO: Add safe check to prevent timing attacks.
                if (user.VerifyHashedPassword(signInModel.Password) == false)
                {
                    throw new InvalidAuthenticationException();
                }

                RefreshTokens refreshToken = CreateRefreshToken(user);
                await refreshTokensRepository.InsertRefreshTokenAsync(refreshToken);
                await dbContext.SaveChangesAsync();

                AuthenticationResponseModel authenticationModel = mapper.Map<AuthenticationResponseModel>(user);
                authenticationModel.Token = CreateJwtToken(user);
                authenticationModel.RefreshToken = refreshToken.Token;
                authenticationModel.RefreshTokenExpiration = refreshToken.Expiration;

                return authenticationModel;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in service when signing in the user.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(signInModel), signInModel },
                    }
                );
                throw;
            }
        }
        #endregion

        #region Private methods
        private string CreateJwtToken(Users user)
        {
            JwtSecurityTokenHandler tokenHandler = new();

            SecurityToken token = tokenHandler.CreateToken(new SecurityTokenDescriptor()
            {
                Audience = config["JWT:Audience"],

                Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(config["JWT:DurationInMinutes"])),

                Issuer = config["JWT:Issuer"],

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]!)),
                    SecurityAlgorithms.HmacSha256
                ),

                Subject = new ClaimsIdentity(
                    new Claim[] {
                        new("uid", user.ID.ToString()),
                        new(ClaimTypes.NameIdentifier, user.Email ?? string.Empty)
                    }
                ),
            });

            return tokenHandler.WriteToken(token);
        }

        private RefreshTokens CreateRefreshToken(Users user)
        {
            byte[] randomNumber = RandomNumberGenerator.GetBytes(32);

            return dbContext.CreateProxy<RefreshTokens>(x =>
            {
                x.Expiration = DateTime.UtcNow.AddDays(Convert.ToInt32(config["JWT:RefreshTokenExpiration"]));
                x.Token = Convert.ToBase64String(randomNumber);
                x.User = user;
                x.UserID = user.ID;
            });
        }
        #endregion
    }
}