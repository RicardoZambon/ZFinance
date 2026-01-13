using Niten.Core.Entities.Financeiro;

namespace Niten.System.Core.Repositories.Financeiro.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="Niten.Core.Entities.Financeiro.Contratacoes"/>.
    /// </summary>
    public interface IContratacoesRepository
    {
        /// <summary>
        /// Atualiza a contratação de forma assíncrona.
        /// </summary>
        /// <param name="contratacao">A contratação.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task AtualizarContratacaoAsync(Contratacoes contratacao);

        /// <summary>
        /// Encontra a contratação ativa pelo ID do tipo de contratação e ID do cadastro.
        /// </summary>
        /// <param name="tipoContratacaoID">O ID do tipo de contratação.</param>
        /// <param name="cadastroID">O ID do cadastro.</param>
        /// <returns>A contratação, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<Contratacoes?> EncontrarContratacaoAtivaPorTipoContratacaoIDCadastroIDAsync(long tipoContratacaoID, int cadastroID);

        /// <summary>
        /// Encontra a contratação pelo ID de forma assíncrona.
        /// </summary>
        /// <param name="contratacaoID">O ID da contratação.</param>
        /// <returns>A contratação, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<Contratacoes?> EncontrarContratacaoPorIDAsync(long contratacaoID);

        /// <summary>
        /// Exclui a contratação de forma assíncrona.
        /// </summary>
        /// <param name="contratacaoID">O ID da contratação.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">Quando o ID informado for inválido.</exception>
        Task ExcluirContratacaoAsync(long contratacaoID);

        /// <summary>
        /// Insere uma nova contratação de forma assíncrona.
        /// </summary>
        /// <param name="contratacao">A contratação.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task InserirNovaContratacaoAsync(Contratacoes contratacao);

        /// <summary>
        /// Obtêm todos as contratações.
        /// </summary>
        /// <returns>Query com as contratações.</returns>
        IQueryable<Contratacoes> ObterTodasContratacoes();

        /// <summary>
        /// Processa as cobranças para a contratação de forma assíncrona.
        /// </summary>
        /// <param name="contratacao">A contratação.</param>
        /// <param name="competenciaInicioOriginal">A competência inicial original.</param>
        Task ProcessarCobrancasAsync(Contratacoes contratacao, DateTime? competenciaInicioOriginal = null);
    }
}