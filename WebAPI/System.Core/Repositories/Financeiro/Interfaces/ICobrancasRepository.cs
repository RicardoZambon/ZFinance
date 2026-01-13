using Niten.Core.Entities.Financeiro;
using Niten.Core.Repositories.Financeiro.Interfaces;

namespace Niten.System.Core.Repositories.Financeiro.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref=Cobrancas"/>.
    /// </summary>
    /// <seealso cref="ICobrancasRepositoryCore" />
    public interface ICobrancasRepository : ICobrancasRepositoryCore
    {
        /// <summary>
        /// Atualiza a cobrança.
        /// </summary>
        /// <param name="cobranca">A cobrança.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        void AtualizarCobranca(Cobrancas cobranca);

        /// <summary>
        /// Cancela a cobrança pelo ID de forma assíncrona.
        /// </summary>
        /// <param name="cobranca">A cobrança.</param>
        /// <param name="observacoes">Texto com observações do estorno.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task CancelarCobrancaAsync(Cobrancas cobranca, string? observacoes);

        /// <summary>
        /// Encontra a cobrança pelo ID de forma assíncrona.
        /// </summary>
        /// <param name="cobrancaID">O ID da cobrança.</param>
        /// <returns>A cobrança, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<Cobrancas?> EncontrarCobrancaPorIDAsync(long cobrancaID);


        /// <summary>
        /// Exclui a cobrança de forma assíncrona.
        /// </summary>
        /// <param name="cobrancaID">O ID da cobrança.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">Quando o ID informado for inválido.</exception>
        Task ExcluirCobrancaAsync(long cobrancaID);

        /// <summary>
        /// Gera as cobranças para um determinado pagamento online e cadastro de forma assíncrona.
        /// </summary>
        /// <param name="pagamentoOnlineID">O ID do pagamento online.</param>
        /// <param name="cadastroID">O ID do cadastro.</param>
        Task GerarCobrancasPagamentoOnlineIDCadastroIDAsync(long pagamentoOnlineID, int cadastroID);

        /// <summary>
        /// Limpa as cobranças anteriores a competência para um determinado pagamento online e cadastro de forma assíncrona.
        /// </summary>
        /// <param name="pagamentoOnlineID">O ID do pagamento online.</param>
        /// <param name="cadastroID">O ID do cadastro.</param>
        /// <param name="competenciaInicial">A nova competência inicial.</param>
        Task LimparCobrancasPagamentoOnlineIDCadastroIDAsync(long pagamentoOnlineID, int cadastroID, DateTime competenciaInicial);

        /// <summary>
        /// Obtêm todas as cobranças.
        /// </summary>
        /// <returns>Query com as cobranças.</returns>
        IQueryable<Cobrancas> ObterTodasCobrancas();
    }
}