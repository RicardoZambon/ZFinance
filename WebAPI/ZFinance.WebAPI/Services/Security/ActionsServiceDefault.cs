using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore.Storage;
using ZDatabase.Exceptions;
using ZDatabase.Interfaces;
using ZFinance.Core.Entities.Security;
using ZFinance.Core.Repositories.Security.Interfaces;
using ZFinance.Core.Services.Interfaces;
using ZFinance.WebAPI.Models.Security.Action;
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
    public partial class ActionsServiceDefault : IActionsService
    {
        #region Variables
        private readonly IActionsRepository actionsRepository;
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        private readonly IMapper mapper;
        private readonly ISecurityHandler securityHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionsServiceDefault"/> class.
        /// </summary>
        /// <param name="auditService">The <see cref="IAuditService{TUsers, TUsersKey}"/> instance.</param>
        /// <param name="actionsRepository">The <see cref="IActionsRepository"/> instance.</param>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        /// <param name="mapper">The <see cref="IMapper"/> instance.</param>
        /// <param name="securityHandler">The <see cref="ISecurityHandler"/> instance.</param>
        public ActionsServiceDefault(
            IAuditService<Users, long> auditService,
            IActionsRepository actionsRepository,
            IDbContext dbContext,
            IExceptionHandler exceptionHandler,
            IMapper mapper,
            ISecurityHandler securityHandler)
        {
            this.auditService = auditService;
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
            this.mapper = mapper;
            this.actionsRepository = actionsRepository;
            this.securityHandler = securityHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        [ActionMethod]
        public async Task DeleteActionAsync(long actionID)
        {
            try
            {
                await auditService.BeginNewServiceHistoryAsync();

                IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync();
                try
                {
                    await actionsRepository.DeleteActionAsync(actionID);
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
                exceptionHandler.AddBreadcrumb("Error in service when deleting a action.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(actionID), actionID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        [ActionMethod]
        public async Task<ActionsDisplayModel> FindActionByIDAsync(long actionID)
        {
            try
            {
                await securityHandler.ValidateUserHasPermissionAsync();

                if (await actionsRepository.FindActionByIDAsync(actionID) is Actions action)
                {
                    return mapper.Map<ActionsDisplayModel>(action);
                }
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in service when finding a action by ID.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(actionID), actionID },
                    }
                );
                throw;
            }

            throw new EntityNotFoundException<Actions>(actionID);
        }

        /// <inheritdoc />
        [ActionMethod]
        public async Task<ActionsDisplayModel> InsertNewActionAsync(ActionsInsertModel actionModel)
        {
            if (actionModel is null)
            {
                throw new ArgumentNullException(nameof(actionModel));
            }

            try
            {
                await auditService.BeginNewServiceHistoryAsync();

                Actions action = mapper.Map<Actions>(actionModel);

                IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync();
                try
                {
                    await actionsRepository.InsertActionAsync(action);
                    await dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }

                return mapper.Map<ActionsDisplayModel>(action);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in repository when inserting a action.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(actionModel), actionModel },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        [ActionMethod]
        public async Task<IQueryable<ActionsListModel>> ListActionsAsync(IListParameters parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            try
            {
                await securityHandler.ValidateUserHasPermissionAsync();

                return actionsRepository.ListActions()
                    .TryFilter(parameters, x => x.Code, FilterTypes.Like)
                    .TryFilter(parameters, x => x.Description, FilterTypes.Like)
                    .TryFilter(parameters, x => x.Entity, FilterTypes.Like)
                    .TryFilter(parameters, x => x.Name, FilterTypes.Like)
                    .OrderBy(x => x.Entity)
                    .ThenBy(x => x.Name)
                    .GetRange(parameters)
                    .ProjectTo<ActionsListModel>(mapper.ConfigurationProvider);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in service when listing actions.",
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
        public async Task<ActionsDisplayModel> UpdateActionAsync(ActionsUpdateModel actionModel)
        {
            if (actionModel is null)
            {
                throw new ArgumentNullException(nameof(actionModel));
            }

            try
            {
                await auditService.BeginNewServiceHistoryAsync();

                if (await actionsRepository.FindActionByIDAsync(actionModel.ID) is not Actions action)
                {
                    throw new EntityNotFoundException<Actions>(actionModel.ID);
                }

                IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync();
                try
                {
                    await actionsRepository.UpdateActionAsync(mapper.Map(actionModel, action));
                    await dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }

                return mapper.Map<ActionsDisplayModel>(action);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in service when updating a action.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(actionModel), actionModel },
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