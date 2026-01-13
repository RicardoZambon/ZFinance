namespace ZFinance.WebAPI.Models.Security.Action
{
    /// <summary>
    /// Insert model for <see cref="Core.Entities.Security.Actions"/>.
    /// </summary>
    /// <seealso cref="ActionsBaseModel" />
    public class ActionsInsertModel : ActionsBaseModel
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