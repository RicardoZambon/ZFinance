namespace ZFinance.WebAPI.Models.Security.Role
{
    /// <summary>
    /// Insert model for <see cref="Core.Entities.Security.Roles"/>.
    /// </summary>
    /// <seealso cref="RolesBaseModel" />
    public class RolesInsertModel : RolesBaseModel
    {
        /// <summary>
        /// Gets or sets the application identifier.
        /// </summary>
        /// <value>
        /// The application identifier.
        /// </value>
        public long ApplicationID { get; set; }
    }
}