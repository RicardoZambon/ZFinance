namespace ZFinance.WebAPI.Models.Authentication
{
    /// <summary>
    /// Refresh token model for <see cref="Core.Entities.Security.RefreshTokens"/>.
    /// </summary>
    public class AuthenticationRefreshTokenModel
    {
        /// <summary>
        /// Gets or sets the refresh token.
        /// </summary>
        /// <value>
        /// The refresh token.
        /// </value>
        public string? RefreshToken { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public string? Username { get; set; }
    }
}