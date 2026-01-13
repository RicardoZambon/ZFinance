using Niten.Core.Entities.Seguranca;

namespace Niten.System.Core.Repositories.Seguranca.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="Actions"/>.
    /// </summary>
    public interface IActionsRepository
    {
        /// <summary>
        /// Atualiza a action de forma assíncrona.
        /// </summary>
        /// <param name="action">A action.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task AtualizarActionAsync(Actions action);

        /// <summary>
        /// Encontra a action pelo ID de forma assíncrona.
        /// </summary>
        /// <param name="actionID">O ID da action.</param>
        /// <returns>A action, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<Actions?> EncontrarActionPorIDAsync(long actionID);

        /// <summary>
        /// Exclui a action de forma assíncrona.
        /// </summary>
        /// <param name="actionID">O ID da action.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">Quando o ID informado for inválido.</exception>
        Task ExcluirActionAsync(long actionID);

        /// <summary>
        /// Insere uma nova action de forma assíncrona.
        /// </summary>
        /// <param name="action">A action.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task InserirNovaActionAsync(Actions action);

        /// <summary>
        /// Obtêm todas as actions.
        /// </summary>
        /// <returns>Query com as actions.</returns>
        IQueryable<Actions> ObterTodasActions();
    }
}