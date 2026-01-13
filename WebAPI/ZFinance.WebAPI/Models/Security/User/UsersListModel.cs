namespace ZFinance.WebAPI.Models.Security.User
{
    /// <summary>
    /// List model for <see cref="Core.Entities.Security.Users"/>.
    /// </summary>
    public class UsersListModel
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public long ID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Core.Entities.Security.Users"/> is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string? Name { get; set; }
    }
}