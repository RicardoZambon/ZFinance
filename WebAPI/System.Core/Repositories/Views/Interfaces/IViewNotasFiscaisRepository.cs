using Niten.Core.Entities.Views;

namespace Niten.System.Core.Repositories.Views.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="ViewNotasFiscais"/>.
    /// </summary>
    public interface IViewNotasFiscaisRepository
    {
        /// <summary>
        /// Obtêm todas as notas fiscais com status "Autorizadas".
        /// </summary>
        /// <returns>Query com todas as notas fiscais com status "Autorizadas".</returns>
        IQueryable<ViewNotasFiscais> ObterNotasFiscaisStatusAutorizadas();

        /// <summary>
        /// Obtêm todas as notas fiscais com status "Processando".
        /// </summary>
        /// <returns>Query com todas as notas fiscais com status "Processando".</returns>
        IQueryable<ViewNotasFiscais> ObterNotasFiscaisStatusProcessando();
    }
}