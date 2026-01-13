using Microsoft.EntityFrameworkCore;
using Niten.Core.Entities.Financeiro;
using Niten.Core.Entities.Geral;
using Niten.Core.Entities.Integracoes;
using Niten.Core.Repositories.Integracoes;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.Integracoes.Interfaces;
using ZDatabase.Exceptions;
using ZDatabase.Interfaces;

namespace Niten.System.Core.Repositories.Integracoes
{
    /// <inheritdoc />
    public class PerfisRepository : PerfisRepositoryCore, IPerfisRepository
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PerfisRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext" /> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler" /> instance.</param>
        public PerfisRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
            : base(dbContext, exceptionHandler)
        {
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task DesativarPerfilAsync(long perfilID)
        {
            try
            {
                if (await EncontrarPerfilPorIDAsync(perfilID) is not Perfis perfil)
                {
                    throw new EntityNotFoundException<Perfis>(perfilID);
                }
                else if (await dbContext.Set<PagamentosOnline>().AnyAsync(x => x.PerfilID == perfilID))
                {
                    throw new InvalidOperationException("Perfis-Desativar-Failure-EmUsoPagamentosOnline");
                }
                else if (await dbContext.Set<Unidades>().AnyAsync(x => x.PerfilPadraoID == perfilID))
                {
                    throw new InvalidOperationException("Perfis-Desativar-Failure-EmUsoUnidades");
                }

                perfil.Desativado = true;
                dbContext.Set<Perfis>().Update(perfil);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao desativar o perfil.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(perfilID), perfilID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task ReativarPerfilAsync(long perfilID)
        {
            try
            {
                if (await EncontrarPerfilPorIDAsync(perfilID) is not Perfis perfil)
                {
                    throw new EntityNotFoundException<Perfis>(perfilID);
                }

                perfil.Desativado = false;
                dbContext.Set<Perfis>().Update(perfil);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao reativar o perfil.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(perfilID), perfilID },
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