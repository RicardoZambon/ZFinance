using ZTaskScheduler.Entities;

namespace Niten.System.Core.Repositories.TaskScheduler.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="Triggers"/>.
    /// </summary>
    public interface ITriggersRepository
    {
        /// <summary>
        /// Atualiza o trigger de forma assíncrona.
        /// </summary>
        /// <param name="trigger">O trigger.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task AtualizarTriggerAsync(Triggers trigger);

        /// <summary>
        /// Encontra o trigger pelo ID de forma assíncrona.
        /// </summary>
        /// <param name="triggerID">O ID do trigger.</param>
        /// <returns>O trigger, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<Triggers?> EncontrarTriggerPorIDAsync(long triggerID);

        /// <summary>
        /// Exclui o trigger de forma assíncrona.
        /// </summary>
        /// <param name="triggerID">O ID do trigger.</param>
        Task ExcluirTriggerAsync(long triggerID);

        /// <summary>
        /// Insere um novo trigger de forma assíncrona.
        /// </summary>
        /// <param name="trigger">O trigger.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">Quando o ID informado for inválido.</exception>
        Task InserirNovoTriggerAsync(Triggers trigger);

        /// <summary>
        /// Obtêm todos os triggers.
        /// </summary>
        /// <param name="jobID">O ID do job.</param>
        /// <returns>Query com os triggers.</returns>
        IQueryable<Triggers> ObterTodosTriggers(long jobID);
    }
}