using Niten.Core.Repositories.Historico;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.Historico.Interfaces;
using ZDatabase.Interfaces;

namespace Niten.System.Core.Repositories.Historico
{
    /// <inheritdoc />
    public class CadastrosHistoricoRepository : CadastrosHistoricoRepositoryCore, ICadastrosHistoricoRepository
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <inheritdoc />
        public CadastrosHistoricoRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
            : base(dbContext, exceptionHandler)
        {

        }
        #endregion

        #region Public methods
        #endregion

        #region Private methods
        #endregion
    }
}