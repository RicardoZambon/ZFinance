using ZFinance.WebAPI.Models;
using ZFinance.WebAPI.Models.Security.Role;
using ZWebAPI.Interfaces;

namespace ZFinance.WebAPI.Services.Security.Interfaces
{
    public partial interface IRolesService
    {
        /// <summary>
        /// Lists the users assigned to the role asynchronous.
        /// </summary>
        /// <param name="roleID">The role identifier.</param>
        /// <param name="parameters">The parameters for the list. <see cref="IListParameters"/>.</param>
        /// <returns>List with the users assigned to the role accordingly to the parameters.</returns>
        Task<IQueryable<RolesUsersListModel>> ListRoleUsersAsync(long roleID, IListParameters parameters);

        /// <summary>
        /// Updates the relationship between role and users asynchronous.
        /// </summary>
        /// <param name="roleID">The role identifier.</param>
        /// <param name="relationshipUpdateModel">The relationship update model.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">When the entity was not found.</exception>
        Task UpdateRelationshipRoleUsersAsync(long roleID, RelationshipUpdateModel<long> relationshipUpdateModel);
    }
}