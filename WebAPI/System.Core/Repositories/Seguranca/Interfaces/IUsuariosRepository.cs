using Niten.Core.Entities.Seguranca;
using ZSecurity.Repositories.Interfaces;

namespace Niten.System.Core.Repositories.Seguranca.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="Niten.Core.Entities.Seguranca.Usuarios"/>.
    /// </summary>
    public interface IUsuariosRepository : IBaseUsersRepository<Actions, int>
    {
        /// <summary>
        /// Atualiza o usuário de forma assíncrona.
        /// </summary>
        /// <param name="usuario">O usuário.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task AtualizarUsuarioAsync(Usuarios usuario);

        /// <summary>
        /// Encontra o usuário pelo ID de forma assíncrona.
        /// </summary>
        /// <param name="usuarioID">O ID do usuário.</param>
        /// <returns>O usuário, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<Usuarios?> EncontrarUsuarioPorIDAsync(uint usuarioID);

        /// <summary>
        /// Encontra o usuário pelo nome de usuário de forma assíncrona.
        /// </summary>
        /// <param name="usuario">O nome de usuário.</param>
        /// <returns>O usuário, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<Usuarios?> EncontrarUsuarioPorUsuarioAsync(string usuario);

        /// <summary>
        /// Exclui o usuário de forma assíncrona.
        /// </summary>
        /// <param name="usuarioID">O ID do usuário.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">Quando o ID informado for inválido.</exception>
        Task ExcluirUsuarioAsync(uint usuarioID);

        /// <summary>
        /// Insere um novo usuário de forma assíncrona.
        /// </summary>
        /// <param name="usuario">O usuário.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task InserirNovoUsuarioAsync(Usuarios usuario);

        /// <summary>
        /// Obtêm todos os usuários.
        /// </summary>
        /// <returns>Query com os usuários.</returns>
        IQueryable<Usuarios> ObterTodosUsuarios();
    }
}