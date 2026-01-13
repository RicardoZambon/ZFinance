using ZFinance.WebAPI.Models.Security.Action;
using ZWebAPI.Interfaces;

namespace ZFinance.WebAPI.Services.Security.Interfaces
{
    /// <summary>
    /// Service for <see cref="Core.Entities.Security.Actions"/>.
    /// </summary>
    public partial interface IActionsService
    {
        /// <summary>
        /// Deletes the action asynchronous.
        /// </summary>
        /// <param name="actionID">The action identifier.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">When the entity was not found.</exception>
        Task DeleteActionAsync(long actionID);

        /// <summary>
        /// Finds the action by identifier asynchronous.
        /// </summary>
        /// <param name="actionID">The action identifier.</param>
        /// <returns>The display model from the action.</returns>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">When the entity was not found.</exception>
        Task<ActionsDisplayModel> FindActionByIDAsync(long actionID);

        /// <summary>
        /// Inserts a new action asynchronous.
        /// </summary>
        /// <param name="actionModel">The action model.</param>
        /// <returns>The display model from the action.</returns>
        /// <exception cref="ArgumentNullException">When the parameters are <c>null</c>.</exception>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">When the entity validation fails. Check validation result for the validation details.</exception>
        Task<ActionsDisplayModel> InsertNewActionAsync(ActionsInsertModel actionModel);

        /// <summary>
        /// Lists the actions asynchronous.
        /// </summary>
        /// <param name="parameters">The parameters for the list. <see cref="ZWebAPI.Interfaces.IListParameters"/>.</param>
        /// <returns>List with the actions accordingly to the parameters.</returns>
        /// <exception cref="ArgumentNullException">When the parameters are <c>null</c>.</exception>
        Task<IQueryable<ActionsListModel>> ListActionsAsync(IListParameters parameters);

        /// <summary>
        /// Updates the action asynchronous.
        /// </summary>
        /// <param name="actionModel">The action model.</param>
        /// <returns>The display model from the action.</returns>
        /// <exception cref="ArgumentNullException">When the parameters are <c>null</c>.</exception>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">When the entity was not found.</exception>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">When the entity validation fails. Check validation result for the validation details.</exception>
        Task<ActionsDisplayModel> UpdateActionAsync(ActionsUpdateModel actionModel);
    }
}