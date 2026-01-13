namespace ZFinance.WebAPI.Models.Authentication
{
    /// <summary>
    /// Response model when authenticating <see cref="Core.Entities.Security.Users"/>.
    /// </summary>
    public class AuthenticationResponseModel
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the refresh token.
        /// </summary>
        /// <value>
        /// The refresh token.
        /// </value>
        public string? RefreshToken { get; set; }

        /// <summary>
        /// Gets or sets the refresh token expiration.
        /// </summary>
        /// <value>
        /// The refresh token expiration.
        /// </value>
        public DateTime RefreshTokenExpiration { get; set; }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        public string? Token { get; set; }
    }
}