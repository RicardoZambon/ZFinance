using Niten.Core.Entities.Financeiro;

namespace Niten.System.Core.Repositories.Financeiro.Interfaces
{
    public interface IComissoesTransacoesRepository
    {
        /// <summary>
        /// Encontra a comissão X transação pelo ID de forma assíncrona.
        /// </summary>
        /// <param name="comissaoTransacaoID">O ID da comissão X transação.</param>
        /// <returns>A comissão X transação, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<ComissoesTransacoes?> EncontrarComissaoTransacaoPorIDAsync(long comissaoTransacaoID);

        /// <summary>
        /// Exclui a comissão X transação de forma assíncrona.
        /// </summary>
        /// <param name="comissaoTransacaoID">O ID da comissão X transação.</param>
        Task ExcluirComissaoTransacaoAsync(long comissaoTransacaoID);

        /// <summary>
        /// Insere uma nova comissão X transação de forma assíncrona.
        /// </summary>
        /// <param name="comissaoTransacao">A comissão X transação.</param>

        Task InserirNovaComissaoTransacaoAsync(ComissoesTransacoes comissaoTransacao);

        /// <summary>
        /// Obtêm as comissões x transações pelo ID da comissão.
        /// </summary>
        /// <param name="comissaoID">O ID da comissão.</param>
        /// <returns>Query com as comissões x transações.</returns>
        IQueryable<ComissoesTransacoes> ObterComissoesTransacaoPorComissaoID(long comissaoID);

        /// <summary>
        /// Verificas se a comissão X transação existe.
        /// </summary>
        /// <param name="transacaoID">O ID da transação.</param>
        /// <param name="definicaoComissaoID">O ID da definicão de comissão.</param>
        /// <param name="mesReferencia">O mês de referência.</param>
        /// <param name="unidadeID">O ID da unidade.</param>
        /// <param name="cadastroID">O ID do cadastro.</param>
        /// <returns></returns>
        Task<bool> VerificaComissoesTransacoesExiste(int transacaoID, long definicaoComissaoID, DateTime mesReferencia, int unidadeID, int? cadastroID);
    }
}