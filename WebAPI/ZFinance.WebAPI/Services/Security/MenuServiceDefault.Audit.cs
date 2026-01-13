using ZFinance.Core.Entities.Security;
using ZSecurity.Attributes;
using ZWebAPI.Interfaces;
using ZWebAPI.Models.Audit.OperationHistory;
using ZWebAPI.Models.Audit.ServiceHistory;
using ZWebAPI.Services.Interfaces;

namespace ZFinance.WebAPI.Services.Security
{
    public partial class MenusServiceDefault
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
        public async Task<IQueryable<OperationsHistoryListModel>> AuditMenuOperationsHistoryAsync(long menuID, long serviceHistoryID, IListParameters parameters)
        {
            try
            {
                await securityHandler.ValidateUserHasPermissionAsync();

                return await auditService.ListEntityOperationsHistoryAsync<Menus>(menuID, serviceHistoryID, parameters);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in service when auditing the operations history of the menu.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(menuID), menuID },
                        { nameof(serviceHistoryID), serviceHistoryID },
                        { nameof(parameters), parameters },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        [ActionMethod]
        public async Task<IQueryable<ServicesHistoryListModel>> AuditMenuServicesHistoryAsync(long menuID, IListParameters parameters)
        {
            try
            {
                await securityHandler.ValidateUserHasPermissionAsync();

                return await auditService.ListEntityServicesHistoryAsync<Menus>(menuID, parameters);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in service when auditing the services history of the menu.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(menuID), menuID },
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