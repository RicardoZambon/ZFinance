using Niten.Core.Entities.Financeiro;

namespace Niten.System.Core.Repositories.Financeiro.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="NotasFiscais"/>.
    /// </summary>
    public interface INotasFiscaisRepository
    {
        /// <summary>
        /// Encontra uma nota fiscal pelo ID de forma assíncrona.
        /// </summary>
        /// <param name="notaFiscalID">O ID da nota fiscal.</param>
        /// <returns>A nota fiscal, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<NotasFiscais?> EncontrarNotaFiscalPorIDAsync(int notaFiscalID);
    }
}