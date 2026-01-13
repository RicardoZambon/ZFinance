using ZFinance.WebAPI.Models;
using ZFinance.WebAPI.Models.Security.Role;
using ZWebAPI.Interfaces;

namespace ZFinance.WebAPI.Services.Security.Interfaces
{
    public partial interface IRolesService
    {
        /// <summary>
        /// Lists the menus assigned to the role asynchronous.
        /// </summary>
        /// <param name="roleID">The role identifier.</param>
        /// <param name="parameters">The parameters for the list. <see cref="ZWebAPI.Interfaces.IListParameters"/>.</param>
        /// <returns>List with the menus assigned to the role accordingly to the parameters.</returns>
        Task<IQueryable<RolesMenusListModel>> ListRoleMenusAsync(long roleID, IListParameters parameters);

        /// <summary>
        /// Updates the relationship between role and menus asynchronous.
        /// </summary>
        /// <param name="roleID">The role identifier.</param>
        /// <param name="relationshipUpdateModel">The relationship update model.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">When the entity was not found.</exception>
        Task UpdateRelationshipRoleMenusAsync(long roleID, RelationshipUpdateModel<long> relationshipUpdateModel);
    }
}