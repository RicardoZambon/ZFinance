using Niten.Core.Entities.Financeiro;

namespace Niten.System.Core.Repositories.Financeiro.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="Niten.Core.Entities.Financeiro.TabelasValoresMateriais"/>.
    /// </summary>
    public interface ITabelasValoresMateriaisRepository
    {
        /// <summary>
        /// Atualiza o material da tabela de valores de forma assíncrona.
        /// </summary>
        /// <param name="tabelaValoresMaterial">O material da tabela de valores.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task AtualizarTabelaValoresMaterialAsync(TabelasValoresMateriais tabelaValoresMaterial);

        /// <summary>
        /// Encontra o material da tabela de valores pelo ID de forma assíncrona.
        /// </summary>
        /// <param name="tabelaValoresMaterialID">O ID do material da tabela de valores.</param>
        /// <returns>O material da tabela de valores, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<TabelasValoresMateriais?> EncontrarTabelaValoresMaterialPorIDAsync(long tabelaValoresMaterialID);

        /// <summary>
        /// Exclui o material da tabela de valores de forma assíncrona.
        /// </summary>
        /// <param name="tabelaValoresMaterialID">O ID do material da tabela de valores.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">Quando o ID informado for inválido.</exception>
        Task ExcluirTabelaValoresMaterialAsync(long tabelaValoresMaterialID);

        /// <summary>
        /// Insere um novo material da tabela de valores de forma assíncrona.
        /// </summary>
        /// <param name="tabelaValoresMaterial">O material da tabela de valores.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task InserirNovaTabelaValoresMaterialAsync(TabelasValoresMateriais tabelaValoresMaterial);

        /// <summary>
        /// Obtêm todos os materiais da tabela de valores pelo ID da tabela de valores.
        /// </summary>
        /// <param name="tabelaValoresID">O ID da tabela de valores.</param>
        /// <returns>Query com os materiais da tabela de valores.</returns>
        IQueryable<TabelasValoresMateriais> ObterTodosTabelaValoresMateriaisPorTabelaValores(long tabelaValoresID);

        /// <summary>
        /// Obtêm todos os materiais da tabela de valores pelo ID da unidade.
        /// </summary>
        /// <param name="unidadeID">O ID da unidade.</param>
        /// <returns>Query com os materiais da unidade.</returns>
        IQueryable<TabelasValoresMateriais> ObterTodosTabelaValoresMateriaisPorUnidade(int unidadeID);
    }
}