using ZFinance.Core.Entities.Security;

namespace ZFinance.Core.Repositories.Security.Interfaces
{
    /// <summary>
    /// Repository for <see cref="Users"/>.
    /// </summary>
    public interface IUsersRepository
    {
        /// <summary>
        /// Deletes the user asynchronous.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        Task DisableUserAsync(long userID);

        /// <summary>
        /// Finds the user by identifier asynchronous.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns>The user, if found; otherwise, <c>null</c>.</returns>
        Task<Users?> FindUserByIDAsync(long userID);

        /// <summary>
        /// Finds the user by email asynchronous.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>The user, if found; otherwise, <c>null</c>.</returns>
        Task<Users?> FindUserByEmailAsync(string email);

        /// <summary>
        /// Inserts the user asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        Task InsertUserAsync(Users user);

        /// <summary>
        /// Lists all users.
        /// </summary>
        /// <returns>Query with all users.</returns>
        IQueryable<Users> ListUsers();

        /// <summary>
        /// Updates the user asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        Task UpdateUserAsync(Users user);
    }
}
