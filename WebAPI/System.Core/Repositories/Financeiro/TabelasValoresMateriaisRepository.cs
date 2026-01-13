using Niten.Core.Entities.Estoque;
using Niten.Core.Entities.Financeiro;
using Niten.Core.Entities.Geral;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.Financeiro.Interfaces;
using ZDatabase.Exceptions;
using ZDatabase.Interfaces;
using ZDatabase.Validations;

namespace Niten.System.Core.Repositories.Financeiro
{
    /// <inheritdoc />
    public class TabelasValoresMateriaisRepository : ITabelasValoresMateriaisRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="TabelasValoresMateriaisRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        public TabelasValoresMateriaisRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task AtualizarTabelaValoresMaterialAsync(TabelasValoresMateriais tabelaValoresMaterial)
        {
            try
            {
                await ValidarAsync(tabelaValoresMaterial);

                DefineCompetenciaParaPrimeiroDia(tabelaValoresMaterial);

                dbContext.Set<TabelasValoresMateriais>().Update(tabelaValoresMaterial);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao atualizar o material da tabela de valores.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(tabelaValoresMaterial), tabelaValoresMaterial },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<TabelasValoresMateriais?> EncontrarTabelaValoresMaterialPorIDAsync(long tabelaValoresMaterialID)
        {
            try
            {
                return await dbContext.FindAsync<TabelasValoresMateriais>(tabelaValoresMaterialID);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao encontrar o material da tabela de valores pelo ID.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(tabelaValoresMaterialID), tabelaValoresMaterialID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task ExcluirTabelaValoresMaterialAsync(long tabelaValoresMaterialID)
        {
            try
            {
                if (await EncontrarTabelaValoresMaterialPorIDAsync(tabelaValoresMaterialID) is not TabelasValoresMateriais tabelaValoresMaterial)
                {
                    throw new EntityNotFoundException<TabelasValoresMateriais>(tabelaValoresMaterialID);
                }

                tabelaValoresMaterial.IsDeleted = true;
                dbContext.Set<TabelasValoresMateriais>().Update(tabelaValoresMaterial);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao excluir o material da tabela de valores.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(tabelaValoresMaterialID), tabelaValoresMaterialID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task InserirNovaTabelaValoresMaterialAsync(TabelasValoresMateriais tabelaValoresMaterial)
        {
            try
            {
                await ValidarAsync(tabelaValoresMaterial);

                DefineCompetenciaParaPrimeiroDia(tabelaValoresMaterial);

                await dbContext.Set<TabelasValoresMateriais>().AddAsync(tabelaValoresMaterial);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao inserir novo material na tabela de valores.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(tabelaValoresMaterial), tabelaValoresMaterial },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<TabelasValoresMateriais> ObterTodosTabelaValoresMateriaisPorTabelaValores(long tabelaValoresID)
        {
            try
            {
                return from tvm in dbContext.Set<TabelasValoresMateriais>()
                       where tvm.TabelaValoresID == tabelaValoresID
                       select tvm;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter todas os materiais da tabela de valores pelo ID da tabela de valores.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(tabelaValoresID), tabelaValoresID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<TabelasValoresMateriais> ObterTodosTabelaValoresMateriaisPorUnidade(int unidadeID)
        {
            try
            {
                return from tvm in dbContext.Set<TabelasValoresMateriais>()
                       where tvm.UnidadeID == unidadeID
                       select tvm;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter todas os materiais da tabela de valores pelo ID da unidade.",
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
        private void DefineCompetenciaParaPrimeiroDia(TabelasValoresMateriais tabelaValoresMaterial)
        {
            // A competência deve sempre ser o 1º dia do mês.
            if (tabelaValoresMaterial.Competencia is DateTime competencia && competencia.Day != 1)
            {
                tabelaValoresMaterial.Competencia = competencia.AddDays((competencia.Day - 1) * -1);
            }
        }

        private async Task ValidarAsync(TabelasValoresMateriais tabelaValoresMaterial)
        {
            ValidationResult result = new();

            IEnumerable<TabelasValoresMateriais> materiais = dbContext.Set<TabelasValoresMateriais>().Where(x =>
                x.ID != tabelaValoresMaterial.ID
                && x.MaterialID == tabelaValoresMaterial.MaterialID
                && x.TabelaValoresID == tabelaValoresMaterial.TabelaValoresID);

            // Competencia
            if (tabelaValoresMaterial.Competencia is null && materiais.Any(x => x.Competencia == null && x.ID != tabelaValoresMaterial.ID))
            {
                result.SetError(nameof(TabelasValoresMateriais.Competencia), "required");
            }
            else if (tabelaValoresMaterial.Competencia is DateTime competencia)
            {
                if (competencia.Day != 1)
                {
                    competencia = competencia.AddDays((competencia.Day - 1) * -1);
                }

                if (materiais.Any(x => x.Competencia == competencia))
                {
                    result.SetError(nameof(TabelasValoresMateriais.Competencia), "exists");
                }
            }

            // MaterialID
            if (await dbContext.FindAsync<Materiais>(tabelaValoresMaterial.MaterialID) is null)
            {
                result.SetError(nameof(TabelasValoresMateriais.Material), "required");
            }

            // TabelaValor
            if (tabelaValoresMaterial.TabelaValoresID is long tabelaValoresID && await dbContext.FindAsync<TabelasValores>(tabelaValoresID) is null)
            {
                result.SetError(nameof(TabelasValoresMateriais.TabelaValores), "required");
            }

            // Unidade
            if (tabelaValoresMaterial.UnidadeID is int unidadeID && await dbContext.FindAsync<Unidades>(unidadeID) is null)
            {
                result.SetError(nameof(TabelasValoresMateriais.Unidade), "required");
            }

            // Valor
            if (tabelaValoresMaterial.Valor < 0)
            {
                result.SetError(nameof(TabelasValoresMateriais.Valor), "min");
            }

            result.ValidateEntityErrors(tabelaValoresMaterial);
        }
        #endregion
    }
}