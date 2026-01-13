using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.TaskScheduler.Interfaces;
using ZDatabase.Exceptions;
using ZDatabase.Interfaces;
using ZDatabase.Validations;
using ZTaskScheduler.Entities;

namespace Niten.System.Core.Repositories.TaskScheduler
{
    /// <inheritdoc />
    public class TriggersRepository : ITriggersRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="TriggersRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        public TriggersRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task AtualizarTriggerAsync(Triggers trigger)
        {
            try
            {
                await ValidarAsync(trigger);
                dbContext.Set<Triggers>().Update(trigger);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao atualizar o trigger.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(trigger), trigger },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<Triggers?> EncontrarTriggerPorIDAsync(long triggerID)
        {
            try
            {
                return await dbContext.FindAsync<Triggers>(triggerID);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao encontrar o trigger pelo ID.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(triggerID), triggerID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task ExcluirTriggerAsync(long triggerID)
        {
            try
            {
                if (await EncontrarTriggerPorIDAsync(triggerID) is not Triggers trigger)
                {
                    throw new EntityNotFoundException<Triggers>(triggerID);
                }

                trigger.IsDeleted = true;
                dbContext.Set<Triggers>().Update(trigger);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao excluir o trigger.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(triggerID), triggerID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task InserirNovoTriggerAsync(Triggers trigger)
        {
            try
            {
                await ValidarAsync(trigger);
                await dbContext.Set<Triggers>().AddAsync(trigger);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao inserir novo trigger.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(trigger), trigger },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<Triggers> ObterTodosTriggers(long jobID)
        {
            try
            {
                return from t in dbContext.Set<Triggers>()
                       where t.JobID == jobID
                       select t;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter todos os triggers.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(jobID), jobID },
                    }
                );
                throw;
            }
        }
        #endregion

        #region Private methods
        private async Task ValidarAsync(Triggers trigger)
        {
            ValidationResult result = new();

            // ActivatesOn
            if (trigger.ActivatesOn == default)
            {
                result.SetError(nameof(Triggers.ActivatesOn), "required");
            }

            // DaysOfWeek
            if (string.IsNullOrWhiteSpace(trigger.DaysOfWeek))
            {
                result.SetError(nameof(Triggers.DaysOfWeek), "required");
            }
            else if (trigger.DaysOfWeek.Length != 7)
            {
                result.SetError(nameof(Triggers.DaysOfWeek), "invalid");
            }

            // IntervalQuantity
            if (trigger.IntervalQuantity <= 0)
            {
                result.SetError(nameof(Triggers.IntervalQuantity), "min");
            }

            // JobID
            if (await dbContext.FindAsync<Jobs>(trigger.JobID) is null)
            {
                result.SetError(nameof(Triggers.JobID), "required");
            }

            result.ValidateEntityErrors(trigger);
        }
        #endregion
    }
}