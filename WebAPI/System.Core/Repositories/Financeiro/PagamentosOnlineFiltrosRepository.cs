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
    public class PagamentosOnlineFiltrosRepository : IPagamentosOnlineFiltrosRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PagamentosOnlineFiltrosRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        public PagamentosOnlineFiltrosRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task AtualizarPagamentoOnlineFiltroAsync(PagamentosOnlineFiltros pagamentoOnlineFiltro)
        {
            try
            {
                await ValidarAsync(pagamentoOnlineFiltro);
                dbContext.Set<PagamentosOnlineFiltros>().Update(pagamentoOnlineFiltro);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao atualizar o filtro do pagamento online.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(pagamentoOnlineFiltro), pagamentoOnlineFiltro },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<PagamentosOnlineFiltros?> EncontrarPagamentoOnlineFiltroPorIDAsync(long pagamentoOnlineFiltroID)
        {
            try
            {
                return await dbContext.FindAsync<PagamentosOnlineFiltros>(pagamentoOnlineFiltroID);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao encontrar o filtro do pagamento online pelo ID.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(pagamentoOnlineFiltroID), pagamentoOnlineFiltroID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task ExcluirPagamentoOnlineFiltroAsync(long pagamentoOnlineFiltroID)
        {
            try
            {
                if (await EncontrarPagamentoOnlineFiltroPorIDAsync(pagamentoOnlineFiltroID) is not PagamentosOnlineFiltros pagamentoOnlineFiltro)
                {
                    throw new EntityNotFoundException<PagamentosOnlineFiltros>(pagamentoOnlineFiltroID);
                }

                pagamentoOnlineFiltro.IsDeleted = true;
                dbContext.Set<PagamentosOnlineFiltros>().Update(pagamentoOnlineFiltro);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao excluir o filtro do pagamento online.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(pagamentoOnlineFiltroID), pagamentoOnlineFiltroID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task InserirNovoPagamentoOnlineFiltroAsync(PagamentosOnlineFiltros pagamentoOnlineFiltro)
        {
            try
            {
                await ValidarAsync(pagamentoOnlineFiltro);
                await dbContext.Set<PagamentosOnlineFiltros>().AddAsync(pagamentoOnlineFiltro);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao inserir novo filtro no pagamento online.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(pagamentoOnlineFiltro), pagamentoOnlineFiltro },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<PagamentosOnlineFiltros> ObterTodosPagamentoOnlineFiltros(long pagamentoOnlineID)

        {
            try
            {
                return from po in dbContext.Set<PagamentosOnlineFiltros>()
                       where po.PagamentoOnlineID == pagamentoOnlineID
                       select po;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter todos os filtros do pagamento online pelo ID do pagamento online.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(pagamentoOnlineID), pagamentoOnlineID },
                    }
                );
                throw;
            }
        }
        #endregion

        #region Private methods
        private async Task ValidarAsync(PagamentosOnlineFiltros pagamentoOnlineFiltro)
        {
            ValidationResult result = new();

            // DataTermino
            if (pagamentoOnlineFiltro.DataTermino is DateTime dataTermino && pagamentoOnlineFiltro.DataInicio is DateTime dataInicio && dataTermino < dataInicio)
            {
                result.SetError(nameof(PagamentosOnlineFiltros.DataTermino), "invalid");
            }

            // Nome
            if (string.IsNullOrWhiteSpace(pagamentoOnlineFiltro.Nome))
            {
                result.SetError(nameof(PagamentosOnlineFiltros.Nome), "required");
            }
            else if (await dbContext.Set<PagamentosOnlineFiltros>().AnyAsync(x => x.PagamentoOnlineID == pagamentoOnlineFiltro.PagamentoOnlineID && EF.Functions.Like(x.Nome!, pagamentoOnlineFiltro.Nome) && x.ID != pagamentoOnlineFiltro.ID))
            {
                result.SetError(nameof(PagamentosOnlineFiltros.Nome), "exists");
            }

            // PagamentoOnlineID
            if (await dbContext.FindAsync<PagamentosOnline>(pagamentoOnlineFiltro.PagamentoOnlineID) is null)
            {
                result.SetError(nameof(PagamentosOnlineFiltros.PagamentoOnlineID), "required");
            }

            result.ValidateEntityErrors(pagamentoOnlineFiltro);
        }
        #endregion
    }
}