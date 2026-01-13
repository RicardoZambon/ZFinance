using Microsoft.EntityFrameworkCore;
using Niten.Core.Entities.Financeiro;
using Niten.Core.Enums;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.Financeiro.Interfaces;
using ZDatabase.Exceptions;
using ZDatabase.Interfaces;
using ZDatabase.Validations;

namespace Niten.System.Core.Repositories.Financeiro
{
    /// <inheritdoc />
    public class ComissoesTransacoesRepository : IComissoesTransacoesRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ComissoesTransacoesRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        public ComissoesTransacoesRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task<ComissoesTransacoes?> EncontrarComissaoTransacaoPorIDAsync(long comissaoTransacaoID)
        {
            try
            {
                return await dbContext.FindAsync<ComissoesTransacoes>(comissaoTransacaoID);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro ao buscar comissão X transãção pelo ID.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(comissaoTransacaoID), comissaoTransacaoID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task ExcluirComissaoTransacaoAsync(long comissaoTransacaoID)
        {
            try
            {
                if (await EncontrarComissaoTransacaoPorIDAsync(comissaoTransacaoID) is not ComissoesTransacoes comissaoTransacao)
                {
                    throw new EntityNotFoundException<ComissoesTransacoes>(comissaoTransacaoID);
                }
                else if (comissaoTransacao.Comissao is Comissoes comissao && comissao.Status != ComissaoStatus.Aberto)
                {
                    throw new InvalidOperationException("This ComissaoID is already closed");
                }

                comissaoTransacao.IsDeleted = true;
                dbContext.Update(comissaoTransacao);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro ao excluir comissão X transação.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(comissaoTransacaoID), comissaoTransacaoID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task InserirNovaComissaoTransacaoAsync(ComissoesTransacoes comissaoTransacao)
        {
            try
            {
                await ValidarAsync(comissaoTransacao);

                comissaoTransacao.ValorTransacao = (comissaoTransacao.Transacao?.ValorBRL ?? 0) * -1L;

                comissaoTransacao.ValorComissao = comissaoTransacao.ValorTransacao
                    * ((decimal)(comissaoTransacao.Comissao?.PercentualComissao ?? 0) / 100L)
                    * ((decimal)(comissaoTransacao.Comissao?.PercentualRateio ?? 0) / 100L);

                await dbContext.AddAsync(comissaoTransacao);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro ao inserir nova comissão X transação.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(comissaoTransacao), comissaoTransacao },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<ComissoesTransacoes> ObterComissoesTransacaoPorComissaoID(long comissaoID)
        {
            try
            {
                return from ct in dbContext.Set<ComissoesTransacoes>()
                       where ct.ComissaoID == comissaoID
                       orderby ct.Transacao!.DataFechamento,
                               ct.Transacao!.Cadastro!.NomeGuerra
                       select ct;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro ao obter comissões X transações pelo ID da comissão.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(comissaoID), comissaoID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<bool> VerificaComissoesTransacoesExiste(int transacaoID, long definicaoComissaoID, DateTime mesReferencia, int unidadeID, int? cadastroID)
        {
            try
            {
                return await dbContext.Set<ComissoesTransacoes>()
                    .AnyAsync(x =>
                        x.TransacaoID == transacaoID
                        && x.Comissao != null
                        && x.Comissao.DefinicaoComissaoID == definicaoComissaoID
                        && x.Comissao.MesReferencia == mesReferencia
                        && x.Comissao.UnidadeID == unidadeID
                        && x.Comissao.CadastroID == cadastroID
                        && (
                            x.Comissao.Status == ComissaoStatus.Aberto
                            || x.Comissao.Status == ComissaoStatus.Aprovado
                        )
                    );
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro ao verificar se existe comissão X transação.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(transacaoID), transacaoID },
                        { nameof(definicaoComissaoID), definicaoComissaoID },
                        { nameof(mesReferencia), mesReferencia },
                        { nameof(unidadeID), unidadeID },
                        { nameof(cadastroID), cadastroID },
                    }
                );
                throw;
            }
        }
        #endregion

        #region Private methods
        private async Task ValidarAsync(ComissoesTransacoes comissaoTransacao)
        {
            ValidationResult result = new();

            try
            {
                // ComissaoID
                Comissoes? comissao = await dbContext.Set<Comissoes>().FindAsync(comissaoTransacao.ComissaoID);
                if (comissao is null
                    && comissaoTransacao.Comissao is not null
                    && comissaoTransacao.Comissao.ID == comissaoTransacao.ComissaoID)
                {
                    comissao = comissaoTransacao.Comissao;
                }

                if (comissao is null)
                {
                    result.SetError(nameof(ComissoesTransacoes.ComissaoID), "invalid");
                }
                else if (comissao.Status != ComissaoStatus.Aberto)
                {
                    result.SetError(nameof(ComissoesTransacoes.ComissaoID), "closed");
                }

                // TransacaoID
                if (await dbContext.Set<Transacoes>().FindAsync(comissaoTransacao.TransacaoID) is null)
                {
                    result.SetError(nameof(ComissoesTransacoes.TransacaoID), "invalid");
                }
                else if (await dbContext.Set<ComissoesTransacoes>().AnyAsync(x =>
                    x.TransacaoID == comissaoTransacao.TransacaoID
                    && x.Comissao != null && comissaoTransacao.Comissao != null
                    && x.ID != comissaoTransacao.ID
                    && x.Comissao.UnidadeID == comissaoTransacao.Comissao.UnidadeID
                    && x.Comissao.CadastroID == comissaoTransacao.Comissao.CadastroID
                    && (
                        x.Comissao.Status == ComissaoStatus.Aberto
                        || x.Comissao.Status == ComissaoStatus.Aprovado
                    )
                ))
                {
                    result.SetError(nameof(ComissoesTransacoes.TransacaoID), "exists");
                }
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro ao validar comissão X transação.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(comissaoTransacao), comissaoTransacao },
                    }
                );
                throw;
            }

            result.ValidateEntityErrors(comissaoTransacao);
        }
        #endregion
    }
}