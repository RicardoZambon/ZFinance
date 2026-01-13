using Microsoft.EntityFrameworkCore;
using Niten.Core.Entities.Configs;
using Niten.Core.Entities.Financeiro;
using Niten.Core.Entities.Geral;
using Niten.Core.Enums;
using Niten.Core.Services.Interfaces;
using ZDatabase.Exceptions;
using ZDatabase.Interfaces;
using ZDatabase.Validations;

namespace Niten.System.Core.Repositories.Financeiro
{
    /// <inheritdoc />
    public class TabelasValoresRepository : ITabelasValoresRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="TabelasValoresRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        public TabelasValoresRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task AtualizarTabelaValoresAsync(TabelasValores tabelaValores)
        {
            try
            {
                await ValidarAsync(tabelaValores);
                dbContext.Set<TabelasValores>().Update(tabelaValores);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao atualizar a tabela de valores.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(tabelaValores), tabelaValores },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<TabelasValores?> EncontrarTabelaValoresPorIDAsync(long tabelaValoresID)
        {
            try
            {
                return await dbContext.FindAsync<TabelasValores>(tabelaValoresID);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao encontrar a tabela de valores pelo ID.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(tabelaValoresID), tabelaValoresID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task ExcluirTabelaValoresAsync(long tabelaValoresID)
        {
            try
            {
                if (await EncontrarTabelaValoresPorIDAsync(tabelaValoresID) is not TabelasValores tabelaValores)
                {
                    throw new EntityNotFoundException<TabelasValores>(tabelaValoresID);
                }

                IEnumerable<UnidadesTabelasValores?> tabelas =
                (await (from utv in dbContext.Set<UnidadesTabelasValores>()
                        where (utv.Competencia == null || utv.Competencia <= DateTime.Today)
                              && utv.Unidade != null
                              && utv.Unidade.Status != UnidadesStatus.Excluida
                        orderby utv.Competencia == null ? DateTime.MinValue : utv.Competencia descending
                        select utv)
                .ToListAsync())
                .GroupBy(x => x.UnidadeID)
                .Select(x => x.FirstOrDefault());

                if (tabelas.Any(x => x?.TabelaValoresID == tabelaValoresID))
                {
                    throw new InvalidOperationException("TabelasValores-Delete-Failure-InUse");
                }

                tabelaValores.IsDeleted = true;
                dbContext.Set<TabelasValores>().Update(tabelaValores);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao excluir a tabela de valores.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(tabelaValoresID), tabelaValoresID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task InserirNovaTabelaValoresAsync(TabelasValores tabelaValores)
        {
            try
            {
                await ValidarAsync(tabelaValores);
                await dbContext.Set<TabelasValores>().AddAsync(tabelaValores);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao inserir nova tabela de valores.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(tabelaValores), tabelaValores },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<TabelasValores> ObterTodasTabelasValores()
        {
            try
            {
                return from tv in dbContext.Set<TabelasValores>()
                       select tv;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter todas as tabelas de valores.");
                throw;
            }
        }
        #endregion

        #region Private methods
        private async Task ValidarAsync(TabelasValores tabelaValores)
        {
            ValidationResult result = new();

            // Nome
            if (string.IsNullOrWhiteSpace(tabelaValores.Nome))
            {
                result.SetError(nameof(TabelasValores.Nome), "required");
            }
            else if (await dbContext.Set<TabelasValores>().AnyAsync(x => EF.Functions.Like(x.Nome!, tabelaValores.Nome) && x.ID != tabelaValores.ID))
            {
                result.SetError(nameof(TabelasValores.Nome), "exists");
            }

            // MoedaID
            if (await dbContext.FindAsync<Moedas>(tabelaValores.MoedaID) is null)
            {
                result.SetError(nameof(TabelasValores.MoedaID), "required");
            }

            result.ValidateEntityErrors(tabelaValores);
        }
        #endregion
    }
}