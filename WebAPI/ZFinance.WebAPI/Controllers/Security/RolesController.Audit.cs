using Microsoft.AspNetCore.Mvc;
using ZDatabase.Exceptions;
using ZFinance.Core.Entities.Audit;
using ZFinance.Core.Entities.Security;
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
        /// Lists the role audit services history asynchronous.
        /// </summary>
        /// <param name="roleID">The role identifier.</param>
        /// <param name="parameters">The parameters for the list. <see cref="ZWebAPI.Interfaces.IListParameters"/>.</param>
        /// <returns>List with the role audit services history accordingly to the parameters.</returns>
        /// <response code="200">OK</response>
        /// <response code="403">Permissions are missing for the current user.</response>
        /// <response code="404">The entity was not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost("{roleID}/[action]")]
        public async Task<IActionResult> Audit([FromRoute] long roleID, [FromBody] ListParametersModel parameters)
        {
            try
            {
                return Ok(await rolesService.AuditRoleServicesHistoryAsync(roleID, parameters));
            }
            catch (MissingUserPermissionException) { return Forbid(); }
            catch (EntityNotFoundException<Roles>) { return NotFound(); }
            catch (Exception ex)
            {
                exceptionHandler.AddBreadcrumb(
                    new Dictionary<string, object?>()
                    {
                        { nameof(roleID), roleID },
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
        /// Lists the role audit operations history asynchronous.
        /// </summary>
        /// <param name="roleID">The role identifier.</param>
        /// <param name="serviceHistoryID">The service history identifier.</param>
        /// <param name="parameters">The parameters for the list. <see cref="ZWebAPI.Interfaces.IListParameters"/>.</param>
        /// <returns>List with the role audit operations history accordingly to the parameters.</returns>
        /// <response code="200">OK</response>
        /// <response code="403">Permissions are missing for the current user.</response>
        /// <response code="404">The entity was not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost("{roleID}/Audit/{serviceHistoryID}")]
        public async Task<IActionResult> Operations([FromRoute] long roleID, [FromRoute] long serviceHistoryID, [FromBody] ListParametersModel parameters)
        {
            try
            {
                return Ok(await rolesService.AuditRoleOperationsHistoryAsync(roleID, serviceHistoryID, parameters));
            }
            catch (MissingUserPermissionException) { return Forbid(); }
            catch (EntityNotFoundException<Roles>) { return NotFound(); }
            catch (EntityNotFoundException<ServicesHistory>) { return NotFound(); }
            catch (Exception ex)
            {
                exceptionHandler.AddBreadcrumb(
                    new Dictionary<string, object?>()
                    {
                        { nameof(roleID), roleID },
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