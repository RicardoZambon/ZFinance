using Niten.Core.Entities.Views;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.Views.Interfaces;
using ZDatabase.Interfaces;

namespace Niten.System.Core.Repositories.Views
{
    /// <inheritdoc />
    public class ViewMatriculasRepository : IViewMatriculasRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewMatriculasRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext" /> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler" /> instance.</param>
        public ViewMatriculasRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public IQueryable<ViewMatriculas> ObterTodasMatriculas()
        {
            try
            {
                return from vm in dbContext.Set<ViewMatriculas>()
                       select vm;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter todas as matrículas pela view.");
                throw;
            }
        }
        #endregion

        #region Private methods
        #endregion
    }
}