using Microsoft.EntityFrameworkCore;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.TaskScheduler.Interfaces;
using ZDatabase.Exceptions;
using ZDatabase.Interfaces;
using ZDatabase.Validations;
using ZTaskScheduler.Entities;

namespace Niten.System.Core.Repositories.TaskScheduler
{
    /// <inheritdoc />
    public class JobsRepository : IJobsRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="JobsRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        public JobsRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task AtualizarJobAsync(Jobs job)
        {
            try
            {
                await ValidarAsync(job);
                dbContext.Set<Jobs>().Update(job);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao atualizar o job.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(job), job },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<Jobs?> EncontrarJobPorIDAsync(long jobID)
        {
            try
            {
                return await dbContext.FindAsync<Jobs>(jobID);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao encontrar o job pelo ID.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(jobID), jobID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task ExcluirJobAsync(long jobID)
        {
            try
            {
                if (await EncontrarJobPorIDAsync(jobID) is not Jobs job)
                {
                    throw new EntityNotFoundException<Jobs>(jobID);
                }

                job.IsDeleted = true;
                dbContext.Set<Jobs>().Update(job);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao excluir o job.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(jobID), jobID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task InserirNovoJobAsync(Jobs job)
        {
            try
            {
                await ValidarAsync(job);
                await dbContext.Set<Jobs>().AddAsync(job);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao inserir novo job.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(job), job },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<Jobs> ObterTodosJobs()
        {
            try
            {
                return from j in dbContext.Set<Jobs>()
                       select j;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter todos os jobs.");
                throw;
            }
        }
        #endregion

        #region Private methods
        private async Task ValidarAsync(Jobs job)
        {
            ValidationResult result = new();

            // Name
            if (string.IsNullOrWhiteSpace(job.Name))
            {
                result.SetError(nameof(Jobs.Name), "required");
            }
            else if (await dbContext.Set<Jobs>().AnyAsync(x => EF.Functions.Like(x.Name!, job.Name) && x.ID != job.ID))
            {
                result.SetError(nameof(Jobs.Name), "exists");
            }

            result.ValidateEntityErrors(job);
        }
        #endregion
    }
}