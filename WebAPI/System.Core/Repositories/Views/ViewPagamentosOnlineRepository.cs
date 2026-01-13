using Niten.Core.Entities.Views;
using Niten.Core.Repositories.Views;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.Views.Interfaces;
using ZDatabase.Interfaces;

namespace Niten.System.Core.Repositories.Views
{
    /// <inheritdoc />
    public class ViewPagamentosOnlineRepository : ViewPagamentosOnlineRepositoryCore, IViewPagamentosOnlineRepository
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewPagamentosOnlineRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        public ViewPagamentosOnlineRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
            : base(dbContext, exceptionHandler)
        {
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public IQueryable<ViewPagamentosOnline> ObterPagamentosOnlinePorCadastroID(int cadastroID)
        {
            try
            {
                return from vp in dbContext.Set<ViewPagamentosOnline>()
                       where vp.CadastroID == cadastroID
                       select vp;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter os pagamentos online pelo ID do cadastro.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(cadastroID), cadastroID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<ViewPagamentosOnline> ObterPagamentosOnlinePorMesAno(DateTime mesAno)
        {
            try
            {
                int mesAnoKey = ObterMesAnoKey(mesAno);

                return from vpo in dbContext.Set<ViewPagamentosOnline>()
                       where vpo.MesAnoKey == mesAnoKey
                       select vpo;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro ao obter pagamentos online pelo mês/ano.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(mesAno), mesAno },
                    }
                );
                throw;
            }
        }
        #endregion

        #region Private methods
        private int ObterMesAnoKey(DateTime competencia)
        {
            return Convert.ToInt32($"{competencia.Year}{competencia.Month:00}");
        }
        #endregion
    }
}