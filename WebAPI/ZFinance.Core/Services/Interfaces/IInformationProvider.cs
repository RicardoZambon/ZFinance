using ZFinance.Core.Entities.Security;
using ZSecurity.Repositories.Interfaces;

namespace ZFinance.Core.Services.Interfaces
{
    /// <summary>
    /// Provides information related to the current user.
    /// </summary>
    /// <seealso cref="IBaseUsersRepository{TActions, TUsersKey}" />
    public interface IInformationProvider : IBaseUsersRepository<Actions, long>
    {
        /// <summary>
        /// Gets the menu by URL asynchronous.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="url">The URL.</param>
        /// <returns>The menu if found and the current user has access, otherwise <c>null</c>.</returns>
        Task<Menus?> GetMenuByUrlAsync(long userID, string url);

        /// <summary>
        /// Lists the allowed menus asynchronous.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="parentMenuID">The parent menu identifier.</param>
        /// <returns>List of allowed menus.</returns>
        Task<IEnumerable<Menus>> ListAllowedMenusAsync(long userID, long? parentMenuID);
    }
}