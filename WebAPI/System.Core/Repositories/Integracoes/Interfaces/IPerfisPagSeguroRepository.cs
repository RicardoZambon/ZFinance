using Niten.Core.Entities.Integracoes;

namespace Niten.System.Core.Repositories.Integracoes.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="PerfisPagSeguro"/>.
    /// </summary>
    public interface IPerfisPagSeguroRepository
    {
        /// <summary>
        /// Atualiza o perfil do PagSeguro de forma assíncrona.
        /// </summary>
        /// <param name="perfilPagSeguro">O perfil do PagSeguro.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task AtualizarPerfilPagSeguroAsync(PerfisPagSeguro perfilPagSeguro);

        /// <summary>
        /// Encontra o perfil do PagSeguro pelo ID de forma assíncrona.
        /// </summary>
        /// <param name="perfilPagSeguroID">O ID do perfil do PagSeguro.</param>
        /// <returns>O perfil do PagSeguro, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<PerfisPagSeguro?> EncontrarPerfilPagSeguroPorIDAsync(long perfilPagSeguroID);

        /// <summary>
        /// Insere um novo oerfil do PagSeguro de forma assíncrona.
        /// </summary>
        /// <param name="perfilPagSeguro">O perfil do PagSeguro.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">Quando o ID informado for inválido.</exception>
        Task InserirNovoPerfilPagSeguroAsync(PerfisPagSeguro perfilPagSeguro);

        /// <summary>
        /// Obtêm todos os perfis do PagSeguro.
        /// </summary>
        /// <returns>Query com os perfis do PagSeguro.</returns>
        IQueryable<PerfisPagSeguro> ObterTodosPerfisPagSeguro();
    }
}