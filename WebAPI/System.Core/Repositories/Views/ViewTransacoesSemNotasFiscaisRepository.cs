using Niten.Core.Entities.Views;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.Views.Interfaces;
using ZDatabase.Interfaces;

namespace Niten.System.Core.Repositories.Views
{
    /// <inheritdoc />
    public class ViewTransacoesSemNotasFiscaisRepository : IViewTransacoesSemNotasFiscaisRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewTransacoesSemNotasFiscaisRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        public ViewTransacoesSemNotasFiscaisRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public IQueryable<ViewTransacoesSemNotasFiscais> ObterTransacoesPendentesDeGerarNotasFiscais()
        {
            try
            {
                return from vtsnf in dbContext.Set<ViewTransacoesSemNotasFiscais>()
                       select vtsnf;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro ao obter transações que não possuem notas fiscais.");
                throw;
            }
        }
        #endregion

        #region Private methods
        #endregion
    }
}