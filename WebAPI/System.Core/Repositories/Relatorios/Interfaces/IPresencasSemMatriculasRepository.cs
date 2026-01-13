using Niten.Core.Entities.Relatorios;

namespace Niten.System.Core.Repositories.Relatorios.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="PresencasSemMatriculas"/>.
    /// </summary>
    public interface IPresencasSemMatriculasRepository
    {
        /// <summary>
        /// Obtêm relatório de presenças sem matrículas.
        /// </summary>
        /// <returns>Query com o relatório de presenças sem matrículas.</returns>
        IQueryable<PresencasSemMatriculas> ObterRelatorioPresencasSemMatriculas();
    }
}