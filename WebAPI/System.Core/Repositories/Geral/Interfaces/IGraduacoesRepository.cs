using Niten.Core.Entities.Geral;

namespace Niten.System.Core.Repositories.Geral.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="Graduacoes"/>.
    /// </summary>
    public interface IGraduacoesRepository
    {
        /// <summary>
        /// Atualiza a graduação de forma assíncrona.
        /// </summary>
        /// <param name="graduacao">A graduação.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task AtualizarGraduacaoAsync(Graduacoes graduacao);

        /// <summary>
        /// Encontra a graduação pelo ID de forma assíncrona.
        /// </summary>
        /// <param name="graduacaoID">O ID da graduação.</param>
        /// <returns>A graduação, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<Graduacoes?> EncontrarGraduacaoPorIDAsync(long graduacaoID);

        /// <summary>
        /// Exclui a graduação de forma assíncrona.
        /// </summary>
        /// <param name="graduacaoID">O ID da graduação.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">Quando o ID informado for inválido.</exception>
        /// <exception cref="InvalidOperationException">Quando a graduação estiver sendo referenciada pelas unidades.</exception>
        Task ExcluirGraduacaoAsync(long graduacaoID);

        /// <summary>
        /// Insere uma nova graduação de forma assíncrona.
        /// </summary>
        /// <param name="graduacao">A graduação.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task InserirNovaGraduacaoAsync(Graduacoes graduacao);

        /// <summary>
        /// Obtêm todas as graduações pela modalidade.
        /// </summary>
        /// <param name="modalidadeID">O ID da modalidade.</param>
        /// <returns>Query com as graduações da modalidade.</returns>
        IQueryable<Graduacoes> ObterTodasGraduacoesPorModalidade(long modalidadeID);
    }
}