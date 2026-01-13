using Niten.Core.Entities.Views;
using Niten.Core.Repositories.Views.Interfaces;

namespace Niten.System.Core.Repositories.Views.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="Niten.Core.Entities.Views.ViewCobrancas"/>.
    /// </summary>
    public interface IViewCobrancasRepository : IViewCobrancasRepositoryCore
    {
        /// <summary>
        /// Encontra a cobrança pelo ID de forma assíncrona.
        /// </summary>
        /// <param name="cobrancaID">O ID da cobrança.</param>
        /// <returns>A cobrança, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<ViewCobrancas?> EncontrarCobrancaPorIDAsync(long cobrancaID);

        /// <summary>
        /// Obtêm cobranças pelo ID do cadastro.
        /// </summary>
        /// <param name="cadastroID">ID do cadastro.</param>
        /// <returns>Query com as cobranças.</returns>
        IQueryable<ViewCobrancas> ObterCobrancasPorCadastroID(int cadastroID);

        /// <summary>
        /// Obtêm cobranças pelo ID do pagamento online e ID do cadastro.
        /// </summary>
        /// <param name="pagamentoOnlineID">ID do pagamento online.</param>
        /// <param name="cadastroID">ID do cadastro.</param>
        /// <returns>Query com as cobranças.</returns>
        IQueryable<ViewCobrancas> ObterCobrancasPorPagamentoOnlineIDCadastroID(long pagamentoOnlineID, int cadastroID);

        /// <summary>
        /// Obtêm todas as cobranças.
        /// </summary>
        /// <returns>Query com as cobranças.</returns>
        IQueryable<ViewCobrancas> ObterTodasCobrancas();
    }
}