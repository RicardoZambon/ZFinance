using ZWebAPI.Interfaces;
using ZWebAPI.Models.Catalog;

namespace ZFinance.WebAPI.Services.Security.Interfaces
{
    public partial interface IRolesService
    {
        /// <summary>
        /// Catalogs the roles asynchronous.
        /// </summary>
        /// <param name="parameters">The parameters. <see cref="ZWebAPI.Interfaces.ICatalogParameters"/>.</param>
        /// <returns>Catalog result with the roles accordingly to the parameters.</returns>
        Task<CatalogResultModel<long>> CatalogRolesAsync(ICatalogParameters parameters);
    }
}