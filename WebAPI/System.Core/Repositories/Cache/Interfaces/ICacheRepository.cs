namespace Niten.System.Core.Repositories.Cache.Interfaces
{
    /// <summary>
    /// Repositório para as entidades de <see cref="Niten.Core.Entities.Cache"/>.
    /// </summary>
    public interface ICacheRepository
    {
        /// <summary>
        /// Atualiza o cache dos pagamentos de forma assíncrona.
        /// </summary>
        /// <param name="cadastroID">O ID do cadastro.</param>
        /// <param name="pagamentoOnlineID">O ID do pagamento online.</param>
        Task AtualizarCachePagamentosAsync(int? cadastroID = null, long? pagamentoOnlineID = null);

        /// <summary>
        /// Atualiza o cache das tabelas de valores de forma assíncrona.
        /// </summary>
        Task AtualizarCacheTabelasValoresAsync();
    }
}
