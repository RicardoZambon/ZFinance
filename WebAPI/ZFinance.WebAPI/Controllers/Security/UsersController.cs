using Microsoft.AspNetCore.Mvc;
using ZDatabase.Exceptions;
using ZFinance.Core.Entities.Security;
using ZFinance.Core.Services.Interfaces;
using ZFinance.WebAPI.Models.Security.User;
using ZFinance.WebAPI.Services.Security.Interfaces;
using ZSecurity.Exceptions;
using ZWebAPI.Models;

namespace ZFinance.WebAPI.Controllers.Security
{
    /// <summary>
    /// Endpoint controller for managing the entity see <see cref="Users"/>.
    /// </summary>
    /// <seealso cref="ControllerBase" />
    [ApiController, Route("api/[controller]")]
    public partial class UsersController : ControllerBase
    {
        #region Variables
        private readonly IExceptionHandler exceptionHandler;
        private readonly IHostEnvironment hostEnvironment;
        private readonly IUsersService usersService;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        /// <param name="hostEnvironment">The <see cref="IHostEnvironment"/> instance.</param>
        /// <param name="usersService">The <see cref="IUsersService"/> instance.</param>
        public UsersController(
            IExceptionHandler exceptionHandler,
            IHostEnvironment hostEnvironment,
            IUsersService usersService)
        {
            this.hostEnvironment = hostEnvironment;
            this.exceptionHandler = exceptionHandler;
            this.usersService = usersService;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Disables the user asynchronously.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <response code="200">OK</response>
        /// <response code="403">Permissions are missing for the current user.</response>
        /// <response code="404">The entity was not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpDelete("{userID}")]
        public async Task<IActionResult> Disable([FromRoute] long userID)
        {
            try
            {
                await usersService.DisableUserAsync(userID);
                return Ok();
            }
            catch (MissingUserPermissionException) { return Forbid(); }
            catch (EntityNotFoundException<Users>) { return NotFound(); }
            catch (Exception ex)
            {
                exceptionHandler.AddBreadcrumb(
                    new Dictionary<string, object?>()
                    {
                        { nameof(userID), userID },
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
        /// Finds the user by identifier asynchronous.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns>The display model from the user.</returns>
        /// <response code="200">OK</response>
        /// <response code="403">Permissions are missing for the current user.</response>
        /// <response code="404">The entity was not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("{userID}")]
        public async Task<IActionResult> Get([FromRoute] long userID)
        {
            try
            {
                return Ok(await usersService.FindUserByIDAsync(userID));
            }
            catch (MissingUserPermissionException) { return Forbid(); }
            catch (EntityNotFoundException<Users>) { return NotFound(); }
            catch (Exception ex)
            {
                exceptionHandler.AddBreadcrumb(
                    new Dictionary<string, object?>()
                    {
                        { nameof(userID), userID },
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
        /// Lists the users asynchronously.
        /// </summary>
        /// <param name="parameters">The parameters for the list. <see cref="ZWebAPI.Interfaces.IListParameters"/>.</param>
        /// <returns>List with the users accordingly to the parameters.</returns>
        /// <response code="200">OK</response>
        /// <response code="403">Permissions are missing for the current user.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost("[action]")]
        public async Task<IActionResult> List([FromBody] ListParametersModel parameters)
        {
            try
            {
                return Ok(await usersService.ListUsersAsync(parameters));
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
        /// Updates the user asynchronously.
        /// </summary>
        /// <param name="model">The user model.</param>
        /// <returns>The display model from the user.</returns>
        /// <response code="200">OK</response>
        /// <response code="400">There were validations errors.</response>
        /// <response code="403">Permissions are missing for the current user.</response>
        /// <response code="404">The entity was not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UsersUpdateModel model)
        {
            try
            {
                return Ok(await usersService.UpdateUserAsync(model));
            }
            catch (MissingUserPermissionException) { return Forbid(); }
            catch (EntityNotFoundException<Users>) { return NotFound(); }
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
        /// Inserts a new user asynchronously.
        /// </summary>
        /// <param name="model">The user model.</param>
        /// <returns>The display model from the user.</returns>
        /// <response code="200">OK</response>
        /// <response code="400">There were validations errors.</response>
        /// <response code="403">Permissions are missing for the current user.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UsersInsertModel model)
        {
            try
            {
                return Ok(await usersService.InsertNewUserAsync(model));
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