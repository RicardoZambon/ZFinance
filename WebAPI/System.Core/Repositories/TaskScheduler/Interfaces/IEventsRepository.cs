using ZTaskScheduler.Entities;

namespace Niten.System.Core.Repositories.TaskScheduler.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="Events"/>.
    /// </summary>
    public interface IEventsRepository
    {
        /// <summary>
        /// Encontra o evento pelo ID de forma assíncrona.
        /// </summary>
        /// <param name="eventoID">O ID do evento.</param>
        /// <returns>O evento, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<Events?> EncontrarEventoPorIDAsync(long eventoID);

        /// <summary>
        /// Insere um novo evento de forma assíncrona.
        /// </summary>
        /// <param name="evento">O evento.</param>
        Task InserirNovoEventoAsync(Events evento);

        /// <summary>
        /// Obtêm todos os eventos.
        /// </summary>
        /// <param name="jobID">O ID do job.</param>
        /// <returns>Query com os eventos.</returns>
        IQueryable<Events> ObterTodosEventos(long jobID);
    }
}