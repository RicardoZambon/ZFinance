using Niten.Core.Entities.Financeiro;
using Niten.Core.Entities.Geral;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.Geral.Interfaces;
using ZDatabase.Exceptions;
using ZDatabase.Interfaces;
using ZDatabase.Validations;

namespace Niten.System.Core.Repositories.Geral
{
    /// <inheritdoc />
    public class UnidadesTabelasValoresRepository : IUnidadesTabelasValoresRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="UnidadesTabelasValoresRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        public UnidadesTabelasValoresRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task AtualizarUnidadeTabelaValoresAsync(UnidadesTabelasValores unidadeTabelaValores)
        {
            try
            {
                await ValidarAsync(unidadeTabelaValores);
                DefineCompetenciaParaPrimeiroDia(unidadeTabelaValores);
                dbContext.Set<UnidadesTabelasValores>().Update(unidadeTabelaValores);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao atualizar o relacionamento da unidade com a tabela de valores.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(unidadeTabelaValores), unidadeTabelaValores },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<UnidadesTabelasValores?> EncontrarUnidadeTabelaValoresPorIDAsync(long unidadeTabelaValoresID)
        {
            try
            {
                return await dbContext.FindAsync<UnidadesTabelasValores>(unidadeTabelaValoresID);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao encontrar o relacionamento da unidade com a tabela de valores pelo ID.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(unidadeTabelaValoresID), unidadeTabelaValoresID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task ExcluirUnidadeTabelaValoresAsync(long unidadeTabelaValoresID)
        {
            try
            {
                if (await EncontrarUnidadeTabelaValoresPorIDAsync(unidadeTabelaValoresID) is not UnidadesTabelasValores unidadeTabelaValores)
                {
                    throw new EntityNotFoundException<UnidadesTabelasValores>(unidadeTabelaValoresID);
                }

                unidadeTabelaValores.IsDeleted = true;
                dbContext.Set<UnidadesTabelasValores>().Update(unidadeTabelaValores);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao excluir o relacionamento da unidade com a tabela de valores.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(unidadeTabelaValoresID), unidadeTabelaValoresID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task InserirNovaUnidadeTabelaValoresAsync(UnidadesTabelasValores unidadeTabelaValores)
        {
            try
            {
                await ValidarAsync(unidadeTabelaValores);
                DefineCompetenciaParaPrimeiroDia(unidadeTabelaValores);
                await dbContext.Set<UnidadesTabelasValores>().AddAsync(unidadeTabelaValores);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao inserir novo relacionamento da unidade com a tabela de valores.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(unidadeTabelaValores), unidadeTabelaValores },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<UnidadesTabelasValores> ObterTodasUnidadesTabelasValores(int unidadeID)
        {
            try
            {
                return from utv in dbContext.Set<UnidadesTabelasValores>()
                       where utv.UnidadeID == unidadeID
                       select utv;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter todos os relacionamento da unidade com a tabela de valores.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(unidadeID), unidadeID },
                    }
                );
                throw;
            }
        }
        #endregion

        #region Private methods
        private void DefineCompetenciaParaPrimeiroDia(UnidadesTabelasValores unidadeTabelaValores)
        {
            // A competência deve sempre ser o 1º dia do mês.
            if (unidadeTabelaValores.Competencia is DateTime competencia && competencia.Day != 1)
            {
                unidadeTabelaValores.Competencia = competencia.AddDays((competencia.Day - 1) * -1);
            }
        }

        private async Task ValidarAsync(UnidadesTabelasValores unidadeTabelaValores)
        {
            ValidationResult result = new();

            IEnumerable<UnidadesTabelasValores> tabelasValores = dbContext.Set<UnidadesTabelasValores>().Where(x =>
                x.ID != unidadeTabelaValores.ID
                && x.UnidadeID == unidadeTabelaValores.UnidadeID);

            // Competencia
            if (unidadeTabelaValores.Competencia is null && tabelasValores.Any())
            {
                result.SetError(nameof(UnidadesTabelasValores.Competencia), "required");
            }
            else if (unidadeTabelaValores.Competencia is DateTime competencia)
            {
                if (competencia.Day != 1)
                {
                    competencia = competencia.AddDays((competencia.Day - 1) * -1);
                }

                if (tabelasValores.Any(x => x.Competencia == competencia))
                {
                    result.SetError(nameof(UnidadesTabelasValores.Competencia), "exists");
                }
            }

            // TabelaValorID
            if (await dbContext.FindAsync<TabelasValores>(unidadeTabelaValores.TabelaValoresID) is null)
            {
                result.SetError(nameof(UnidadesTabelasValores.TabelaValores), "required");
            }

            // UnidadeID
            if (await dbContext.FindAsync<Unidades>(unidadeTabelaValores.UnidadeID) is null)
            {
                result.SetError(nameof(UnidadesTabelasValores.UnidadeID), "required");
            }

            result.ValidateEntityErrors(unidadeTabelaValores);
        }
        #endregion
    }
}