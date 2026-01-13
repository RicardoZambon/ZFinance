using Microsoft.EntityFrameworkCore;
using Niten.Core.Entities.Seguranca;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.Seguranca.Interfaces;
using ZDatabase.Exceptions;
using ZDatabase.Interfaces;
using ZDatabase.Validations;

namespace Niten.System.Core.Repositories.Seguranca
{
    /// <inheritdoc />
    public class ActionsRepository : IActionsRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionsRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        public ActionsRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task AtualizarActionAsync(Actions action)
        {
            try
            {
                await ValidarAsync(action);
                dbContext.Set<Actions>().Update(action);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao atualizar a action.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(action), action },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<Actions?> EncontrarActionPorIDAsync(long actionID)
        {
            try
            {
                return await dbContext.FindAsync<Actions>(actionID);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao encontrar a action pelo ID.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(actionID), actionID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task ExcluirActionAsync(long actionID)
        {
            try
            {
                if (await EncontrarActionPorIDAsync(actionID) is not Actions action)
                {
                    throw new EntityNotFoundException<Actions>(actionID);
                }

                action.IsDeleted = true;
                dbContext.Set<Actions>().Update(action);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao excluir a action.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(actionID), actionID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task InserirNovaActionAsync(Actions action)
        {
            try
            {
                await ValidarAsync(action);
                await dbContext.Set<Actions>().AddAsync(action);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao inserir nova action.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(action), action },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<Actions> ObterTodasActions()
        {
            try
            {
                return from a in dbContext.Set<Actions>()
                       select a;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter todas as actions.");
                throw;
            }
        }
        #endregion

        #region Private methods
        private async Task ValidarAsync(Actions action)
        {
            ValidationResult result = new();

            // Code
            if (string.IsNullOrWhiteSpace(action.Code))
            {
                result.SetError(nameof(action.Code), "required");
            }
            else if (await dbContext.Set<Actions>().AnyAsync(x => x.Code == action.Code && x.ID != action.ID))
            {
                result.SetError(nameof(action.Code), "exists");
            }

            // Description
            if (string.IsNullOrWhiteSpace(action.Description))
            {
                result.SetError(nameof(action.Description), "required");
            }

            // Entity
            if (string.IsNullOrWhiteSpace(action.Entity))
            {
                result.SetError(nameof(action.Entity), "required");
            }

            // Name
            if (string.IsNullOrWhiteSpace(action.Name))
            {
                result.SetError(nameof(action.Name), "required");
            }
            else if (await dbContext.Set<Actions>().AnyAsync(x => x.Name == action.Name && x.ID != action.ID))
            {
                result.SetError(nameof(action.Name), "exists");
            }

            result.ValidateEntityErrors(action);
        }
        #endregion
    }
}