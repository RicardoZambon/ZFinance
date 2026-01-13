using ZFinance.WebAPI.Models.Security.Menu;
using ZWebAPI.Interfaces;

namespace ZFinance.WebAPI.Services.Security.Interfaces
{
    /// <summary>
    /// Service for <see cref="Core.Entities.Security.Menus"/>.
    /// </summary>
    public partial interface IMenusService
    {
        /// <summary>
        /// Deletes the menu asynchronous.
        /// </summary>
        /// <param name="menuID">The menu identifier.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">When the entity was not found.</exception>
        Task DeleteMenuAsync(long menuID);

        /// <summary>
        /// Finds the menu by identifier asynchronous.
        /// </summary>
        /// <param name="menuID">The menu identifier.</param>
        /// <returns>The display model from the menu.</returns>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">When the entity was not found.</exception>
        Task<MenusDisplayModel> FindMenuByIDAsync(long menuID);

        /// <summary>
        /// Inserts a new menu asynchronous.
        /// </summary>
        /// <param name="menuModel">The menu model.</param>
        /// <returns>The display model from the menu.</returns>
        /// <exception cref="ArgumentNullException">When the parameters are <c>null</c>.</exception>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">When the entity validation fails. Check validation result for the validation details.</exception>
        Task<MenusDisplayModel> InsertNewMenuAsync(MenusInsertModel menuModel);

        /// <summary>
        /// Lists the menus asynchronous.
        /// </summary>
        /// <param name="parameters">The parameters for the list. <see cref="ZWebAPI.Interfaces.IListParameters"/>.</param>
        /// <returns>List with the menus accordingly to the parameters.</returns>
        /// <exception cref="ArgumentNullException">When the parameters are <c>null</c>.</exception>
        Task<IQueryable<MenusListModel>> ListMenusAsync(IListParameters parameters);

        /// <summary>
        /// Updates the menu asynchronous.
        /// </summary>
        /// <param name="menuModel">The menu model.</param>
        /// <returns>The display model from the menu.</returns>
        /// <exception cref="ArgumentNullException">When the parameters are <c>null</c>.</exception>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">When the entity was not found.</exception>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">When the entity validation fails. Check validation result for the validation details.</exception>
        Task<MenusDisplayModel> UpdateMenuAsync(MenusUpdateModel menuModel);
    }
}