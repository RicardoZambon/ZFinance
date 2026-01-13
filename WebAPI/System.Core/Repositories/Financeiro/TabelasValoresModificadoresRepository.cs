using Microsoft.EntityFrameworkCore;
using Niten.Core.Entities.Estoque;
using Niten.Core.Entities.Financeiro;
using Niten.Core.Entities.Geral;
using Niten.Core.Enums;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.Financeiro.Interfaces;
using ZDatabase.Exceptions;
using ZDatabase.Interfaces;
using ZDatabase.Validations;

namespace Niten.System.Core.Repositories.Financeiro
{
    /// <inheritdoc />
    public class TabelasValoresModificadoresRepository : ITabelasValoresModificadoresRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="TabelasValoresModificadoresRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        public TabelasValoresModificadoresRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task AtualizarTabelaValoresModificadorAsync(TabelasValoresModificadores tabelaValoresModificador)
        {
            try
            {
                await ValidarAsync(tabelaValoresModificador);
                dbContext.Set<TabelasValoresModificadores>().Update(tabelaValoresModificador);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao atualizar o modificador da tabela de valores.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(tabelaValoresModificador), tabelaValoresModificador },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<TabelasValoresModificadores?> EncontrarTabelaValoresModificadorPorIDAsync(long tabelaValoresModificadorID)
        {
            try
            {
                return await dbContext.FindAsync<TabelasValoresModificadores>(tabelaValoresModificadorID);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao encontrar o modificador da tabela de valores pelo ID.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(tabelaValoresModificadorID), tabelaValoresModificadorID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task ExcluirTabelaValoresModificadorAsync(long tabelaValoresModificadorID)
        {
            try
            {
                if (await EncontrarTabelaValoresModificadorPorIDAsync(tabelaValoresModificadorID) is not TabelasValoresModificadores tabelaValoresModificador)
                {
                    throw new EntityNotFoundException<TabelasValoresModificadores>(tabelaValoresModificadorID);
                }

                tabelaValoresModificador.IsDeleted = true;
                dbContext.Set<TabelasValoresModificadores>().Update(tabelaValoresModificador);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao excluir o modificador da tabela de valores.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(tabelaValoresModificadorID), tabelaValoresModificadorID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task InserirNovaTabelaValoresModificadorAsync(TabelasValoresModificadores tabelaValoresModificador)
        {
            try
            {
                await ValidarAsync(tabelaValoresModificador);
                await dbContext.Set<TabelasValoresModificadores>().AddAsync(tabelaValoresModificador);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao inserir novo modificador na tabela de valores.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(tabelaValoresModificador), tabelaValoresModificador },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<TabelasValoresModificadores> ObterTodosTabelaValoresModificadoresPorTabelaValores(long tabelaValoresID)
        {
            try
            {
                return from tvm in dbContext.Set<TabelasValoresModificadores>()
                       where tvm.TabelaValoresID == tabelaValoresID
                       select tvm;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter todos os modificadores da tabela de valores.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(tabelaValoresID), tabelaValoresID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<TabelasValoresModificadores> ObterTodosTabelaValoresModificadoresPorUnidade(long unidadeID)
        {
            try
            {
                return from tvm in dbContext.Set<TabelasValoresModificadores>()
                       where tvm.UnidadeID == unidadeID
                       select tvm;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter todos os modificadores da unidade.",
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
        private async Task ValidarAsync(TabelasValoresModificadores tabelaValoresModificador)
        {
            ValidationResult result = new();

            // DataTermino
            if (tabelaValoresModificador.DataInicio is DateTime dataInicio
                && tabelaValoresModificador.DataTermino is DateTime dataTermino
                && dataTermino < dataInicio)
            {
                result.SetError(nameof(TabelasValoresModificadores.DataTermino), "invalid");
            }

            // MaterialID
            if (await dbContext.FindAsync<Materiais>(tabelaValoresModificador.MaterialID) is null)
            {
                result.SetError(nameof(TabelasValoresModificadores.MaterialID), "required");
            }

            // Nome
            if (string.IsNullOrWhiteSpace(tabelaValoresModificador.Nome))
            {
                result.SetError(nameof(TabelasValoresModificadores.Nome), "required");
            }
            else if (await dbContext.Set<TabelasValoresModificadores>().AnyAsync(x =>
                (
                    (tabelaValoresModificador.TabelaValoresID != null && x.TabelaValoresID == tabelaValoresModificador.TabelaValoresID)
                    || (tabelaValoresModificador.UnidadeID != null && x.UnidadeID == tabelaValoresModificador.UnidadeID)
                )
                && EF.Functions.Like(x.Nome!, tabelaValoresModificador.Nome)
                && x.ID != tabelaValoresModificador.ID)
            )
            {
                result.SetError(nameof(TabelasValoresModificadores.Nome), "exists");
            }

            // Ordem
            if (tabelaValoresModificador.Ordem <= 0)
            {
                result.SetError(nameof(TabelasValoresModificadores.Ordem), "min");
            }

            // Percentual
            if (tabelaValoresModificador.TipoValor == TabelasValoresTiposValores.Porcentagem)
            {
                if (tabelaValoresModificador.Percentual <= 0)
                {
                    result.SetError(nameof(TabelasValoresModificadores.Percentual), "min");
                }
                else if (tabelaValoresModificador.Percentual > 100)
                {
                    result.SetError(nameof(TabelasValoresModificadores.Percentual), "max");
                }
            }
            else
            {
                tabelaValoresModificador.Percentual = null;
            }

            // TabelaValoresID
            if (tabelaValoresModificador.TabelaValoresID is long tabelaValoresID && await dbContext.FindAsync<TabelasValores>(tabelaValoresID) is null)
            {
                result.SetError(nameof(TabelasValoresModificadores.TabelaValoresID), "required");
            }

            // UnidadeID
            if (tabelaValoresModificador.UnidadeID is int unidadeID && await dbContext.FindAsync<Unidades>(unidadeID) is null)
            {
                result.SetError(nameof(TabelasValoresModificadores.UnidadeID), "required");
            }

            // Valor
            if (tabelaValoresModificador.TipoValor == TabelasValoresTiposValores.Valor)
            {
                if (tabelaValoresModificador.Valor <= 0)
                {
                    result.SetError(nameof(TabelasValoresModificadores.Valor), "min");
                }
            }
            else
            {
                tabelaValoresModificador.Valor = null;
            }

            result.ValidateEntityErrors(tabelaValoresModificador);
        }
        #endregion
    }
}