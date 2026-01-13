using Niten.Core.Entities.Geral;

namespace Niten.System.Core.Repositories.Geral.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="Modalidades"/>.
    /// </summary>
    public interface IModalidadesRepository
    {
        /// <summary>
        /// Atualiza a modalidade de forma assíncrona.
        /// </summary>
        /// <param name="modalidade">A modalidade.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task AtualizarModalidadeAsync(Modalidades modalidade);

        /// <summary>
        /// Encontra a modalidade pelo ID de forma assíncrona.
        /// </summary>
        /// <param name="modalidadeID">O ID da modalidade.</param>
        /// <returns>A modalidade, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<Modalidades?> EncontrarModalidadePorIDAsync(long modalidadeID);

        /// <summary>
        /// Exclui a modalidade de forma assíncrona.
        /// </summary>
        /// <param name="modalidadeID">O ID da modalidade.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">Quando o ID informado for inválido.</exception>
        /// <exception cref="InvalidOperationException">Quando a modalidade estiver sendo referenciada pelas unidades.</exception>
        Task ExcluirModalidadeAsync(long modalidadeID);

        /// <summary>
        /// Insere uma nova modalidade de forma assíncrona.
        /// </summary>
        /// <param name="modalidade">A modalidade.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task InserirNovaModalidadeAsync(Modalidades modalidade);

        /// <summary>
        /// Obtêm todas as modalidades.
        /// </summary>
        /// <returns>Query com as modalidades.</returns>
        IQueryable<Modalidades> ObterTodasModalidades();
    }
}