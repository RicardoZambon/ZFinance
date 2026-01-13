using ZFinance.Core.Entities.Audit;
using ZFinance.Core.Entities.Security;
using ZDatabase.Interfaces;
using ZDatabase.Services;
using ZDatabase.Services.Interfaces;

namespace ZFinance.Core.Services
{
    /// <inheritdoc />
    public class CoreAuditHandler : AuditHandler<ServicesHistory, OperationsHistory, Users, long>, IAuditHandler
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CoreAuditHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext" /> instance.</param>
        public CoreAuditHandler(IDbContext dbContext)
            : base(dbContext)
        {

        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public override OperationsHistory InstantiateOperationsHistory() => new();
        #endregion

        #region Private methods
        #endregion
    }
}