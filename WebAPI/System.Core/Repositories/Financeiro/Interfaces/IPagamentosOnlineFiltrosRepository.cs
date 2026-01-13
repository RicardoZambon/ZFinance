using Niten.Core.Entities.Financeiro;

namespace Niten.System.Core.Repositories.Financeiro.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="PagamentosOnlineFiltros"/>.
    /// </summary>
    public interface IPagamentosOnlineFiltrosRepository
    {
        /// <summary>
        /// Atualiza o filtro do pagamento online de forma assíncrona.
        /// </summary>
        /// <param name="pagamentoOnlineFiltro">O filtro do pagamento online.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task AtualizarPagamentoOnlineFiltroAsync(PagamentosOnlineFiltros pagamentoOnlineFiltro);

        /// <summary>
        /// Encontra o filtro do pagamento online pelo ID de forma assíncrona.
        /// </summary>
        /// <param name="pagamentoOnlineFiltroID">O ID do filtro do pagamento online.</param>
        /// <returns>O filtro do pagamento online, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<PagamentosOnlineFiltros?> EncontrarPagamentoOnlineFiltroPorIDAsync(long pagamentoOnlineFiltroID);

        /// <summary>
        /// Exclui o filtro do pagamento online de forma assíncrona.
        /// </summary>
        /// <param name="pagamentoOnlineFiltroID">O ID do filtro do pagamento online.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">Quando o ID informado for inválido.</exception>
        Task ExcluirPagamentoOnlineFiltroAsync(long pagamentoOnlineFiltroID);

        /// <summary>
        /// Insere um novo filtro do pagamento online de forma assíncrona.
        /// </summary>
        /// <param name="pagamentoOnlineFiltro">O filtro do pagamento online.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task InserirNovoPagamentoOnlineFiltroAsync(PagamentosOnlineFiltros pagamentoOnlineFiltro);

        /// <summary>
        /// Obtêm todos os filtros do pagamento online.
        /// </summary>
        /// <param name="pagamentoOnlineID">O ID do pagamento online.</param>
        /// <returns>Query com os filtros do pagamento online.</returns>
        IQueryable<PagamentosOnlineFiltros> ObterTodosPagamentoOnlineFiltros(long pagamentoOnlineID);
    }
}