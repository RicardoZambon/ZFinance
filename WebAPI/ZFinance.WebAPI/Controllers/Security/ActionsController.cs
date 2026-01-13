using Microsoft.AspNetCore.Mvc;
using ZDatabase.Exceptions;
using ZFinance.Core.Entities.Security;
using ZFinance.Core.Services.Interfaces;
using ZFinance.WebAPI.Models.Security.Action;
using ZFinance.WebAPI.Services.Security.Interfaces;
using ZSecurity.Exceptions;
using ZWebAPI.Models;

namespace ZFinance.WebAPI.Controllers.Security
{
    /// <summary>
    /// Endpoint controller for managing the entity see <see cref="Actions"/>.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiController, Route("api/[controller]")]
    public partial class ActionsController : ControllerBase
    {
        #region Variables
        private readonly IActionsService actionsService;
        private readonly IExceptionHandler exceptionHandler;
        private readonly IHostEnvironment hostEnvironment;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionsController"/> class.
        /// </summary>
        /// <param name="actionsService">The <see cref="IActionsService"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        /// <param name="hostEnvironment">The <see cref="IHostEnvironment"/> instance.</param>
        public ActionsController(
            IActionsService actionsService,
            IExceptionHandler exceptionHandler,
            IHostEnvironment hostEnvironment)
        {
            this.actionsService = actionsService;
            this.hostEnvironment = hostEnvironment;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Deletes the action asynchronous.
        /// </summary>
        /// <param name="actionID">The action identifier.</param>
        /// <response code="200">OK</response>
        /// <response code="403">Permissions are missing for the current user.</response>
        /// <response code="404">The entity was not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpDelete("{actionID}")]
        public async Task<IActionResult> Delete([FromRoute] long actionID)
        {
            try
            {
                await actionsService.DeleteActionAsync(actionID);
                return Ok();
            }
            catch (MissingUserPermissionException) { return Forbid(); }
            catch (EntityNotFoundException<Actions>) { return NotFound(); }
            catch (Exception ex)
            {
                exceptionHandler.AddBreadcrumb(
                    new Dictionary<string, object?>()
                    {
                        { nameof(actionID), actionID },
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
        /// Finds the action by identifier asynchronous.
        /// </summary>
        /// <param name="actionID">The action identifier.</param>
        /// <returns>The display model from the action.</returns>
        /// <response code="200">OK</response>
        /// <response code="403">Permissions are missing for the current user.</response>
        /// <response code="404">The entity was not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("{actionID}")]
        public async Task<IActionResult> Get([FromRoute] long actionID)
        {
            try
            {
                return Ok(await actionsService.FindActionByIDAsync(actionID));
            }
            catch (MissingUserPermissionException) { return Forbid(); }
            catch (EntityNotFoundException<Actions>) { return NotFound(); }
            catch (Exception ex)
            {
                exceptionHandler.AddBreadcrumb(
                    new Dictionary<string, object?>()
                    {
                        { nameof(actionID), actionID },
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
        /// Lists the actions asynchronous.
        /// </summary>
        /// <param name="parameters">The parameters for the list. <see cref="ZWebAPI.Interfaces.IListParameters"/>.</param>
        /// <returns>List with the actions accordingly to the parameters.</returns>
        /// <response code="200">OK</response>
        /// <response code="403">Permissions are missing for the current user.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost("[action]")]
        public async Task<IActionResult> List([FromBody] ListParametersModel parameters)
        {
            try
            {
                return Ok(await actionsService.ListActionsAsync(parameters));
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
        /// Updates the action asynchronous.
        /// </summary>
        /// <param name="model">The action model.</param>
        /// <returns>The display model from the action.</returns>
        /// <response code="200">OK</response>
        /// <response code="400">There were validations errors.</response>
        /// <response code="403">Permissions are missing for the current user.</response>
        /// <response code="404">The entity was not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ActionsUpdateModel model)
        {
            try
            {
                return Ok(await actionsService.UpdateActionAsync(model));
            }
            catch (MissingUserPermissionException) { return Forbid(); }
            catch (EntityNotFoundException<Actions>) { return NotFound(); }
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
        /// Inserts a new action asynchronous.
        /// </summary>
        /// <param name="model">The action model.</param>
        /// <returns>The display model from the action.</returns>
        /// <response code="200">OK</response>
        /// <response code="400">There were validations errors.</response>
        /// <response code="403">Permissions are missing for the current user.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ActionsInsertModel model)
        {
            try
            {
                return Ok(await actionsService.InsertNewActionAsync(model));
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