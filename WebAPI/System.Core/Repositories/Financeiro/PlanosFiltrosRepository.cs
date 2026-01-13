using Microsoft.EntityFrameworkCore;
using Niten.Core.Entities.Financeiro;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.Financeiro.Interfaces;
using ZDatabase.Exceptions;
using ZDatabase.Interfaces;
using ZDatabase.Validations;

namespace Niten.System.Core.Repositories.Financeiro
{
    /// <inheritdoc />
    public class PlanosFiltrosRepository : IPlanosFiltrosRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PlanosFiltrosRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        public PlanosFiltrosRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task AtualizarPlanoFiltroAsync(PlanosFiltros planoFiltro)
        {
            try
            {
                await ValidarAsync(planoFiltro);

                dbContext.Set<PlanosFiltros>().Update(planoFiltro);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao atualizar o filtro do plano.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(planoFiltro), planoFiltro },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<PlanosFiltros?> EncontrarPlanoFiltroPorIDAsync(long planoFiltroID)
        {
            try
            {
                return await dbContext.FindAsync<PlanosFiltros>(planoFiltroID);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao encontrar o filtro do plano pelo ID.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(planoFiltroID), planoFiltroID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task ExcluirPlanoFiltroAsync(long planoFiltroID)
        {
            try
            {
                if (await EncontrarPlanoFiltroPorIDAsync(planoFiltroID) is not PlanosFiltros planoFiltro)
                {
                    throw new EntityNotFoundException<PlanosFiltros>(planoFiltroID);
                }

                planoFiltro.IsDeleted = true;
                dbContext.Set<PlanosFiltros>().Update(planoFiltro);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao excluir o filtro do plano.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(planoFiltroID), planoFiltroID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task InserirNovoPlanoFiltroAsync(PlanosFiltros planoFiltro)
        {
            try
            {
                await ValidarAsync(planoFiltro);

                await dbContext.Set<PlanosFiltros>().AddAsync(planoFiltro);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao inserir novo filtro no plano.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(planoFiltro), planoFiltro },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<PlanosFiltros> ObterTodosPlanoFiltros(long planoID)

        {
            try
            {
                return from pf in dbContext.Set<PlanosFiltros>()
                       where pf.PlanoID == planoID
                       select pf;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter todos os filtros do plano pelo ID do plano.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(planoID), planoID },
                    }
                );
                throw;
            }
        }
        #endregion

        #region Private methods
        private async Task ValidarAsync(PlanosFiltros planoFiltro)
        {
            ValidationResult result = new();

            // DataTermino
            if (planoFiltro.DataTermino is DateTime dataTermino && planoFiltro.DataInicio is DateTime dataInicio && dataTermino < dataInicio)
            {
                result.SetError(nameof(PlanosFiltros.DataTermino), "invalid");
            }

            // Nome
            if (string.IsNullOrWhiteSpace(planoFiltro.Nome))
            {
                result.SetError(nameof(PlanosFiltros.Nome), "required");
            }
            else if (await dbContext.Set<PlanosFiltros>().AnyAsync(x => x.PlanoID == planoFiltro.PlanoID && EF.Functions.Like(x.Nome!, planoFiltro.Nome) && x.ID != planoFiltro.ID))
            {
                result.SetError(nameof(PlanosFiltros.Nome), "exists");
            }

            // PlanoID
            if (await dbContext.FindAsync<Planos>(planoFiltro.PlanoID) is null)
            {
                result.SetError(nameof(PlanosFiltros.PlanoID), "required");
            }

            result.ValidateEntityErrors(planoFiltro);
        }
        #endregion
    }
}