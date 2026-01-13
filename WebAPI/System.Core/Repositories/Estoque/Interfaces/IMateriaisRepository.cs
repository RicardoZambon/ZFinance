using Niten.Core.Entities.Estoque;

namespace Niten.System.Core.Repositories.Estoque.Interfaces
{
    /// <summary>
    /// Repositório para gerenciamento dos materiais.
    /// </summary>
    public interface IMateriaisRepository
    {
        /// <summary>
        /// Encontra um material pelo ID do material pai e pelo campo extra de forma assíncrona.
        /// </summary>
        /// <param name="materialPaiID">O ID do material pai.</param>
        /// <param name="extra">O campo extra do material.</param>
        /// <returns>O material, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<Materiais?> EncontrarMaterialPorMaterialPaiIDExtraAsync(int materialPaiID, string extra);

        /// <summary>
        /// Encontra um material pelo ID do material pai e pelo nome de forma assíncrona.
        /// </summary>
        /// <param name="materialPaiID">O ID do material pai.</param>
        /// <param name="nome">O nome do material.</param>
        /// <returns>O material, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<Materiais?> EncontrarMaterialPorMaterialPaiIDNomeAsync(int materialPaiID, string nome);

        /// <summary>
        /// Insere um novo material de forma assíncrona.
        /// </summary>
        /// <param name="material">O material.</param>
        /// <returns></returns>
        Task InserirNovoMaterialAsync(Materiais material);

        /// <summary>
        /// Obtêm todos os materiais pelo ID do material pai.
        /// </summary>
        /// <param name="materialPaiID">O ID do material pai.</param>
        /// <returns>Query com todos os materiais.</returns>
        IQueryable<Materiais> ObterTodosMateriaisPorMaterialPaiID(int materialPaiID);
    }
}