using Niten.Core.Entities.Configs;
using Niten.Core.Entities.Financeiro;
using Niten.Core.Entities.Geral;

namespace Niten.System.Core.Repositories.Financeiro.Interfaces
{
    /// <summary>
    /// Repositório para <see cref="Niten.Core.Entities.Financeiro.Comissoes"/>.
    /// </summary>
    public interface IComissoesRepository
    {
        /// <summary>
        /// Atualiza uma comissão de forma assíncrona.
        /// </summary>
        /// <param name="comissao">A comissão.</param>
        Task AtualizarComissaoAsync(Comissoes comissao);

        /// <summary>
        /// Encontra uma comissão pelo ID de forma assíncrona.
        /// </summary>
        /// <param name="comissaoID">O ID da comissão.</param>
        /// <returns>A comissão, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<Comissoes?> EncontrarComissaoPorIDAsync(long comissaoID);

        /// <summary>
        /// Encontra uma comissão com status <see cref="Niten.Core.Enums.ComissaoStatus.Aberto"/> pela Unidade, Cadastro, Definição de comissão, e Percentual de rateio de forma assíncrona.
        /// </summary>
        /// <param name="unidadeId">O ID da unidade.</param>
        /// <param name="cadastroId">O ID do cadastro.</param>
        /// <param name="definicaoComissao">A definição de comissão.</param>
        /// <param name="percentualRateio">O percentual de rateio.</param>
        /// <returns>A comissão, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<Comissoes?> EncontrarComissaoAberta(int unidadeId, int? cadastroId, DefinicaoComissoes definicaoComissao, int percentualRateio);

        /// <summary>
        /// Insere uma nova comissão de forma assíncrona.
        /// </summary>
        /// <param name="definicaoComissao">A definição de comissão.</param>
        /// <param name="unidade">A unidade.</param>
        /// <param name="cadastro">O cadastro.</param>
        /// <returns>A comissão inserida.</returns>
        Task<Comissoes> InserirNovaComissaoAsync(DefinicaoComissoes definicaoComissao, Unidades unidade, Cadastros? cadastro);

        /// <summary>
        /// Obtêm todas as comissões.
        /// </summary>
        /// <returns>Query com todas as comissões.</returns>
        IQueryable<Comissoes> ObterTodasComissoes();
    }
}