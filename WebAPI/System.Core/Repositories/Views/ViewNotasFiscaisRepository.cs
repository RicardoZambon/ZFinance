using Niten.Core.Entities.Views;
using Niten.Core.Enums;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.Views.Interfaces;
using ZDatabase.Interfaces;

namespace Niten.System.Core.Repositories.Views
{
    /// <inheritdoc />
    public class ViewNotasFiscaisRepository : IViewNotasFiscaisRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewNotasFiscaisRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        public ViewNotasFiscaisRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public IQueryable<ViewNotasFiscais> ObterNotasFiscaisStatusAutorizadas()
        {
            try
            {
                return from vnf in dbContext.Set<ViewNotasFiscais>()
                       where vnf.Status == StatusNotaFiscal.Autorizado
                       orderby vnf.ID descending
                       select vnf;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao buscar por notas fiscais com status \"Processando\".");
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<ViewNotasFiscais> ObterNotasFiscaisStatusProcessando()
        {
            try
            {
                return from vnf in dbContext.Set<ViewNotasFiscais>()
                       where vnf.Status != StatusNotaFiscal.Autorizado
                             && vnf.Status != StatusNotaFiscal.Cancelado
                       orderby vnf.ID descending
                       select vnf;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao buscar por notas fiscais com status \"Processando\".");
                throw;
            }
        }
        #endregion

        #region Private methods
        #endregion
    }
}