using ZWebAPI.Interfaces;
using ZWebAPI.Models.Audit.OperationHistory;
using ZWebAPI.Models.Audit.ServiceHistory;

namespace ZFinance.WebAPI.Services.Security.Interfaces
{
    public partial interface IMenusService
    {
        /// <summary>
        /// Lists the menu audit operations history asynchronous.
        /// </summary>
        /// <param name="menuID">The menu identifier.</param>
        /// <param name="serviceHistoryID">The service history identifier.</param>
        /// <param name="parameters">The parameters for the list. <see cref="ZWebAPI.Interfaces.IListParameters"/>.</param>
        /// <returns>List with the menu audit operations history accordingly to the parameters.</returns>
        /// <exception cref="System.ArgumentNullException">When the parameters are <c>null</c>.</exception>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">When the entity was not found.</exception>
        Task<IQueryable<OperationsHistoryListModel>> AuditMenuOperationsHistoryAsync(long menuID, long serviceHistoryID, IListParameters parameters);

        /// <summary>
        /// Lists the menu audit services history asynchronous.
        /// </summary>
        /// <param name="menuID">The menu identifier.</param>
        /// <param name="parameters">The parameters for the list. <see cref="ZWebAPI.Interfaces.IListParameters"/>.</param>
        /// <returns>List with the menu audit services history accordingly to the parameters.</returns>
        /// <exception cref="System.ArgumentNullException">When the parameters are <c>null</c>.</exception>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">When the entity was not found.</exception>
        Task<IQueryable<ServicesHistoryListModel>> AuditMenuServicesHistoryAsync(long menuID, IListParameters parameters);
    }
}