using Microsoft.EntityFrameworkCore;
using Niten.Core.Entities.Estoque;
using Niten.Core.Entities.Geral;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.Geral.Interfaces;
using ZDatabase.Exceptions;
using ZDatabase.Interfaces;
using ZDatabase.Validations;

namespace Niten.System.Core.Repositories.Geral
{
    /// <inheritdoc />
    public class GraduacoesRepository : IGraduacoesRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="GraduacoesRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        public GraduacoesRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task AtualizarGraduacaoAsync(Graduacoes graduacao)
        {
            try
            {
                await ValidarAsync(graduacao);
                dbContext.Set<Graduacoes>().Update(graduacao);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao atualizar a graduação.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(graduacao), graduacao },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<Graduacoes?> EncontrarGraduacaoPorIDAsync(long graduacaoID)
        {
            try
            {
                return await dbContext.FindAsync<Graduacoes>(graduacaoID);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao encontrar a graduação pelo ID.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(graduacaoID), graduacaoID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task ExcluirGraduacaoAsync(long graduacaoID)
        {
            try
            {
                if (await EncontrarGraduacaoPorIDAsync(graduacaoID) is not Graduacoes graduacao)
                {
                    throw new EntityNotFoundException<Graduacoes>(graduacaoID);
                }

                graduacao.IsDeleted = true;
                dbContext.Set<Graduacoes>().Update(graduacao);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao excluir a graduação.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(graduacaoID), graduacaoID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task InserirNovaGraduacaoAsync(Graduacoes graduacao)
        {
            try
            {
                await ValidarAsync(graduacao);
                await dbContext.Set<Graduacoes>().AddAsync(graduacao);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao inserir nova graduação.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(graduacao), graduacao },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<Graduacoes> ObterTodasGraduacoesPorModalidade(long modalidadeID)
        {
            try
            {
                return from g in dbContext.Set<Graduacoes>()
                       where g.ModalidadeID == modalidadeID
                       orderby g.Ordem ascending, g.Descricao ascending
                       select g;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro ao obter todas as graduações pela modalidade.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(modalidadeID), modalidadeID },
                    }
                );
                throw;
            }
        }
        #endregion

        #region Private methods
        private async Task ValidarAsync(Graduacoes graduacao)
        {
            ValidationResult result = new();

            // CodigoREI
            if (graduacao.CodigoREI is null)
            {
                result.SetError(nameof(Graduacoes.CodigoREI), "required");
            }

            // Descricao
            if (string.IsNullOrWhiteSpace(graduacao.Descricao))
            {
                result.SetError(nameof(Graduacoes.Descricao), "required");
            }
            else if (await dbContext.Set<Graduacoes>().AnyAsync(x => EF.Functions.Like(x.Descricao!, graduacao.Descricao) && x.ModalidadeID == graduacao.ModalidadeID && x.ID != graduacao.ID))
            {
                result.SetError(nameof(Graduacoes.Descricao), "exists");
            }

            // MaterialID
            if (graduacao.MaterialID is not null && await dbContext.FindAsync<Materiais>(graduacao.MaterialID) is null)
            {
                result.SetError(nameof(Graduacoes.MaterialID), "invalid");
            }

            // ModalidadeID
            if (await dbContext.FindAsync<Modalidades>(graduacao.ModalidadeID) is null)
            {
                result.SetError(nameof(Graduacoes.ModalidadeID), "required");
            }

            // Ordem
            if (graduacao.Ordem < 0)
            {
                result.SetError(nameof(Graduacoes.Ordem), "min");
            }

            // TempoMedio
            if (graduacao.TempoMedio < 0)
            {
                result.SetError(nameof(Graduacoes.TempoMedio), "min");
            }

            result.ValidateEntityErrors(graduacao);
        }
        #endregion
    }
}