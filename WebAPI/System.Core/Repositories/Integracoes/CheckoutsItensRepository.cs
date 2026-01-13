using Niten.Core.Entities.Integracoes;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.Integracoes.Interfaces;
using ZDatabase.Interfaces;

namespace Niten.System.Core.Repositories.Integracoes
{
    /// <inheritdoc />
    public class CheckoutsItensRepository : ICheckoutsItensRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckoutsRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        public CheckoutsItensRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public IQueryable<CheckoutsItens> ObterTodosItensCheckout(long checkoutID)
        {
            try
            {
                return from ci in dbContext.Set<CheckoutsItens>()
                       where ci.CheckoutID == checkoutID
                       select ci;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter todos os itens do checkout.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(checkoutID), checkoutID },
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