using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZDatabase.Exceptions;
using ZFinance.Core.Entities.Security;
using ZFinance.WebAPI.Models.Authentication;
using ZFinance.WebAPI.Services.Interfaces;
using ZFinance.WebAPI.Exceptions;

namespace ZFinance.WebAPI.Controllers
{
    /// <summary>
    /// Endpoints controller for authentication management.
    /// </summary>
    /// <seealso cref="ControllerBase" />
    [ApiController, Route("api/[controller]")]
    public partial class AuthenticationController : ControllerBase
    {
        #region Variables
        private readonly IAuthenticationService authenticationService;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationController"/> class.
        /// </summary>
        /// <param name="authenticationService">The <see cref="IAuthenticationService"/> instance.</param>
        public AuthenticationController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Gets the user actions asynchronous.
        /// </summary>
        /// <returns>Returns a list of user actions.</returns>
        /// <response code="200">OK</response>
        /// <response code="404">The entity was not found.</response>
        /// <response code="500">Internal server error. Check response body.</response>
        [HttpPost, Route("[action]")]
        public async Task<IActionResult> GetActions()
        {
            try
            {
                return Ok(await authenticationService.GetActionsAsync());
            }
            catch (EntityNotFoundException<Users>) { return NotFound(); }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    InnerExceptionMessage = ex.InnerException?.Message,
                    ex.Message,
                    ex.StackTrace,
                });
            }
        }

        /// <summary>
        /// Refreshes the token asynchronous.
        /// </summary>
        /// <param name="refreshTokenModel">The refresh token model.</param>
        /// <returns>Returns authentication response model <see cref="AuthenticationResponseModel" />.</returns>
        /// <response code="200">OK</response>
        /// <response code="401">Invalid refresh token or is expired.</response>
        /// <response code="500">Internal server error. Check response body.</response>
        [HttpPost, Route("[action]"), AllowAnonymous]
        public async Task<IActionResult> RefreshToken(AuthenticationRefreshTokenModel refreshTokenModel)
        {
            try
            {
                return Ok(await authenticationService.RefreshTokenAsync(refreshTokenModel));
            }
            catch (InvalidRefreshTokenException) { return Unauthorized(); }
            catch (RefreshTokenNotFoundException) { return Unauthorized(); }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    InnerExceptionMessage = ex.InnerException?.Message,
                    ex.Message,
                    ex.StackTrace,
                });
            }
        }

        /// <summary>
        /// Signs in the user asynchronous.
        /// </summary>
        /// <param name="signInModel">The sign in model.</param>
        /// <returns>Returns authentication response model <see cref="AuthenticationResponseModel" />.</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /SignIn
        ///     {
        ///         "UserName": "root",
        ///         "Password": "root"
        ///     }
        ///     
        /// </remarks>
        /// <response code="200">OK</response>
        /// <response code="401">Invalid sign in credentials.</response>
        /// <response code="500">Internal server error. Check response body.</response>
        [HttpPost, Route("[action]"), AllowAnonymous]
        public async Task<IActionResult> SignIn(AuthenticationSignInModel signInModel)
        {
            try
            {
                return Ok(await authenticationService.SignInAsync(signInModel));
            }
            catch (InvalidAuthenticationException) { return Unauthorized(); }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    InnerExceptionMessage = ex.InnerException?.Message,
                    ex.Message,
                    ex.StackTrace,
                });
            }
        }
        #endregion

        #region Private methods
        #endregion
    }
}