using Niten.Core.Entities.Configs;

namespace Niten.System.Core.Repositories.Configs.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="CamposFiltrosOpcoes"/>.
    /// </summary>
    public interface ICamposFiltrosOpcoesRepository
    {
        /// <summary>
        /// Atualiza a opção do campo de filtro de forma assíncrona.
        /// </summary>
        /// <param name="opcao">A opção do campo de filtro.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma ou mais falhas de validação dos dados.</exception>
        Task AtualizarOpcaoAsync(CamposFiltrosOpcoes opcao);

        /// <summary>
        /// Encontra a opção pelo ID de forma assíncrona.
        /// </summary>
        /// <param name="opcaoID">O ID da opção.</param>
        /// <returns>A opção, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<CamposFiltrosOpcoes?> EncontrarOpcaoPorIDAsync(long opcaoID);

        /// <summary>
        /// Exclui a opção de forma assíncrona.
        /// </summary>
        /// <param name="opcaoID">O ID da opção.</param>
        Task ExcluirOpcaoAsync(long opcaoID);

        /// <summary>
        /// Insere uma nova opção de forma assíncrona.
        /// </summary>
        /// <param name="opcao">A opção do campo de filtro.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma ou mais falhas de validação dos dados.</exception>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">Quando o ID informado for inválido.</exception>
        Task InserirNovaOpcaoAsync(CamposFiltrosOpcoes opcao);

        /// <summary>
        /// Obtém todas as opções do campo de filtro.
        /// </summary>
        /// <param name="campoFiltroID">O ID do campo de filtro.</param>
        /// <returns>Query com as opções do campo de filtro.</returns>
        IQueryable<CamposFiltrosOpcoes> ObterTodasOpcoes(long campoFiltroID);
    }
}