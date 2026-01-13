using Niten.Core.Entities.Integracoes;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.Integracoes.Interfaces;
using ZDatabase.Interfaces;

namespace Niten.System.Core.Repositories.Integracoes
{
    /// <inheritdoc />
    public class CheckoutsPagamentosRepository : ICheckoutsPagamentosRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckoutsPagamentosRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        public CheckoutsPagamentosRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public IQueryable<CheckoutsPagamentos> ObterTodosPagamentosCheckout(long checkoutID)
        {
            try
            {
                return from cp in dbContext.Set<CheckoutsPagamentos>()
                       where cp.CheckoutID == checkoutID
                       select cp;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter todos os pagamentos do checkout.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(checkoutID), checkoutID },
                    });
                throw;
            }
        }
        #endregion

        #region Private methods
        #endregion
    }
}