using Niten.Core.Entities.Integracoes;

namespace Niten.System.Core.Repositories.Integracoes.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="Niten.Core.Entities.Integracoes.CheckoutsItens"/>.
    /// </summary>
    public interface ICheckoutsItensRepository
    {
        /// <summary>
        /// Obtêm todos os itens do checkouts.
        /// </summary>
        /// <param name="checkoutID">ID do checkout.</param>
        /// <returns>Query com os itens do checkout.</returns>
        IQueryable<CheckoutsItens> ObterTodosItensCheckout(long checkoutID);
    }
}