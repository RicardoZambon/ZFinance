using Niten.Core.Entities.Configs;

namespace Niten.System.Core.Repositories.Configs.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="Niten.Core.Entities.Configs.TiposContratacoes"/>.
    /// </summary>
    public interface ITiposContratacoesRepository
    {
        /// <summary>
        /// Atualiza o tipo de contratação de forma assíncrona.
        /// </summary>
        /// <param name="tipoContratacao">O tipo de contratação.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task AtualizarTipoContratacaoAsync(TiposContratacoes tipoContratacao);

        /// <summary>
        /// Encontra o tipo de contratação pelo ID de forma assíncrona.
        /// </summary>
        /// <param name="tipoContratacaoID">O ID do tipo de contratação.</param>
        /// <returns>O tipo de contratação, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<TiposContratacoes?> EncontrarTipoContratacaoPorIDAsync(long tipoContratacaoID);

        /// <summary>
        /// Exclui o tipo de contratação de forma assíncrona.
        /// </summary>
        /// <param name="tipoContratacaoID">O ID do tipo de contratação.</param>
        Task ExcluirTipoContratacaoAsync(long tipoContratacaoID);

        /// <summary>
        /// Insere um novo tipo de contratação de forma assíncrona.
        /// </summary>
        /// <param name="tipoContratacao">O tipo de contratação.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">Quando o ID informado for inválido.</exception>
        Task InserirNovoTipoContratacaoAsync(TiposContratacoes tipoContratacao);

        /// <summary>
        /// Obtêm todos os tipos de contratações.
        /// </summary>
        /// <returns>Query com os tipos de contratações.</returns>
        IQueryable<TiposContratacoes> ObterTodosTiposContratacoes();
    }
}