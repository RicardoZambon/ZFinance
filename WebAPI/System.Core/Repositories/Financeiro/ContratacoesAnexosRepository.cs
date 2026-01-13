using Niten.Core.Entities.Financeiro;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.Financeiro.Interfaces;
using ZDatabase.Exceptions;
using ZDatabase.Interfaces;
using ZDatabase.Validations;

namespace Niten.System.Core.Repositories.Financeiro
{
    /// <inheritdoc />
    public class ContratacoesAnexosRepository : IContratacoesAnexosRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ContratacoesAnexosRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        public ContratacoesAnexosRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task AtualizarContratacaoAnexoAsync(ContratacoesAnexos contratacaoAnexo)
        {
            try
            {
                await ValidarAsync(contratacaoAnexo);
                dbContext.Set<ContratacoesAnexos>().Update(contratacaoAnexo);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao atualizar o anexo de contratação.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(contratacaoAnexo), contratacaoAnexo },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<ContratacoesAnexos?> EncontrarContratacaoAnexoPorIDAsync(long contratacaoAnexoID)
        {
            try
            {
                return await dbContext.FindAsync<ContratacoesAnexos>(contratacaoAnexoID);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao encontrar o anexo de contratação pelo ID.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(contratacaoAnexoID), contratacaoAnexoID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task ExcluirContratacaoAnexoAsync(long contratacaoID, long contratacaoAnexoID)
        {
            try
            {
                if (await EncontrarContratacaoAnexoPorIDAsync(contratacaoAnexoID) is not ContratacoesAnexos contratacaoAnexo
                    || contratacaoAnexo.ContratacaoID != contratacaoID)
                {
                    throw new EntityNotFoundException<ContratacoesAnexos>(contratacaoAnexoID);
                }

                contratacaoAnexo.IsDeleted = true;
                dbContext.Set<ContratacoesAnexos>().Update(contratacaoAnexo);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao excluir o anexo de contratação.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(contratacaoAnexoID), contratacaoAnexoID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task InserirNovoContratacaoAnexoAsync(ContratacoesAnexos contratacaoAnexo)
        {
            try
            {
                await ValidarAsync(contratacaoAnexo);
                await dbContext.Set<ContratacoesAnexos>().AddAsync(contratacaoAnexo);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao inserir novo anexo de contratação.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(contratacaoAnexo), contratacaoAnexo },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<ContratacoesAnexos> ObterTodosContratacoesAnexos(long contratacaoID)
        {
            try
            {
                return from ca in dbContext.Set<ContratacoesAnexos>()
                       where ca.ContratacaoID == contratacaoID
                       select ca;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter todos os anexos de contratações.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(contratacaoID), contratacaoID },
                    }
                );
                throw;
            }
        }
        #endregion

        #region Private methods
        private async Task ValidarAsync(ContratacoesAnexos contratacaoAnexo)
        {
            ValidationResult result = new();

            // ContratacaoID
            if (await dbContext.FindAsync<Contratacoes>(contratacaoAnexo.ContratacaoID) is null)
            {
                result.SetError(nameof(ContratacoesAnexos.ContratacaoID), "required");
            }

            result.ValidateEntityErrors(contratacaoAnexo);
        }
        #endregion
    }
}