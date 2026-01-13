using Niten.Core.Entities.Geral;
using Niten.Core.Repositories.Geral.Interfaces;

namespace Niten.System.Core.Repositories.Geral.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="Unidades"/>.
    /// </summary>
    public interface IUnidadesRepository : IUnidadesRepositoryCore
    {
        /// <summary>
        /// Atualiza a unidade de forma assíncrona.
        /// </summary>
        /// <param name="unidade">A unidade.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task AtualizarUnidadeAsync(Unidades unidade);

        /// <summary>
        /// Encontra as unidades pelos IDs de forma assíncrona.
        /// </summary>
        /// <param name="unidadesIDs">Os IDs das unidades.</param>
        /// <returns>Query com as unidades encontradas.</returns>
        IQueryable<Unidades> EncontrarUnidadesPorIDs(IEnumerable<int> unidadesIDs);

        /// <summary>
        /// Exclui a unidade de forma assíncrona.
        /// </summary>
        /// <param name="unidadeID">O ID da unidade.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">Quando o ID informado for inválido.</exception>
        Task ExcluirUnidadeAsync(int unidadeID);

        /// <summary>
        /// Insere uma nova unidade de forma assíncrona.
        /// </summary>
        /// <param name="unidade">A unidade.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task InserirNovaUnidadeAsync(Unidades unidade);
    }
}