using ZWebAPI.Interfaces;
using ZWebAPI.Models.Audit.OperationHistory;
using ZWebAPI.Models.Audit.ServiceHistory;

namespace ZFinance.WebAPI.Services.Security.Interfaces
{
    public partial interface IUsersService
    {
        /// <summary>
        /// Lists the user audit operations history asynchronous.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="serviceHistoryID">The service history identifier.</param>
        /// <param name="parameters">The parameters for the list. <see cref="IListParameters"/>.</param>
        /// <returns>List with the user audit operations history accordingly to the parameters.</returns>
        /// <exception cref="ArgumentNullException">When the parameters are <c>null</c>.</exception>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">When the entity was not found.</exception>
        Task<IQueryable<OperationsHistoryListModel>> AuditUserOperationsHistoryAsync(long userID, long serviceHistoryID, IListParameters parameters);

        /// <summary>
        /// Lists the user audit services history asynchronous.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="parameters">The parameters for the list. <see cref="IListParameters"/>.</param>
        /// <returns>List with the user audit services history accordingly to the parameters.</returns>
        /// <exception cref="ArgumentNullException">When the parameters are <c>null</c>.</exception>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">When the entity was not found.</exception>
        Task<IQueryable<ServicesHistoryListModel>> AuditUserServicesHistoryAsync(long userID, IListParameters parameters);
    }
}