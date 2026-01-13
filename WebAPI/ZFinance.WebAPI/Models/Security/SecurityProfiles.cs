using AutoMapper;
using ZFinance.Core.Entities.Security;
using ZFinance.WebAPI.Models.Security.Action;
using ZFinance.WebAPI.Models.Security.Menu;
using ZFinance.WebAPI.Models.Security.Role;
using ZFinance.WebAPI.Models.Security.User;

namespace ZFinance.WebAPI.Models.Security
{
    /// <summary>
    /// Mapping configuration between entities and models for <see cref="Security"/> area.
    /// </summary>
    /// <seealso cref="Profile" />
    public class SecurityProfiles : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityProfiles"/> class.
        /// </summary>
        public SecurityProfiles()
        {
            #region Actions
            CreateMap<Actions, ActionsDisplayModel>();
            CreateMap<Actions, ActionsListModel>();
            CreateMap<ActionsInsertModel, Actions>();
            CreateMap<ActionsUpdateModel, Actions>();
            #endregion

            #region Menus
            CreateMap<Menus, MenusDisplayModel>();
            CreateMap<Menus, MenusListForDrawerModel>()
                .ForMember(x => x.ChildCount, x => x.MapFrom(y => y.ChildMenus != null ? y.ChildMenus.Count : 0));
            CreateMap<Menus, MenusListModel>();
            CreateMap<MenusInsertModel, Menus>();
            CreateMap<MenusUpdateModel, Menus>();
            #endregion

            #region Roles
            CreateMap<Actions, RolesActionsListModel>();
            CreateMap<Menus, RolesMenusListModel>();
            CreateMap<Roles, RolesDisplayModel>();
            CreateMap<Roles, RolesListModel>();
            CreateMap<Roles, RolesListPermissionsModel>();
            CreateMap<RolesInsertModel, Roles>();
            CreateMap<RolesUpdateModel, Roles>();
            CreateMap<Users, RolesUsersListModel>();
            #endregion

            #region Users
            CreateMap<Roles, UsersRolesListModel>();
            CreateMap<Users, UsersDisplayModel>();
            CreateMap<Users, UsersListModel>();
            CreateMap<UsersInsertModel, Users>();
            CreateMap<UsersUpdateModel, Users>();
            #endregion
        }
    }
}