using Niten.Core.Entities.Views;
using Niten.Core.Repositories.Views.Interfaces;

namespace Niten.System.Core.Repositories.Views.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="ViewPagamentosOnline"/>.
    /// </summary>
    public interface IViewPagamentosOnlineRepository : IViewPagamentosOnlineRepositoryCore
    {
        /// <summary>
        /// Obtêm todos os pagamentos online pelo ID do cadastro.
        /// </summary>
        /// <param name="cadastroID">O ID do cadastro.</param>
        /// <returns>Query com os pagamentos online pelo ID do cadastro.</returns>
        IQueryable<ViewPagamentosOnline> ObterPagamentosOnlinePorCadastroID(int cadastroID);

        /// <summary>
        /// Obtêm todos os pagamentos online pelo mês/ano.
        /// </summary>
        /// <param name="mesAno">O mês/ano.</param>
        /// <returns>Query com os pagamentos online pelo mês/ano.</returns>
        IQueryable<ViewPagamentosOnline> ObterPagamentosOnlinePorMesAno(DateTime mesAno);
    }
}