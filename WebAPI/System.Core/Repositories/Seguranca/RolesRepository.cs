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
    public class RolesRepository : IRolesRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RolesRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        public RolesRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task AtualizarRoleAsync(Roles role)
        {
            try
            {
                await ValidarAsync(role);
                dbContext.Set<Roles>().Update(role);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao atualizar a role.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(role), role },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<Roles?> EncontrarRolePorIDAsync(long roleID)
        {
            try
            {
                return await dbContext.FindAsync<Roles>(roleID);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao encontrar a role pelo ID.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(roleID), roleID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task ExcluirRoleAsync(long roleID)
        {
            try
            {
                if (await EncontrarRolePorIDAsync(roleID) is not Roles role)
                {
                    throw new EntityNotFoundException<Roles>(roleID);
                }

                role.IsDeleted = true;
                dbContext.Set<Roles>().Update(role);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao excluir a role.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(roleID), roleID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task InserirNovaRoleAsync(Roles role)
        {
            try
            {
                await ValidarAsync(role);
                await dbContext.Set<Roles>().AddAsync(role);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao inserir nova role.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(role), role },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<Roles> ObterTodasRoles()
        {
            try
            {
                return from r in dbContext.Set<Roles>()
                       select r;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter todas as roles.");
                throw;
            }
        }
        #endregion

        #region Private methods
        private async Task ValidarAsync(Roles role)
        {
            ValidationResult result = new();

            // Name
            if (string.IsNullOrWhiteSpace(role.Name))
            {
                result.SetError(nameof(Roles.Name), "required");
            }
            else if (await dbContext.Set<Roles>().AnyAsync(x => EF.Functions.Like(x.Name!, role.Name) && x.ID != role.ID))
            {
                result.SetError(nameof(Roles.Name), "exists");
            }

            result.ValidateEntityErrors(role);
        }
        #endregion
    }
}