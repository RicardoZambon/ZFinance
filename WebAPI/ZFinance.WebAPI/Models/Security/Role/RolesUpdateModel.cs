namespace ZFinance.WebAPI.Models.Security.Role
{
    /// <summary>
    /// Update model for <see cref="Core.Entities.Security.Roles"/>.
    /// </summary>
    /// <seealso cref="RolesBaseModel" />
    public class RolesUpdateModel : RolesBaseModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public long ID { get; set; }
    }
}