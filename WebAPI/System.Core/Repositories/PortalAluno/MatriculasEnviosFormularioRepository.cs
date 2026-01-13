using Niten.Core.Entities.PortalAluno;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.PortalAluno.Interfaces;
using ZDatabase.Interfaces;

namespace Niten.System.Core.Repositories.PortalAluno
{
    /// <inheritdoc />
    public class MatriculasEnviosFormularioRepository : IMatriculasEnviosFormularioRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MatriculasEnviosFormularioRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        public MatriculasEnviosFormularioRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public IQueryable<MatriculasEnviosFormulario> ObterTodosEnviosFormularioPorMatricula(long matriculaID)
        {
            try
            {
                return from mev in dbContext.Set<MatriculasEnviosFormulario>()
                       where mev.MatriculaID == matriculaID
                       select mev;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter todos os envios do formulário pelo ID da matrícula.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(matriculaID), matriculaID },
                    }
                );
                throw;
            }
        }
        #endregion

        #region Private methods
        #endregion
    }
}
