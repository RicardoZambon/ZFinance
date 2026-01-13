using ZFinance.Core.Entities.Security;
using ZSecurity.Attributes;
using ZWebAPI.Interfaces;
using ZWebAPI.Models.Audit.OperationHistory;
using ZWebAPI.Models.Audit.ServiceHistory;
using ZWebAPI.Services.Interfaces;

namespace ZFinance.WebAPI.Services.Security
{
    public partial class UsersServiceDefault
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
        public async Task<IQueryable<OperationsHistoryListModel>> AuditUserOperationsHistoryAsync(long userID, long serviceHistoryID, IListParameters parameters)
        {
            try
            {
                await securityHandler.ValidateUserHasPermissionAsync();

                return await auditService.ListEntityOperationsHistoryAsync<Users>(userID, serviceHistoryID, parameters);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in service when auditing the operations history of the user.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(userID), userID },
                        { nameof(serviceHistoryID), serviceHistoryID },
                        { nameof(parameters), parameters },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        [ActionMethod]
        public async Task<IQueryable<ServicesHistoryListModel>> AuditUserServicesHistoryAsync(long userID, IListParameters parameters)
        {
            try
            {
                await securityHandler.ValidateUserHasPermissionAsync();

                return await auditService.ListEntityServicesHistoryAsync<Users>(userID, parameters);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in service when auditing the services history of the user.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(userID), userID },
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