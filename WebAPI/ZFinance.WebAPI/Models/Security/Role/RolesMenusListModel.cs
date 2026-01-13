namespace ZFinance.WebAPI.Models.Security.Role
{
    /// <summary>
    /// List model for relationship between <see cref="Core.Entities.Security.Roles"/> and <see cref="Core.Entities.Security.Menus"/>.
    /// </summary>
    public class RolesMenusListModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public long ID { get; set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        public string? Label { get; set; }
    }
}