using Niten.Core.Entities.Views;
using Niten.Core.Repositories.Views;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.Views.Interfaces;
using ZDatabase.Interfaces;

namespace Niten.System.Core.Repositories.Views
{
    /// <inheritdoc />
    public class ViewCobrancasRepository : ViewCobrancasRepositoryCore, IViewCobrancasRepository
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewCobrancasRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="ZDatabase.Interfaces.IDbContext" /> instance.</param>
        /// <param name="exceptionHandler">The <see cref="Niten.Core.Services.Interfaces.IExceptionHandler" /> instance.</param>
        public ViewCobrancasRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
            : base(dbContext, exceptionHandler)
        {
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task<ViewCobrancas?> EncontrarCobrancaPorIDAsync(long cobrancaID)
        {
            try
            {
                return await dbContext.FindAsync<ViewCobrancas>(cobrancaID);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao encontrar na view de cobranças pelo ID.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(cobrancaID), cobrancaID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<ViewCobrancas> ObterCobrancasParaAtualizarTransacao()
        {
            try
            {
                return from vc in dbContext.Set<ViewCobrancas>()
                       where vc.TransacaoID != null
                             && vc.Cobranca != null
                             && vc.Cobranca.TransacaoID == null
                       select vc;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter as cobranças para atualizar a transação.");
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<ViewCobrancas> ObterCobrancasPorCadastroID(int cadastroID)
        {
            try
            {
                return from vc in dbContext.Set<ViewCobrancas>()
                       where vc.CadastroID == cadastroID
                       select vc;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter as cobranças pelo ID do cadastro.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(cadastroID), cadastroID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<ViewCobrancas> ObterCobrancasPorPagamentoOnlineIDCadastroID(long pagamentoOnlineID, int cadastroID)
        {
            try
            {
                return from vc in dbContext.Set<ViewCobrancas>()
                       where vc.PagamentoOnlineID == pagamentoOnlineID
                             && vc.CadastroID == cadastroID
                       select vc;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter as cobranças pelo ID do pagamento online e ID do cadastro.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(pagamentoOnlineID), pagamentoOnlineID },
                        { nameof(cadastroID), cadastroID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<ViewCobrancas> ObterTodasCobrancas()
        {
            try
            {
                return from c in dbContext.Set<ViewCobrancas>()
                       select c;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter pela view todas as cobranças.");
                throw;
            }
        }
        #endregion

        #region Private methods
        #endregion
    }
}