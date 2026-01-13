using Microsoft.AspNetCore.Mvc;
using ZDatabase.Exceptions;
using ZFinance.Core.Entities.Security;
using ZFinance.Core.Services.Interfaces;
using ZFinance.WebAPI.Models.Security.Role;
using ZFinance.WebAPI.Services.Security.Interfaces;
using ZSecurity.Exceptions;
using ZWebAPI.Models;

namespace ZFinance.WebAPI.Controllers.Security
{
    /// <summary>
    /// Endpoint controller for managing the entity see <see cref="Roles"/>.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiController, Route("api/[controller]")]
    public partial class RolesController : ControllerBase
    {
        #region Variables
        private readonly IExceptionHandler exceptionHandler;
        private readonly IHostEnvironment hostEnvironment;
        private readonly IRolesService rolesService;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RolesController"/> class.
        /// </summary>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        /// <param name="hostEnvironment">The <see cref="IHostEnvironment"/> instance.</param>
        /// <param name="rolesService">The <see cref="IRolesService"/> instance.</param>
        public RolesController(
            IExceptionHandler exceptionHandler,
            IHostEnvironment hostEnvironment,
            IRolesService rolesService)
        {
            this.rolesService = rolesService;
            this.hostEnvironment = hostEnvironment;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Deletes the role asynchronous.
        /// </summary>
        /// <param name="roleID">The role identifier.</param>
        /// <response code="200">OK</response>
        /// <response code="403">Permissions are missing for the current user.</response>
        /// <response code="404">The entity was not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpDelete("{roleID}")]
        public async Task<IActionResult> Delete([FromRoute] long roleID)
        {
            try
            {
                await rolesService.DeleteRoleAsync(roleID);
                return Ok();
            }
            catch (MissingUserPermissionException) { return Forbid(); }
            catch (EntityNotFoundException<Roles>) { return NotFound(); }
            catch (Exception ex)
            {
                exceptionHandler.AddBreadcrumb(
                    new Dictionary<string, object?>()
                    {
                        { nameof(roleID), roleID },
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
        /// Finds the role by identifier asynchronous.
        /// </summary>
        /// <param name="roleID">The role identifier.</param>
        /// <returns>The display model from the role.</returns>
        /// <response code="200">OK</response>
        /// <response code="403">Permissions are missing for the current user.</response>
        /// <response code="404">The entity was not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("{roleID}")]
        public async Task<IActionResult> Get([FromRoute] long roleID)
        {
            try
            {
                return Ok(await rolesService.FindRoleByIDAsync(roleID));
            }
            catch (MissingUserPermissionException) { return Forbid(); }
            catch (EntityNotFoundException<Roles>) { return NotFound(); }
            catch (Exception ex)
            {
                exceptionHandler.AddBreadcrumb(
                    new Dictionary<string, object?>()
                    {
                        { nameof(roleID), roleID },
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
        /// Lists the roles asynchronous.
        /// </summary>
        /// <param name="parameters">The parameters for the list. <see cref="ZWebAPI.Interfaces.IListParameters"/>.</param>
        /// <returns>List with the roles accordingly to the parameters.</returns>
        /// <response code="200">OK</response>
        /// <response code="403">Permissions are missing for the current user.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost("[action]")]
        public async Task<IActionResult> List([FromBody] ListParametersModel parameters)
        {
            try
            {
                return Ok(await rolesService.ListRolesAsync(parameters));
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

        /// <summary>
        /// Updates the role asynchronous.
        /// </summary>
        /// <param name="model">The role model.</param>
        /// <returns>The display model from the role.</returns>
        /// <response code="200">OK</response>
        /// <response code="400">There were validations errors.</response>
        /// <response code="403">Permissions are missing for the current user.</response>
        /// <response code="404">The entity was not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RolesUpdateModel model)
        {
            try
            {
                return Ok(await rolesService.UpdateRoleAsync(model));
            }
            catch (MissingUserPermissionException) { return Forbid(); }
            catch (EntityNotFoundException<Roles>) { return NotFound(); }
            catch (EntityValidationFailureException<long> validationEx) { return ValidationProblem(new ValidationProblemDetails(validationEx.ValidationResult.Errors)); }
            catch (Exception ex)
            {
                exceptionHandler.AddBreadcrumb(
                    new Dictionary<string, object?>()
                    {
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

        /// <summary>
        /// Inserts a new role asynchronous.
        /// </summary>
        /// <param name="model">The role model.</param>
        /// <returns>The display model from the role.</returns>
        /// <response code="200">OK</response>
        /// <response code="400">There were validations errors.</response>
        /// <response code="403">Permissions are missing for the current user.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] RolesInsertModel model)
        {
            try
            {
                return Ok(await rolesService.InsertNewRoleAsync(model));
            }
            catch (MissingUserPermissionException) { return Forbid(); }
            catch (EntityValidationFailureException<long> validationEx) { return ValidationProblem(new ValidationProblemDetails(validationEx.ValidationResult.Errors)); }
            catch (Exception ex)
            {
                exceptionHandler.AddBreadcrumb(
                    new Dictionary<string, object?>()
                    {
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