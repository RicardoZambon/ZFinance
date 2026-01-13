using Niten.Core.Entities.PortalAluno;

namespace Niten.System.Core.Repositories.PortalAluno.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="PostagensMuralTraducoes"/>.
    /// </summary>
    public interface IPostagensMuralTraducoesRepository
    {
        /// <summary>
        /// Atualiza a tradução da postagem de forma assíncrona.
        /// </summary>
        /// <param name="postagemMuralTraducao">A tradução da postagem.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task AtualizarPostagemMuralTraducaoAsync(PostagensMuralTraducoes postagemMuralTraducao);

        /// <summary>
        /// Encontra a tradução da postagem pelo ID de forma assíncrona.
        /// </summary>
        /// <param name="postagemMuralTraducaoID">O ID da tradução da postagem.</param>
        /// <returns>A tradução da postagem, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<PostagensMuralTraducoes?> EncontrarPostagemMuralTraducaoPorIDAsync(long postagemMuralTraducaoID);

        /// <summary>
        /// Exclui a tradução da postagem de forma assíncrona.
        /// </summary>
        /// <param name="postagemMuralTraducaoID">O ID da tradução da postagem.</param>
        Task ExcluirPostagemMuralTraducaoAsync(long postagemMuralTraducaoID);

        /// <summary>
        /// Insere uma nova tradução da postagem de forma assíncrona.
        /// </summary>
        /// <param name="postagemMuralTraducao">A tradução da postagem.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">Quando o ID informado for inválido.</exception>
        Task InserirNovaPostagemMuralTraducaoAsync(PostagensMuralTraducoes postagemMuralTraducao);

        /// <summary>
        /// Obtêm todas as traduções a partir do ID da postagem.
        /// </summary>
        /// <param name="postagemMuralID">O ID da postagem.</param>
        /// <returns>Query com as traduções da postagem.</returns>
        IQueryable<PostagensMuralTraducoes> ObterTodasTraducoesPorPostagemMural(long postagemMuralID);
    }
}