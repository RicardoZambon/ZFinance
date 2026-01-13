using Microsoft.EntityFrameworkCore;
using Niten.Core.Entities.PortalAluno;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.PortalAluno.Interfaces;
using ZDatabase.Exceptions;
using ZDatabase.Interfaces;
using ZDatabase.Validations;

namespace Niten.System.Core.Repositories.PortalAluno
{
    /// <inheritdoc />
    public class TiposPlanosRepository : ITiposPlanosRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="TiposPlanosRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        public TiposPlanosRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task AtualizarTipoPlanoAsync(TiposPlanos tipoPlano)
        {
            try
            {
                await ValidarAsync(tipoPlano);
                dbContext.Set<TiposPlanos>().Update(tipoPlano);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao atualizar o tipo de plano.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(tipoPlano), tipoPlano },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<TiposPlanos?> EncontrarTipoPlanoPorIDAsync(long tipoPlanoID)
        {
            try
            {
                return await dbContext.FindAsync<TiposPlanos>(tipoPlanoID);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao encontrar o tipo de plano pelo ID.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(tipoPlanoID), tipoPlanoID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task ExcluirTipoPlanoAsync(long tipoPlanoID)
        {
            try
            {
                if (await EncontrarTipoPlanoPorIDAsync(tipoPlanoID) is not TiposPlanos tipoPlano)
                {
                    throw new EntityNotFoundException<TiposPlanos>(tipoPlanoID);
                }

                tipoPlano.IsDeleted = true;
                dbContext.Set<TiposPlanos>().Update(tipoPlano);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao excluir o tipo de plano.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(tipoPlanoID), tipoPlanoID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task InserirNovoTipoPlanoAsync(TiposPlanos tipoPlano)
        {
            try
            {
                await ValidarAsync(tipoPlano);
                await dbContext.Set<TiposPlanos>().AddAsync(tipoPlano);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao inserir novo tipo de plano.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(tipoPlano), tipoPlano },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<TiposPlanos> ObterTodosTiposPlanos()
        {
            try
            {
                return from tp in dbContext.Set<TiposPlanos>()
                       select tp;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter todos os tipos de planos.");
                throw;
            }
        }

        #endregion

        #region Private methods
        private async Task ValidarAsync(TiposPlanos tipoPlano)
        {
            ValidationResult result = new();

            // Nome
            if (string.IsNullOrWhiteSpace(tipoPlano.Nome))
            {
                result.SetError(nameof(TiposPlanos.Nome), "required");
            }
            else if (await dbContext.Set<TiposPlanos>().AnyAsync(x => EF.Functions.Like(x.Nome, tipoPlano.Nome) && x.ID != tipoPlano.ID))
            {
                result.SetError(nameof(TiposPlanos.Nome), "exists");
            }

            // NomeExibicao
            if (string.IsNullOrWhiteSpace(tipoPlano.NomeExibicao))
            {
                result.SetError(nameof(TiposPlanos.NomeExibicao), "required");
            }
            else if (await dbContext.Set<TiposPlanos>().AnyAsync(x => EF.Functions.Like(x.NomeExibicao, tipoPlano.NomeExibicao) && x.ID != tipoPlano.ID))
            {
                result.SetError(nameof(TiposPlanos.NomeExibicao), "exists");
            }

            // Quantidade
            if (tipoPlano.Quantidade < 0)
            {
                result.SetError(nameof(TiposPlanos.Quantidade), "min");
            }

            result.ValidateEntityErrors(tipoPlano);
        }
        #endregion
    }
}