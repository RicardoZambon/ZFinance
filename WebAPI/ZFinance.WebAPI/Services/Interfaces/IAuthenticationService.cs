using ZFinance.WebAPI.Models.Authentication;

namespace ZFinance.WebAPI.Services.Interfaces
{
    /// <summary>
    /// Service for user authentication.
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Gets the user actions asynchronous.
        /// </summary>
        /// <returns>Returns a list of user actions.</returns>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">When the current user identifier is invalid.</exception>
        Task<IEnumerable<string?>> GetActionsAsync();

        /// <summary>
        /// Refreshes the token asynchronous.
        /// </summary>
        /// <param name="refreshTokenModel">The refresh token model.</param>
        /// <returns>Returns authentication response model <see cref="AuthenticationResponseModel" />.</returns>
        /// <exception cref="Exceptions.RefreshTokenNotFoundException">When the refresh token is not valid.</exception>
        /// <exception cref="Exceptions.InvalidRefreshTokenException">Then the refresh token is expired.</exception>
        Task<AuthenticationResponseModel> RefreshTokenAsync(AuthenticationRefreshTokenModel refreshTokenModel);

        /// <summary>
        /// Signs in the user asynchronous.
        /// </summary>
        /// <param name="signInModel">The sign in model.</param>
        /// <returns>Returns authentication response model <see cref="AuthenticationResponseModel" />.</returns>
        /// <exception cref="Exceptions.InvalidAuthenticationException">When the user credentials are not valid.</exception>
        Task<AuthenticationResponseModel> SignInAsync(AuthenticationSignInModel signInModel);
    }
}