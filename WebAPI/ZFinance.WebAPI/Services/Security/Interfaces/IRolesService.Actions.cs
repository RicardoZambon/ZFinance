using ZFinance.WebAPI.Models;
using ZFinance.WebAPI.Models.Security.Role;
using ZWebAPI.Interfaces;

namespace ZFinance.WebAPI.Services.Security.Interfaces
{
    public partial interface IRolesService
    {
        /// <summary>
        /// Lists the actions assigned to the role asynchronous.
        /// </summary>
        /// <param name="roleID">The role identifier.</param>
        /// <param name="parameters">The parameters for the list. <see cref="ZWebAPI.Interfaces.IListParameters"/>.</param>
        /// <returns>List with the actions assigned to the role accordingly to the parameters.</returns>
        Task<IQueryable<RolesActionsListModel>> ListRoleActionsAsync(long roleID, IListParameters parameters);

        /// <summary>
        /// Updates the relationship between role and actions asynchronous.
        /// </summary>
        /// <param name="roleID">The role identifier.</param>
        /// <param name="relationshipUpdateModel">The relationship update model.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">When the entity was not found.</exception>
        Task UpdateRelationshipRoleActionsAsync(long roleID, RelationshipUpdateModel<long> relationshipUpdateModel);
    }
}