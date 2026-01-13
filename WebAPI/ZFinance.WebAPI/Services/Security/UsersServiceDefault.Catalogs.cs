using ZSecurity.Attributes;
using ZWebAPI.ExtensionMethods;
using ZWebAPI.Interfaces;
using ZWebAPI.Models.Catalog;

namespace ZFinance.WebAPI.Services.Security
{
    public partial class UsersServiceDefault
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
        public async Task<CatalogResultModel<long>> CatalogUsersAsync(ICatalogParameters parameters)
        {
            try
            {
                await securityHandler.ValidateUserHasPermissionAsync();

                return usersRepository.ListUsers()
                    .Where(x => x.IsActive)
                    .GetCatalog(
                        parameters: parameters,
                        valueSelector: x => x.ID,
                        displaySelector: x => x.Email
                    );
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in service when cataloging users.",
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