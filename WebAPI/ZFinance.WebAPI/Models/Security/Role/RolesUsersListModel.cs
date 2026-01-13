namespace ZFinance.WebAPI.Models.Security.Role
{
    /// <summary>
    /// List model for relationship between <see cref="Core.Entities.Security.Roles"/> and <see cref="Core.Entities.Security.Users"/>.
    /// </summary>
    public class RolesUsersListModel
    {
        /// <summary>
        /// Gets or sets the email of the user.
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
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string? Name { get; set; }
    }
}