namespace ZFinance.WebAPI.Models.Security.Menu
{
    /// <summary>
    /// Insert model for <see cref="Core.Entities.Security.Menus"/>.
    /// </summary>
    /// <seealso cref="MenusBaseModel" />
    public class MenusInsertModel : MenusBaseModel
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