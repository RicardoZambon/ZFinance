namespace ZFinance.WebAPI.Models.Security.User
{
    /// <summary>
    /// Display model for <see cref="Core.Entities.Security.Users"/>.
    /// </summary>
    /// <seealso cref="UsersUpdateModel" />
    public class UsersDisplayModel : UsersUpdateModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Core.Entities.Security.Users"/> is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive { get; set; }
    }
}