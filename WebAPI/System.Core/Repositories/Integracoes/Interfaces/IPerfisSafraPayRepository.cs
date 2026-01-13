using Niten.Core.Entities.Integracoes;

namespace Niten.System.Core.Repositories.Integracoes.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="PerfisSafraPay"/>.
    /// </summary>
    public interface IPerfisSafraPayRepository
    {
        /// <summary>
        /// Atualiza o perfil do SafraPay de forma assíncrona.
        /// </summary>
        /// <param name="perfilSafraPay">O perfil do SafraPay.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task AtualizarPerfilSafraPayAsync(PerfisSafraPay perfilSafraPay);

        /// <summary>
        /// Encontra o perfil do SafraPay pelo ID de forma assíncrona.
        /// </summary>
        /// <param name="perfilSafraPayID">O ID do perfil do SafraPay.</param>
        /// <returns>O perfil do SafraPay, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<PerfisSafraPay?> EncontrarPerfilSafraPayPorIDAsync(long perfilSafraPayID);

        /// <summary>
        /// Insere um novo oerfil do SafraPay de forma assíncrona.
        /// </summary>
        /// <param name="perfilSafraPay">O perfil do SafraPay.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">Quando o ID informado for inválido.</exception>
        Task InserirNovoPerfilSafraPayAsync(PerfisSafraPay perfilSafraPay);

        /// <summary>
        /// Obtêm todos os perfis do SafraPay.
        /// </summary>
        /// <returns>Query com os perfis do SafraPay.</returns>
        IQueryable<PerfisSafraPay> ObterTodosPerfisSafraPay();
    }
}