using Niten.Core.Entities.Integracoes;

namespace Niten.System.Core.Repositories.Integracoes.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="Niten.Core.Entities.Integracoes.CheckoutsPagamentos"/>.
    /// </summary>
    public interface ICheckoutsPagamentosRepository
    {
        /// <summary>
        /// Obtêm todos os pagamentos do checkout.
        /// </summary>
        /// <returns>Query com os pagamentos do checkout.</returns>
        IQueryable<CheckoutsPagamentos> ObterTodosPagamentosCheckout(long checkoutID);
    }
}