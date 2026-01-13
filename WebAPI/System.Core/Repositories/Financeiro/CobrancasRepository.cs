using Microsoft.EntityFrameworkCore;
using Niten.Core.Entities.Financeiro;
using Niten.Core.Entities.Views;
using Niten.Core.Enums;
using Niten.Core.Repositories.Financeiro;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.Financeiro.Interfaces;
using Niten.System.Core.Repositories.Views.Interfaces;
using ZDatabase.Exceptions;
using ZDatabase.Interfaces;

namespace Niten.System.Core.Repositories.Financeiro
{
    /// <inheritdoc />
    public class CobrancasRepository : CobrancasRepositoryCore, ICobrancasRepository
    {
        #region Variables
        private readonly IViewCobrancasRepository viewCobrancas;
        private readonly IViewPagamentosOnlineRepository viewPagamentosOnlineRepository;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CobrancasRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        /// <param name="viewCobrancas">The <see cref="IViewCobrancasRepository"/> instance.</param>
        /// <param name="viewPagamentosOnlineRepository">The <see cref="IViewPagamentosOnlineRepository"/> instance.</param>
        public CobrancasRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler,
            IViewCobrancasRepository viewCobrancas,
            IViewPagamentosOnlineRepository viewPagamentosOnlineRepository)
            : base(dbContext, exceptionHandler)
        {
            this.viewCobrancas = viewCobrancas;
            this.viewPagamentosOnlineRepository = viewPagamentosOnlineRepository;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public void AtualizarCobranca(Cobrancas cobranca)
        {
            try
            {
                dbContext.Set<Cobrancas>().Update(cobranca);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao atualizar cobrança.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(cobranca), cobranca },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task CancelarCobrancaAsync(Cobrancas cobranca, string? observacoes)
        {
            try
            {
                if (cobranca.Status != CobrancasStatus.Cancelado)
                {
                    cobranca.Status = CobrancasStatus.Cancelado;
                    cobranca.StatusObservacoes = observacoes;

                    if (cobranca.TransacaoID is null
                        && cobranca.ValorBruto == 0
                        && await viewCobrancas.EncontrarCobrancaPorIDAsync(cobranca.ID) is ViewCobrancas viewCobranca)
                    {
                        if (cobranca.ValorBruto == 0)
                        {
                            cobranca.ValorBruto = viewCobranca.ValorBruto;

                            if (cobranca.ValorLiquido == 0)
                            {
                                cobranca.ValorLiquido = viewCobranca.ValorLiquido;
                            }
                        }
                    }

                    AtualizarCobranca(cobranca);
                }
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao cancelar cobrança.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(cobranca), cobranca },
                        { nameof(observacoes), observacoes },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<Cobrancas?> EncontrarCobrancaPorIDAsync(long cobrancaID)
        {
            try
            {
                return await dbContext.FindAsync<Cobrancas>(cobrancaID);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao encontrar a cobrança pelo ID.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(cobrancaID), cobrancaID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task ExcluirCobrancaAsync(long cobrancaID)
        {
            try
            {
                if (await EncontrarCobrancaPorIDAsync(cobrancaID) is not Cobrancas cobranca)
                {
                    throw new EntityNotFoundException<Contratacoes>(cobrancaID);
                }
                else if (cobranca.Transacao != null && !cobranca.Transacao.Estornado)
                {
                    throw new InvalidOperationException("Cobrancas-Button-Excluir-Modal-Failed-Pago");
                }

                cobranca.IsDeleted = true;
                dbContext.Set<Cobrancas>().Update(cobranca);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao excluir a cobrança.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(cobrancaID), cobrancaID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task GerarCobrancasPagamentoOnlineIDCadastroIDAsync(long pagamentoOnlineID, int cadastroID)
        {
            try
            {
                int competenciaAtualKey = ObterMesAnoKey(DateTime.Today);

                IList<ViewPagamentosOnline> pagamentosOnline = await viewPagamentosOnlineRepository.ObterPagamentosOnlinePorPagamentoOnlineIDCadastroID(pagamentoOnlineID, cadastroID)
                    .Where(x => x.MesAnoKey < competenciaAtualKey)
                    .ToListAsync();

                if (pagamentosOnline.Any())
                {
                    foreach (ViewPagamentosOnline pagamentoOnline in pagamentosOnline)
                    {
                        await InserirNovaCobrancaAsync(pagamentoOnline);
                    }
                }
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao gerar cobranças para o pagamento online.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(pagamentoOnlineID), pagamentoOnlineID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task LimparCobrancasPagamentoOnlineIDCadastroIDAsync(long pagamentoOnlineID, int cadastroID, DateTime competenciaInicial)
        {
            try
            {
                int novaCompetenciaInicialKey = ObterMesAnoKey(competenciaInicial);

                IList<Cobrancas> cobrancas = await (from c in ObterTodasCobrancas()
                                                    where c.PagamentoOnlineID == pagamentoOnlineID
                                                          && c.CadastroID == cadastroID
                                                          && c.MesAnoKey < novaCompetenciaInicialKey
                                                          && (c.Transacao == null || c.Transacao.Estornado)
                                                    select c).ToListAsync();

                if (cobrancas.Any())
                {
                    foreach (Cobrancas cobranca in cobrancas)
                    {
                        await ExcluirCobrancaAsync(cobranca.ID);
                    }
                }
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao limpar as cobranças pelo ID do pagamento online e ID do cadastro.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(pagamentoOnlineID), pagamentoOnlineID },
                        { nameof(cadastroID), cadastroID },
                        { nameof(competenciaInicial), competenciaInicial },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<Cobrancas> ObterTodasCobrancas()
        {
            try
            {
                return from c in dbContext.Set<Cobrancas>()
                       select c;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter todas as cobranças.");
                throw;
            }
        }
        #endregion

        #region Private methods
        private int ObterMesAnoKey(DateTime competencia)
        {
            return Convert.ToInt32($"{competencia.Year}{competencia.Month:00}");
        }
        #endregion
    }
}