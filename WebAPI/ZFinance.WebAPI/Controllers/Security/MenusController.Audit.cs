using Microsoft.AspNetCore.Mvc;
using ZFinance.Core.Entities.Audit;
using ZFinance.Core.Entities.Security;
using ZDatabase.Exceptions;
using ZSecurity.Exceptions;
using ZWebAPI.Models;

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
        /// Lists the menu audit services history asynchronous.
        /// </summary>
        /// <param name="menuID">The menu identifier.</param>
        /// <param name="parameters">The parameters for the list. <see cref="ZWebAPI.Interfaces.IListParameters"/>.</param>
        /// <returns>List with the menu audit services history accordingly to the parameters.</returns>
        /// <response code="200">OK</response>
        /// <response code="403">Permissions are missing for the current user.</response>
        /// <response code="404">The entity was not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost("{menuID}/[action]")]
        public async Task<IActionResult> Audit([FromRoute] long menuID, [FromBody] ListParametersModel parameters)
        {
            try
            {
                return Ok(await menusService.AuditMenuServicesHistoryAsync(menuID, parameters));
            }
            catch (MissingUserPermissionException) { return Forbid(); }
            catch (EntityNotFoundException<Menus>) { return NotFound(); }
            catch (Exception ex)
            {
                exceptionHandler.AddBreadcrumb(
                    new Dictionary<string, object?>()
                    {
                        { nameof(menuID), menuID },
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

        /// <summary>
        /// Lists the menu audit operations history asynchronous.
        /// </summary>
        /// <param name="menuID">The menu identifier.</param>
        /// <param name="serviceHistoryID">The service history identifier.</param>
        /// <param name="parameters">The parameters for the list. <see cref="ZWebAPI.Interfaces.IListParameters"/>.</param>
        /// <returns>List with the menu audit operations history accordingly to the parameters.</returns>
        /// <response code="200">OK</response>
        /// <response code="403">Permissions are missing for the current user.</response>
        /// <response code="404">The entity was not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost("{menuID}/Audit/{serviceHistoryID}")]
        public async Task<IActionResult> Operations([FromRoute] long menuID, [FromRoute] long serviceHistoryID, [FromBody] ListParametersModel parameters)
        {
            try
            {
                return Ok(await menusService.AuditMenuOperationsHistoryAsync(menuID, serviceHistoryID, parameters));
            }
            catch (MissingUserPermissionException) { return Forbid(); }
            catch (EntityNotFoundException<Menus>) { return NotFound(); }
            catch (EntityNotFoundException<ServicesHistory>) { return NotFound(); }
            catch (Exception ex)
            {
                exceptionHandler.AddBreadcrumb(
                    new Dictionary<string, object?>()
                    {
                        { nameof(menuID), menuID },
                        { nameof(serviceHistoryID), serviceHistoryID },
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