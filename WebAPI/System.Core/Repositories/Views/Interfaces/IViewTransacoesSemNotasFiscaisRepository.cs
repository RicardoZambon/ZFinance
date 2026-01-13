using Niten.Core.Entities.Views;

namespace Niten.System.Core.Repositories.Views.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="ViewTransacoesSemNotasFiscais"/>.
    /// </summary>
    public interface IViewTransacoesSemNotasFiscaisRepository
    {
        /// <summary>
        /// Obtêm as transações pendentes de gerar notas fiscais.
        /// </summary>
        /// <returns>Query com as transações pendentes de gerar notas fiscais.</returns>
        IQueryable<ViewTransacoesSemNotasFiscais> ObterTransacoesPendentesDeGerarNotasFiscais();
    }
}