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
    public class CamposFiltrosRepository : ICamposFiltrosRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CamposFiltrosRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        public CamposFiltrosRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task AtualizarCampoFiltroAsync(CamposFiltros campoFiltro)
        {
            try
            {
                await ValidarAsync(campoFiltro);
                dbContext.Set<CamposFiltros>().Update(campoFiltro);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao atualizar o campo de filtro.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(campoFiltro), campoFiltro },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<CamposFiltros?> EncontrarCampoFiltroPorIDAsync(long campoFiltroID)
        {
            try
            {
                return await dbContext.FindAsync<CamposFiltros>(campoFiltroID);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao encontrar o campo de filtro pelo ID.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(campoFiltroID), campoFiltroID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task ExcluirCampoFiltroAsync(long campoFiltroID)
        {
            try
            {
                if (await EncontrarCampoFiltroPorIDAsync(campoFiltroID) is not CamposFiltros campoFiltro)
                {
                    throw new EntityNotFoundException<CamposFiltros>(campoFiltroID);
                }
                campoFiltro.IsDeleted = true;
                dbContext.Set<CamposFiltros>().Update(campoFiltro);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao excluir o campo de filtro.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(campoFiltroID), campoFiltroID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task InserirNovoCampoFiltroAsync(CamposFiltros campoFiltro)
        {
            try
            {
                await ValidarAsync(campoFiltro);
                await dbContext.Set<CamposFiltros>().AddAsync(campoFiltro);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao inserir novo campo de filtro.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(campoFiltro), campoFiltro },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<CamposFiltros> ObterTodosCamposFiltros()
        {
            try
            {
                return from cf in dbContext.Set<CamposFiltros>()
                       select cf;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter todos os campos de filtro.");
                throw;
            }
        }
        #endregion

        #region Private methods
        private async Task ValidarAsync(CamposFiltros campoFiltro)
        {
            ValidationResult result = new();

            // Campo
            if (string.IsNullOrWhiteSpace(campoFiltro.Campo))
            {
                result.SetError(nameof(CamposFiltros.Campo), "required");
            }
            else if (await dbContext.Set<CamposFiltros>().AnyAsync(x => EF.Functions.Like(x.Campo!, campoFiltro.Campo) && x.ID != campoFiltro.ID))
            {
                result.SetError(nameof(CamposFiltros.Campo), "exists");
            }

            result.ValidateEntityErrors(campoFiltro);
        }
        #endregion
    }
}