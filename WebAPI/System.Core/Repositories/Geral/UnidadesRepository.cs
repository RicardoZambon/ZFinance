using Microsoft.EntityFrameworkCore;
using Niten.Core.Entities.Configs;
using Niten.Core.Entities.Geral;
using Niten.Core.Entities.Integracoes;
using Niten.Core.Enums;
using Niten.Core.Repositories.Geral;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.Geral.Interfaces;
using ZDatabase.Exceptions;
using ZDatabase.Interfaces;
using ZDatabase.Validations;

namespace Niten.System.Core.Repositories.Geral
{
    /// <inheritdoc />
    public class UnidadesRepository : UnidadesRepositoryCore, IUnidadesRepository
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="UnidadesRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        public UnidadesRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
            : base(dbContext, exceptionHandler)
        {
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task AtualizarUnidadeAsync(Unidades unidade)
        {
            try
            {
                await ValidarAsync(unidade);
                dbContext.Set<Unidades>().Update(unidade);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao atualizar a unidade.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(unidade), unidade },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<Unidades> EncontrarUnidadesPorIDs(IEnumerable<int> unidadesIDs)
        {
            try
            {
                return dbContext.Set<Unidades>().Where(x => unidadesIDs.Contains(x.ID));
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao encontrar as unidades pelos IDs.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(unidadesIDs), unidadesIDs },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task ExcluirUnidadeAsync(int unidadeID)
        {
            try
            {
                if (await EncontrarUnidadePorIDAsync(unidadeID) is not Unidades unidade)
                {
                    throw new EntityNotFoundException<Unidades>(unidadeID);
                }

                unidade.Status = UnidadesStatus.Excluida;
                dbContext.Set<Unidades>().Update(unidade);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao excluir a unidade.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(unidadeID), unidadeID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task InserirNovaUnidadeAsync(Unidades unidade)
        {
            try
            {
                await ValidarAsync(unidade);
                await dbContext.Set<Unidades>().AddAsync(unidade);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao inserir nova unidade.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(unidade), unidade },
                    }
                );
                throw;
            }
        }
        #endregion

        #region Private methods
        private async Task ValidarAsync(Unidades unidade)
        {
            ValidationResult result = new();

            // Bandeira
            if (string.IsNullOrWhiteSpace(unidade.Bandeira))
            {
                result.SetError(nameof(Unidades.Bandeira), "required");
            }

            // DataInicioComissoes
            if (unidade.GerarComissoes && unidade.DataInicioComissoes is null)
            {
                result.SetError(nameof(Unidades.DataInicioComissoes), "required");
            }

            // Idioma
            if (string.IsNullOrWhiteSpace(unidade.Idioma))
            {
                result.SetError(nameof(Unidades.Idioma), "required");
            }

            // Moeda
            if (string.IsNullOrWhiteSpace(unidade.Moeda))
            {
                result.SetError(nameof(Unidades.Moeda), "required");
            }

            // NomeFantasia
            if (string.IsNullOrWhiteSpace(unidade.NomeFantasia))
            {
                result.SetError(nameof(Unidades.NomeFantasia), "required");
            }

            // NomeSystem
            if (string.IsNullOrWhiteSpace(unidade.NomeSystem))
            {
                result.SetError(nameof(Unidades.NomeSystem), "required");
            }
            else if (await dbContext.Set<Unidades>().AnyAsync(x => EF.Functions.Like(x.NomeSystem, unidade.NomeSystem) && x.ID != unidade.ID))
            {
                result.SetError(nameof(Unidades.NomeSystem), "exists");
            }

            // Ordem
            if (unidade.Ordem < 1)
            {
                result.SetError(nameof(Unidades.Ordem), "min");
            }

            // PerfilID
            if (unidade.PerfilPadraoID is not null && await dbContext.FindAsync<Perfis>(unidade.PerfilPadraoID) is null)
            {
                result.SetError(nameof(Unidades.PerfilPadraoID), "invalid");
            }

            // PeriodicidadeAtestadosMedicos
            if (unidade.PeriodicidadeAtestadosMedicos is int periodicidade && periodicidade < 1)
            {
                result.SetError(nameof(Unidades.PeriodicidadeAtestadosMedicos), "min");
            }

            // Sigla
            if (string.IsNullOrWhiteSpace(unidade.Sigla))
            {
                result.SetError(nameof(Unidades.Sigla), "required");
            }

            // TipoContratacaoMatriculasID
            if (unidade.Status == UnidadesStatus.Ativa && await dbContext.FindAsync<TiposContratacoes>(unidade.TipoContratacaoMatriculasID) is null)
            {
                result.SetError(nameof(Unidades.TipoContratacaoMatriculasID), "required");
            }

            // URLHotsite
            if (string.IsNullOrWhiteSpace(unidade.URLHotsite))
            {
                result.SetError(nameof(Unidades.URLHotsite), "required");
            }

            if (result.HasErrors)
            {
                throw new EntityValidationFailureException<int>(nameof(Unidades), unidade.ID, result);
            }
        }
        #endregion
    }
}