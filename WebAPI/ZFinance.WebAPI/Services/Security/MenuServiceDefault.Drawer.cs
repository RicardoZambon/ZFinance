using AutoMapper.QueryableExtensions;
using ZDatabase.Services.Interfaces;
using ZFinance.Core.Entities.Security;
using ZFinance.Core.Services.Interfaces;
using ZFinance.WebAPI.Models.Security.Menu;
using ZSecurity.Attributes;

namespace ZFinance.WebAPI.Services.Security
{
    public partial class MenusServiceDefault
    {
        #region Variables
        private readonly ICurrentUserProvider<long> currentUserProvider;
        private readonly IInformationProvider informationProvider;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Public methods
        /// <inheritdoc />
        [ActionMethod]
        public async Task<MenusListForDrawerModel?> FindMenuByURLForDrawerAsync(string? url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            if (currentUserProvider.CurrentUserID is not long currentUserID)
            {
                return null;
            }

            try
            {
                await securityHandler.ValidateUserHasPermissionAsync();

                if (await informationProvider.GetMenuByUrlAsync(currentUserID, url) is Menus menu)
                {
                    return mapper.Map<MenusListForDrawerModel>(menu);
                }
                return null;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in service when finding the menu by URL for the drawer.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(currentUserID), currentUserID },
                        { nameof(url), url },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        [ActionMethod]
        public async Task<IQueryable<MenusListForDrawerModel>> ListMenusForDrawerAsync(long? parentMenuID = null)
        {
            if (currentUserProvider.CurrentUserID is not long currentUserID)
            {
                return Enumerable.Empty<MenusListForDrawerModel>().AsQueryable();
            }

            try
            {
                await securityHandler.ValidateUserHasPermissionAsync();

                return (await informationProvider.ListAllowedMenusAsync(currentUserID, parentMenuID))
                    .AsQueryable()
                    .ProjectTo<MenusListForDrawerModel>(mapper.ConfigurationProvider);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in service when listing menus for the drawer.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(currentUserID), currentUserID },
                        { nameof(parentMenuID), parentMenuID },
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