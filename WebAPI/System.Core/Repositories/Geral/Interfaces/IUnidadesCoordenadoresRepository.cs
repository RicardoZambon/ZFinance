using Niten.Core.Entities.Geral;

namespace Niten.System.Core.Repositories.Geral.Interfaces
{
    /// <summary>
    /// Repositório para a entidade <see cref="UnidadesCoordenadores"/>.
    /// </summary>
    public interface IUnidadesCoordenadoresRepository
    {
        /// <summary>
        /// Atualiza o relacionamento de unidade com o coordenador de forma assíncrona.
        /// </summary>
        /// <param name="unidadeCoordenador">O relacionamento de unidade com o coordenador.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task AtualizarUnidadeCoordenadorAsync(UnidadesCoordenadores unidadeCoordenador);

        /// <summary>
        /// Encontra o relacionamento de unidade com o coordenador pelo ID de forma assíncrona.
        /// </summary>
        /// <param name="unidadeCoordenadorID">O ID do relacionamento de unidade com o coordenador.</param>
        /// <returns>O relacionamento de unidade com o coordenador, se encontrar; caso contrário, <c>null</c>.</returns>
        Task<UnidadesCoordenadores?> EncontrarUnidadeCoordenadorPorIDAsync(long unidadeCoordenadorID);

        /// <summary>
        /// Encontra o percentual de rateio pelo ID da unidade e ID do cadastro de forma assíncrona.
        /// </summary>
        /// <param name="unidadeID">O ID da unidade.</param>
        /// <param name="cadastroID">O ID do cadastro.</param>
        /// <returns>O percentual de rateio.</returns>
        Task<int> EncontrarPercentualPorUnidadeIDCadastroIDAsync(int unidadeID, int? cadastroID);

        /// <summary>
        /// Exclui o relacionamento de unidade com o coordenador de forma assíncrona.
        /// </summary>
        /// <param name="unidadeCoordenadorID">O ID do relacionamento de unidade com o coordenador.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityNotFoundException{TEntity}">Quando o ID informado for inválido.</exception>
        Task ExcluirUnidadeCoordenadorAsync(long unidadeCoordenadorID);

        /// <summary>
        /// Insere um novo relacionamento de unidade com o coordenador de forma assíncrona.
        /// </summary>
        /// <param name="unidadeCoordenador">O relacionamento de unidade com o coordenador.</param>
        /// <exception cref="ZDatabase.Exceptions.EntityValidationFailureException{TKey}">Quando houver uma mais falhas de validação dos dados.</exception>
        Task InserirNovoUnidadeCoordenadorAsync(UnidadesCoordenadores unidadeCoordenador);

        /// <summary>
        /// Obtêm todos os relacionamentos de unidade com os coordenadores.
        /// </summary>
        /// <param name="unidadeID">O ID da unidade.</param>
        /// <returns>Query com os relacionamentos de unidade com os coordenadores.</returns>
        IQueryable<UnidadesCoordenadores> ObterTodosUnidadesCoordenadores(int unidadeID);
    }
}