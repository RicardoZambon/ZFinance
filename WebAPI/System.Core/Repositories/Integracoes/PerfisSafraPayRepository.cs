using Microsoft.EntityFrameworkCore;
using Niten.Core.Entities.Integracoes;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.Integracoes.Interfaces;
using ZDatabase.Interfaces;
using ZDatabase.Validations;

namespace Niten.System.Core.Repositories.Integracoes
{
    /// <inheritdoc />
    public class PerfisSafraPayRepository : IPerfisSafraPayRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PerfisSafraPayRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        public PerfisSafraPayRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task AtualizarPerfilSafraPayAsync(PerfisSafraPay perfilSafraPay)
        {
            try
            {
                await ValidarAsync(perfilSafraPay);
                dbContext.Set<PerfisSafraPay>().Update(perfilSafraPay);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao atualizar o perfil do SafraPay.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(perfilSafraPay), perfilSafraPay },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<PerfisSafraPay?> EncontrarPerfilSafraPayPorIDAsync(long perfilSafraPayID)
        {
            try
            {
                return await dbContext.FindAsync<PerfisSafraPay>(perfilSafraPayID);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao encontrar o perfil do SafraPay pelo ID.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(perfilSafraPayID), perfilSafraPayID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task InserirNovoPerfilSafraPayAsync(PerfisSafraPay perfilSafraPay)
        {
            try
            {
                await ValidarAsync(perfilSafraPay);
                await dbContext.Set<PerfisSafraPay>().AddAsync(perfilSafraPay);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao inserir novo perfil do SafraPay.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(perfilSafraPay), perfilSafraPay },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<PerfisSafraPay> ObterTodosPerfisSafraPay()
        {
            try
            {
                return from psp in dbContext.Set<PerfisSafraPay>()
                       select psp;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter todos os perfis do SafraPay.");
                throw;
            }
        }
        #endregion

        #region Private methods
        private async Task ValidarAsync(PerfisSafraPay perfilSafraPay)
        {
            ValidationResult result = new();

            // ConfigPathToken
            if (string.IsNullOrWhiteSpace(perfilSafraPay.ConfigPathToken))
            {
                result.SetError(nameof(PerfisSafraPay.ConfigPathToken), "required");
            }

            // LimiteParcelamento
            if (perfilSafraPay.LimiteParcelamento is not null)
            {
                if (perfilSafraPay.LimiteParcelamento < 1)
                {
                    result.SetError(nameof(PerfisSafraPay.LimiteParcelamento), "min");
                }

                if (perfilSafraPay.NumeroDeParcelas is not null)
                {
                    result.SetError(nameof(PerfisSafraPay.LimiteParcelamento), "invalid");
                }
            }

            // Nome
            if (string.IsNullOrWhiteSpace(perfilSafraPay.Nome))
            {
                result.SetError(nameof(PerfisSafraPay.Nome), "required");
            }
            else if (await dbContext.Set<PerfisSafraPay>().AnyAsync(x => EF.Functions.Like(x.Nome!, perfilSafraPay.Nome) && x.ID != perfilSafraPay.ID))
            {
                result.SetError(nameof(PerfisSafraPay.Nome), "exists");
            }

            // NumeroDeParcelas
            if (perfilSafraPay.NumeroDeParcelas is not null && perfilSafraPay.NumeroDeParcelas < 1)
            {
                result.SetError(nameof(PerfisSafraPay.NumeroDeParcelas), "min");
            }

            result.ValidateEntityErrors(perfilSafraPay);
        }
        #endregion
    }
}