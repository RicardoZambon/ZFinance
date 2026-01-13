using Niten.Core.Entities.Configs;
using Niten.Core.Entities.Financeiro;
using Niten.Core.Entities.Geral;
using Niten.Core.Enums;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.Financeiro.Interfaces;
using ZDatabase.Interfaces;

namespace Niten.System.Core.Repositories.Financeiro
{
    /// <inheritdoc />
    public class TransacoesRepository : ITransacoesRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="TransacoesRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        public TransacoesRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task<Transacoes?> EncontrarTransacaoPorIDAsync(int transacaoID)
        {
            try
            {
                return await dbContext.FindAsync<Transacoes>(transacaoID);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro ao buscar transação pelo ID.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(transacaoID), transacaoID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<Transacoes> ObterTransacoesPendentesDeGerarComissao(DefinicaoComissoes definicaoComissao)
        {
            try
            {
                IQueryable<Transacoes> transacoes = from t in dbContext.Set<Transacoes>()
                                                    join c in dbContext.Set<Cadastros>() on t.CadastroID equals c.ID
                                                    where !t.Estornado
                                                    select t;

                switch (definicaoComissao.TransacaoTipo)
                {
                    case TransacaoTipos.Mensal:
                        var unidadesIds = definicaoComissao.Unidades?.Select(x => x.ID)?.ToArray() ?? Array.Empty<int>();

                        return transacoes
                            .Where(x => x.Tipo == "Mensal"
                                && x.Material != null && x.Material.Extra == $"{definicaoComissao.MesReferencia:yyyy-MM}"
                                && (definicaoComissao.TransacoesAte == null || x.DataFechamento.Date <= definicaoComissao.TransacoesAte.Value.Date)
                                && (definicaoComissao.TransacoesAPartir == null || x.DataFechamento.Date >= definicaoComissao.TransacoesAPartir.Value.Date)
                            );

                    case TransacaoTipos.Kyu:
                        return transacoes
                            .Where(x => x.Tipo == "Kyu"
                                && (x.Cadastro != null && (x.Cadastro.Situacao == "Aluno" || x.Cadastro.Situacao == "Monitor")) // TODO: Revisar para incluir a situação na definição de comissões.
                                && x.DataFechamento.Date >= new DateTime(definicaoComissao.MesReferencia.Year, definicaoComissao.MesReferencia.Month, 1)
                                && x.DataFechamento.Date < new DateTime(definicaoComissao.MesReferencia.Year, definicaoComissao.MesReferencia.Month, 1).AddMonths(1)
                            );

                    default:
                        return Array.Empty<Transacoes>().AsQueryable();
                }
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro ao obter transações pendentes de gerar comissões.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(definicaoComissao), definicaoComissao },
                    }
                );
                throw;
            }
        }
        #endregion

        #region Private methods
        #endregion
    }
}