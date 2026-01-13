namespace ZFinance.WebAPI.Models.Security.User
{
    /// <summary>
    /// Update model for <see cref="Core.Entities.Security.Users"/>.
    /// </summary>
    /// <seealso cref="UserBaseModel" />
    public class UsersUpdateModel : UserBaseModel
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