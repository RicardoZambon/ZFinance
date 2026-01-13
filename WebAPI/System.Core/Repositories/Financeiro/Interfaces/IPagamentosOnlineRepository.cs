using Niten.Core.Entities.Financeiro;

namespace Niten.System.Core.Repositories.Financeiro.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="PagamentosOnline"/>.
    /// </summary>
    public interface IPagamentosOnlineRepository
    {
        /// <summary>
        /// Atualiza o pagamento online de forma assíncrona.
        /// </summary>
        /// <param name="pagamentoOnline">O pagamento online.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task AtualizarPagamentoOnlineAsync(PagamentosOnline pagamentoOnline);

        /// <summary>
        /// Encontra o pagamento online pelo ID de forma assíncrona.
        /// </summary>
        /// <param name="pagamentoOnlineID">O ID do pagamento online.</param>
        /// <returns>O pagamento online, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<PagamentosOnline?> EncontrarPagamentoOnlinePorIDAsync(long pagamentoOnlineID);

        /// <summary>
        /// Exclui o pagamento online de forma assíncrona.
        /// </summary>
        /// <param name="pagamentoOnlineID">O ID do pagamento online.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">Quando o ID informado for inválido.</exception>
        Task ExcluirPagamentoOnlineAsync(long pagamentoOnlineID);

        /// <summary>
        /// Insere um novo pagamento online de forma assíncrona.
        /// </summary>
        /// <param name="pagamentoOnline">O pagamento online.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task InserirNovoPagamentoOnlineAsync(PagamentosOnline pagamentoOnline);

        /// <summary>
        /// Obtêm todos os pagamentos online.
        /// </summary>
        /// <returns>Query com os pagamentos online.</returns>
        IQueryable<PagamentosOnline> ObterTodosPagamentosOnline();
    }
}