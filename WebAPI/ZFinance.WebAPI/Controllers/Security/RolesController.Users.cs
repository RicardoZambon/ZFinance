using Microsoft.AspNetCore.Mvc;
using ZDatabase.Exceptions;
using ZFinance.Core.Entities.Security;
using ZFinance.WebAPI.Models;
using ZFinance.WebAPI.Exceptions;
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
        /// Lists the users assigned to the role asynchronous.
        /// </summary>
        /// <param name="roleID">The role identifier.</param>
        /// <param name="parameters">The parameters for the list. <see cref="ZWebAPI.Interfaces.IListParameters"/>.</param>
        /// <returns>List with the users assigned to the role accordingly to the parameters.</returns>
        /// <response code="200">OK</response>
        /// <response code="403">Permissions are missing for the current user.</response>
        /// <response code="404">The entity was not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost("{roleID}/[action]/List")]
        public async Task<IActionResult> Users([FromRoute] long roleID, [FromBody] ListParametersModel parameters)
        {
            try
            {
                return Ok(await rolesService.ListRoleUsersAsync(roleID, parameters));
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
        /// Updates the relationship between role and user asynchronous.
        /// </summary>
        /// <param name="roleID">The role identifier.</param>
        /// <param name="model">The relationship update model.</param>
        /// <response code="200">OK</response>
        /// <response code="400">There were validations errors.</response>
        /// <response code="403">Permissions are missing for the current user.</response>
        /// <response code="404">The entity was not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost("{roleID}/[action]")]
        public async Task<IActionResult> Users([FromRoute] long roleID, [FromBody] RelationshipUpdateModel<long> model)
        {
            try
            {
                await rolesService.UpdateRelationshipRoleUsersAsync(roleID, model);
                return Ok();
            }
            catch (MissingUserPermissionException) { return Forbid(); }
            catch (EntityNotFoundException<Roles>) { return NotFound(); }
            catch (EntityValidationFailureException<long> validationEx) { return ValidationProblem(new EntityValidationProblemDetails<long>(validationEx)); }
            catch (Exception ex)
            {
                exceptionHandler.AddBreadcrumb(
                    new Dictionary<string, object?>()
                    {
                        { nameof(roleID), roleID },
                        { nameof(model), model },
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