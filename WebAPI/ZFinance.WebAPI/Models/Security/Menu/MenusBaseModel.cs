namespace ZFinance.WebAPI.Models.Security.Menu
{
    /// <summary>
    /// Abstract model for <see cref="Core.Entities.Security.Menus"/>.
    /// </summary>
    public abstract class MenusBaseModel
    {
        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        /// <value>
        /// The icon.
        /// </value>
        public string? Icon { get; set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        public string? Label { get; set; }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the parent menu identifier.
        /// </summary>
        /// <value>
        /// The parent menu identifier.
        /// </value>
        public long? ParentMenuID { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string? URL { get; set; }
    }
}