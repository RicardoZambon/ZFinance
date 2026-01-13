using ZWebAPI.Services;

namespace ZFinance.WebAPI.Services
{
    /// <inheritdoc />
    /// <seealso cref="CurrentUserProviderAbstract{TUsersKey}" />
    public partial class CurrentUserProviderDefault : CurrentUserProviderAbstract<long>
    {
        #region Variables
        private readonly IHttpContextAccessor httpContextAccessor;
        #endregion

        #region Properties
        /// <inheritdoc />
        protected override long DefaultServiceUserID => 1;

        /// <inheritdoc />
        public override long? UserID
        {
            get
            {
                if (Convert.ToInt64(httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(a => a.Type == "uid")?.Value) is long userID && userID > 0)
                {
                    return userID;
                }
                return null;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentUserProviderDefault"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The <see cref="IHttpContextAccessor"/> instance.</param>
        public CurrentUserProviderDefault(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Public methods
        #endregion

        #region Private methods
        #endregion
    }
}