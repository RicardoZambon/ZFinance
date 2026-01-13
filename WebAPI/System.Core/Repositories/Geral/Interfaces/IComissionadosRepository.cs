using Niten.Core.Entities.Geral;

namespace Niten.System.Core.Repositories.Geral.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="Comissionados"/>.
    /// </summary>
    public interface IComissionadosRepository
    {
        /// <summary>
        /// Atualiza o comissionado de forma assíncrona.
        /// </summary>
        /// <param name="comissionado">O comissionado.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task AtualizarComissionadoAsync(Comissionados comissionado);

        /// <summary>
        /// Encontra o comissionado pelo ID de forma assíncrona.
        /// </summary>
        /// <param name="comissionadoID">O ID do comissionado.</param>
        /// <returns>O comissionado, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<Comissionados?> EncontrarComissionadoPorIDAsync(long comissionadoID);

        /// <summary>
        /// Exclui o comissionado de forma assíncrona.
        /// </summary>
        /// <param name="comissionadoID">O ID do comissionado.</param>
        Task ExcluirComissionadoAsync(long comissionadoID);

        /// <summary>
        /// Insere um novo comissionado de forma assíncrona.
        /// </summary>
        /// <param name="comissionado">O comissionado.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">Quando o ID informado for inválido.</exception>
        Task InserirNovoComissionadoAsync(Comissionados comissionado);

        /// <summary>
        /// Obtêm todos os comissionados.
        /// </summary>
        /// <returns>Query com os comissionados.</returns>
        IQueryable<Comissionados> ObterTodosComissionados();
    }
}