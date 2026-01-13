using Microsoft.EntityFrameworkCore;
using ZDatabase.Interfaces;
using ZFinance.Core.Entities.Security;
using ZFinance.Core.Repositories.Security.Interfaces;
using ZFinance.Core.Services.Interfaces;

namespace ZFinance.Core.Repositories.Security
{
    /// <inheritdoc />
    public class RefreshTokensRepository : IRefreshTokensRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RolesRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext" /> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler" /> instance.</param>
        public RefreshTokensRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task<RefreshTokens?> FindRefreshTokenByUserEmailAndTokenAsync(string email, string token)
        {
            try
            {
                return await (from rt in dbContext.Set<RefreshTokens>()
                              where EF.Functions.Like(rt.User != null ? rt.User.Email ?? string.Empty : string.Empty, email)
                                    && EF.Functions.Like(rt.Token ?? string.Empty, token)
                              select rt).FirstOrDefaultAsync();
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in repository when finding a refresh token by user and token.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(email), email },
                        { nameof(token), token },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task InsertRefreshTokenAsync(RefreshTokens refreshToken)
        {
            try
            {
                await dbContext.AddAsync(refreshToken);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in repository when inserting a refresh token.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(refreshToken), refreshToken },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public void RevokeRefreshToken(RefreshTokens refreshToken)
        {
            try
            {
                refreshToken.RevokedOn = DateTime.UtcNow;
                dbContext.Update(refreshToken);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in repository when revoking a refresh token.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(refreshToken), refreshToken },
                    }
                );
                throw;
            }
        }
        #endregion

        #region Private methods
        #endregion
    }
}