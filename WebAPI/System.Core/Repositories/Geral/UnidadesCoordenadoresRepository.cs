using Microsoft.EntityFrameworkCore;
using Niten.Core.Entities.Geral;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.Geral.Interfaces;
using ZDatabase.Exceptions;
using ZDatabase.Interfaces;
using ZDatabase.Validations;

namespace Niten.System.Core.Repositories.Geral
{
    /// <inheritdoc />
    public class UnidadesCoordenadoresRepository : IUnidadesCoordenadoresRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        private readonly IUnidadesRepository unidadesRepository;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="UnidadesCoordenadoresRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        /// <param name="unidadesRepository">The <see cref="IUnidadesRepository"/> instance.</param>
        public UnidadesCoordenadoresRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler,
            IUnidadesRepository unidadesRepository)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
            this.unidadesRepository = unidadesRepository;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task AtualizarUnidadeCoordenadorAsync(UnidadesCoordenadores unidadeCoordenador)
        {
            try
            {
                await ValidarAsync(unidadeCoordenador);
                GarantirIntegridade(unidadeCoordenador);
                dbContext.Set<UnidadesCoordenadores>().Update(unidadeCoordenador);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao atualizar o relacionamento da unidade com o coordenador.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(unidadeCoordenador), unidadeCoordenador },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<UnidadesCoordenadores?> EncontrarUnidadeCoordenadorPorIDAsync(long unidadeCoordenadorID)
        {
            try
            {
                return await dbContext.FindAsync<UnidadesCoordenadores>(unidadeCoordenadorID);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao encontrar o relacionamento da unidade com o coordenador pelo ID.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(unidadeCoordenadorID), unidadeCoordenadorID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<int> EncontrarPercentualPorUnidadeIDCadastroIDAsync(int unidadeID, int? cadastroID)
        {
            try
            {
                if (await dbContext.Set<UnidadesCoordenadores>().FirstOrDefaultAsync(x => x.UnidadeID == unidadeID && x.CadastroID == cadastroID && x.RecebeComissao) is UnidadesCoordenadores unidadeCoordenador)
                {
                    return unidadeCoordenador.Percentual ?? 0;
                }
                else if (await unidadesRepository.EncontrarUnidadePorIDAsync(unidadeID) is Unidades unidade)
                {
                    return 100 - (unidade.Coordenadores?.Sum(x => x.Percentual ?? 0) ?? 0);
                }
                return 0;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao encontrar o percentual de ratio pelo ID da unidade e ID do cadastro.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(unidadeID), unidadeID },
                        { nameof(cadastroID), cadastroID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task ExcluirUnidadeCoordenadorAsync(long unidadeCoordenadorID)
        {
            try
            {
                if (await EncontrarUnidadeCoordenadorPorIDAsync(unidadeCoordenadorID) is not UnidadesCoordenadores unidadeCoordenador)
                {
                    throw new EntityNotFoundException<UnidadesCoordenadores>(unidadeCoordenadorID);
                }

                unidadeCoordenador.IsDeleted = true;
                dbContext.Set<UnidadesCoordenadores>().Update(unidadeCoordenador);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao excluir o relacionamento da unidade com o coordenador.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(unidadeCoordenadorID), unidadeCoordenadorID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task InserirNovoUnidadeCoordenadorAsync(UnidadesCoordenadores unidadeCoordenador)
        {
            try
            {
                await ValidarAsync(unidadeCoordenador);
                GarantirIntegridade(unidadeCoordenador);
                await dbContext.Set<UnidadesCoordenadores>().AddAsync(unidadeCoordenador);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao inserir novo relacionamento da unidade com o coordenador.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(unidadeCoordenador), unidadeCoordenador },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<UnidadesCoordenadores> ObterTodosUnidadesCoordenadores(int unidadeID)
        {
            try
            {
                return from uc in dbContext.Set<UnidadesCoordenadores>()
                       where uc.UnidadeID == unidadeID
                       select uc;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter todos os relacionamento da unidade com os coordenadores.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(unidadeID), unidadeID },
                    }
                );
                throw;
            }
        }
        #endregion

        #region Private methods
        private void GarantirIntegridade(UnidadesCoordenadores unidadeCoordenador)
        {
            if (!unidadeCoordenador.RecebeComissao)
            {
                unidadeCoordenador.Percentual = null;
            }
        }

        private async Task ValidarAsync(UnidadesCoordenadores unidadeCoordenador)
        {
            ValidationResult result = new();

            // CadastroID
            if (await dbContext.Set<Cadastros>().FindAsync(unidadeCoordenador.CadastroID) is null)
            {
                result.SetError(nameof(UnidadesCoordenadores.CadastroID), "required");
            }
            else if (await dbContext.Set<UnidadesCoordenadores>().AnyAsync(x => x.UnidadeID == unidadeCoordenador.UnidadeID && x.CadastroID == unidadeCoordenador.CadastroID && x.ID != unidadeCoordenador.ID))
            {
                result.SetError(nameof(UnidadesCoordenadores.CadastroID), "exists");
            }

            // Percentual
            if (unidadeCoordenador.Percentual <= 0)
            {
                result.SetError(nameof(UnidadesCoordenadores.Percentual), "min");
            }
            else if (unidadeCoordenador.Percentual > 100)
            {
                result.SetError(nameof(UnidadesCoordenadores.Percentual), "max");
            }

            // UnidadeID
            if (await dbContext.Set<Unidades>().FindAsync(unidadeCoordenador.UnidadeID) is null)
            {
                result.SetError(nameof(UnidadesCoordenadores.UnidadeID), "required");
            }

            result.ValidateEntityErrors(unidadeCoordenador);
        }
        #endregion
    }
}