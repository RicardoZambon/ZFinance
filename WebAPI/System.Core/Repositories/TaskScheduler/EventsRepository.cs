using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.TaskScheduler.Interfaces;
using ZDatabase.Interfaces;
using ZTaskScheduler.Entities;

namespace Niten.System.Core.Repositories.TaskScheduler
{
    /// <inheritdoc />
    public class EventsRepository : IEventsRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="EventsRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        public EventsRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task<Events?> EncontrarEventoPorIDAsync(long eventoID)
        {
            try
            {
                return await dbContext.FindAsync<Events>(eventoID);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao encontrar o evento pelo ID.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(eventoID), eventoID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task InserirNovoEventoAsync(Events vevento)
        {
            try
            {
                await dbContext.Set<Events>().AddAsync(vevento);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao inserir novo evento.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(vevento), vevento },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<Events> ObterTodosEventos(long jobID)
        {
            try
            {
                return from e in dbContext.Set<Events>()
                       where e.JobID == jobID
                       select e;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter todos os eventos.");
                throw;
            }
        }
        #endregion

        #region Private methods
        #endregion
    }
}