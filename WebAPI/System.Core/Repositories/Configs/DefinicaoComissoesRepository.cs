using Microsoft.EntityFrameworkCore;
using Niten.Core.Entities.Configs;
using Niten.Core.Entities.Geral;
using Niten.Core.Enums;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.Configs.Interfaces;
using Niten.System.Core.Repositories.Geral.Interfaces;
using System.Data;
using ZDatabase.Interfaces;
using ZDatabase.Validations;

namespace Niten.System.Core.Repositories.Configs
{
    /// <inheritdoc />
    public class DefinicaoComissoesRepository : IDefinicaoComissoesRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DefinicaoComissoesRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        public DefinicaoComissoesRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task AtualizarDefinicaoComissaoAsync(DefinicaoComissoes definicaoComissao)
        {
            try
            {
                await ValidarAsync(definicaoComissao);
                dbContext.Update(definicaoComissao);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro ao atualizar definição de comissão.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(definicaoComissao), definicaoComissao },
                    }
                );
                throw;
            }
        }

        /// <inehritdoc />
        public async Task<DefinicaoComissoes?> EncontrarDefinicaoComissaoPorIDAsync(long definicaoComissaoID)
        {
            try
            {
                return await dbContext.FindAsync<DefinicaoComissoes>(definicaoComissaoID);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro ao buscar definição de comissão pelo ID.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(definicaoComissaoID), definicaoComissaoID },
                    }
                );
                throw;
            }
        }

        /// <inehritdoc />
        public async Task InserirNovaDefinicaoComissaoAsync(DefinicaoComissoes definicaoComissao)
        {
            try
            {
                await ValidarAsync(definicaoComissao);

                if (definicaoComissao.DataProximoCalculo <= DateTime.Today)
                {
                    definicaoComissao.Status = DefinicaoComissoesStatus.Pausado;
                }
                else if (definicaoComissao.Status != DefinicaoComissoesStatus.Ativo)
                {
                    definicaoComissao.Status = DefinicaoComissoesStatus.Ativo;
                }

                await dbContext.AddAsync(definicaoComissao);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro ao inserir nova definição de comissão.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(definicaoComissao), definicaoComissao },
                    }
                );
                throw;
            }
        }

        /// <inehritdoc />
        public IQueryable<DefinicaoComissoes> ObterDefinicoesComissoesPendentesDeGerarComissoes()
        {
            try
            {
                return from df in dbContext.Set<DefinicaoComissoes>()
                       where df.Status == DefinicaoComissoesStatus.Ativo
                             && df.DataProximoCalculo.Date <= DateTime.Today
                       select df;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro ao obter as definições de comissão pendentes de gerar comissões.");
                throw;
            }
        }

        /// <inehritdoc />
        public IQueryable<DefinicaoComissoes> ObterTodasDefinicoesComissoes()
        {
            try
            {
                return from dc in dbContext.Set<DefinicaoComissoes>()
                       orderby dc.Nome
                       select dc;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro ao obter todas as definições de comissão.");
                throw;
            }
        }
        #endregion

        #region Private methods
        private async Task ValidarAsync(DefinicaoComissoes definicaoComissao)
        {
            ValidationResult result = new();

            // Nome
            if (string.IsNullOrWhiteSpace(definicaoComissao.Nome))
            {
                result.SetError(nameof(DefinicaoComissoes.Nome), "required");
            }
            else if (await dbContext.Set<DefinicaoComissoes>().AnyAsync(x => EF.Functions.Like(x.Nome!, definicaoComissao.Nome) && x.ID != definicaoComissao.ID))
            {
                result.SetError(nameof(DefinicaoComissoes.Nome), "exists");
            }

            // Percentual
            if (definicaoComissao.Percentual <= 0)
            {
                result.SetError(nameof(DefinicaoComissoes.Percentual), "min");
            }
            else if (definicaoComissao.Percentual > 100)
            {
                result.SetError(nameof(DefinicaoComissoes.Percentual), "max");
            }

            // MesReferencia
            if (definicaoComissao.MesReferencia.Year > definicaoComissao.DataProximoCalculo.Year
                || (definicaoComissao.MesReferencia.Year == definicaoComissao.DataProximoCalculo.Year
                    && definicaoComissao.MesReferencia.Month >= definicaoComissao.DataProximoCalculo.Month)
                )
            {
                result.SetError(nameof(definicaoComissao.MesReferencia), "mesReferenciaLessThanProximoCalculo");
            }

            if (definicaoComissao.MesReferencia.Day > 1)
            {
                definicaoComissao.MesReferencia = new DateTime(definicaoComissao.MesReferencia.Year, definicaoComissao.MesReferencia.Month, 1);
            }

            // TransacoesAPartir
            if (definicaoComissao.TransacaoTipo == TransacaoTipos.Mensal)
            {
                if (definicaoComissao.TransacoesAPartir != null
                    && definicaoComissao.TransacoesAPartir.Value.Date >= definicaoComissao.DataProximoCalculo.Date)
                {
                    result.SetError(nameof(definicaoComissao.TransacoesAPartir), "lessThanControl");
                }
            }
            else
            {
                if (definicaoComissao.TransacoesAPartir != null)
                {
                    definicaoComissao.TransacoesAPartir = null;
                }
            }

            // TransacoesAte
            if (definicaoComissao.TransacaoTipo == TransacaoTipos.Mensal)
            {
                if (definicaoComissao.TransacoesAte != null && definicaoComissao.TransacoesAPartir != null)
                {
                    result.SetError(nameof(definicaoComissao.TransacoesAte), "transacoesDatas");
                }
                else if (definicaoComissao.TransacoesAte != null
                    && definicaoComissao.TransacoesAte.Value.Date >= definicaoComissao.DataProximoCalculo.Date)
                {
                    result.SetError(nameof(definicaoComissao.TransacoesAte), "lessThanControl");
                }
            }
            else
            {
                if (definicaoComissao.TransacoesAte != null)
                {
                    definicaoComissao.TransacoesAte = null;
                }
            }

            result.ValidateEntityErrors(definicaoComissao);
        }
        #endregion
    }

    public class DefinicaoComissoesComissionadosRepository : IDefinicaoComissoesComissionadosRepository
    {
        private readonly IDbContext dbContext;
        private readonly IDefinicaoComissoesRepository definicaoComissoesRepository;
        private readonly IComissionadosRepository comissionadosRepository;

        public DefinicaoComissoesComissionadosRepository(
            IDbContext dbContext,
            IDefinicaoComissoesRepository definicaoComissoesRepository,
            IComissionadosRepository comissionadosRepository)
        {
            this.dbContext = dbContext;
            this.definicaoComissoesRepository = definicaoComissoesRepository;
            this.comissionadosRepository = comissionadosRepository;
        }


        // Lists
        public async Task<IQueryable<Comissionados>> ListComissionados(long definicaoComissaoId)
        {
            if (await definicaoComissoesRepository.EncontrarDefinicaoComissaoPorIDAsync(definicaoComissaoId) is not DefinicaoComissoes definicaoComissao)
            {
                throw new ArgumentNullException(nameof(definicaoComissaoId));
            }

            return (definicaoComissao.Comissionados ?? Enumerable.Empty<Comissionados>()).AsQueryable();
        }


        // Search
        public async Task<int> GetValorRateioAsync(long definicaoComissaoId, int cadastroId)
        {
            if (await definicaoComissoesRepository.EncontrarDefinicaoComissaoPorIDAsync(definicaoComissaoId) is DefinicaoComissoes definicaoComissao
                && definicaoComissao.Comissionados?.FirstOrDefault(x => x.CadastroID == cadastroId) is Comissionados comissionado)
            {
                return (int)Math.Round(100M / definicaoComissao.Comissionados.Count, 0);
            }
            return 0;
        }


        // Update
        public async Task UpdateComissionadosAsync(long definicaoComissaoId, IEnumerable<long> comissionadosIdsToAdd, IEnumerable<long> comissionadosIdsToRemove)
        {
            if (await definicaoComissoesRepository.EncontrarDefinicaoComissaoPorIDAsync(definicaoComissaoId) is not DefinicaoComissoes definicaoComissao)
            {
                throw new ArgumentNullException(nameof(definicaoComissaoId));
            }

            if (definicaoComissao.Comissionados == null)
            {
                definicaoComissao.Comissionados = new List<Comissionados>();
            }

            foreach (long comissionadoId in comissionadosIdsToAdd)
            {
                if (await comissionadosRepository.EncontrarComissionadoPorIDAsync(comissionadoId) is Comissionados comissionado
                    && !definicaoComissao.Comissionados.Contains(comissionado))
                {
                    definicaoComissao.Comissionados.Add(comissionado);
                }
            }

            foreach (long comissionadoId in comissionadosIdsToRemove)
            {
                if (await comissionadosRepository.EncontrarComissionadoPorIDAsync(comissionadoId) is Comissionados comissionado
                    && definicaoComissao.Comissionados.Contains(comissionado))
                {
                    definicaoComissao.Comissionados.Remove(comissionado);
                }
            }

            await definicaoComissoesRepository.AtualizarDefinicaoComissaoAsync(definicaoComissao);
        }
    }

    public class DefinicaoComissoesUnidadesRepository : IDefinicaoComissoesUnidadesRepository
    {
        private readonly IDefinicaoComissoesRepository definicaoComissoesRepository;
        private readonly IUnidadesRepository unidadesRepository;

        public DefinicaoComissoesUnidadesRepository(
            IDefinicaoComissoesRepository definicaoComissoesRepository,
            IUnidadesRepository unidadesRepository)
        {
            this.definicaoComissoesRepository = definicaoComissoesRepository;
            this.unidadesRepository = unidadesRepository;
        }


        // Lists
        public async Task<IQueryable<Unidades>> ListUnidades(long definicaoComissaoId)
        {
            if (await definicaoComissoesRepository.EncontrarDefinicaoComissaoPorIDAsync(definicaoComissaoId) is not DefinicaoComissoes definicaoComissao)
            {
                throw new ArgumentNullException(nameof(definicaoComissaoId));
            }

            return (definicaoComissao.Unidades ?? Enumerable.Empty<Unidades>())
                .Where(x => x.Status != UnidadesStatus.Excluida)
                .OrderBy(x => x.Ordem)
                .ThenBy(x => x.NomeSystem)
                .AsQueryable();
        }


        // Update
        public async Task UpdateUnidadesAsync(long definicaoComissaoId, IEnumerable<int> UnidadesIdsToAdd, IEnumerable<int> unidadesIdsToRemove)
        {
            if (await definicaoComissoesRepository.EncontrarDefinicaoComissaoPorIDAsync(definicaoComissaoId) is not DefinicaoComissoes definicaoComissao)
            {
                throw new ArgumentNullException(nameof(definicaoComissaoId));
            }

            if (definicaoComissao.Unidades == null)
            {
                definicaoComissao.Unidades = new List<Unidades>();
            }

            if (UnidadesIdsToAdd != null && UnidadesIdsToAdd.Any())
            {
                var unidadesToAdd = unidadesRepository.EncontrarUnidadesPorIDs(UnidadesIdsToAdd);

                foreach (var unidade in unidadesToAdd)
                {
                    definicaoComissao.Unidades.Add(unidade);
                }
            }

            if (unidadesIdsToRemove != null && unidadesIdsToRemove.Any())
            {
                var unidadesToRemove = unidadesRepository.EncontrarUnidadesPorIDs(unidadesIdsToRemove);

                foreach (var unidade in unidadesToRemove)
                {
                    definicaoComissao.Unidades.Remove(unidade);
                }
            }

            await definicaoComissoesRepository.AtualizarDefinicaoComissaoAsync(definicaoComissao);
        }
    }
}