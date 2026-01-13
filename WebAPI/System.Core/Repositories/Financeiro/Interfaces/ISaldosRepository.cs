using Niten.Core.Entities.Financeiro;

namespace Niten.System.Core.Repositories.Financeiro.Interfaces
{
    /// <summary>
    /// Serviço para gerenciamento das <see cref="Niten.Core.Entities.Financeiro.Saldos"/>
    /// </summary>
    public interface ISaldosRepository
    {
        /// <summary>
        /// Encontra o último saldo pelo ID do cadastro de forma assíncrona.
        /// </summary>
        /// <param name="cadastroID">O ID do cadastro.</param>
        /// <returns>O saldo, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<Saldos?> EncontrarUltimoSaldoDoCadastro(int cadastroID);

        /// <summary>
        /// Encontra o último saldo pelo ID da unidade de forma assíncrona.
        /// </summary>
        /// <param name="unidadeID">O ID da unidade.</param>
        /// <returns>O saldo, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<Saldos?> EncontrarUltimoSaldoDaUnidade(int unidadeID);

        /// <summary>
        /// Insere um novo saldo de forma assíncrona.
        /// </summary>
        /// <param name="saldo">O saldo.</param>
        /// <exception cref="InvalidOperationException">Quando houver falha ao obter a chave de lock para o saldo.</exception>
        /// <returns>A chave de lock gerada.</returns>
        Task<string> InserirNovoSaldoAsync(Saldos saldo);

        /// <summary>
        /// Libera qualquer lock que estiver aberto para a chave.
        /// </summary>
        /// <param name="chaveLock">A chave de lock.</param>
        void LiberarLock(string chaveLock);

        /// <summary>
        /// Obtêm os saldos pelo ID do cadastro.
        /// </summary>
        /// <param name="cadastroID">O ID do cadastro.</param>
        /// <returns>Query com os saldos do cadastro.</returns>
        IQueryable<Saldos> ObterSaldosPorCadastroID(int cadastroID);

        /// <summary>
        /// Obtêm os saldos pelo ID da unidade.
        /// </summary>
        /// <param name="unidadeID">O ID da uniadde.</param>
        /// <returns>Query com os saldos da unidade.</returns>
        IQueryable<Saldos> ObterSaldosPorUnidadeID(int unidadeID);
    }
}