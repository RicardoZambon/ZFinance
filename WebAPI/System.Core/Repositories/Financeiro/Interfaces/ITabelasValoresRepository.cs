using Niten.Core.Entities.Financeiro;

namespace Niten.System.Core.Repositories.Financeiro
{
    /// <summary>
    /// Repositório para a entidade <see cref="Niten.Core.Entities.Financeiro.TabelasValores"/>.
    /// </summary>
    public interface ITabelasValoresRepository
    {
        /// <summary>
        /// Atualiza a tabela de valores de forma assíncrona.
        /// </summary>
        /// <param name="tabelaValores">A tabela de valores.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task AtualizarTabelaValoresAsync(TabelasValores tabelaValores);

        /// <summary>
        /// Encontra a tabela de valores pelo ID de forma assíncrona.
        /// </summary>
        /// <param name="tabelaValoresID">O ID da tabela de valores.</param>
        /// <returns>A tabela de valores, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<TabelasValores?> EncontrarTabelaValoresPorIDAsync(long tabelaValoresID);

        /// <summary>
        /// Exclui a tabela de valores de forma assíncrona.
        /// </summary>
        /// <param name="tabelaValoresID">O ID da tabela de valores.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">Quando o ID informado for inválido.</exception>
        /// <exception cref="InvalidOperationException">Quando a tabela de valores estiver sendo referenciada pelas unidades.</exception>
        Task ExcluirTabelaValoresAsync(long tabelaValoresID);

        /// <summary>
        /// Insere uma nova tabela de valores de forma assíncrona.
        /// </summary>
        /// <param name="tabelaValores">A tabela de valores.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task InserirNovaTabelaValoresAsync(TabelasValores tabelaValores);

        /// <summary>
        /// Obtêm todas as tabelas de valores.
        /// </summary>
        /// <returns>Query com as tabelas de valores.</returns>
        IQueryable<TabelasValores> ObterTodasTabelasValores();
    }
}