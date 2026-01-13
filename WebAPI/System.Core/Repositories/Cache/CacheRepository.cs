using Microsoft.EntityFrameworkCore;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.Cache.Interfaces;
using ZDatabase.Interfaces;

namespace Niten.System.Core.Repositories.Cache
{
    /// <inheritdoc />
    public class CacheRepository : ICacheRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        public CacheRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public Methods
        /// <inheritdoc />
        public async Task AtualizarCachePagamentosAsync(int? cadastroID = null, long? pagamentoOnlineID = null)
        {
            try
            {
                await dbContext.Database.ExecuteSqlAsync($"CALL sp_Atualiza_Caches_Pagamentos({cadastroID}, {pagamentoOnlineID});");
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao atualizar o cache dos pagamentos.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(cadastroID), cadastroID },
                        { nameof(pagamentoOnlineID), pagamentoOnlineID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task AtualizarCacheTabelasValoresAsync()
        {
            try
            {
                await dbContext.Database.ExecuteSqlAsync($"CALL sp_Atualiza_Caches_TabelasValores();");
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao atualizar o cache das tabelas de valores.");
                throw;
            }
        }
        #endregion

        #region Private Methods
        #endregion
    }
}
