using ZFinance.Core.Entities.Security;

namespace ZFinance.Core.Repositories.Security.Interfaces
{
    /// <summary>
    /// Repository for <see cref="Roles"/>.
    /// </summary>
    public interface IRolesRepository
    {
        /// <summary>
        /// Deletes the role asynchronous.
        /// </summary>
        /// <param name="roleID">The role identifier.</param>
        Task DeleteRoleAsync(long roleID);

        /// <summary>
        /// Finds the role by identifier asynchronous.
        /// </summary>
        /// <param name="roleID">The role identifier.</param>
        /// <returns>The role, if found; otherwise, <c>null</c>.</returns>
        Task<Roles?> FindRoleByIDAsync(long roleID);

        /// <summary>
        /// Inserts the role asynchronous.
        /// </summary>
        /// <param name="role">The role.</param>
        Task InsertRoleAsync(Roles role);

        /// <summary>
        /// Lists all role.
        /// </summary>
        /// <returns>Query with all role.</returns>
        IQueryable<Roles> ListRoles();

        /// <summary>
        /// Updates the role asynchronous.
        /// </summary>
        /// <param name="role">The role.</param>
        Task UpdateRoleAsync(Roles role);
    }
}