using AutoMapper;
using ZFinance.Core.Entities.Audit;
using ZWebAPI.Models.Audit.OperationHistory;
using ZWebAPI.Models.Audit.ServiceHistory;

namespace ZFinance.WebAPI.Models.Audit
{
    /// <summary>
    /// Set mapping classes for Audit.
    /// </summary>
    /// <seealso cref="AutoMapper.Profile" />
    public class AuditProfiles : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuditProfiles"/> class.
        /// </summary>
        public AuditProfiles()
        {
            #region OperationsHistory
            CreateMap<OperationsHistory, OperationsHistoryListModel>();
            #endregion

            #region ServicesHistory
            CreateMap<ServicesHistory, ServicesHistoryListModel>();
            #endregion
        }
    }
}