using Niten.Core.Entities.PortalAluno;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.PortalAluno.Interfaces;
using ZDatabase.Interfaces;

namespace Niten.System.Core.Repositories.PortalAluno
{
    /// <inheritdoc />
    public class MatriculasRepository : IMatriculasRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MatriculasRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        public MatriculasRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task<Matriculas?> EncontrarMatriculaPorIDAsync(long matriculaID)
        {
            try
            {
                return await dbContext.FindAsync<Matriculas>(matriculaID);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao encontrar a matrícula pelo ID.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(matriculaID), matriculaID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<Matriculas> ObterTodasMatriculas()
        {
            try
            {
                return from m in dbContext.Set<Matriculas>()
                       select m;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter todas as matrículas.");
                throw;
            }
        }
        #endregion

        #region Private methods
        #endregion
    }
}
