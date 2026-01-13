using Niten.Core.Entities.Configs;

namespace Niten.System.Core.Repositories.Configs.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="CamposFiltros"/>.
    /// </summary>
    public interface ICamposFiltrosRepository
    {
        /// <summary>
        /// Atualiza o campo de filtro de forma assíncrona.
        /// </summary>
        /// <param name="campoFiltro">O campo de filtro.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma ou mais falhas de validação dos dados.</exception>
        Task AtualizarCampoFiltroAsync(CamposFiltros campoFiltro);

        /// <summary>
        /// Encontra o campo de filtro pelo ID de forma assíncrona.
        /// </summary>
        /// <param name="campoFiltroID">O ID do campo de filtro.</param>
        /// <returns>O campo de filtro, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<CamposFiltros?> EncontrarCampoFiltroPorIDAsync(long campoFiltroID);

        /// <summary>
        /// Exclui o campo de filtro de forma assíncrona.
        /// </summary>
        /// <param name="campoFiltroID">O ID do campo de filtro.</param>
        Task ExcluirCampoFiltroAsync(long campoFiltroID);

        /// <summary>
        /// Insere um novo campo de filtro de forma assíncrona.
        /// </summary>
        /// <param name="campoFiltro">O campo de filtro.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma ou mais falhas de validação dos dados.</exception>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">Quando o ID informado for inválido.</exception>
        Task InserirNovoCampoFiltroAsync(CamposFiltros campoFiltro);

        /// <summary>
        /// Obtém todos os campos de filtro.
        /// </summary>
        /// <returns>Query com os campos de filtro.</returns>
        IQueryable<CamposFiltros> ObterTodosCamposFiltros();
    }
}