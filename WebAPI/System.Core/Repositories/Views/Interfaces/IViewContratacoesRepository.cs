using Niten.Core.Entities.Views;

namespace Niten.System.Core.Repositories.Views.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="ViewContratacoes"/>.
    /// </summary>
    public interface IViewContratacoesRepository
    {
        /// <summary>
        /// Obtêm as contratacoes pela view e ID do cadastro.
        /// </summary>
        /// <param name="cadastroID">O ID do cadastro.</param>
        /// <returns>Query com as contratações do cadastro.</returns>
        IQueryable<ViewContratacoes> ObterContratacoesPorCadastroID(int cadastroID);

        /// <summary>
        /// Obtêm todas as contratações pela view.
        /// </summary>
        /// <returns>Query com as contratações.</returns>
        IQueryable<ViewContratacoes> ObterTodasContratacoes();
    }
}