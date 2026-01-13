using ZWebAPI.Interfaces;
using ZWebAPI.Models.Audit.OperationHistory;
using ZWebAPI.Models.Audit.ServiceHistory;

namespace ZFinance.WebAPI.Services.Security.Interfaces
{
    public partial interface IRolesService
    {
        /// <summary>
        /// Lists the role audit operations history asynchronous.
        /// </summary>
        /// <param name="roleID">The role identifier.</param>
        /// <param name="serviceHistoryID">The service history identifier.</param>
        /// <param name="parameters">The parameters for the list. <see cref="ZWebAPI.Interfaces.IListParameters"/>.</param>
        /// <returns>List with the role audit operations history accordingly to the parameters.</returns>
        /// <exception cref="System.ArgumentNullException">When the parameters are <c>null</c>.</exception>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">When the entity was not found.</exception>
        Task<IQueryable<OperationsHistoryListModel>> AuditRoleOperationsHistoryAsync(long roleID, long serviceHistoryID, IListParameters parameters);

        /// <summary>
        /// Lists the role audit services history asynchronous.
        /// </summary>
        /// <param name="roleID">The role identifier.</param>
        /// <param name="parameters">The parameters for the list. <see cref="ZWebAPI.Interfaces.IListParameters"/>.</param>
        /// <returns>List with the role audit services history accordingly to the parameters.</returns>
        /// <exception cref="System.ArgumentNullException">When the parameters are <c>null</c>.</exception>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">When the entity was not found.</exception>
        Task<IQueryable<ServicesHistoryListModel>> AuditRoleServicesHistoryAsync(long roleID, IListParameters parameters);
    }
}