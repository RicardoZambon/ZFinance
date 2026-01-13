using Microsoft.EntityFrameworkCore;
using Niten.Core.Entities.Configs;
using Niten.Core.Entities.Financeiro;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.Configs.Interfaces;
using ZDatabase.Exceptions;
using ZDatabase.Interfaces;
using ZDatabase.Validations;

namespace Niten.System.Core.Repositories.Configs
{
    /// <inheritdoc />
    public class TiposContratacoesRepository : ITiposContratacoesRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="TiposContratacoesRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        public TiposContratacoesRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task AtualizarTipoContratacaoAsync(TiposContratacoes tipoContratacao)
        {
            try
            {
                await ValidarAsync(tipoContratacao);
                dbContext.Set<TiposContratacoes>().Update(tipoContratacao);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao atualizar o tipo de contratação.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(tipoContratacao), tipoContratacao },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<TiposContratacoes?> EncontrarTipoContratacaoPorIDAsync(long tipoContratacaoID)
        {
            try
            {
                return await dbContext.FindAsync<TiposContratacoes>(tipoContratacaoID);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao encontrar o tipo de contratação pelo ID.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(tipoContratacaoID), tipoContratacaoID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task ExcluirTipoContratacaoAsync(long tipoContratacaoID)
        {
            try
            {
                if (await EncontrarTipoContratacaoPorIDAsync(tipoContratacaoID) is not TiposContratacoes tipoContratacao)
                {
                    throw new EntityNotFoundException<TiposContratacoes>(tipoContratacaoID);
                }

                tipoContratacao.IsDeleted = true;
                dbContext.Set<TiposContratacoes>().Update(tipoContratacao);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao excluir o tipo de contratação.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(tipoContratacaoID), tipoContratacaoID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task InserirNovoTipoContratacaoAsync(TiposContratacoes tipoContratacao)
        {
            try
            {
                await ValidarAsync(tipoContratacao);
                await dbContext.Set<TiposContratacoes>().AddAsync(tipoContratacao);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao inserir novo tipo de contratação.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(tipoContratacao), tipoContratacao },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<TiposContratacoes> ObterTodosTiposContratacoes()
        {
            try
            {
                return from tc in dbContext.Set<TiposContratacoes>()
                       select tc;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter todos os tipos de contratações.");
                throw;
            }
        }
        #endregion

        #region Private methods
        private async Task ValidarAsync(TiposContratacoes tipoContratacao)
        {
            ValidationResult result = new();

            // Nome
            if (string.IsNullOrWhiteSpace(tipoContratacao.Nome))
            {
                result.SetError(nameof(TiposContratacoes.Nome), "required");
            }
            else if (await dbContext.Set<TiposContratacoes>().AnyAsync(x => EF.Functions.Like(x.Nome!, tipoContratacao.Nome) && x.ID != tipoContratacao.ID))
            {
                result.SetError(nameof(TiposContratacoes.Nome), "exists");
            }

            // PagamentoOnlineID
            if (await dbContext.FindAsync<PagamentosOnline>(tipoContratacao.PagamentoOnlineID) is null)
            {
                result.SetError(nameof(TiposContratacoes.PagamentoOnlineID), "required");
            }
            else if (await dbContext.Set<TiposContratacoes>().AnyAsync(x => x.PagamentoOnlineID == tipoContratacao.PagamentoOnlineID && x.ID != tipoContratacao.ID))
            {
                result.SetError(nameof(TiposContratacoes.PagamentoOnlineID), "exists");
            }

            result.ValidateEntityErrors(tipoContratacao);
        }
        #endregion
    }
}