using Microsoft.EntityFrameworkCore;
using Niten.Core.Entities.Geral;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.Geral.Interfaces;
using ZDatabase.Exceptions;
using ZDatabase.Interfaces;
using ZDatabase.Validations;

namespace Niten.System.Core.Repositories.Geral
{
    /// <inheritdoc />
    public class ModalidadesRepository : IModalidadesRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ModalidadesRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        public ModalidadesRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task AtualizarModalidadeAsync(Modalidades modalidade)
        {
            try
            {
                await ValidarAsync(modalidade);
                dbContext.Set<Modalidades>().Update(modalidade);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao atualizar a modalidade.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(modalidade), modalidade },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<Modalidades?> EncontrarModalidadePorIDAsync(long modalidadeID)
        {
            try
            {
                return await dbContext.FindAsync<Modalidades>(modalidadeID);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao encontrar a modalidade pelo ID.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(modalidadeID), modalidadeID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task ExcluirModalidadeAsync(long modalidadeID)
        {
            try
            {
                if (await EncontrarModalidadePorIDAsync(modalidadeID) is not Modalidades modalidade)
                {
                    throw new EntityNotFoundException<Modalidades>(modalidadeID);
                }

                modalidade.IsDeleted = true;
                dbContext.Set<Modalidades>().Update(modalidade);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao excluir a modalidade.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(modalidadeID), modalidadeID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task InserirNovaModalidadeAsync(Modalidades modalidade)
        {
            try
            {
                await ValidarAsync(modalidade);
                await dbContext.Set<Modalidades>().AddAsync(modalidade);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao inserir nova modalidade.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(modalidade), modalidade },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<Modalidades> ObterTodasModalidades()
        {
            try
            {
                return from m in dbContext.Set<Modalidades>()
                       select m;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro ao obter todas as modalidades.");
                throw;
            }
        }
        #endregion

        #region Private methods
        private async Task ValidarAsync(Modalidades modalidade)
        {
            ValidationResult result = new();

            // CodigoREI
            if (!string.IsNullOrWhiteSpace(modalidade.CodigoREI) && await dbContext.Set<Modalidades>().AnyAsync(x => EF.Functions.Like(x.CodigoREI!, modalidade.CodigoREI) && x.ID != modalidade.ID))
            {
                result.SetError(nameof(Modalidades.CodigoREI), "exists");
            }

            // Descricao
            if (string.IsNullOrWhiteSpace(modalidade.Descricao))
            {
                result.SetError(nameof(Modalidades.Descricao), "required");
            }
            else if (await dbContext.Set<Modalidades>().AnyAsync(x => EF.Functions.Like(x.Descricao!, modalidade.Descricao) && x.ID != modalidade.ID))
            {
                result.SetError(nameof(Modalidades.Descricao), "exists");
            }

            // Ordem
            if (modalidade.Ordem < 0)
            {
                result.SetError(nameof(Modalidades.Ordem), "min");
            }

            result.ValidateEntityErrors(modalidade);
        }
        #endregion
    }
}