using ZWebAPI.Interfaces;
using ZWebAPI.Models.Catalog;

namespace ZFinance.WebAPI.Services.Security.Interfaces
{
    public partial interface IUsersService
    {
        /// <summary>
        /// Catalogs the users asynchronous.
        /// </summary>
        /// <param name="parameters">The parameters. <see cref="ICatalogParameters"/>.</param>
        /// <returns>Catalog result with the users accordingly to the parameters.</returns>
        Task<CatalogResultModel<long>> CatalogUsersAsync(ICatalogParameters parameters);
    }
}