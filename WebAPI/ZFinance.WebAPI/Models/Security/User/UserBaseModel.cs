namespace ZFinance.WebAPI.Models.Security.User
{
    /// <summary>
    /// Abstract model for <see cref="Core.Entities.Security.Users"/>.
    /// </summary>
    public abstract class UserBaseModel
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string? Name { get; set; }
    }
}