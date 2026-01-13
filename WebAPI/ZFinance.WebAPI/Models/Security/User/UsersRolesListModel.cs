namespace ZFinance.WebAPI.Models.Security.User
{
    /// <summary>
    /// List model for relationship between <see cref="Core.Entities.Security.Users"/> and <see cref="Core.Entities.Security.Roles"/>.
    /// </summary>
    public class UsersRolesListModel
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