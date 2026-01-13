using Niten.Core.Entities.Financeiro;

namespace Niten.System.Core.Repositories.Financeiro.Interfaces
{
    /// <summary>
    /// Repositório para gerenciamento dos modificadores das tabelas de valores.
    /// </summary>
    public interface ITabelasValoresModificadoresRepository
    {
        /// <summary>
        /// Atualiza o modificador da tabela de valores de forma assíncrona.
        /// </summary>
        /// <param name="tabelaValoresModificador">O modificador da tabela de valores.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task AtualizarTabelaValoresModificadorAsync(TabelasValoresModificadores tabelaValoresModificador);

        /// <summary>
        /// Encontra o modificador da tabela de valores pelo ID de forma assíncrona.
        /// </summary>
        /// <param name="tabelaValoresModificadorID">O ID do modificador da tabela de valores.</param>
        /// <returns>O modificador da tabela de valores, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<TabelasValoresModificadores?> EncontrarTabelaValoresModificadorPorIDAsync(long tabelaValoresModificadorID);

        /// <summary>
        /// Exclui o modificador da tabela de valores de forma assíncrona.
        /// </summary>
        /// <param name="tabelaValoresModificadorID">O ID do modificador da tabela de valores.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">Quando o ID informado for inválido.</exception>
        Task ExcluirTabelaValoresModificadorAsync(long tabelaValoresModificadorID);

        /// <summary>
        /// Insere um novo modificador da tabela de valores de forma assíncrona.
        /// </summary>
        /// <param name="tabelaValoresModificador">O modificador da tabela de valores.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task InserirNovaTabelaValoresModificadorAsync(TabelasValoresModificadores tabelaValoresModificador);

        /// <summary>
        /// Obtêm todos os modificadores de materiais da tabela de valores.
        /// </summary>
        /// <param name="tabelaValoresID">O ID da tabela de valor.</param>
        /// <returns>Query com os modificadores da tabela de valores.</returns>
        IQueryable<TabelasValoresModificadores> ObterTodosTabelaValoresModificadoresPorTabelaValores(long tabelaValoresID);

        /// <summary>
        /// Obtêm todos os modificadores de materiais da tabela de valores.
        /// </summary>
        /// <param name="unidadeID">O ID da unidade.</param>
        /// <returns>Query com os modificadores da tabela de valores.</returns>
        IQueryable<TabelasValoresModificadores> ObterTodosTabelaValoresModificadoresPorUnidade(long unidadeID);
    }
}