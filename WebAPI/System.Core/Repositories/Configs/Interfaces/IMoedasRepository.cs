using Niten.Core.Entities.Configs;

namespace Niten.System.Core.Repositories.Configs.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="Niten.Core.Entities.Configs.Moedas"/>.
    /// </summary>
    public interface IMoedasRepository
    {
        /// <summary>
        /// Atualiza a moeda de forma assíncrona.
        /// </summary>
        /// <param name="moeda">A moeda.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task AtualizarMoedaAsync(Moedas moeda);

        /// <summary>
        /// Encontra a moeda pelo ID de forma assíncrona.
        /// </summary>
        /// <param name="moedaID">O ID da moeda.</param>
        /// <returns>A moeda, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<Moedas?> EncontrarMoedaPorIDAsync(long moedaID);

        /// <summary>
        /// Exclui a moeda de forma assíncrona.
        /// </summary>
        /// <param name="moedaID">O ID da moeda.</param>
        Task ExcluirMoedaAsync(long moedaID);

        /// <summary>
        /// Insere uma nova moeda de forma assíncrona.
        /// </summary>
        /// <param name="moeda">A moeda.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">Quando o ID informado for inválido.</exception>
        Task InserirNovaMoedaAsync(Moedas moeda);

        /// <summary>
        /// Obtêm todas as moedas.
        /// </summary>
        /// <returns>Query com as moedas.</returns>
        IQueryable<Moedas> ObterTodasMoedas();
    }
}