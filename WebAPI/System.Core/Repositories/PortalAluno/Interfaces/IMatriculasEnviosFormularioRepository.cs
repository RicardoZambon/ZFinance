using Niten.Core.Entities.PortalAluno;

namespace Niten.System.Core.Repositories.PortalAluno.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="MatriculasEnviosFormulario"/>.
    /// </summary>
    public interface IMatriculasEnviosFormularioRepository
    {
        /// <summary>
        /// Obtêm todos os envios do formulário pelo ID da matrícula.
        /// </summary>
        /// <param name="matriculaID">O ID da matrícula.</param>
        /// <returns>Query com oa envios do formulário.</returns>
        IQueryable<MatriculasEnviosFormulario> ObterTodosEnviosFormularioPorMatricula(long matriculaID);
    }
}