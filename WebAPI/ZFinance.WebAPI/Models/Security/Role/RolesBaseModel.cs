namespace ZFinance.WebAPI.Models.Security.Role
{
    /// <summary>
    /// Abstract model for <see cref="Core.Entities.Security.Roles"/>.
    /// </summary>
    public abstract class RolesBaseModel
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string? Name { get; set; }
    }
}