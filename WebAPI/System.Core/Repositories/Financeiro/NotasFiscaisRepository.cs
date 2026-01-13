using Niten.Core.Entities.Financeiro;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.Financeiro.Interfaces;
using ZDatabase.Interfaces;

namespace Niten.System.Core.Repositories.Financeiro
{
    /// <inheritdoc />
    public class NotasFiscaisRepository : INotasFiscaisRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="NotasFiscaisRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        public NotasFiscaisRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task<NotasFiscais?> EncontrarNotaFiscalPorIDAsync(int notaFiscalID)
        {
            try
            {
                return await dbContext.FindAsync<NotasFiscais>(notaFiscalID);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro ao obter nota fiscal pelo ID.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(notaFiscalID), notaFiscalID },
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