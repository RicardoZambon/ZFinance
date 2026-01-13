using ZFinance.WebAPI.Models.Security.Role;
using ZWebAPI.Interfaces;

namespace ZFinance.WebAPI.Services.Security.Interfaces
{
    /// <summary>
    /// Service for <see cref="Core.Entities.Security.Roles"/>.
    /// </summary>
    public partial interface IRolesService
    {
        /// <summary>
        /// Deletes the role asynchronous.
        /// </summary>
        /// <param name="roleID">The role identifier.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">When the entity was not found.</exception>
        Task DeleteRoleAsync(long roleID);

        /// <summary>
        /// Finds the role by identifier asynchronous.
        /// </summary>
        /// <param name="roleID">The role identifier.</param>
        /// <returns>The display model from the role.</returns>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">When the entity was not found.</exception>
        Task<RolesDisplayModel> FindRoleByIDAsync(long roleID);

        /// <summary>
        /// Inserts a new role asynchronous.
        /// </summary>
        /// <param name="roleModel">The role model.</param>
        /// <returns>The display model from the role.</returns>
        /// <exception cref="ArgumentNullException">When the parameters are <c>null</c>.</exception>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">When the entity validation fails. Check validation result for the validation details.</exception>
        Task<RolesDisplayModel> InsertNewRoleAsync(RolesInsertModel roleModel);

        /// <summary>
        /// Lists the roles asynchronous.
        /// </summary>
        /// <param name="parameters">The parameters for the list. <see cref="ZWebAPI.Interfaces.IListParameters"/>.</param>
        /// <returns>List with the roles accordingly to the parameters.</returns>
        /// <exception cref="ArgumentNullException">When the parameters are <c>null</c>.</exception>
        Task<IQueryable<RolesListModel>> ListRolesAsync(IListParameters parameters);

        /// <summary>
        /// Updates the role asynchronous.
        /// </summary>
        /// <param name="roleModel">The role model.</param>
        /// <returns>The display model from the role.</returns>
        /// <exception cref="ArgumentNullException">When the parameters are <c>null</c>.</exception>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">When the entity was not found.</exception>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">When the entity validation fails. Check validation result for the validation details.</exception>
        Task<RolesDisplayModel> UpdateRoleAsync(RolesUpdateModel roleModel);
    }
}