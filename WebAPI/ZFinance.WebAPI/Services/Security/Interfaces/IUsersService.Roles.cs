using ZFinance.WebAPI.Models;
using ZFinance.WebAPI.Models.Security.User;
using ZWebAPI.Interfaces;

namespace ZFinance.WebAPI.Services.Security.Interfaces
{
    public partial interface IUsersService
    {
        /// <summary>
        /// Lists the roles assigned to the user asynchronously.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="parameters">The parameters for the list. <see cref="IListParameters"/>.</param>
        /// <returns>List with the roles assigned to the user accordingly to the parameters.</returns>
        Task<IQueryable<UsersRolesListModel>> ListUserRolesAsync(long userID, IListParameters parameters);

        /// <summary>
        /// Updates the relationship between user and roles asynchronous.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="relationshipUpdateModel">The relationship update model.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">When the entity was not found.</exception>
        Task UpdateRelationshipUserRolesAsync(long userID, RelationshipUpdateModel<long> relationshipUpdateModel);
    }
}