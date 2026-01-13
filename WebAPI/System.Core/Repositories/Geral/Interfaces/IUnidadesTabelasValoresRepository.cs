using Niten.Core.Entities.Geral;

namespace Niten.System.Core.Repositories.Geral.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="UnidadesTabelasValores"/>.
    /// </summary>
    public interface IUnidadesTabelasValoresRepository
    {
        /// <summary>
        /// Atualiza o relacionamento de unidade com a tabela de valores de forma assíncrona.
        /// </summary>
        /// <param name="unidadeTabelaValores">O relacionamento de unidade com a tabela de valores.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task AtualizarUnidadeTabelaValoresAsync(UnidadesTabelasValores unidadeTabelaValores);

        /// <summary>
        /// Encontra o relacionamento de unidade com a tabela de valores pelo ID de forma assíncrona.
        /// </summary>
        /// <param name="unidadeTabelaValoresID">O ID do relacionamento de unidade com a tabela de valores.</param>
        /// <returns>O relacionamento de unidade com a tabela de valores, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<UnidadesTabelasValores?> EncontrarUnidadeTabelaValoresPorIDAsync(long unidadeTabelaValoresID);

        /// <summary>
        /// Exclui o relacionamento de unidade com a tabela de valores de forma assíncrona.
        /// </summary>
        /// <param name="unidadeTabelaValoresID">O ID do relacionamento de unidade com a tabela de valores.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">Quando o ID informado for inválido.</exception>
        Task ExcluirUnidadeTabelaValoresAsync(long unidadeTabelaValoresID);

        /// <summary>
        /// Insere um novo relacionamento de unidade com a tabela de valores de forma assíncrona.
        /// </summary>
        /// <param name="unidadeTabelaValores">O relacionamento de unidade com a tabela de valores.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task InserirNovaUnidadeTabelaValoresAsync(UnidadesTabelasValores unidadeTabelaValores);

        /// <summary>
        /// Obtêm todos os relacionamentos de unidade com as tabelas de valores.
        /// </summary>
        /// <param name="unidadeID">O ID da unidade.</param>
        /// <returns>Query com os relacionamentos de unidade com as tabelas de valores.</returns>
        IQueryable<UnidadesTabelasValores> ObterTodasUnidadesTabelasValores(int unidadeID);
    }
}