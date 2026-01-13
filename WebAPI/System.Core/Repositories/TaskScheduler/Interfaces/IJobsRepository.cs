using ZTaskScheduler.Entities;

namespace Niten.System.Core.Repositories.TaskScheduler.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="Jobs"/>.
    /// </summary>
    public interface IJobsRepository
    {
        /// <summary>
        /// Atualiza o job de forma assíncrona.
        /// </summary>
        /// <param name="job">O job.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task AtualizarJobAsync(Jobs job);

        /// <summary>
        /// Encontra o job pelo ID de forma assíncrona.
        /// </summary>
        /// <param name="jobID">O ID do job.</param>
        /// <returns>O job, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<Jobs?> EncontrarJobPorIDAsync(long jobID);

        /// <summary>
        /// Exclui o job de forma assíncrona.
        /// </summary>
        /// <param name="jobID">O ID do job.</param>
        Task ExcluirJobAsync(long jobID);

        /// <summary>
        /// Insere um novo job de forma assíncrona.
        /// </summary>
        /// <param name="job">O job.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">Quando o ID informado for inválido.</exception>
        Task InserirNovoJobAsync(Jobs job);

        /// <summary>
        /// Obtêm todos os jobs.
        /// </summary>
        /// <returns>Query com os jobs.</returns>
        IQueryable<Jobs> ObterTodosJobs();
    }
}