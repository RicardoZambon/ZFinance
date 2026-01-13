using ZSecurity.Attributes;
using ZWebAPI.ExtensionMethods;
using ZWebAPI.Interfaces;
using ZWebAPI.Models.Catalog;

namespace ZFinance.WebAPI.Services.Security
{
    public partial class RolesServiceDefault
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Public methods
        /// <inheritdoc />
        [ActionMethod]
        public async Task<CatalogResultModel<long>> CatalogRolesAsync(ICatalogParameters parameters)
        {
            try
            {
                await securityHandler.ValidateUserHasPermissionAsync();

                return rolesRepository.ListRoles()
                    .GetCatalog(
                        parameters: parameters,
                        valueSelector: x => x.ID,
                        displaySelector: x => x.Name
                    );
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in service when cataloging roles.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(parameters), parameters },
                    }
                );
                throw;
            }
        }
        #endregion

        #region Private methods
        #endregion
    }
}