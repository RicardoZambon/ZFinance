using Niten.Core.Entities.Relatorios;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.Relatorios.Interfaces;
using Niten.System.Core.Repositories.Views;
using ZDatabase.Interfaces;

namespace Niten.System.Core.Repositories.Relatorios
{
    /// <inheritdoc />
    public class PresencasSemMatriculasRepository : IPresencasSemMatriculasRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewCobrancasRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext" /> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler" /> instance.</param>
        public PresencasSemMatriculasRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public IQueryable<PresencasSemMatriculas> ObterRelatorioPresencasSemMatriculas()
        {
            try
            {
                return from vc in dbContext.Set<PresencasSemMatriculas>()
                       select vc;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter o relatório de presenças sem matrículas.");
                throw;
            }
        }
        #endregion

        #region Private methods
        #endregion
    }
}