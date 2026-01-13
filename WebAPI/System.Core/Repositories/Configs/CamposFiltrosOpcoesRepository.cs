using Microsoft.EntityFrameworkCore;
using Niten.Core.Entities.Configs;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.Configs.Interfaces;
using ZDatabase.Exceptions;
using ZDatabase.Interfaces;
using ZDatabase.Validations;

namespace Niten.System.Core.Repositories.Configs
{
    /// <inheritdoc />
    public class CamposFiltrosOpcoesRepository : ICamposFiltrosOpcoesRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CamposFiltrosOpcoesRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        public CamposFiltrosOpcoesRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task AtualizarOpcaoAsync(CamposFiltrosOpcoes opcao)
        {
            try
            {
                await ValidarAsync(opcao);
                dbContext.Set<CamposFiltrosOpcoes>().Update(opcao);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao atualizar a opção do campo de filtro.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(opcao), opcao },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<CamposFiltrosOpcoes?> EncontrarOpcaoPorIDAsync(long opcaoID)
        {
            try
            {
                return await dbContext.FindAsync<CamposFiltrosOpcoes>(opcaoID);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao encontrar a opção pelo ID.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(opcaoID), opcaoID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task ExcluirOpcaoAsync(long opcaoID)
        {
            try
            {
                if (await EncontrarOpcaoPorIDAsync(opcaoID) is not CamposFiltrosOpcoes opcao)
                {
                    throw new EntityNotFoundException<CamposFiltrosOpcoes>(opcaoID);
                }
                opcao.IsDeleted = true;
                dbContext.Set<CamposFiltrosOpcoes>().Update(opcao);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao excluir a opção do campo de filtro.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(opcaoID), opcaoID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task InserirNovaOpcaoAsync(CamposFiltrosOpcoes opcao)
        {
            try
            {
                await ValidarAsync(opcao);
                await dbContext.Set<CamposFiltrosOpcoes>().AddAsync(opcao);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao inserir nova opção do campo de filtro.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(opcao), opcao },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<CamposFiltrosOpcoes> ObterTodasOpcoes(long campoFiltroID)
        {
            try
            {
                return from cfo in dbContext.Set<CamposFiltrosOpcoes>()
                       where cfo.CampoFiltroID == campoFiltroID
                       select cfo;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter todas as opções de campos de filtro.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(campoFiltroID), campoFiltroID },
                    }
                );
                throw;
            }
        }
        #endregion

        #region Private methods
        private async Task ValidarAsync(CamposFiltrosOpcoes opcao)
        {
            ValidationResult result = new();

            // Valor
            if (string.IsNullOrWhiteSpace(opcao.Valor))
            {
                result.SetError(nameof(CamposFiltrosOpcoes.Valor), "required");
            }
            else if (await dbContext.Set<CamposFiltrosOpcoes>().AnyAsync(x => EF.Functions.Like(x.Valor!, opcao.Valor) && x.CampoFiltroID == opcao.CampoFiltroID && x.ID != opcao.ID))
            {
                result.SetError(nameof(CamposFiltrosOpcoes.Valor), "exists");
            }

            result.ValidateEntityErrors(opcao);
        }
        #endregion
    }
}