namespace ZFinance.WebAPI.Models.Security.Action
{
    /// <summary>
    /// Update model for <see cref="Core.Entities.Security.Actions"/>.
    /// </summary>
    /// <seealso cref="ActionsBaseModel" />
    public class ActionsUpdateModel : ActionsBaseModel
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