using Niten.Core.Entities.Views;

namespace Niten.System.Core.Repositories.Views.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="ViewMatriculas"/>.
    /// </summary>
    public interface IViewMatriculasRepository
    {
        /// <summary>
        /// Obtêm todas as matrículas pela view.
        /// </summary>
        /// <returns>Query com as matrículas.</returns>
        IQueryable<ViewMatriculas> ObterTodasMatriculas();
    }
}