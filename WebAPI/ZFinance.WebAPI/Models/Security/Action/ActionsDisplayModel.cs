namespace ZFinance.WebAPI.Models.Security.Action
{
    /// <summary>
    /// Display model for <see cref="Core.Entities.Security.Actions"/>.
    /// </summary>
    /// <seealso cref="ActionsUpdateModel" />
    public class ActionsDisplayModel : ActionsUpdateModel
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