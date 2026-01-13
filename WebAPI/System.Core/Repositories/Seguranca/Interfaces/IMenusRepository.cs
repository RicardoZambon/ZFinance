using Niten.Core.Entities.Seguranca;

namespace Niten.System.Core.Repositories.Seguranca.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="Menus"/>.
    /// </summary>
    public interface IMenusRepository
    {
        /// <summary>
        /// Atualiza o menu de forma assíncrona.
        /// </summary>
        /// <param name="menu">O menu.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task AtualizarMenuAsync(Menus menu);

        /// <summary>
        /// Encontra o menu pelo ID de forma assíncrona.
        /// </summary>
        /// <param name="menuID">O ID do menu.</param>
        /// <returns>O menu, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<Menus?> EncontrarMenuPorIDAsync(long menuID);

        /// <summary>
        /// Encontra o menu pela URL de forma assíncrona.
        /// </summary>
        /// <param name="url">A URL do menu.</param>
        /// <returns>O menu, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<Menus?> EncontrarMenuPorURLAsync(string url);

        /// <summary>
        /// Exclui o menu de forma assíncrona.
        /// </summary>
        /// <param name="menuID">O ID do menu.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">Quando o ID informado for inválido.</exception>
        Task ExcluirMenuAsync(long menuID);

        /// <summary>
        /// Insere um novo menu de forma assíncrona.
        /// </summary>
        /// <param name="menu">O menu.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task InserirNovoMenuAsync(Menus menu);

        /// <summary>
        /// Obtêm apenas os menus permitidos para o usuário em <see cref="System.Core.Services.Interfaces.ISystemCurrentUserProvider"/>.
        /// </summary>
        /// <param name="parentMenuID">O ID do menu pai.</param>
        /// <returns>Lista com os menus.</returns>
        Task<IEnumerable<Menus>> ObterMenusPermitidosAsync(long? parentMenuID = null);

        /// <summary>
        /// Obtêm todos os menus.
        /// </summary>
        /// <returns>Query com os menus.</returns>
        IQueryable<Menus> ObterTodosMenus();
    }
}