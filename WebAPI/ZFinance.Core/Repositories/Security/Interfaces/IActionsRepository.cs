using ZFinance.Core.Entities.Security;

namespace ZFinance.Core.Repositories.Security.Interfaces
{
    /// <summary>
    /// Repository for <see cref="Actions"/>.
    /// </summary>
    public interface IActionsRepository
    {
        /// <summary>
        /// Deletes the action asynchronous.
        /// </summary>
        /// <param name="actionID">The action identifier.</param>
        Task DeleteActionAsync(long actionID);

        /// <summary>
        /// Finds the action by identifier asynchronous.
        /// </summary>
        /// <param name="actionID">The action identifier.</param>
        /// <returns>The action, if found; otherwise, <c>null</c>.</returns>
        Task<Actions?> FindActionByIDAsync(long actionID);

        /// <summary>
        /// Inserts the action asynchronous.
        /// </summary>
        /// <param name="action">The action.</param>
        Task InsertActionAsync(Actions action);

        /// <summary>
        /// Lists all action.
        /// </summary>
        /// <returns>Query with all action.</returns>
        IQueryable<Actions> ListActions();

        /// <summary>
        /// Updates the action asynchronous.
        /// </summary>
        /// <param name="action">The action.</param>
        Task UpdateActionAsync(Actions action);
    }
}