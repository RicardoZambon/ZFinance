using ZWebAPI.Interfaces;
using ZWebAPI.Models.Audit.OperationHistory;
using ZWebAPI.Models.Audit.ServiceHistory;

namespace ZFinance.WebAPI.Services.Security.Interfaces
{
    public partial interface IActionsService
    {
        /// <summary>
        /// Lists the action audit operations history asynchronous.
        /// </summary>
        /// <param name="actionID">The action identifier.</param>
        /// <param name="serviceHistoryID">The service history identifier.</param>
        /// <param name="parameters">The parameters for the list. <see cref="ZWebAPI.Interfaces.IListParameters"/>.</param>
        /// <returns>List with the action audit operations history accordingly to the parameters.</returns>
        /// <exception cref="System.ArgumentNullException">When the parameters are <c>null</c>.</exception>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">When the entity was not found.</exception>
        Task<IQueryable<OperationsHistoryListModel>> AuditActionOperationsHistoryAsync(long actionID, long serviceHistoryID, IListParameters parameters);

        /// <summary>
        /// Lists the action audit services history asynchronous.
        /// </summary>
        /// <param name="actionID">The action identifier.</param>
        /// <param name="parameters">The parameters for the list. <see cref="ZWebAPI.Interfaces.IListParameters"/>.</param>
        /// <returns>List with the action audit services history accordingly to the parameters.</returns>
        /// <exception cref="System.ArgumentNullException">When the parameters are <c>null</c>.</exception>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">When the entity was not found.</exception>
        Task<IQueryable<ServicesHistoryListModel>> AuditActionServicesHistoryAsync(long actionID, IListParameters parameters);
    }
}