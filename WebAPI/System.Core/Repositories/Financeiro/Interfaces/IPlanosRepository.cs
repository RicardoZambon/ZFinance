using Niten.Core.Entities.Financeiro;

namespace Niten.System.Core.Repositories.Financeiro.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="Planos"/>.
    /// </summary>
    public interface IPlanosRepository
    {
        /// <summary>
        /// Atualiza o plano de forma assíncrona.
        /// </summary>
        /// <param name="plano">O plano.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task AtualizarPlanoAsync(Planos plano);

        /// <summary>
        /// Encontra o plano pelo ID de forma assíncrona.
        /// </summary>
        /// <param name="planoID">O ID do plano.</param>
        /// <returns>O plano, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<Planos?> EncontrarPlanoPorIDAsync(long planoID);

        /// <summary>
        /// Exclui o plano de forma assíncrona.
        /// </summary>
        /// <param name="planoID">O ID do plano.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">Quando o ID informado for inválido.</exception>
        Task ExcluirPlanoAsync(long planoID);

        /// <summary>
        /// Insere um novo plano de forma assíncrona.
        /// </summary>
        /// <param name="plano">O plano.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task InserirNovoPlanoAsync(Planos plano);

        /// <summary>
        /// Obtêm todos os pagamentos online.
        /// </summary>
        /// <returns>Query com os pagamentos online.</returns>
        IQueryable<Planos> ObterTodosPlanos();
    }
}