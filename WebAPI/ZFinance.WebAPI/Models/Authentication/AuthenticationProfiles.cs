using AutoMapper;
using ZFinance.Core.Entities.Security;

namespace ZFinance.WebAPI.Models.Authentication
{
    /// <summary>
    /// Mapping configuration between entities and models for <see cref="Authentication"/> area.
    /// </summary>
    /// <seealso cref="Profile" />
    public class AuthenticationProfiles : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationProfiles"/> class.
        /// </summary>
        public AuthenticationProfiles()
        {
            #region Users
            CreateMap<Users, AuthenticationResponseModel>();
            #endregion
        }
    }
}