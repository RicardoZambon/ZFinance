using ZFinance.WebAPI.Models.Security.User;
using ZWebAPI.Interfaces;

namespace ZFinance.WebAPI.Services.Security.Interfaces
{
    /// <summary>
    /// Service for <see cref="Core.Entities.Security.Users"/>.
    /// </summary>
    public partial interface IUsersService
    {
        /// <summary>
        /// Disables the user asynchronous.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">When the entity was not found.</exception>
        Task DisableUserAsync(long userID);

        /// <summary>
        /// Finds the user by identifier asynchronous.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns>The display model from the user.</returns>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">When the entity was not found.</exception>
        Task<UsersDisplayModel> FindUserByIDAsync(long userID);

        /// <summary>
        /// Inserts a new user asynchronous.
        /// </summary>
        /// <param name="userModel">The user model.</param>
        /// <returns>The display model from the user.</returns>
        /// <exception cref="ArgumentNullException">When the parameters are <c>null</c>.</exception>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">When the entity validation fails. Check validation result for the validation details.</exception>
        Task<UsersDisplayModel> InsertNewUserAsync(UsersInsertModel userModel);

        /// <summary>
        /// Lists the users asynchronously.
        /// </summary>
        /// <param name="parameters">The parameters for the list. <see cref="IListParameters"/>.</param>
        /// <returns>List with the users accordingly to the parameters.</returns>
        /// <exception cref="ArgumentNullException">When the parameters are <c>null</c>.</exception>
        Task<IQueryable<UsersListModel>> ListUsersAsync(IListParameters parameters);

        /// <summary>
        /// Updates the user asynchronous.
        /// </summary>
        /// <param name="userModel">The user model.</param>
        /// <returns>The display model from the user.</returns>
        /// <exception cref="ArgumentNullException">When the parameters are <c>null</c>.</exception>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">When the entity was not found.</exception>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">When the entity validation fails. Check validation result for the validation details.</exception>
        Task<UsersDisplayModel> UpdateUserAsync(UsersUpdateModel userModel);
    }
}