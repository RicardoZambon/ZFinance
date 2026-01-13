using Niten.Core.Entities.Configs;
using Niten.Core.Entities.Geral;

namespace Niten.System.Core.Repositories.Configs.Interfaces
{
    /// <summary>
    /// Repositório para <see cref="DefinicaoComissoes"/>.
    /// </summary>
    public interface IDefinicaoComissoesRepository
    {
        /// <summary>
        /// Atualiza a definição de comissão de forma assíncrona.
        /// </summary>
        /// <param name="definicaoComissao">A definição de comissão.</param>
        Task AtualizarDefinicaoComissaoAsync(DefinicaoComissoes definicaoComissao);

        /// <summary>
        /// Encontra uma definição de comissão pelo ID de forma assíncrona.
        /// </summary>
        /// <param name="definicaoComissaoID">O ID da definição de comissão.</param>
        /// <returns>A definição de comissão, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<DefinicaoComissoes?> EncontrarDefinicaoComissaoPorIDAsync(long definicaoComissaoID);

        /// <summary>
        /// Insere uma nova definição de comissão de forma assíncrona.
        /// </summary>
        /// <param name="definicaoComissao">A definição de comissão.</param>
        Task InserirNovaDefinicaoComissaoAsync(DefinicaoComissoes definicaoComissao);

        /// <summary>
        /// Obtêm as definições de comissões pendentes de gerar comissões.
        /// </summary>
        /// <returns>Query com as definições de comissões pendentes de gerar comissões.</returns>
        IQueryable<DefinicaoComissoes> ObterDefinicoesComissoesPendentesDeGerarComissoes();

        /// <summary>
        /// Obtêm todas as definições de comissões.
        /// </summary>
        /// <returns>Query com todas as definições de comissões.</returns>
        IQueryable<DefinicaoComissoes> ObterTodasDefinicoesComissoes();
    }

    public interface IDefinicaoComissoesComissionadosRepository
    {
        Task<int> GetValorRateioAsync(long definicaoComissaoId, int cadastroId);
        Task<IQueryable<Comissionados>> ListComissionados(long definicaoComissaoId);
        Task UpdateComissionadosAsync(long definicaoComissaoId, IEnumerable<long> comissionadosIdsToAdd, IEnumerable<long> comissionadosIdsToRemove);
    }

    public interface IDefinicaoComissoesUnidadesRepository
    {
        Task<IQueryable<Unidades>> ListUnidades(long definicaoComissaoId);
        Task UpdateUnidadesAsync(long definicaoComissaoId, IEnumerable<int> UnidadesIdsToAdd, IEnumerable<int> unidadesIdsToRemove);
    }
}