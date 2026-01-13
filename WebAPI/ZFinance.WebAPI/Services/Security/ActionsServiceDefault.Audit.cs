using ZFinance.Core.Entities.Security;
using ZSecurity.Attributes;
using ZWebAPI.Interfaces;
using ZWebAPI.Models.Audit.OperationHistory;
using ZWebAPI.Models.Audit.ServiceHistory;
using ZWebAPI.Services.Interfaces;

namespace ZFinance.WebAPI.Services.Security
{
    public partial class ActionsServiceDefault
    {
        #region Variables
        private readonly IAuditService<Users, long> auditService;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Public methods
        /// <inheritdoc />
        [ActionMethod]
        public async Task<IQueryable<OperationsHistoryListModel>> AuditActionOperationsHistoryAsync(long actionID, long serviceHistoryID, IListParameters parameters)
        {
            try
            {
                await securityHandler.ValidateUserHasPermissionAsync();

                return await auditService.ListEntityOperationsHistoryAsync<Actions>(actionID, serviceHistoryID, parameters);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in service when auditing the operations history of the action.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(actionID), actionID },
                        { nameof(serviceHistoryID), serviceHistoryID },
                        { nameof(parameters), parameters },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        [ActionMethod]
        public async Task<IQueryable<ServicesHistoryListModel>> AuditActionServicesHistoryAsync(long actionID, IListParameters parameters)
        {
            try
            {
                await securityHandler.ValidateUserHasPermissionAsync();

                return await auditService.ListEntityServicesHistoryAsync<Actions>(actionID, parameters);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in service when auditing the services history of the action.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(actionID), actionID },
                        { nameof(parameters), parameters },
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