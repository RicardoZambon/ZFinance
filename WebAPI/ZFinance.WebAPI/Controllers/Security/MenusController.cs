using Microsoft.AspNetCore.Mvc;
using ZDatabase.Exceptions;
using ZFinance.Core.Entities.Security;
using ZFinance.Core.Services.Interfaces;
using ZFinance.WebAPI.Models.Security.Menu;
using ZFinance.WebAPI.Services.Security.Interfaces;
using ZSecurity.Exceptions;
using ZWebAPI.Models;

namespace ZFinance.WebAPI.Controllers.Security
{
    /// <summary>
    /// Endpoint controller for managing the entity see <see cref="Menus"/>.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiController, Route("api/[controller]")]
    public partial class MenusController : ControllerBase
    {
        #region Variables
        private readonly IExceptionHandler exceptionHandler;
        private readonly IHostEnvironment hostEnvironment;
        private readonly IMenusService menusService;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MenusController"/> class.
        /// </summary>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        /// <param name="hostEnvironment">The <see cref="IHostEnvironment"/> instance.</param>
        /// <param name="menusService">The <see cref="IMenusService"/> instance.</param>
        public MenusController(
            IHostEnvironment hostEnvironment,
            IMenusService menusService,
            IExceptionHandler exceptionHandler)
        {
            this.hostEnvironment = hostEnvironment;
            this.exceptionHandler = exceptionHandler;
            this.menusService = menusService;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Deletes the menu asynchronous.
        /// </summary>
        /// <param name="menuID">The menu identifier.</param>
        /// <response code="200">OK</response>
        /// <response code="403">Permissions are missing for the current user.</response>
        /// <response code="404">The entity was not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpDelete("{menuID}")]
        public async Task<IActionResult> Delete([FromRoute] long menuID)
        {
            try
            {
                await menusService.DeleteMenuAsync(menuID);
                return Ok();
            }
            catch (MissingUserPermissionException) { return Forbid(); }
            catch (EntityNotFoundException<Menus>) { return NotFound(); }
            catch (Exception ex)
            {
                exceptionHandler.AddBreadcrumb(
                    new Dictionary<string, object?>()
                    {
                        { nameof(menuID), menuID },
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
        /// Finds the menu by identifier asynchronous.
        /// </summary>
        /// <param name="menuID">The menu identifier.</param>
        /// <returns>The display model from the menu.</returns>
        /// <response code="200">OK</response>
        /// <response code="403">Permissions are missing for the current user.</response>
        /// <response code="404">The entity was not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("{menuID}")]
        public async Task<IActionResult> Get([FromRoute] long menuID)
        {
            try
            {
                return Ok(await menusService.FindMenuByIDAsync(menuID));
            }
            catch (MissingUserPermissionException) { return Forbid(); }
            catch (EntityNotFoundException<Menus>) { return NotFound(); }
            catch (Exception ex)
            {
                exceptionHandler.AddBreadcrumb(
                    new Dictionary<string, object?>()
                    {
                        { nameof(menuID), menuID },
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
        /// Lists the menus asynchronous.
        /// </summary>
        /// <param name="parameters">The parameters for the list. <see cref="ZWebAPI.Interfaces.IListParameters"/>.</param>
        /// <returns>List with the menus accordingly to the parameters.</returns>
        /// <response code="200">OK</response>
        /// <response code="403">Permissions are missing for the current user.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost("[action]")]
        public async Task<IActionResult> List([FromBody] ListParametersModel parameters)
        {
            try
            {
                return Ok(await menusService.ListMenusAsync(parameters));
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
        /// Updates the menu asynchronous.
        /// </summary>
        /// <param name="model">The menu model.</param>
        /// <returns>The display model from the menu.</returns>
        /// <response code="200">OK</response>
        /// <response code="400">There were validations errors.</response>
        /// <response code="403">Permissions are missing for the current user.</response>
        /// <response code="404">The entity was not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MenusUpdateModel model)
        {
            try
            {
                return Ok(await menusService.UpdateMenuAsync(model));
            }
            catch (MissingUserPermissionException) { return Forbid(); }
            catch (EntityNotFoundException<Menus>) { return NotFound(); }
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
        /// Inserts a new menu asynchronous.
        /// </summary>
        /// <param name="model">The menu model.</param>
        /// <returns>The display model from the menu.</returns>
        /// <response code="200">OK</response>
        /// <response code="400">There were validations errors.</response>
        /// <response code="403">Permissions are missing for the current user.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] MenusInsertModel model)
        {
            try
            {
                return Ok(await menusService.InsertNewMenuAsync(model));
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