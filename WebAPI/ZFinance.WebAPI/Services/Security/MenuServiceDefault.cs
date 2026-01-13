using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore.Storage;
using ZDatabase.Exceptions;
using ZDatabase.Interfaces;
using ZDatabase.Services.Interfaces;
using ZFinance.Core.Entities.Security;
using ZFinance.Core.Repositories.Security.Interfaces;
using ZFinance.Core.Services.Interfaces;
using ZFinance.WebAPI.Models.Security.Menu;
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
    public partial class MenusServiceDefault : IMenusService
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        private readonly IMapper mapper;
        private readonly IMenusRepository menusRepository;
        private readonly ISecurityHandler securityHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MenusServiceDefault"/> class.
        /// </summary>
        /// <param name="auditService">The <see cref="IAuditService{TUsers, TUsersKey}"/> instance.</param>
        /// <param name="currentUserProvider">The <see cref="ICurrentUserProvider{TUserKey}"/> instance.</param>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        /// <param name="informationProvider">The <see cref="IInformationProvider"/> instance.</param>
        /// <param name="mapper">The <see cref="IMapper"/> instance.</param>
        /// <param name="menusRepository">The <see cref="IMenusRepository"/> instance.</param>
        /// <param name="securityHandler">The <see cref="ISecurityHandler"/> instance.</param>
        public MenusServiceDefault(
            IAuditService<Users, long> auditService,
            ICurrentUserProvider<long> currentUserProvider,
            IDbContext dbContext,
            IExceptionHandler exceptionHandler,
            IInformationProvider informationProvider,
            IMapper mapper,
            IMenusRepository menusRepository,
            ISecurityHandler securityHandler)
        {
            this.auditService = auditService;
            this.currentUserProvider = currentUserProvider;
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
            this.informationProvider = informationProvider;
            this.mapper = mapper;
            this.menusRepository = menusRepository;
            this.securityHandler = securityHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        [ActionMethod]
        public async Task DeleteMenuAsync(long menuID)
        {
            try
            {
                await auditService.BeginNewServiceHistoryAsync();

                IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync();
                try
                {
                    await menusRepository.DeleteMenuAsync(menuID);
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
                exceptionHandler.AddBreadcrumb("Error in service when deleting a menu.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(menuID), menuID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        [ActionMethod]
        public async Task<MenusDisplayModel> FindMenuByIDAsync(long menuID)
        {
            try
            {
                await securityHandler.ValidateUserHasPermissionAsync();

                if (await menusRepository.FindMenuByIDAsync(menuID) is Menus menu)
                {
                    return mapper.Map<MenusDisplayModel>(menu);
                }
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in service when finding a menu by ID.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(menuID), menuID },
                    }
                );
                throw;
            }

            throw new EntityNotFoundException<Menus>(menuID);
        }

        /// <inheritdoc />
        [ActionMethod]
        public async Task<MenusDisplayModel> InsertNewMenuAsync(MenusInsertModel menuModel)
        {
            if (menuModel is null)
            {
                throw new ArgumentNullException(nameof(menuModel));
            }

            try
            {
                await auditService.BeginNewServiceHistoryAsync();

                Menus menu = mapper.Map<Menus>(menuModel);

                IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync();
                try
                {
                    await menusRepository.InsertMenuAsync(menu);
                    await dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }

                return mapper.Map<MenusDisplayModel>(menu);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in repository when inserting a menu.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(menuModel), menuModel },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        [ActionMethod]
        public async Task<IQueryable<MenusListModel>> ListMenusAsync(IListParameters parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            try
            {
                await securityHandler.ValidateUserHasPermissionAsync();

                return menusRepository.ListMenus()
                    .TryFilter(parameters, x => x.Label, FilterTypes.Like)
                    .TryFilter(parameters, x => x.ParentMenuID, FilterTypes.Equals)
                    .TryFilter(parameters, x => x.URL, FilterTypes.Like)
                    .OrderBy(x => x.Label)
                    .GetRange(parameters)
                    .ProjectTo<MenusListModel>(mapper.ConfigurationProvider);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in service when listing menus.",
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
        public async Task<MenusDisplayModel> UpdateMenuAsync(MenusUpdateModel menuModel)
        {
            if (menuModel is null)
            {
                throw new ArgumentNullException(nameof(menuModel));
            }

            try
            {
                await auditService.BeginNewServiceHistoryAsync();

                if (await menusRepository.FindMenuByIDAsync(menuModel.ID) is not Menus menu)
                {
                    throw new EntityNotFoundException<Menus>(menuModel.ID);
                }

                IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync();
                try
                {
                    await menusRepository.UpdateMenuAsync(mapper.Map(menuModel, menu));
                    await dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }

                return mapper.Map<MenusDisplayModel>(menu);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in service when updating a menu.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(menuModel), menuModel },
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