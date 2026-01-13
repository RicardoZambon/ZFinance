using Niten.Core.Entities.PortalAluno;

namespace Niten.System.Core.Repositories.PortalAluno.Interfaces
{
    /// <summary>
    /// Repositório para gerenciamento dos grupos.
    /// </summary>
    public interface IGruposRepository
    {
        /// <summary>
        /// Atualiza o grupo de forma assíncrona.
        /// </summary>
        /// <param name="grupo">O grupo.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task AtualizarGrupoAsync(Grupos grupo);

        /// <summary>
        /// Encontra o grupo pelo ID de forma assíncrona.
        /// </summary>
        /// <param name="grupoID">O ID do grupo.</param>
        /// <returns>O grupo, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<Grupos?> EncontrarGrupoPorIDAsync(long grupoID);

        /// <summary>
        /// Exclui o grupo de forma assíncrona.
        /// </summary>
        /// <param name="grupoID">O ID do grupo.</param>
        Task ExcluirGrupoAsync(long grupoID);

        /// <summary>
        /// Insere um novo grupo de forma assíncrona.
        /// </summary>
        /// <param name="grupo">O grupo.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">Quando o ID informado for inválido.</exception>
        Task InserirNovoGrupoAsync(Grupos grupo);

        /// <summary>
        /// Obtêm todos os grupos.
        /// </summary>
        /// <returns>Query com os grupos.</returns>
        IQueryable<Grupos> ObterTodosGrupos();
    }
}