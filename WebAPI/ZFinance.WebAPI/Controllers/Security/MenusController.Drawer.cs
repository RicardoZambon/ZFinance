using Microsoft.AspNetCore.Mvc;
using ZSecurity.Exceptions;

namespace ZFinance.WebAPI.Controllers.Security
{
    public partial class MenusController
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Public methods
        /// <summary>
        /// Lists the menus for the drawer asynchronous.
        /// </summary>
        /// <returns>List with the menus from the root level and the current user has access.</returns>
        /// <response code="200">OK</response>
        /// <response code="403">Missing permissions to current user.</response>
        /// <response code="500">Internal server error. Check response body.</response>
        [HttpPost("[action]")]
        public async Task<IActionResult> Drawer()
        {
            try
            {
                return Ok(await menusService.ListMenusForDrawerAsync());
            }
            catch (MissingUserPermissionException) { return Forbid(); }
            catch (Exception ex)
            {
                exceptionHandler.AddBreadcrumb();

                Guid? exceptionID = exceptionHandler.CaptureException(ex);
                if (hostEnvironment.IsDevelopment())
                {
                    return StatusCode(500, new
                    {
                        exceptionID,
                        InnerExceptionMessage = ex.InnerException?.Message,
                        ex.Message,
                        ex.StackTrace,
                    });
                }

                return StatusCode(500, exceptionID);
            }
        }

        /// <summary>
        /// Lists the menus for the drawer asynchronous.
        /// </summary>
        /// <param name="parentMenuID">The parent menu identifier.</param>
        /// <returns>List with the menus that belong to the parent menu and the current user has access.</returns>
        /// <response code="200">OK</response>
        /// <response code="403">Missing permissions to current user.</response>
        /// <response code="500">Internal server error. Check response body.</response>
        [HttpPost("[action]/{parentMenuID}")]
        public async Task<IActionResult> Drawer([FromRoute] long parentMenuID)
        {
            try
            {
                return Ok(await menusService.ListMenusForDrawerAsync(parentMenuID));
            }
            catch (MissingUserPermissionException) { return Forbid(); }
            catch (Exception ex)
            {
                exceptionHandler.AddBreadcrumb(
                    new Dictionary<string, object?>()
                    {
                        { nameof(parentMenuID), parentMenuID },
                    }
                );

                Guid? exceptionID = exceptionHandler.CaptureException(ex);
                if (hostEnvironment.IsDevelopment())
                {
                    return StatusCode(500, new
                    {
                        exceptionID,
                        InnerExceptionMessage = ex.InnerException?.Message,
                        ex.Message,
                        ex.StackTrace,
                    });
                }

                return StatusCode(500, exceptionID);
            }
        }

        /// <summary>
        /// Finds the menu by URL for drawer asynchronous.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>The drawer model from the menu.</returns>
        /// <response code="200">OK</response>
        /// <response code="403">Missing permissions to current user.</response>
        /// <response code="500">Internal server error. Check response body.</response>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetItem([FromQuery] string? url)
        {
            try
            {
                return Ok(await menusService.FindMenuByURLForDrawerAsync(url));
            }
            catch (MissingUserPermissionException) { return Forbid(); }
            catch (Exception ex)
            {
                exceptionHandler.AddBreadcrumb(
                    new Dictionary<string, object?>()
                    {
                        { nameof(url), url },
                    }
                );

                Guid? exceptionID = exceptionHandler.CaptureException(ex);
                if (hostEnvironment.IsDevelopment())
                {
                    return StatusCode(500, new
                    {
                        exceptionID,
                        InnerExceptionMessage = ex.InnerException?.Message,
                        ex.Message,
                        ex.StackTrace,
                    });
                }

                return StatusCode(500, exceptionID);
            }
        }
        #endregion

        #region Private methods
        #endregion
    }
}