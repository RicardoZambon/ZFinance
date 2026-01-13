using Niten.Core.Entities.Financeiro;

namespace Niten.System.Core.Repositories.Financeiro.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="ContratacoesAnexos"/>.
    /// </summary>
    public interface IContratacoesAnexosRepository
    {
        /// <summary>
        /// Atualiza o anexo de contratação de forma assíncrona.
        /// </summary>
        /// <param name="contratacaoAnexo">O aneox de contratação.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task AtualizarContratacaoAnexoAsync(ContratacoesAnexos contratacaoAnexo);

        /// <summary>
        /// Encontra o anexo de contratação pelo ID de forma assíncrona.
        /// </summary>
        /// <param name="contratacaoAnexoID">O ID do anexo de contratação.</param>
        /// <returns>O anexo de contratação, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<ContratacoesAnexos?> EncontrarContratacaoAnexoPorIDAsync(long contratacaoAnexoID);

        /// <summary>
        /// Exclui o anexo de contratação de forma assíncrona.
        /// </summary>
        /// <param name="contratacaoID">O ID da contratação.</param>
        /// <param name="contratacaoAnexoID">O ID do anexo de contratação.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">Quando o ID informado for inválido.</exception>
        Task ExcluirContratacaoAnexoAsync(long contratacaoID, long contratacaoAnexoID);

        /// <summary>
        /// Insere um novo anexo de contratação de forma assíncrona.
        /// </summary>
        /// <param name="contratacaoAnexo">O anexo de contratação.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task InserirNovoContratacaoAnexoAsync(ContratacoesAnexos contratacaoAnexo);

        /// <summary>
        /// Obtêm todos os anexos de contratações.
        /// </summary>
        /// <param name="contratacaoID">O ID da contratação.</param>
        /// <returns>Query com os anexos de contratações.</returns>
        IQueryable<ContratacoesAnexos> ObterTodosContratacoesAnexos(long contratacaoID);
    }
}