using Niten.Core.Entities.Seguranca;

namespace Niten.System.Core.Repositories.Seguranca.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="Niten.Core.Entities.Seguranca.Roles"/>.
    /// </summary>
    public interface IRolesRepository
    {
        /// <summary>
        /// Atualiza a role de forma assíncrona.
        /// </summary>
        /// <param name="role">A role.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task AtualizarRoleAsync(Roles role);

        /// <summary>
        /// Encontra a role pelo ID de forma assíncrona.
        /// </summary>
        /// <param name="roleID">O ID da role.</param>
        /// <returns>A role, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<Roles?> EncontrarRolePorIDAsync(long roleID);

        /// <summary>
        /// Exclui a role de forma assíncrona.
        /// </summary>
        /// <param name="roleID">O ID da role.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">Quando o ID informado for inválido.</exception>
        Task ExcluirRoleAsync(long roleID);

        /// <summary>
        /// Insere uma nova role de forma assíncrona.
        /// </summary>
        /// <param name="role">A role.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task InserirNovaRoleAsync(Roles role);

        /// <summary>
        /// Obtêm todas as roles.
        /// </summary>
        /// <returns>Query com as roles.</returns>
        IQueryable<Roles> ObterTodasRoles();
    }
}