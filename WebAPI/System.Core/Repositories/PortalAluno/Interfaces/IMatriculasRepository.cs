using Niten.Core.Entities.PortalAluno;

namespace Niten.System.Core.Repositories.PortalAluno.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="Matriculas"/>.
    /// </summary>
    public interface IMatriculasRepository
    {
        /// <summary>
        /// Encontra a matrícula pelo ID de forma assíncrona.
        /// </summary>
        /// <param name="matriculaID">O ID da matrícula.</param>
        /// <returns>A matrícula, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<Matriculas?> EncontrarMatriculaPorIDAsync(long matriculaID);

        /// <summary>
        /// Obtêm todas as matrículas.
        /// </summary>
        /// <returns>Query com as matrículas.</returns>
        IQueryable<Matriculas> ObterTodasMatriculas();
    }
}