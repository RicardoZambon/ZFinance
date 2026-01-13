using Niten.Core.Entities.Views;

namespace Niten.System.Core.Repositories.Views.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="ViewGraduacoesPendentesGerarCobrancas"/>.
    /// </summary>
    public interface IViewGraduacoesPendentesGerarCobrancasRepository
    {
        /// <summary>
        /// Obtêm graduações pendentes de gerar cobranças.
        /// </summary>
        /// <returns>Query com as graduações pendentes de gerar cobranças.</returns>
        IQueryable<ViewGraduacoesPendentesGerarCobrancas> ObterGraduacoesPendentesDeGerarCobrancas();
    }
}