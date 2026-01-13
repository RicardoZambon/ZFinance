using Niten.Core.Entities.Configs;

namespace Niten.System.Core.Repositories.Configs.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="Idiomas"/>.
    /// </summary>
    public interface IIdiomasRepository
    {
        /// <summary>
        /// Atualiza o idioma de forma assíncrona.
        /// </summary>
        /// <param name="idioma">O idioma.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task AtualizarIdiomaAsync(Idiomas idioma);

        /// <summary>
        /// Encontra o idioma pelo ID de forma assíncrona.
        /// </summary>
        /// <param name="idiomaID">O ID do idioma.</param>
        /// <returns>O idioma, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<Idiomas?> EncontrarIdiomaPorIDAsync(long idiomaID);

        /// <summary>
        /// Exclui o idioma de forma assíncrona.
        /// </summary>
        /// <param name="idiomaID">O ID do idioma.</param>
        Task ExcluirIdiomaAsync(long idiomaID);

        /// <summary>
        /// Insere um nova idioma de forma assíncrona.
        /// </summary>
        /// <param name="idioma">O idioma.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">Quando o ID informado for inválido.</exception>
        Task InserirNovoIdiomaAsync(Idiomas idioma);

        /// <summary>
        /// Obtêm todos os idiomas.
        /// </summary>
        /// <returns>Query com os idiomas.</returns>
        IQueryable<Idiomas> ObterTodosIdiomas();
    }
}