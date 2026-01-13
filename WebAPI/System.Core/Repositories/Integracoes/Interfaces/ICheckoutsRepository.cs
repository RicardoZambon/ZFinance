using Niten.Core.Entities.Integracoes;
using Niten.Core.Repositories.Integracoes.Interfaces;

namespace Niten.System.Core.Repositories.Integracoes.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="Niten.Core.Entities.Integracoes.Checkouts"/>.
    /// </summary>
    /// <seealso cref="ICheckoutsRepositoryCore" />
    public interface ICheckoutsRepository : ICheckoutsRepositoryCore
    {
        /// <summary>
        /// Obtém todos os checkouts filtrados por CadastroID e MaterialID.
        /// </summary>
        /// <param name="cadastroID">O ID do cadatro.</param>
        /// <param name="materialID">O ID do material</param>
        /// <returns>Query com os checkouts de acordo com os parâmetros.</returns>
        IQueryable<Checkouts> ObterCheckoutsPorCadastroIDMaterialID(int cadastroID, int materialID);

        /// <summary>
        /// Obtêm todos os checkouts.
        /// </summary>
        /// <returns>Query com os checkouts.</returns>
        IQueryable<Checkouts> ObterTodosCheckouts();
    }
}