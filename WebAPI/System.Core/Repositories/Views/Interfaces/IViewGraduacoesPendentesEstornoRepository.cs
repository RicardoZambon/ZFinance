using Niten.Core.Entities.Views;

namespace Niten.System.Core.Repositories.Views.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="Niten.Core.Entities.Views.ViewGraduacoesPendentesEstorno"/>.
    /// </summary>
    public interface IViewGraduacoesPendentesEstornoRepository
    {
        /// <summary>
        /// Obtêm graduações pendentes de estorno.
        /// </summary>
        /// <returns>Query com as graduações pendentes de estorno.</returns>
        IQueryable<ViewGraduacoesPendentesEstorno> ObterGraduacoesPendentesDeEstorno();
    }
}