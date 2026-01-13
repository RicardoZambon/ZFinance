using ZWebAPI.Interfaces;
using ZWebAPI.Models.Catalog;

namespace ZFinance.WebAPI.Services.Security.Interfaces
{
    public partial interface IMenusService
    {
        /// <summary>
        /// Catalogs the menus asynchronous.
        /// </summary>
        /// <param name="parameters">The parameters. <see cref="ZWebAPI.Interfaces.ICatalogParameters"/>.</param>
        /// <returns>Catalog result with the menus accordingly to the parameters.</returns>
        Task<CatalogResultModel<long>> CatalogMenusAsync(ICatalogParameters parameters);
    }
}