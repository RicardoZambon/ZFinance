using Microsoft.EntityFrameworkCore;
using Niten.Core.Entities.Integracoes;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.Integracoes.Interfaces;
using ZDatabase.Interfaces;
using ZDatabase.Validations;

namespace Niten.System.Core.Repositories.Integracoes
{
    /// <inheritdoc />
    public class PerfisPagSeguroRepository : IPerfisPagSeguroRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PerfisPagSeguroRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        public PerfisPagSeguroRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task AtualizarPerfilPagSeguroAsync(PerfisPagSeguro perfilPagSeguro)
        {
            try
            {
                await ValidarAsync(perfilPagSeguro);
                dbContext.Set<PerfisPagSeguro>().Update(perfilPagSeguro);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao atualizar o perfil do PagSeguro.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(perfilPagSeguro), perfilPagSeguro },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<PerfisPagSeguro?> EncontrarPerfilPagSeguroPorIDAsync(long perfilPagSeguroID)
        {
            try
            {
                return await dbContext.FindAsync<PerfisPagSeguro>(perfilPagSeguroID);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao encontrar o perfil do PagSeguro pelo ID.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(perfilPagSeguroID), perfilPagSeguroID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task InserirNovoPerfilPagSeguroAsync(PerfisPagSeguro perfilPagSeguro)
        {
            try
            {
                await ValidarAsync(perfilPagSeguro);
                await dbContext.Set<PerfisPagSeguro>().AddAsync(perfilPagSeguro);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao inserir novo perfil do PagSeguro.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(perfilPagSeguro), perfilPagSeguro },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<PerfisPagSeguro> ObterTodosPerfisPagSeguro()
        {
            try
            {
                return from pps in dbContext.Set<PerfisPagSeguro>()
                       select pps;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter todos os perfis do PagSeguro.");
                throw;
            }
        }
        #endregion

        #region Private methods
        private async Task ValidarAsync(PerfisPagSeguro perfilPagSeguro)
        {
            ValidationResult result = new();

            // ConfigPathToken
            if (string.IsNullOrWhiteSpace(perfilPagSeguro.ConfigPathToken))
            {
                result.SetError(nameof(PerfisPagSeguro.ConfigPathToken), "required");
            }

            // LimiteParcelamento
            if (perfilPagSeguro.LimiteParcelamento is not null && perfilPagSeguro.LimiteParcelamento < 1)
            {
                result.SetError(nameof(PerfisPagSeguro.LimiteParcelamento), "min");
            }

            // LimiteParcelamentoSemJuros
            if (perfilPagSeguro.LimiteParcelamentoSemJuros is not null && perfilPagSeguro.LimiteParcelamentoSemJuros < 1)
            {
                result.SetError(nameof(PerfisPagSeguro.LimiteParcelamentoSemJuros), "min");
            }

            // Nome
            if (string.IsNullOrWhiteSpace(perfilPagSeguro.Nome))
            {
                result.SetError(nameof(PerfisPagSeguro.Nome), "required");
            }
            else if (await dbContext.Set<PerfisPagSeguro>().AnyAsync(x => EF.Functions.Like(x.Nome!, perfilPagSeguro.Nome) && x.ID != perfilPagSeguro.ID))
            {
                result.SetError(nameof(PerfisPagSeguro.Nome), "exists");
            }

            result.ValidateEntityErrors(perfilPagSeguro);
        }
        #endregion
    }
}