using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using ZDatabase.Exceptions;
using ZDatabase.Interfaces;
using ZFinance.Core.Entities.Security;
using ZFinance.Core.Repositories.Security.Interfaces;
using ZFinance.Core.Services.Interfaces;
using ZFinance.WebAPI.Models.Security.User;
using ZFinance.WebAPI.Services.Security.Interfaces;
using ZSecurity.Attributes;
using ZSecurity.Services;
using ZWebAPI.Enums;
using ZWebAPI.ExtensionMethods;
using ZWebAPI.Interfaces;
using ZWebAPI.Services.Interfaces;

namespace ZFinance.WebAPI.Services.Security
{
    /// <inheritdoc />
    public partial class UsersServiceDefault : IUsersService
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        private readonly IMapper mapper;
        private readonly ISecurityHandler securityHandler;
        private readonly IUsersRepository usersRepository;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersServiceDefault"/> class.
        /// </summary>
        /// <param name="auditService">The <see cref="IAuditService{TUsers, TUsersKey}"/> instance.</param>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        /// <param name="mapper">The <see cref="IMapper"/> instance.</param>
        /// <param name="rolesRepository">The <see cref="IRolesRepository"/> instance.</param>
        /// <param name="securityHandler">The <see cref="ISecurityHandler"/> instance.</param>
        /// <param name="usersRepository">The <see cref="IUsersRepository"/> instance.</param>
        public UsersServiceDefault(
            IAuditService<Users, long> auditService,
            IDbContext dbContext,
            IExceptionHandler exceptionHandler,
            IMapper mapper,
            IRolesRepository rolesRepository,
            ISecurityHandler securityHandler,
            IUsersRepository usersRepository)
        {
            this.auditService = auditService;
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
            this.mapper = mapper;
            this.rolesRepository = rolesRepository;
            this.securityHandler = securityHandler;
            this.usersRepository = usersRepository;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        [ActionMethod]
        public async Task DisableUserAsync(long userID)
        {
            try
            {
                await auditService.BeginNewServiceHistoryAsync();

                IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync();
                try
                {
                    await usersRepository.DisableUserAsync(userID);
                    await dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in service when disabling a user.",
                    new Dictionary<string, object?>
                    {
                        { nameof(userID), userID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        [ActionMethod]
        public async Task<UsersDisplayModel> FindUserByIDAsync(long userID)
        {
            try
            {
                await securityHandler.ValidateUserHasPermissionAsync();

                if (await usersRepository.FindUserByIDAsync(userID) is Users user)
                {
                    return mapper.Map<UsersDisplayModel>(user);
                }
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in service when finding a user by ID.",
                    new Dictionary<string, object?>
                    {
                        { nameof(userID), userID },
                    }
                );
                throw;
            }

            throw new EntityNotFoundException<Users>(userID);
        }

        /// <inheritdoc />
        [ActionMethod]
        public async Task<UsersDisplayModel> InsertNewUserAsync(UsersInsertModel userModel)
        {
            if (userModel is null)
            {
                throw new ArgumentNullException(nameof(userModel));
            }

            try
            {
                await auditService.BeginNewServiceHistoryAsync();

                Users user = mapper.Map<Users>(userModel);

                IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync();
                try
                {
                    await usersRepository.InsertUserAsync(user);
                    await dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }

                return mapper.Map<UsersDisplayModel>(user);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in repository when inserting a user.",
                    new Dictionary<string, object?>
                    {
                        { nameof(userModel), userModel },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        [ActionMethod]
        public async Task<IQueryable<UsersListModel>> ListUsersAsync(IListParameters parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            try
            {
                await securityHandler.ValidateUserHasPermissionAsync();

                return usersRepository.ListUsers()
                    .TryFilter(parameters, x => x.Email, FilterTypes.Like)
                    .OrderBy(x => x.Email)
                    .GetRange(parameters)
                    .ProjectTo<UsersListModel>(mapper.ConfigurationProvider);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in service when listing users.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(parameters), parameters },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        [ActionMethod]
        public async Task<UsersDisplayModel> UpdateUserAsync(UsersUpdateModel userModel)
        {
            if (userModel is null)
            {
                throw new ArgumentNullException(nameof(userModel));
            }
            try
            {
                await auditService.BeginNewServiceHistoryAsync();

                if (await usersRepository.FindUserByIDAsync(userModel.ID) is not Users user)
                {
                    throw new EntityNotFoundException<Users>(userModel.ID);
                }

                IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync();
                try
                {
                    await usersRepository.UpdateUserAsync(mapper.Map(userModel, user));
                    await dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }

                return mapper.Map<UsersDisplayModel>(user);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in service when updating a user.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(userModel), userModel },
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