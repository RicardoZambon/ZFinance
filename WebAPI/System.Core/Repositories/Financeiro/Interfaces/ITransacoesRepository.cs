using Niten.Core.Entities.Configs;
using Niten.Core.Entities.Financeiro;

namespace Niten.System.Core.Repositories.Financeiro.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="Transacoes"/>.
    /// </summary>
    public interface ITransacoesRepository
    {
        /// <summary>
        /// Encontra uma transação pelo ID de forma assíncrona.
        /// </summary>
        /// <param name="transacaoID">O ID da transação.</param>
        /// <returns>A transação, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<Transacoes?> EncontrarTransacaoPorIDAsync(int transacaoID);

        /// <summary>
        /// Obtêm as transações pendentes de gerar comissão.
        /// </summary>
        /// <param name="definicaoComissao">Definição da comissão.</param>
        /// <returns>Query com as transações pendentes de gerar comissão.</returns>
        IQueryable<Transacoes> ObterTransacoesPendentesDeGerarComissao(DefinicaoComissoes definicaoComissao);
    }
}