using Niten.Core.Entities.Configs;

namespace Niten.System.Core.Repositories.Configs.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="Niten.Core.Entities.Configs.Recorrencias"/>.
    /// </summary>
    public interface IRecorrenciasRepository
    {
        /// <summary>
        /// Atualiza a recorrência de forma assíncrona.
        /// </summary>
        /// <param name="recorrencia">A recorrência.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task AtualizarRecorrenciaAsync(Recorrencias recorrencia);

        /// <summary>
        /// Encontra a recorrência pelo ID de forma assíncrona.
        /// </summary>
        /// <param name="recorrenciaID">O ID da recorrência.</param>
        /// <returns>A recorrência, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<Recorrencias?> EncontrarRecorrenciaPorIDAsync(long recorrenciaID);

        /// <summary>
        /// Exclui a recorrência de forma assíncrona.
        /// </summary>
        /// <param name="recorrenciaID">O ID da recorrência.</param>
        Task ExcluirRecorrenciaAsync(long recorrenciaID);

        /// <summary>
        /// Insere uma nova recorrência de forma assíncrona.
        /// </summary>
        /// <param name="recorrencia">A recorrência.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">Quando o ID informado for inválido.</exception>
        Task InserirNovaRecorrenciaAsync(Recorrencias recorrencia);

        /// <summary>
        /// Obtêm todas as recorrências.
        /// </summary>
        /// <returns>Query com as recorrências.</returns>
        IQueryable<Recorrencias> ObterTodasRecorrencias();
    }
}