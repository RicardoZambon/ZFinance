using ZFinance.Core.Entities.Security;
using ZSecurity.Attributes;
using ZWebAPI.Interfaces;
using ZWebAPI.Models.Audit.OperationHistory;
using ZWebAPI.Models.Audit.ServiceHistory;
using ZWebAPI.Services.Interfaces;

namespace ZFinance.WebAPI.Services.Security
{
    public partial class RolesServiceDefault
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
        public async Task<IQueryable<OperationsHistoryListModel>> AuditRoleOperationsHistoryAsync(long roleID, long serviceHistoryID, IListParameters parameters)
        {
            try
            {
                await securityHandler.ValidateUserHasPermissionAsync();

                return await auditService.ListEntityOperationsHistoryAsync<Roles>(roleID, serviceHistoryID, parameters);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in service when auditing the operations history of the role.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(roleID), roleID },
                        { nameof(serviceHistoryID), serviceHistoryID },
                        { nameof(parameters), parameters },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        [ActionMethod]
        public async Task<IQueryable<ServicesHistoryListModel>> AuditRoleServicesHistoryAsync(long roleID, IListParameters parameters)
        {
            try
            {
                await securityHandler.ValidateUserHasPermissionAsync();

                return await auditService.ListEntityServicesHistoryAsync<Roles>(roleID, parameters);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in service when auditing the services history of the role.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(roleID), roleID },
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