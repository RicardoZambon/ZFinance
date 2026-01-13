namespace ZFinance.WebAPI.Models.Security.Menu
{
    /// <summary>
    /// Update model for <see cref="Core.Entities.Security.Menus"/>.
    /// </summary>
    /// <seealso cref="MenusBaseModel" />
    public class MenusUpdateModel : MenusBaseModel
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