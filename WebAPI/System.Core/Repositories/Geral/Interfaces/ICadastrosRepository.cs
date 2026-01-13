using Niten.Core.Entities.Geral;
using Niten.Core.Repositories.Geral.Interfaces;

namespace Niten.System.Core.Repositories.Geral.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="Cadastros"/>.
    /// </summary>
    public interface ICadastrosRepository : ICadastrosRepositoryCore
    {
        /// <summary>
        /// Obtêm todos os cadastros.
        /// </summary>
        /// <returns>Query com os cadastros.</returns>
        IQueryable<Cadastros> ObterTodosCadastros();
    }
}