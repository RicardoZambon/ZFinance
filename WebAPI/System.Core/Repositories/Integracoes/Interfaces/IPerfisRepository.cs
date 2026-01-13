using Niten.Core.Entities.Integracoes;

namespace Niten.System.Core.Repositories.Integracoes.Interfaces
{
    /// <summary>
    /// Repositório para <see cref="Perfis"/>.
    /// </summary>
    public interface IPerfisRepository
    {
        /// <summary>
        /// Desativa o perfil de forma assíncrona.
        /// </summary>
        /// <param name="perfilID">O ID do perfil.</param>
        Task DesativarPerfilAsync(long perfilID);

        /// <summary>
        /// Reativa o perfil de forma assíncrona.
        /// </summary>
        /// <param name="perfilID">O ID do perfil.</param>
        Task ReativarPerfilAsync(long perfilID);

        /// <summary>
        /// Encontra o perfil pelo ID de forma assíncrona.
        /// </summary>
        /// <param name="perfilID">O ID do perfil.</param>
        /// <returns>O perfil, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<Perfis?> EncontrarPerfilPorIDAsync(long perfilID);

        /// <summary>
        /// Obtêm todos os perfis.
        /// </summary>
        /// <returns>Query com todos os perfis.</returns>
        IQueryable<Perfis> ObterTodosPerfis();
    }
}