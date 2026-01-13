using Microsoft.EntityFrameworkCore;
using Niten.Core.Entities.Configs;
using Niten.Core.Entities.Financeiro;
using Niten.Core.Entities.Geral;
using Niten.Core.Entities.Views;
using Niten.Core.Enums;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.Financeiro.Interfaces;
using ZDatabase.Exceptions;
using ZDatabase.Interfaces;
using ZDatabase.Validations;

namespace Niten.System.Core.Repositories.Financeiro
{
    /// <inheritdoc />
    public class ContratacoesRepository : IContratacoesRepository
    {
        #region Variables
        private readonly ICobrancasRepository cobrancasRepository;
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ContratacoesRepository"/> class.
        /// </summary>
        /// <param name="cobrancasRepository">The <see cref="ICobrancasRepository"/> instance.</param>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        public ContratacoesRepository(
            ICobrancasRepository cobrancasRepository,
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.cobrancasRepository = cobrancasRepository;
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task AtualizarContratacaoAsync(Contratacoes contratacao)
        {
            try
            {
                await ValidarAsync(contratacao);

                DefineInicioParaPrimeiroDia(contratacao);
                DefineTerminoParaUltimoDia(contratacao);

                dbContext.Set<Contratacoes>().Update(contratacao);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao atualizar a contratação.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(contratacao), contratacao },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<Contratacoes?> EncontrarContratacaoAtivaPorTipoContratacaoIDCadastroIDAsync(long tipoContratacaoID, int cadastroID)
        {
            try
            {
                return await dbContext.Set<Contratacoes>().FirstOrDefaultAsync(x => x.TipoContratacaoID == tipoContratacaoID && x.CadastroID == cadastroID && x.Status == ContratacoesStatus.Ativo);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao encontrar a contratação ativa pelo ID tipo de contratação e ID do cadastro.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(tipoContratacaoID), tipoContratacaoID },
                        { nameof(cadastroID), cadastroID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<Contratacoes?> EncontrarContratacaoPorIDAsync(long contratacaoID)
        {
            try
            {
                return await dbContext.FindAsync<Contratacoes>(contratacaoID);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao encontrar a contratação pelo ID.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(contratacaoID), contratacaoID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task ExcluirContratacaoAsync(long contratacaoID)
        {
            try
            {
                if (await EncontrarContratacaoPorIDAsync(contratacaoID) is not Contratacoes contratacao)
                {
                    throw new EntityNotFoundException<Contratacoes>(contratacaoID);
                }
                else if (contratacao.Status != ContratacoesStatus.Inativo)
                {
                    throw new InvalidOperationException("Contratacoes-Button-Excluir-Modal-Failed-Status");
                }

                contratacao.IsDeleted = true;
                dbContext.Set<Contratacoes>().Update(contratacao);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao excluir a contratação.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(contratacaoID), contratacaoID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task InserirNovaContratacaoAsync(Contratacoes contratacao)
        {
            try
            {
                await ValidarAsync(contratacao);
                DefineInicioParaPrimeiroDia(contratacao);
                await dbContext.Set<Contratacoes>().AddAsync(contratacao);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao inserir nova contratação.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(contratacao), contratacao },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<Contratacoes> ObterTodasContratacoes()
        {
            try
            {
                return from c in dbContext.Set<Contratacoes>()
                       select c;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter todas as contratações.");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task ProcessarCobrancasAsync(Contratacoes contratacao, DateTime? competenciaInicioOriginal = null)
        {
            try
            {
                if (contratacao.Status != ContratacoesStatus.Ativo || contratacao.TipoContratacao is null)
                {
                    return;
                }

                if (competenciaInicioOriginal is not null && competenciaInicioOriginal < contratacao.CompetenciaInicio)
                {
                    await cobrancasRepository.LimparCobrancasPagamentoOnlineIDCadastroIDAsync(contratacao.TipoContratacao.PagamentoOnlineID, contratacao.CadastroID, contratacao.CompetenciaInicio);
                }
                else
                {
                    await cobrancasRepository.GerarCobrancasPagamentoOnlineIDCadastroIDAsync(contratacao.TipoContratacao.PagamentoOnlineID, contratacao.CadastroID);
                }
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao processar as cobranças da contratação.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(contratacao), contratacao },
                        { nameof(competenciaInicioOriginal), competenciaInicioOriginal },
                    }
                );
                throw;
            }
        }
        #endregion

        #region Private methods
        private void DefineInicioParaPrimeiroDia(Contratacoes contratacao)
        {
            // A competência de início deve sempre ser o 1º dia do mês.
            if (contratacao.CompetenciaInicio.Day != 1)
            {
                contratacao.CompetenciaInicio = contratacao.CompetenciaInicio.AddDays((contratacao.CompetenciaInicio.Day - 1) * -1);
            }
        }

        private void DefineTerminoParaUltimoDia(Contratacoes contratacao)
        {
            // A competência de término deve sempre ser o último dia do mês.
            if (contratacao.Status == ContratacoesStatus.Finalizado && contratacao.CompetenciaTermino is DateTime competenciaTermino)
            {
                int ultimoDia = new DateTime(competenciaTermino.Year, competenciaTermino.Month, 1).AddMonths(1).AddDays(-1).Day;
                if (competenciaTermino.Day != ultimoDia)
                {
                    contratacao.CompetenciaTermino = competenciaTermino.AddDays(ultimoDia - competenciaTermino.Day);
                }
            }
        }

        private async Task ValidarAsync(Contratacoes contratacao)
        {
            ValidationResult result = new();

            // TipoContratacaoID
            TiposContratacoes? tipoContratacao = await dbContext.FindAsync<TiposContratacoes>(contratacao.TipoContratacaoID);
            if (tipoContratacao is null)
            {
                result.SetError(nameof(Contratacoes.TipoContratacaoID), "required");
            }

            // CadastroID
            if (await dbContext.FindAsync<Cadastros>(contratacao.CadastroID) is null)
            {
                result.SetError(nameof(Contratacoes.CadastroID), "required");
            }
            else if (await dbContext.Set<Contratacoes>().AnyAsync(x => x.CadastroID == contratacao.CadastroID && x.TipoContratacaoID == contratacao.TipoContratacaoID && (x.Status == ContratacoesStatus.Ativo || x.Status == ContratacoesStatus.Inativo) && x.ID != contratacao.ID))
            {
                result.SetError(nameof(Contratacoes.CadastroID), "exists");
            }

            if (tipoContratacao is not null)
            {
                // CompetenciaInicio
                if (contratacao.CompetenciaInicio == DateTime.MinValue)
                {
                    result.SetError(nameof(Contratacoes.CompetenciaInicio), "required");
                }
                else if (await dbContext.Set<Contratacoes>().AnyAsync(x => x.CadastroID == contratacao.CadastroID && x.TipoContratacaoID == contratacao.TipoContratacaoID && x.Status == ContratacoesStatus.Finalizado && contratacao.CompetenciaInicio >= x.CompetenciaInicio && contratacao.CompetenciaInicio <= x.CompetenciaTermino && x.ID != contratacao.ID))
                {
                    result.SetError(nameof(Contratacoes.CompetenciaInicio), "exists");
                }
                else if (contratacao.Status != ContratacoesStatus.Inativo)
                {
                    if (contratacao.ID > 0)
                    {
                        DateTime competenciaInicioOriginal = (DateTime)dbContext.Entry(contratacao).OriginalValues[nameof(Contratacoes.CompetenciaInicio)]!;
                        if (contratacao.CompetenciaInicio > competenciaInicioOriginal)
                        {
                            int competenciaInicioOriginalKey = Convert.ToInt32($"{competenciaInicioOriginal:yyyyMM}");
                            int competenciaInicioKey = Convert.ToInt32($"{contratacao.CompetenciaInicio:yyyyMM}");

                            if (dbContext.Set<ViewCobrancas>().Any(x =>
                                x.PagamentoOnlineID == tipoContratacao.PagamentoOnlineID
                                && x.CadastroID == contratacao.CadastroID
                                && x.MesAnoKey >= competenciaInicioOriginalKey
                                && x.MesAnoKey < competenciaInicioKey
                                && (x.Transacao != null || x.Status == CobrancasStatus.Cancelado))
                            )
                            {
                                result.SetError(nameof(Contratacoes.CompetenciaInicio), "pagamento");
                            }
                        }
                    }
                }

                // CompetenciaTermino
                if (contratacao.Status == ContratacoesStatus.Finalizado)
                {
                    DateTime? competenciaUltimoPagamento = null;

                    if (await dbContext.Set<ViewPagamentosOnline>().Where(x =>
                            x.PagamentoOnlineID == tipoContratacao.PagamentoOnlineID
                            && x.CadastroID == contratacao.CadastroID
                            && x.Transacao != null
                            && !x.Transacao.Estornado)
                        .OrderByDescending(x => x.Competencia)
                        .FirstOrDefaultAsync() is ViewPagamentosOnline pagamentoOnline
                        && pagamentoOnline.Competencia is DateTime pagamentoOnlineCompetencia)
                    {
                        competenciaUltimoPagamento = pagamentoOnlineCompetencia;
                    }

                    if (await dbContext.Set<ViewCobrancas>().Where(x =>
                            x.CadastroID == contratacao.CadastroID
                            && x.PagamentoOnlineID == tipoContratacao.PagamentoOnlineID
                            && x.Transacao != null
                            && !x.Transacao.Estornado)
                        .OrderByDescending(x => x.Competencia)
                        .FirstOrDefaultAsync() is ViewCobrancas cobrancas
                        && cobrancas.Competencia is DateTime cobrancaCompetencia
                        && (competenciaUltimoPagamento is null || cobrancaCompetencia >= competenciaUltimoPagamento))
                    {
                        competenciaUltimoPagamento = cobrancaCompetencia;
                    }

                    DateTime competencia = DateTime.Now;
                    if (competenciaUltimoPagamento is not null && competencia < competenciaUltimoPagamento)
                    {
                        competencia = competenciaUltimoPagamento.Value;
                    }

                    if (contratacao.CompetenciaTermino is not DateTime competenciaTermino)
                    {
                        result.SetError(nameof(Contratacoes.CompetenciaTermino), "required");
                    }
                    else if (competenciaTermino < contratacao.CompetenciaInicio)
                    {
                        result.SetError(nameof(Contratacoes.CompetenciaTermino), "min");
                    }
                    else if (competenciaUltimoPagamento is not null && competenciaUltimoPagamento > competenciaTermino)
                    {
                        result.SetError(nameof(Contratacoes.CompetenciaTermino), "minDate");
                    }
                    else if (await dbContext.Set<Contratacoes>().AnyAsync(x => x.CadastroID == contratacao.CadastroID && x.TipoContratacaoID == contratacao.TipoContratacaoID && x.Status == ContratacoesStatus.Finalizado && competenciaTermino >= x.CompetenciaInicio && competenciaTermino <= x.CompetenciaTermino && x.ID != contratacao.ID))
                    {
                        result.SetError(nameof(Contratacoes.CompetenciaTermino), "exists");
                    }
                }
            }

            // DiaVencimento
            if (contratacao.DiaVencimento is not null)
            {
                if (contratacao.DiaVencimento < 1)
                {
                    result.SetError(nameof(Contratacoes.CompetenciaInicio), "min");
                }
                else if (contratacao.DiaVencimento > 31)
                {
                    result.SetError(nameof(Contratacoes.CompetenciaInicio), "max");
                }
            }

            // TipoContratacaoID
            if (await dbContext.FindAsync<TiposContratacoes>(contratacao.TipoContratacaoID) is null)
            {
                result.SetError(nameof(Contratacoes.TipoContratacaoID), "required");
            }

            result.ValidateEntityErrors(contratacao);
        }
        #endregion
    }
}