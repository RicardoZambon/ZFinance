using Microsoft.AspNetCore.Mvc;
using ZSecurity.Exceptions;
using ZWebAPI.Models;

namespace ZFinance.WebAPI.Controllers.Security
{
    public partial class RolesController
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Public methods
        /// <summary>
        /// Catalogs the roles asynchronous.
        /// </summary>
        /// <param name="parameters">The parameters. <see cref="ZWebAPI.Interfaces.ICatalogParameters"/>.</param>
        /// <returns>Catalog result with the roles accordingly to the parameters.</returns>
        /// <response code="200">OK</response>
        /// <response code="403">Permissions are missing for the current user.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost("[action]")]
        public async Task<IActionResult> Catalog([FromBody] CatalogParametersModel parameters)
        {
            try
            {
                return Ok(await rolesService.CatalogRolesAsync(parameters));
            }
            catch (MissingUserPermissionException) { return Forbid(); }
            catch (Exception ex)
            {
                exceptionHandler.AddBreadcrumb(
                    new Dictionary<string, object?>()
                    {
                        { nameof(parameters), parameters },
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