namespace ZFinance.WebAPI.Models.Security.Role
{
    /// <summary>
    /// List model for relationship between <see cref="Core.Entities.Security.Roles"/> and <see cref="Core.Entities.Security.Users"/>.
    /// </summary>
    public class RolesUsersListModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public long ID { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string? Name { get; set; }
    }
}