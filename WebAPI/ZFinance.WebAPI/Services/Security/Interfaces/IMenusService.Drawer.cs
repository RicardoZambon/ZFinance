using ZFinance.WebAPI.Models.Security.Menu;

namespace ZFinance.WebAPI.Services.Security.Interfaces
{
    public partial interface IMenusService
    {
        /// <summary>
        /// Finds the menu by URL for drawer asynchronous.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>The drawer model from the menu.</returns>
        Task<MenusListForDrawerModel?> FindMenuByURLForDrawerAsync(string? url);

        /// <summary>
        /// Lists the menus for the drawer asynchronous.
        /// </summary>
        /// <param name="parentMenuID">The parent menu identifier; <c>null</c> for the top level.</param>
        /// <returns>List with the menus that belong to the parent menu, and the current user has access.</returns>
        Task<IQueryable<MenusListForDrawerModel>> ListMenusForDrawerAsync(long? parentMenuID = null);
    }
}