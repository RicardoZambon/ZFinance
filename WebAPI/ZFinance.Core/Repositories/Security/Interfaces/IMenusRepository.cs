using ZFinance.Core.Entities.Security;

namespace ZFinance.Core.Repositories.Security.Interfaces
{
    /// <summary>
    /// Repository for <see cref="Menus"/>.
    /// </summary>
    public interface IMenusRepository
    {
        /// <summary>
        /// Deletes the menu asynchronous.
        /// </summary>
        /// <param name="menuID">The menu identifier.</param>
        Task DeleteMenuAsync(long menuID);

        /// <summary>
        /// Finds the menu by identifier asynchronous.
        /// </summary>
        /// <param name="menuID">The menu identifier.</param>
        /// <returns>The menu, if found; otherwise, <c>null</c>.</returns>
        Task<Menus?> FindMenuByIDAsync(long menuID);

        /// <summary>
        /// Inserts the menu asynchronous.
        /// </summary>
        /// <param name="menu">The menu.</param>
        Task InsertMenuAsync(Menus menu);

        /// <summary>
        /// Lists all menu.
        /// </summary>
        /// <returns>Query with all menu.</returns>
        IQueryable<Menus> ListMenus();

        /// <summary>
        /// Updates the menu asynchronous.
        /// </summary>
        /// <param name="menu">The menu.</param>
        Task UpdateMenuAsync(Menus menu);
    }
}