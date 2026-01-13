namespace ZFinance.WebAPI.Models.Security.Role
{
    /// <summary>
    /// Display model for <see cref="Core.Entities.Security.Roles"/>.
    /// </summary>
    /// <seealso cref="RolesUpdateModel" />
    public class RolesDisplayModel : RolesUpdateModel
    {
        /// <summary>
        /// Gets or sets the application identifier.
        /// </summary>
        /// <value>
        /// The application identifier.
        /// </value>
        public long ApplicationID { get; set; }

        /// <summary>
        /// Gets or sets the name of the application.
        /// </summary>
        /// <value>
        /// The name of the application.
        /// </value>
        public string? ApplicationName { get; set; }
    }
}