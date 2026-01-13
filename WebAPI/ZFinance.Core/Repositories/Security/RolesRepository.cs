using Microsoft.EntityFrameworkCore;
using ZDatabase.Exceptions;
using ZDatabase.Interfaces;
using ZDatabase.Validations;
using ZFinance.Core.Entities.Security;
using ZFinance.Core.Repositories.Security.Interfaces;
using ZFinance.Core.Services.Interfaces;

namespace ZFinance.Core.Repositories.Security
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
        /// <param name="dbContext">The <see cref="IDbContext" /> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler" /> instance.</param>
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
        public async Task DeleteRoleAsync(long roleID)
        {
            try
            {
                if (await FindRoleByIDAsync(roleID) is not Roles role)
                {
                    throw new EntityNotFoundException<Roles>(roleID);
                }
                else if ((role.Actions?.Count ?? 0) > 0)
                {
                    throw new InvalidOperationException("Roles-Button-Delete-Actions-Assigned");
                }
                else if ((role.Users?.Count ?? 0) > 0)
                {
                    throw new InvalidOperationException("Roles-Button-Delete-Users-Assigned");
                }
                else if ((role.Menus?.Count ?? 0) > 0)
                {
                    throw new InvalidOperationException("Roles-Button-Delete-Menus-Assigned");
                }

                role.IsDeleted = true;
                dbContext.Set<Roles>().Update(role);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in repository when deleting a role.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(roleID), roleID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<Roles?> FindRoleByIDAsync(long roleID)
        {
            try
            {
                return await dbContext.FindAsync<Roles>(roleID);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in repository when finding a role by ID.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(roleID), roleID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task InsertRoleAsync(Roles role)
        {
            try
            {
                await ValidateAsync(role);
                await dbContext.Set<Roles>().AddAsync(role);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in repository when inserting a role.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(role), role },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<Roles> ListRoles()
        {
            try
            {
                return from r in dbContext.Set<Roles>()
                       select r;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in repository when listing roles.");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task UpdateRoleAsync(Roles role)
        {
            try
            {
                await ValidateAsync(role);
                dbContext.Set<Roles>().Update(role);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in repository when updating a role.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(role), role },
                    }
                );
                throw;
            }
        }
        #endregion

        #region Private methods
        private async Task ValidateAsync(Roles role)
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