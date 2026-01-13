using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using ZDatabase.Interfaces;
using ZFinance.Core.Entities.Security;

namespace ZFinance.Core.Initializers
{
    /// <summary>
    /// Data initializer for version 1.0.0.
    /// </summary>
    /// <seealso cref="BaseInitializer" />
    [ExcludeFromCodeCoverage]
    public class V100Initializer : BaseInitializer
    {
        #region Variables
        private const string ADMINISTRATORS_ACTION_CODE = "AdministrativeMaster";
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="V100Initializer"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        public V100Initializer(IDbContext dbContext)
            : base(dbContext)
        {
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public override void Initialize()
        {
            // Security
            InsertActions();
            InsertParentMenus();
            InsertChildMenus();
            InsertRoles();
        }
        #endregion

        #region Private methods
        private void InsertActions()
        {
            IList<Actions> actions = [
                new() { ID = 0, Name = "Administrative", Description = "Administrative action to be used only by administrators.", Code = ADMINISTRATORS_ACTION_CODE },

                // Actions
                new() { ID = 0, Entity = "Actions", Name = "AuditActionsOperationsHistoryAsync", Description = "Lists the action audit operations history asynchronous.", Code = "IActionsService.AuditActionsOperationsHistoryAsync" },
                new() { ID = 0, Entity = "Actions", Name = "AuditActionsServicesHistoryAsync", Description = "Lists the action audit services history asynchronous.", Code = "IActionsService.AuditActionsServicesHistoryAsync" },
                new() { ID = 0, Entity = "Actions", Name = "DeleteActionAsync", Description = "Deletes the action asynchronous.", Code = "IActionsService.DeleteActionAsync" },
                new() { ID = 0, Entity = "Actions", Name = "FindActionByIDAsync", Description = "Finds the action by identifier asynchronous.", Code = "IActionsService.FindActionByIDAsync" },
                new() { ID = 0, Entity = "Actions", Name = "InsertNewActionAsync", Description = "Inserts a new action asynchronous.", Code = "IActionsService.InsertNewActionAsync" },
                new() { ID = 0, Entity = "Actions", Name = "ListActionsAsync", Description = "Lists the actions asynchronous.", Code = "IActionsService.ListActionsAsync" },
                new() { ID = 0, Entity = "Actions", Name = "UpdateActionAsync", Description = "Updates the action asynchronous.", Code = "IActionsService.UpdateActionAsync" },

                // Menus
                new() { ID = 0, Entity = "Menus", Name = "AuditMenusOperationsHistoryAsync", Description = "Lists the menu audit operations history asynchronous.", Code = "IMenusService.AuditMenusOperationsHistoryAsync" },
                new() { ID = 0, Entity = "Menus", Name = "AuditMenusServicesHistoryAsync", Description = "Lists the menu audit services history asynchronous.", Code = "IMenusService.AuditMenusServicesHistoryAsync" },
                new() { ID = 0, Entity = "Menus", Name = "CatalogMenusAsync", Description = "Catalogs the menus asynchronous.", Code = "IMenusService.CatalogMenusAsync" },
                new() { ID = 0, Entity = "Menus", Name = "ListMenusForDrawerAsync", Description = "Lists the menus for the drawer asynchronous.", Code = "IMenusService.ListMenusForDrawerAsync" },
                new() { ID = 0, Entity = "Menus", Name = "DeleteMenuAsync", Description = "Deletes the menu asynchronous.", Code = "IMenusService.DeleteMenuAsync" },
                new() { ID = 0, Entity = "Menus", Name = "FindMenuByIDAsync", Description = "Finds the menu by identifier asynchronous.", Code = "IMenusService.FindMenuByIDAsync" },
                new() { ID = 0, Entity = "Menus", Name = "InsertNewMenuAsync", Description = "Inserts a new menu asynchronous.", Code = "IMenusService.InsertNewMenuAsync" },
                new() { ID = 0, Entity = "Menus", Name = "ListMenusAsync", Description = "Lists the menus asynchronous.", Code = "IMenusService.ListMenusAsync" },
                new() { ID = 0, Entity = "Menus", Name = "UpdateMenuAsync", Description = "Updates the menu asynchronous.", Code = "IMenusService.UpdateMenuAsync" },

                // Roles
                new() { ID = 0, Entity = "Roles", Name = "ListRoleActionsAsync", Description = "Lists the actions assigned to the role asynchronous.", Code = "IRolesService.ListRoleActionsAsync" },
                new() { ID = 0, Entity = "Roles", Name = "UpdateRelationshipRoleActionsAsync", Description = "Updates the relationship between role and actions asynchronous.", Code = "IRolesService.UpdateRelationshipRoleActionsAsync" },
                new() { ID = 0, Entity = "Roles", Name = "AuditRoleOperationsHistoryAsync", Description = "Lists the role audit operations history asynchronous.", Code = "IRolesService.AuditRoleOperationsHistoryAsync" },
                new() { ID = 0, Entity = "Roles", Name = "AuditRoleServicesHistoryAsync", Description = "Lists the role audit services history asynchronous.", Code = "IRolesService.AuditRoleServicesHistoryAsync" },
                new() { ID = 0, Entity = "Roles", Name = "CatalogRolesAsync", Description = "Catalogs the roles asynchronous.", Code = "IRolesService.CatalogRolesAsync" },
                new() { ID = 0, Entity = "Roles", Name = "ListRoleMenusAsync", Description = "Lists the menus assigned to the role asynchronous.", Code = "IRolesService.ListRoleMenusAsync" },
                new() { ID = 0, Entity = "Roles", Name = "UpdateRelationshipRoleMenusAsync", Description = "Updates the relationship between role and menus asynchronous.", Code = "IRolesService.UpdateRelationshipRoleMenusAsync" },
                new() { ID = 0, Entity = "Roles", Name = "ListRoleUsersAsync", Description = "Lists the users assigned to the role asynchronous.", Code = "IRolesService.ListRoleUsersAsync" },
                new() { ID = 0, Entity = "Roles", Name = "UpdateRelationshipRoleUsersAsync", Description = "Updates the relationship between role and users asynchronous.", Code = "IRolesService.UpdateRelationshipRoleUsersAsync" },
                new() { ID = 0, Entity = "Roles", Name = "DeleteRoleAsync", Description = "Deletes the role asynchronous.", Code = "IRolesService.DeleteRoleAsync" },
                new() { ID = 0, Entity = "Roles", Name = "FindRoleByIDAsync", Description = "Finds the role by identifier asynchronous.", Code = "IRolesService.FindRoleByIDAsync" },
                new() { ID = 0, Entity = "Roles", Name = "InsertNewRoleAsync", Description = "Inserts a new role asynchronous.", Code = "IRolesService.InsertNewRoleAsync" },
                new() { ID = 0, Entity = "Roles", Name = "ListRolesAsync", Description = "Lists the roles asynchronous.", Code = "IRolesService.ListRolesAsync" },
                new() { ID = 0, Entity = "Roles", Name = "UpdateRoleAsync", Description = "Updates the role asynchronous.", Code = "IRolesService.UpdateRoleAsync" },
            ];

            if (actions.Any())
            {
                SaveContext(actions, nameof(InsertActions), x => x.Code);
            }
        }

        private void InsertChildMenus()
        {
            IList<Menus> menus = [];

            if (dbContext.Set<Menus>().FirstOrDefault(x => x.Label == "Menu-Security") is Menus securityMenu)
            {
                menus.Add(new() { ID = 0, ParentMenuID = securityMenu.ID, Label = "Menu-Security-Actions", URL = "/security/actions", Order = 1 });
                menus.Add(new() { ID = 0, ParentMenuID = securityMenu.ID, Label = "Menu-Security-Menus", URL = "/security/menus", Order = 2 });
                menus.Add(new() { ID = 0, ParentMenuID = securityMenu.ID, Label = "Menu-Security-Roles", URL = "/security/roles", Order = 3 });
                menus.Add(new() { ID = 0, ParentMenuID = securityMenu.ID, Label = "Menu-Security-Users", URL = "/security/users", Order = 4 });
            }

            if (menus.Any())
            {
                SaveContext(menus, nameof(InsertChildMenus), EntityState.Added);
            }
        }

        private void InsertParentMenus()
        {
            IList<Menus> menus = [
                new() { ID = 0, Label = "Menu-Dashboards", Icon = "fa-chart-line", Order = 1 },
                new() { ID = 0, Label = "Menu-Transactions", Icon = "fa-dollar-sign", Order = 2 },
                new() { ID = 0, Label = "Menu-Configurations", Icon = "fa-gear", Order = 3 },
                new() { ID = 0, Label = "Menu-Security", Icon = "fa-shield-halved", Order = 4 },
            ];

            if (menus.Any())
            {
                SaveContext(menus, nameof(InsertParentMenus), x => x.Label);
            }
        }

        private void InsertRoles()
        {
            IList<Roles> roles = [
                new() {
                    ID = 0,
                    Name = "Administrators",
                    Actions = [.. dbContext.Set<Actions>().Where(x => x.Code == ADMINISTRATORS_ACTION_CODE)],
                },
            ];

            if (roles.Any())
            {
                SaveContext(roles, nameof(InsertRoles), x => x.Name);
            }
        }
        #endregion
    }
}
