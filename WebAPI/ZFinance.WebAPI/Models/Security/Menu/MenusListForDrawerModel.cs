namespace ZFinance.WebAPI.Models.Security.Menu
{
    /// <summary>
    /// List for drawer model for <see cref="Core.Entities.Security.Menus"/>.
    /// </summary>
    public class MenusListForDrawerModel
    {
        /// <summary>
        /// Gets or sets the child count.
        /// </summary>
        /// <value>
        /// The child count.
        /// </value>
        public int ChildCount { get; set; }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        /// <value>
        /// The icon.
        /// </value>
        public string? Icon { get; set; }

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

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string? URL { get; set; }
    }
}