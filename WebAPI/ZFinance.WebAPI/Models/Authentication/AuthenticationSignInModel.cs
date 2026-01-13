namespace ZFinance.WebAPI.Models.Authentication
{
    /// <summary>
    /// Model for authenticate and sign in <see cref="Core.Entities.Security.Users"/>.
    /// </summary>
    public class AuthenticationSignInModel
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string? Password { get; set; }
    }
}