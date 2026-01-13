using Niten.Core.Entities.PortalAluno;

namespace Niten.System.Core.Repositories.PortalAluno.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="PostagensMural"/>.
    /// </summary>
    public interface IPostagensMuralRepository
    {
        /// <summary>
        /// Atualiza a postagem de forma assíncrona.
        /// </summary>
        /// <param name="postagemMural">A postagem.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task AtualizarPostagemMuralAsync(PostagensMural postagemMural);

        /// <summary>
        /// Encontra a postagem pelo ID de forma assíncrona.
        /// </summary>
        /// <param name="postagemMuralID">O ID da postagem.</param>
        /// <returns>A postagem, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<PostagensMural?> EncontrarPostagemMuralPorIDAsync(long postagemMuralID);

        /// <summary>
        /// Exclui a postagem de forma assíncrona.
        /// </summary>
        /// <param name="postagemMuralID">O ID da postagem.</param>
        Task ExcluirPostagemMuralAsync(long postagemMuralID);

        /// <summary>
        /// Insere uma nova postagem de forma assíncrona.
        /// </summary>
        /// <param name="postagemMural">A postagem.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">Quando o ID informado for inválido.</exception>
        Task InserirNovaPostagemMuralAsync(PostagensMural postagemMural);

        /// <summary>
        /// Obtêm todas as postagens.
        /// </summary>
        /// <returns>Query com as postagens.</returns>
        IQueryable<PostagensMural> ObterTodasPostagensMural();
    }
}