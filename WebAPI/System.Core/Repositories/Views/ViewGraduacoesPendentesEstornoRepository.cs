using Niten.Core.Entities.Views;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.Views.Interfaces;
using ZDatabase.Interfaces;

namespace Niten.System.Core.Repositories.Views
{
    /// <inheritdoc />
    public class ViewGraduacoesPendentesEstornoRepository : IViewGraduacoesPendentesEstornoRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewGraduacoesPendentesEstornoRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="ZDatabase.Interfaces.IDbContext" /> instance.</param>
        /// <param name="exceptionHandler">The <see cref="Niten.Core.Services.Interfaces.IExceptionHandler" /> instance.</param>
        public ViewGraduacoesPendentesEstornoRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public IQueryable<ViewGraduacoesPendentesEstorno> ObterGraduacoesPendentesDeEstorno()
        {
            try
            {
                return from vgpgc in dbContext.Set<ViewGraduacoesPendentesEstorno>()
                       select vgpgc;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter as graduações pendentes de estorno.");
                throw;
            }
        }
        #endregion

        #region Private methods
        #endregion
    }
}