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
    public class MenusRepository : IMenusRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MenusRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext" /> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler" /> instance.</param>
        public MenusRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task DeleteMenuAsync(long menuID)
        {
            try
            {
                if (await FindMenuByIDAsync(menuID) is not Menus menu)
                {
                    throw new EntityNotFoundException<Menus>(menuID);
                }
                else if ((menu.ChildMenus?.Count ?? 0) > 0)
                {
                    throw new InvalidOperationException("Menus-Button-Delete-ChildMenus-Assigned");
                }
                else if ((menu.Roles?.Count ?? 0) > 0)
                {
                    throw new InvalidOperationException("Menus-Button-Delete-Roles-Assigned");
                }

                menu.IsDeleted = true;
                dbContext.Set<Menus>().Update(menu);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in repository when deleting a menu.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(menuID), menuID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<Menus?> FindMenuByIDAsync(long menuID)
        {
            try
            {
                return await dbContext.FindAsync<Menus>(menuID);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in repository when finding a menu by ID.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(menuID), menuID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task InsertMenuAsync(Menus menu)
        {
            try
            {
                await ValidateAsync(menu);
                await dbContext.Set<Menus>().AddAsync(menu);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in repository when inserting a menu.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(menu), menu },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<Menus> ListMenus()
        {
            try
            {
                return from m in dbContext.Set<Menus>()
                       select m;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in repository when listing menus.");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task UpdateMenuAsync(Menus menu)
        {
            try
            {
                await ValidateAsync(menu);
                dbContext.Set<Menus>().Update(menu);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in repository when updating a menu.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(menu), menu },
                    }
                );
                throw;
            }
        }
        #endregion

        #region Private methods
        private async Task ValidateAsync(Menus menu)
        {
            ValidationResult result = new();

            // Label
            if (string.IsNullOrWhiteSpace(menu.Label))
            {
                result.SetError(nameof(Menus.Label), "required");
            }
            else if (await dbContext.Set<Menus>().AnyAsync(x => EF.Functions.Like(x.Label!, menu.Label) && x.ID != menu.ID && ((x.ParentMenuID == null && menu.ParentMenuID == null) || menu.ParentMenuID == x.ParentMenuID)))
            {
                result.SetError(nameof(Menus.Label), "exists");
            }

            // Order
            if (menu.Order < 0)
            {
                result.SetError(nameof(Menus.Order), "min");
            }

            // ParentMenuID
            if (menu.ParentMenuID is long parentMenuID && await dbContext.FindAsync<Menus>(parentMenuID) is null)
            {
                result.SetError(nameof(Menus.ParentMenuID), "required");
            }

            result.ValidateEntityErrors(menu);
        }
        #endregion
    }
}
