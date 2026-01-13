using Microsoft.EntityFrameworkCore;
using Niten.Core.Entities.Estoque;
using Niten.Core.Entities.Financeiro;
using Niten.Core.Entities.Integracoes;
using Niten.Core.Enums;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.Financeiro.Interfaces;
using ZDatabase.Exceptions;
using ZDatabase.Interfaces;
using ZDatabase.Validations;

namespace Niten.System.Core.Repositories.Financeiro
{
    /// <inheritdoc />
    public class PlanosRepository : IPlanosRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PlanosRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        public PlanosRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task AtualizarPlanoAsync(Planos plano)
        {
            try
            {
                await ValidarAsync(plano);

                dbContext.Set<Planos>().Update(plano);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao atualizar o plano.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(plano), plano },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<Planos?> EncontrarPlanoPorIDAsync(long planoID)
        {
            try
            {
                return await dbContext.FindAsync<Planos>(planoID);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao encontrar o plano pelo ID.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(planoID), planoID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task ExcluirPlanoAsync(long planoID)
        {
            try
            {
                if (await EncontrarPlanoPorIDAsync(planoID) is not Planos plano)
                {
                    throw new EntityNotFoundException<Planos>(planoID);
                }
                else if (plano.Status != PlanosStatus.Inativo)
                {
                    throw new InvalidOperationException("Planos-Button-Excluir-Modal-Failed-Status");
                }

                plano.IsDeleted = true;
                dbContext.Set<Planos>().Update(plano);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao excluir o plano.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(planoID), planoID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task InserirNovoPlanoAsync(Planos plano)
        {
            try
            {
                await ValidarAsync(plano);

                await dbContext.Set<Planos>().AddAsync(plano);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao inserir novo plano.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(plano), plano },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<Planos> ObterTodosPlanos()
        {
            try
            {
                return from p in dbContext.Set<Planos>()
                       select p;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter todos os plano.");
                throw;
            }
        }

        #endregion

        #region Private methods
        private async Task ValidarAsync(Planos plano)
        {
            ValidationResult result = new();

            // DataInicio
            if (plano.DataInicio is DateTime dataInicio && plano.DataTermino is DateTime dataTermino
                && dataInicio > dataTermino)
            {
                result.SetError(nameof(Planos.DataInicio), "invalid");
            }

            // MaterialID
            if (await dbContext.FindAsync<Materiais>(plano.MaterialID) is null)
            {
                result.SetError(nameof(Planos.MaterialID), "required");
            }

            // Nome
            if (string.IsNullOrWhiteSpace(plano.Nome))
            {
                result.SetError(nameof(Planos.Nome), "required");
            }
            else if (await dbContext.Set<Planos>().AnyAsync(x => EF.Functions.Like(x.Nome!, plano.Nome) && x.ID != plano.ID))
            {
                result.SetError(nameof(Planos.Nome), "exists");
            }

            // PagamentoOnlineID
            if (await dbContext.FindAsync<PagamentosOnline>(plano.PagamentoOnlineID) is not PagamentosOnline pagamentoOnline)
            {
                result.SetError(nameof(Planos.PagamentoOnlineID), "required");
            }
            else if (pagamentoOnline.RecorrenciaID is null)
            {
                result.SetError(nameof(Planos.PagamentoOnlineID), "invalid");
            }

            // PerfilID
            if (plano.PerfilID is not null && await dbContext.FindAsync<Perfis>(plano.PerfilID) is null)
            {
                result.SetError(nameof(Planos.PerfilID), "invalid");
            }

            // Quantidade
            if (plano.Quantidade <= 0)
            {
                result.SetError(nameof(Planos.Quantidade), "min");
            }
            else if (dbContext.Set<Planos>().Any(x => x.ID != plano.ID && x.PagamentoOnlineID == plano.PagamentoOnlineID && x.MaterialID == plano.MaterialID && x.Quantidade == plano.Quantidade))
            {
                result.SetError(nameof(Planos.Quantidade), "exists");
            }

            // TipoPlanoID
            if (plano.TipoPlanoID is null)
            {
                result.SetError(nameof(Planos.TipoPlanoID), "required");
            }

            result.ValidateEntityErrors(plano);
        }
        #endregion
    }
}