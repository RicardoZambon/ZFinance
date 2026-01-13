using Microsoft.EntityFrameworkCore;
using Niten.Core.Entities.Configs;
using Niten.Core.Entities.Financeiro;
using Niten.Core.Entities.Geral;
using Niten.Core.Enums;
using Niten.System.Core.Repositories.Configs.Interfaces;
using Niten.System.Core.Repositories.Financeiro.Interfaces;
using Niten.System.Core.Repositories.Geral.Interfaces;
using System.Data;
using ZDatabase.Interfaces;
using ZDatabase.Validations;

namespace Niten.System.Core.Repositories.Financeiro
{
    /// <inheritdoc />
    public class ComissoesRepository : IComissoesRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IDefinicaoComissoesComissionadosRepository definicaoComissoesComissionadosRepository;
        private readonly IUnidadesCoordenadoresRepository unidadesCoordenadoresRepository;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ComissoesRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="definicaoComissoesComissionadosRepository">The <see cref="IDefinicaoComissoesComissionadosRepository"/> instance.</param>
        /// <param name="unidadesCoordenadoresRepository">The <see cref="IUnidadesCoordenadoresRepository"/> instance.</param>
        public ComissoesRepository(
            IDbContext dbContext,
            IDefinicaoComissoesComissionadosRepository definicaoComissoesComissionadosRepository,
            IUnidadesCoordenadoresRepository unidadesCoordenadoresRepository
            )
        {
            this.dbContext = dbContext;
            this.definicaoComissoesComissionadosRepository = definicaoComissoesComissionadosRepository;
            this.unidadesCoordenadoresRepository = unidadesCoordenadoresRepository;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task AtualizarComissaoAsync(Comissoes comissao)
        {
            await ValidateAsync(comissao);

            if (comissao.ID <= 0)
            {
                await dbContext.Set<Comissoes>().AddAsync(comissao);
            }
            else
            {
                // Status
                if (await EncontrarComissaoPorIDAsync(comissao.ID) is Comissoes dbComissao
                    && dbComissao.Status != ComissaoStatus.Aberto)
                {
                    throw new ApplicationException("This ComissaoID is already closed.");
                }

                dbContext.Set<Comissoes>().Update(comissao);
            }
        }

        /// <inheritdoc />
        public async Task<Comissoes?> EncontrarComissaoAberta(int unidadeID, int? cadastroID, DefinicaoComissoes definicaoComissao, int percentualRateio)
            => await dbContext.Set<Comissoes>().FirstOrDefaultAsync(x =>
                x.UnidadeID == unidadeID
                && x.CadastroID == cadastroID
                && x.DefinicaoComissaoID == definicaoComissao.ID
                && x.MesReferencia == definicaoComissao.MesReferencia
                && x.PercentualComissao == definicaoComissao.Percentual
                && x.PercentualRateio == percentualRateio
                && x.Status == ComissaoStatus.Aberto
            );

        /// <inheritdoc />
        public async Task<Comissoes?> EncontrarComissaoPorIDAsync(long comissaoID)
            => await dbContext.FindAsync<Comissoes>(comissaoID);

        /// <inheritdoc />
        public async Task<Comissoes> InserirNovaComissaoAsync(DefinicaoComissoes definicaoComissao, Unidades unidade, Cadastros? cadastro)
        {
            int rateio = 0;
            switch (definicaoComissao.TipoBeneficiarios)
            {
                case DefinicaoComissoesTiposBeneficiarios.CoordenadoresUnidades:
                    rateio = await unidadesCoordenadoresRepository.EncontrarPercentualPorUnidadeIDCadastroIDAsync(unidade.ID, cadastro?.ID);
                    break;
                case DefinicaoComissoesTiposBeneficiarios.Comissionados:
                    rateio = await definicaoComissoesComissionadosRepository.GetValorRateioAsync(definicaoComissao.ID, cadastro?.ID ?? 0);
                    break;
            }

            Comissoes? comissao = await EncontrarComissaoAberta(unidade.ID, cadastro?.ID, definicaoComissao, rateio);

            if (comissao is null)
            {
                comissao = new()
                {
                    Cadastro = cadastro,
                    CadastroID = cadastro?.ID ?? null,
                    DefinicaoComissao = definicaoComissao,
                    DefinicaoComissaoID = definicaoComissao.ID,
                    MesReferencia = definicaoComissao.MesReferencia,
                    PercentualComissao = definicaoComissao.Percentual,
                    PercentualRateio = rateio,
                    Unidade = unidade,
                    UnidadeID = unidade.ID,
                };

                await ValidateAsync(comissao);

                await dbContext.Set<Comissoes>().AddAsync(comissao);
            }

            return comissao;
        }

        /// <inheritdoc />
        public IQueryable<Comissoes> ObterTodasComissoes()
            => from c in dbContext.Set<Comissoes>()
               select c;
        #endregion

        #region Private methods
        private async Task ValidateAsync(Comissoes comissao)
        {
            ValidationResult result = new();

            // DefinicaoComissaoID
            if (await dbContext.Set<DefinicaoComissoes>().FindAsync(comissao.DefinicaoComissaoID) is null)
            {
                result.SetError(nameof(Comissoes.DefinicaoComissaoID), "invalid");
            }

            // UnidadeID
            else if (await dbContext.Set<Unidades>().FindAsync(comissao.UnidadeID) is null)
            {
                result.SetError(nameof(Comissoes.UnidadeID), "invalid");
            }

            // CadastroID
            else if (comissao.CadastroID is not null && await dbContext.Set<Cadastros>().FindAsync(comissao.CadastroID) is null)
            {
                result.SetError(nameof(Comissoes.CadastroID), "invalid");
            }

            result.ValidateEntityErrors(comissao);
        }
        #endregion
    }
}