using Microsoft.EntityFrameworkCore;
using Niten.Core.Entities.Configs;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.Configs.Interfaces;
using ZDatabase.Exceptions;
using ZDatabase.Interfaces;
using ZDatabase.Validations;

namespace Niten.System.Core.Repositories.Configs
{
    /// <inheritdoc />
    public class MoedasRepository : IMoedasRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MoedasRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        public MoedasRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task AtualizarMoedaAsync(Moedas moeda)
        {
            try
            {
                NormalizarCodigo(moeda);
                await ValidarAsync(moeda);
                dbContext.Set<Moedas>().Update(moeda);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao atualizar a moeda.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(moeda), moeda },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<Moedas?> EncontrarMoedaPorIDAsync(long moedaID)
        {
            try
            {
                return await dbContext.FindAsync<Moedas>(moedaID);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao encontrar a moeda pelo ID.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(moedaID), moedaID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task ExcluirMoedaAsync(long moedaID)
        {
            try
            {
                if (await EncontrarMoedaPorIDAsync(moedaID) is not Moedas moeda)
                {
                    throw new EntityNotFoundException<Moedas>(moedaID);
                }

                moeda.IsDeleted = true;
                dbContext.Set<Moedas>().Update(moeda);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao excluir a moeda.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(moedaID), moedaID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task InserirNovaMoedaAsync(Moedas moeda)
        {
            try
            {
                NormalizarCodigo(moeda);
                await ValidarAsync(moeda);
                await dbContext.Set<Moedas>().AddAsync(moeda);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao inserir nova moeda.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(moeda), moeda },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<Moedas> ObterTodasMoedas()
        {
            try
            {
                return from m in dbContext.Set<Moedas>()
                       select m;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter todas as moedas.");
                throw;
            }
        }
        #endregion

        #region Private methods
        private void NormalizarCodigo(Moedas moeda)
        {
            moeda.Codigo = moeda.Codigo?.ToUpper();
        }

        private async Task ValidarAsync(Moedas moeda)
        {
            ValidationResult result = new();

            // Codigo
            if (string.IsNullOrWhiteSpace(moeda.Codigo))
            {
                result.SetError(nameof(Moedas.Codigo), "required");
            }
            else if (await dbContext.Set<Moedas>().AnyAsync(x => EF.Functions.Like(x.Codigo!, moeda.Codigo) && x.ID != moeda.ID))
            {
                result.SetError(nameof(Moedas.Codigo), "exists");
            }

            // Nome
            if (string.IsNullOrWhiteSpace(moeda.Nome))
            {
                result.SetError(nameof(Moedas.Nome), "required");
            }
            else if (await dbContext.Set<Moedas>().AnyAsync(x => EF.Functions.Like(x.Nome!, moeda.Nome) && x.ID != moeda.ID))
            {
                result.SetError(nameof(Moedas.Nome), "exists");
            }

            // Simbolo
            if (string.IsNullOrWhiteSpace(moeda.Simbolo))
            {
                result.SetError(nameof(Moedas.Simbolo), "required");
            }

            result.ValidateEntityErrors(moeda);
        }
        #endregion
    }
}