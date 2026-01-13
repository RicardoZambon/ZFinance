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
    public class ComissionadosRepository : IComissionadosRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ComissionadosRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        public ComissionadosRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task AtualizarComissionadoAsync(Comissionados comissionado)
        {
            try
            {
                await ValidarAsync(comissionado);
                dbContext.Set<Comissionados>().Update(comissionado);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao atualizar o comissionado.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(comissionado), comissionado },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<Comissionados?> EncontrarComissionadoPorIDAsync(long comissionadoID)
        {
            try
            {
                return await dbContext.FindAsync<Comissionados>(comissionadoID);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao encontrar o comissionado pelo ID.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(comissionadoID), comissionadoID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task ExcluirComissionadoAsync(long comissionadoID)
        {
            try
            {
                if (await EncontrarComissionadoPorIDAsync(comissionadoID) is not Comissionados comissionado)
                {
                    throw new EntityNotFoundException<Comissionados>(comissionadoID);
                }

                comissionado.IsDeleted = true;
                dbContext.Set<Comissionados>().Update(comissionado);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao excluir o comissionado.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(comissionadoID), comissionadoID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task InserirNovoComissionadoAsync(Comissionados comissionado)
        {
            try
            {
                await ValidarAsync(comissionado);
                await dbContext.Set<Comissionados>().AddAsync(comissionado);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao inserir novo comissionado.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(comissionado), comissionado },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<Comissionados> ObterTodosComissionados()
        {
            try
            {
                return from c in dbContext.Set<Comissionados>()
                       select c;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter todos os comissionados.");
                throw;
            }
        }
        #endregion

        #region Private methods
        private async Task ValidarAsync(Comissionados comissionado)
        {
            ValidationResult result = new();

            // CadastroID
            if (await dbContext.Set<Cadastros>().FindAsync(comissionado.CadastroID) is null)
            {
                result.SetError(nameof(Comissionados.CadastroID), "required");
            }
            else if (await dbContext.Set<Comissionados>().AnyAsync(x => x.CadastroID == comissionado.CadastroID && x.ID != comissionado.ID))
            {
                result.SetError(nameof(Comissionados.CadastroID), "exists");
            }

            result.ValidateEntityErrors(comissionado);
        }
        #endregion
    }
}