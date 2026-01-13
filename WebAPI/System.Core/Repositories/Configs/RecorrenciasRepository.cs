using Microsoft.EntityFrameworkCore;
using Niten.Core.Entities.Configs;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.Configs.Interfaces;
using ZDatabase.Exceptions;
using ZDatabase.Interfaces;
using ZDatabase.Validations;

namespace Niten.System.Core.Repositories.Configs
{
    /// <inheritdoc />
    public class RecorrenciasRepository : IRecorrenciasRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RecorrenciasRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        public RecorrenciasRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task AtualizarRecorrenciaAsync(Recorrencias recorrencia)
        {
            try
            {
                await ValidarAsync(recorrencia);
                dbContext.Set<Recorrencias>().Update(recorrencia);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao atualizar a recorrência.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(recorrencia), recorrencia },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<Recorrencias?> EncontrarRecorrenciaPorIDAsync(long recorrenciaID)
        {
            try
            {
                return await dbContext.FindAsync<Recorrencias>(recorrenciaID);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao encontrar a recorrência pelo ID.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(recorrenciaID), recorrenciaID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task ExcluirRecorrenciaAsync(long recorrenciaID)
        {
            try
            {
                if (await EncontrarRecorrenciaPorIDAsync(recorrenciaID) is not Recorrencias recorrencia)
                {
                    throw new EntityNotFoundException<Recorrencias>(recorrenciaID);
                }

                recorrencia.IsDeleted = true;
                dbContext.Set<Recorrencias>().Update(recorrencia);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao excluir a recorrência.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(recorrenciaID), recorrenciaID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task InserirNovaRecorrenciaAsync(Recorrencias recorrencia)
        {
            try
            {
                await ValidarAsync(recorrencia);
                await dbContext.Set<Recorrencias>().AddAsync(recorrencia);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao inserir nova recorrência.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(recorrencia), recorrencia },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<Recorrencias> ObterTodasRecorrencias()
        {
            try
            {
                return from r in dbContext.Set<Recorrencias>()
                       select r;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter todas as recorrências.");
                throw;
            }
        }
        #endregion

        #region Private methods
        private async Task ValidarAsync(Recorrencias recorrencia)
        {
            ValidationResult result = new();

            // IntervaloMeses
            if (recorrencia.IntervaloMeses <= 0)
            {
                result.SetError(nameof(Recorrencias.IntervaloMeses), "min");
            }
            else if (await dbContext.Set<Recorrencias>().AnyAsync(x => x.IntervaloMeses == recorrencia.IntervaloMeses && x.ID != recorrencia.ID))
            {
                result.SetError(nameof(Recorrencias.IntervaloMeses), "exists");
            }

            // Nome
            if (string.IsNullOrWhiteSpace(recorrencia.Nome))
            {
                result.SetError(nameof(Recorrencias.Nome), "required");
            }
            else if (await dbContext.Set<Recorrencias>().AnyAsync(x => EF.Functions.Like(x.Nome!, recorrencia.Nome) && x.ID != recorrencia.ID))
            {
                result.SetError(nameof(Recorrencias.Nome), "exists");
            }

            result.ValidateEntityErrors(recorrencia);
        }
        #endregion
    }
}