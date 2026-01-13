using Niten.Core.Entities.Integracoes;
using Niten.Core.Repositories.Integracoes;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.Integracoes.Interfaces;
using ZDatabase.Interfaces;

namespace Niten.System.Core.Repositories.Integracoes
{
    /// <inheritdoc />
    public class CheckoutsRepository : CheckoutsRepositoryCore, ICheckoutsRepository
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckoutsRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        public CheckoutsRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
            : base(dbContext, exceptionHandler)
        {
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public IQueryable<Checkouts> ObterCheckoutsPorCadastroIDMaterialID(int cadastroID, int materialID)
        {
            try
            {
                return from c in ObterTodosCheckouts()
                       join ci in dbContext.Set<CheckoutsItens>() on c.ID equals ci.CheckoutID
                       where c.CadastroID == cadastroID
                             && ci.MaterialID == materialID
                       select c;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter os checkouts por ID do cadastro e ID do material.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(cadastroID), cadastroID },
                        { nameof(materialID), materialID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<Checkouts> ObterTodosCheckouts()
        {
            try
            {
                return from c in dbContext.Set<Checkouts>()
                       select c;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter todos os checkouts.");
                throw;
            }
        }
        #endregion

        #region Private methods
        #endregion
    }
}