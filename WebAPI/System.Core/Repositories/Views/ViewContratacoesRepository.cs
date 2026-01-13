using Niten.Core.Entities.Views;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.Views.Interfaces;
using ZDatabase.Interfaces;

namespace Niten.System.Core.Repositories.Views
{
    /// <inheritdoc />
    public class ViewContratacoesRepository : IViewContratacoesRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewContratacoesRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="ZDatabase.Interfaces.IDbContext" /> instance.</param>
        /// <param name="exceptionHandler">The <see cref="Niten.Core.Services.Interfaces.IExceptionHandler" /> instance.</param>
        public ViewContratacoesRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public IQueryable<ViewContratacoes> ObterContratacoesPorCadastroID(int cadastroID)
        {
            try
            {
                return from c in dbContext.Set<ViewContratacoes>()
                       where c.CadastroID == cadastroID
                       select c;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter todas as contratações pela view e ID do cadastro.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(cadastroID), cadastroID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<ViewContratacoes> ObterTodasContratacoes()
        {
            try
            {
                return from vc in dbContext.Set<ViewContratacoes>()
                       select vc;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter todas as contratações pela view.");
                throw;
            }
        }
        #endregion

        #region Private methods
        #endregion
    }
}