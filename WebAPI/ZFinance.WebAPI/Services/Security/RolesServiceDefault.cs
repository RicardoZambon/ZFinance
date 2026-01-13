using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using ZDatabase.Exceptions;
using ZDatabase.Interfaces;
using ZFinance.Core.Entities.Security;
using ZFinance.Core.Repositories.Security.Interfaces;
using ZFinance.Core.Services.Interfaces;
using ZFinance.WebAPI.Models.Security.Role;
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
    public partial class RolesServiceDefault : IRolesService
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        private readonly IMapper mapper;
        private readonly IRolesRepository rolesRepository;
        private readonly ISecurityHandler securityHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RolesServiceDefault"/> class.
        /// </summary>
        /// <param name="actionsRepository">The <see cref="IActionsRepository"/> instance.</param>
        /// <param name="auditService">The <see cref="IAuditService{TUsers, TUserKey}"/> instance.</param>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        /// <param name="mapper">The <see cref="IMapper"/> instance.</param>
        /// <param name="menusRepository">The <see cref="IMenusRepository"/> instance.</param>
        /// <param name="rolesRepository">The <see cref="IRolesRepository"/> instance.</param>
        /// <param name="securityHandler">The <see cref="ISecurityHandler"/> instance.</param>
        /// <param name="usersRepository">The <see cref="IUsersRepository"/> instance.</param>
        public RolesServiceDefault(
            IActionsRepository actionsRepository,
            IAuditService<Users, long> auditService,
            IDbContext dbContext,
            IExceptionHandler exceptionHandler,
            IMapper mapper,
            IMenusRepository menusRepository,
            IRolesRepository rolesRepository,
            ISecurityHandler securityHandler,
            IUsersRepository usersRepository)
        {
            this.actionsRepository = actionsRepository;
            this.auditService = auditService;
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
            this.mapper = mapper;
            this.menusRepository = menusRepository;
            this.rolesRepository = rolesRepository;
            this.securityHandler = securityHandler;
            this.usersRepository = usersRepository;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        [ActionMethod]
        public async Task DeleteRoleAsync(long roleID)
        {
            try
            {
                await auditService.BeginNewServiceHistoryAsync();

                IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync();
                try
                {
                    await rolesRepository.DeleteRoleAsync(roleID);
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
                exceptionHandler.AddBreadcrumb("Error in service when deleting a role.",
                    new Dictionary<string, object?>
                    {
                        { nameof(roleID), roleID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        [ActionMethod]
        public async Task<RolesDisplayModel> FindRoleByIDAsync(long roleID)
        {
            try
            {
                await securityHandler.ValidateUserHasPermissionAsync();

                if (await rolesRepository.FindRoleByIDAsync(roleID) is Roles role)
                {
                    return mapper.Map<RolesDisplayModel>(role);
                }
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in service when finding a role by ID.",
                    new Dictionary<string, object?>
                    {
                        { nameof(roleID), roleID },
                    }
                );
                throw;
            }

            throw new EntityNotFoundException<Roles>(roleID);
        }

        /// <inheritdoc />
        [ActionMethod]
        public async Task<RolesDisplayModel> InsertNewRoleAsync(RolesInsertModel roleModel)
        {
            if (roleModel is null)
            {
                throw new ArgumentNullException(nameof(roleModel));
            }

            try
            {
                await auditService.BeginNewServiceHistoryAsync();

                Roles role = mapper.Map<Roles>(roleModel);

                IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync();
                try
                {
                    await rolesRepository.InsertRoleAsync(role);
                    await dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }

                return mapper.Map<RolesDisplayModel>(role);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in repository when inserting a role.",
                    new Dictionary<string, object?>
                    {
                        { nameof(roleModel), roleModel },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        [ActionMethod]
        public async Task<IQueryable<RolesListModel>> ListRolesAsync(IListParameters parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            try
            {
                await securityHandler.ValidateUserHasPermissionAsync();

                return rolesRepository.ListRoles()
                    .TryFilter(parameters, x => x.Name, FilterTypes.Like)
                    .OrderBy(x => x.Name)
                    .GetRange(parameters)
                    .ProjectTo<RolesListModel>(mapper.ConfigurationProvider);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in service when listing roles.",
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
        public async Task<RolesDisplayModel> UpdateRoleAsync(RolesUpdateModel costCenterModel)
        {
            if (costCenterModel is null)
            {
                throw new ArgumentNullException(nameof(costCenterModel));
            }
            try
            {
                await auditService.BeginNewServiceHistoryAsync();

                if (await rolesRepository.FindRoleByIDAsync(costCenterModel.ID) is not Roles costCenter)
                {
                    throw new EntityNotFoundException<Roles>(costCenterModel.ID);
                }

                IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync();
                try
                {
                    await rolesRepository.UpdateRoleAsync(mapper.Map(costCenterModel, costCenter));
                    await dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }

                return mapper.Map<RolesDisplayModel>(costCenter);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in service when updating a role.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(costCenterModel), costCenterModel },
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