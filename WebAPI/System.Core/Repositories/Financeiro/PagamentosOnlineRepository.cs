using Microsoft.EntityFrameworkCore;
using Niten.Core.Entities.Configs;
using Niten.Core.Entities.Estoque;
using Niten.Core.Entities.Financeiro;
using Niten.Core.Entities.Integracoes;
using Niten.Core.Entities.PortalAluno;
using Niten.Core.Enums;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.Financeiro.Interfaces;
using ZDatabase.Exceptions;
using ZDatabase.Interfaces;
using ZDatabase.Validations;

namespace Niten.System.Core.Repositories.Financeiro
{
    /// <inheritdoc />
    public class PagamentosOnlineRepository : IPagamentosOnlineRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PagamentosOnlineRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        public PagamentosOnlineRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task AtualizarPagamentoOnlineAsync(PagamentosOnline pagamentoOnline)
        {
            try
            {
                await ValidarAsync(pagamentoOnline);

                GarantirIntegridade(pagamentoOnline);

                dbContext.Set<PagamentosOnline>().Update(pagamentoOnline);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao atualizar o pagamento online.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(pagamentoOnline), pagamentoOnline },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<PagamentosOnline?> EncontrarPagamentoOnlinePorIDAsync(long pagamentoOnlineID)
        {
            try
            {
                return await dbContext.FindAsync<PagamentosOnline>(pagamentoOnlineID);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao encontrar o pagamento online pelo ID.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(pagamentoOnlineID), pagamentoOnlineID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task ExcluirPagamentoOnlineAsync(long pagamentoOnlineID)
        {
            try
            {
                if (await EncontrarPagamentoOnlinePorIDAsync(pagamentoOnlineID) is not PagamentosOnline pagamentoOnline)
                {
                    throw new EntityNotFoundException<PagamentosOnline>(pagamentoOnlineID);
                }
                else if (pagamentoOnline.Status != PagamentosOnlineStatus.Inativo)
                {
                    throw new InvalidOperationException("PagamentosOnline-Button-Excluir-Modal-Failed-Status");
                }

                pagamentoOnline.IsDeleted = true;
                dbContext.Set<PagamentosOnline>().Update(pagamentoOnline);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao excluir o pagamento online.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(pagamentoOnlineID), pagamentoOnlineID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task InserirNovoPagamentoOnlineAsync(PagamentosOnline pagamentoOnline)
        {
            try
            {
                await ValidarAsync(pagamentoOnline);

                GarantirIntegridade(pagamentoOnline);

                await dbContext.Set<PagamentosOnline>().AddAsync(pagamentoOnline);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao inserir novo pagamento online.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(pagamentoOnline), pagamentoOnline },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<PagamentosOnline> ObterTodosPagamentosOnline()
        {
            try
            {
                return from po in dbContext.Set<PagamentosOnline>()
                       select po;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter todos os pagamento online.");
                throw;
            }
        }

        #endregion

        #region Private methods
        private void GarantirIntegridade(PagamentosOnline pagamentoOnline)
        {
            // DiaVencimento
            if (pagamentoOnline.DiaVencimento is not null
                && pagamentoOnline.Recorrencia is null)
            {
                pagamentoOnline.DiaVencimento = null;
            }

            // Vencimento
            if (pagamentoOnline.Vencimento is not null
                && pagamentoOnline.Recorrencia is not null)
            {
                pagamentoOnline.Vencimento = null;
            }
        }

        private async Task ValidarAsync(PagamentosOnline pagamentoOnline)
        {
            ValidationResult result = new();

            // DataInicio
            if (pagamentoOnline.DataInicio is DateTime dataInicio && pagamentoOnline.DataTermino is DateTime dataTermino
                && dataInicio > dataTermino)
            {
                result.SetError(nameof(PagamentosOnline.DataInicio), "invalid");
            }

            // GrupoID
            if (!pagamentoOnline.Interno && await dbContext.FindAsync<Grupos>(pagamentoOnline.GrupoID) is null)
            {
                result.SetError(nameof(PagamentosOnline.GrupoID), "required");
            }

            // MaterialID
            if (await dbContext.FindAsync<Materiais>(pagamentoOnline.MaterialID) is null)
            {
                result.SetError(nameof(PagamentosOnline.MaterialID), "required");
            }
            else if (await dbContext.Set<PagamentosOnline>().AnyAsync(x => x.MaterialID == pagamentoOnline.MaterialID && x.ID != pagamentoOnline.ID))
            {
                result.SetError(nameof(PagamentosOnline.MaterialID), "exists");
            }

            // Nome
            if (string.IsNullOrWhiteSpace(pagamentoOnline.Nome))
            {
                result.SetError(nameof(PagamentosOnline.Nome), "required");
            }
            else if (await dbContext.Set<PagamentosOnline>().AnyAsync(x => EF.Functions.Like(x.Nome!, pagamentoOnline.Nome) && x.ID != pagamentoOnline.ID))
            {
                result.SetError(nameof(PagamentosOnline.Nome), "exists");
            }

            // PerfilID
            if (pagamentoOnline.PerfilID is not null && await dbContext.FindAsync<Perfis>(pagamentoOnline.PerfilID) is null)
            {
                result.SetError(nameof(PagamentosOnline.PerfilID), "invalid");
            }

            // RecorrenciaID
            if (pagamentoOnline.RecorrenciaID is not null
                && await dbContext.FindAsync<Recorrencias>(pagamentoOnline.RecorrenciaID) is null)
            {
                result.SetError(nameof(PagamentosOnline.RecorrenciaID), "required");
            }

            // Valor
            if (pagamentoOnline.Valor < 0)
            {
                result.SetError(nameof(PagamentosOnline.Valor), "min");
            }

            result.ValidateEntityErrors(pagamentoOnline);
        }
        #endregion
    }
}