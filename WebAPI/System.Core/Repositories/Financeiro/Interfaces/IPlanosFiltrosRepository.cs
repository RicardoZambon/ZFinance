using Niten.Core.Entities.Financeiro;

namespace Niten.System.Core.Repositories.Financeiro.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="PlanosFiltros"/>.
    /// </summary>
    public interface IPlanosFiltrosRepository
    {
        /// <summary>
        /// Atualiza o filtro do plano de forma assíncrona.
        /// </summary>
        /// <param name="planoFiltro">O filtro do plano.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task AtualizarPlanoFiltroAsync(PlanosFiltros planoFiltro);

        /// <summary>
        /// Encontra o filtro do plano pelo ID de forma assíncrona.
        /// </summary>
        /// <param name="planoFiltroID">O ID do filtro do plano.</param>
        /// <returns>O filtro do plano, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<PlanosFiltros?> EncontrarPlanoFiltroPorIDAsync(long planoFiltroID);

        /// <summary>
        /// Exclui o filtro do plano de forma assíncrona.
        /// </summary>
        /// <param name="planoFiltroID">O ID do filtro do plano.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">Quando o ID informado for inválido.</exception>
        Task ExcluirPlanoFiltroAsync(long planoFiltroID);

        /// <summary>
        /// Insere um novo filtro do plano de forma assíncrona.
        /// </summary>
        /// <param name="planoFiltro">O filtro do plano.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task InserirNovoPlanoFiltroAsync(PlanosFiltros planoFiltro);

        /// <summary>
        /// Obtêm todos os filtros do plano.
        /// </summary>
        /// <param name="planoID">O ID do plano.</param>
        /// <returns>Query com os filtros do plano.</returns>
        IQueryable<PlanosFiltros> ObterTodosPlanoFiltros(long planoID);
    }
}