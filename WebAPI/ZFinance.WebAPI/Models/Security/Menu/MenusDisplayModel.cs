namespace ZFinance.WebAPI.Models.Security.Menu
{
    /// <summary>
    /// Display model for <see cref="Core.Entities.Security.Menus"/>.
    /// </summary>
    /// <seealso cref="MenusUpdateModel" />
    public class MenusDisplayModel : MenusUpdateModel
    {
        /// <summary>
        /// Gets or sets the name of the application.
        /// </summary>
        /// <value>
        /// The name of the application.
        /// </value>
        public string? ApplicationName { get; set; }

        /// <summary>
        /// Gets or sets the parent menu label.
        /// </summary>
        /// <value>
        /// The parent menu label.
        /// </value>
        public string? ParentMenuLabel { get; set; }
    }
}