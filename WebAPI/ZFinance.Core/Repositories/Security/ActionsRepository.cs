using Microsoft.EntityFrameworkCore;
using ZDatabase.Exceptions;
using ZDatabase.Interfaces;
using ZDatabase.Validations;
using ZFinance.Core.Entities.Security;
using ZFinance.Core.Repositories.Security.Interfaces;
using ZFinance.Core.Services.Interfaces;

namespace ZFinance.Core.Repositories.Security
{
    /// <inheritdoc />
    public class ActionsRepository : IActionsRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionsRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext" /> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler" /> instance.</param>
        public ActionsRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task DeleteActionAsync(long actionID)
        {
            try
            {
                if (await FindActionByIDAsync(actionID) is not Actions action)
                {
                    throw new EntityNotFoundException<Actions>(actionID);
                }
                else if ((action.Roles?.Count ?? 0) > 0)
                {
                    throw new InvalidOperationException("Actions-Button-Delete-Roles-Assigned");
                }

                action.IsDeleted = true;
                dbContext.Set<Actions>().Update(action);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in repository when deleting an action.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(actionID), actionID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<Actions?> FindActionByIDAsync(long actionID)
        {
            try
            {
                return await dbContext.FindAsync<Actions>(actionID);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in repository when finding an action by ID.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(actionID), actionID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task InsertActionAsync(Actions action)
        {
            try
            {
                await ValidateAsync(action);
                await dbContext.Set<Actions>().AddAsync(action);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in repository when inserting an action.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(action), action },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<Actions> ListActions()
        {
            try
            {
                return from a in dbContext.Set<Actions>()
                       select a;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in repository when listing actions.");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task UpdateActionAsync(Actions action)
        {
            try
            {
                await ValidateAsync(action);
                dbContext.Set<Actions>().Update(action);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in repository when updating an action.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(action), action },
                    }
                );
                throw;
            }
        }
        #endregion

        #region Private methods
        private async Task ValidateAsync(Actions action)
        {
            ValidationResult result = new();

            // Code
            if (string.IsNullOrWhiteSpace(action.Code))
            {
                result.SetError(nameof(Actions.Code), "required");
            }
            else if (await dbContext.Set<Actions>().AnyAsync(x => EF.Functions.Like(x.Code!, action.Code) && x.ID != action.ID))
            {
                result.SetError(nameof(Actions.Code), "exists");
            }

            // Description
            if (string.IsNullOrWhiteSpace(action.Description))
            {
                result.SetError(nameof(Actions.Description), "required");
            }

            // Entity
            if (string.IsNullOrWhiteSpace(action.Entity))
            {
                result.SetError(nameof(Actions.Entity), "required");
            }

            // Name
            if (string.IsNullOrWhiteSpace(action.Name))
            {
                result.SetError(nameof(Actions.Name), "required");
            }
            else if (await dbContext.Set<Actions>().AnyAsync(x => EF.Functions.Like(x.Name!, action.Name) && x.ID != action.ID))
            {
                result.SetError(nameof(Actions.Name), "exists");
            }

            result.ValidateEntityErrors(action);
        }
        #endregion
    }
}