using Microsoft.EntityFrameworkCore;
using Niten.Core.Entities.Seguranca;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.Seguranca.Interfaces;
using Niten.System.Core.Services.Interfaces;
using System.Data;
using ZDatabase.Exceptions;
using ZDatabase.Interfaces;
using ZDatabase.Validations;
using ZSecurity.Services;

namespace Niten.System.Core.Repositories.Seguranca
{
    /// <inheritdoc />
    public class MenusRepository : IMenusRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        private readonly ISecurityHandler securityHandler;
        private readonly ISystemCurrentUserProvider systemCurrentUserProvider;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MenusRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        /// <param name="securityHandler">The <see cref="ISecurityHandler"/> instance.</param>
        /// <param name="systemCurrentUserProvider">The <see cref="ISystemCurrentUserProvider"/> instance.</param>
        public MenusRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler,
            ISecurityHandler securityHandler,
            ISystemCurrentUserProvider systemCurrentUserProvider)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
            this.securityHandler = securityHandler;
            this.systemCurrentUserProvider = systemCurrentUserProvider;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task AtualizarMenuAsync(Menus menu)
        {
            try
            {
                await ValidarAsync(menu);
                dbContext.Set<Menus>().Update(menu);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao atualizar o menu.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(menu), menu },
                    }
                );
                throw;
            }
        }

        /// <inehritdoc />
        public async Task<Menus?> EncontrarMenuPorIDAsync(long menuID)
        {
            try
            {
                return await dbContext.FindAsync<Menus>(menuID);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao encontrar o menu pelo ID.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(menuID), menuID },
                    }
                );
                throw;
            }
        }

        /// <inehritdoc />
        public async Task<Menus?> EncontrarMenuPorURLAsync(string url)
        {
            try
            {
                return await dbContext.Set<Menus>().FirstOrDefaultAsync(x => x.URL == url);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao encontrar o menu pela URL.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(url), url },
                    }
                );
                throw;
            }
        }

        /// <inehritdoc />
        public async Task ExcluirMenuAsync(long menuID)
        {
            try
            {
                if (await EncontrarMenuPorIDAsync(menuID) is not Menus menu)
                {
                    throw new EntityNotFoundException<Actions>(menuID);
                }

                menu.IsDeleted = true;
                dbContext.Set<Menus>().Update(menu);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao excluir o menu.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(menuID), menuID },
                    }
                );
                throw;
            }
        }

        /// <inehritdoc />
        public async Task InserirNovoMenuAsync(Menus menu)
        {
            try
            {
                await ValidarAsync(menu);
                await dbContext.Set<Menus>().AddAsync(menu);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao inserir novo menu.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(menu), menu },
                    }
                );
                throw;
            }
        }

        /// <inehritdoc />
        public async Task<IEnumerable<Menus>> ObterMenusPermitidosAsync(long? parentMenuID = null)
        {
            try
            {
                bool isAdministrator = await securityHandler.CheckCurrentUserIsAdministratorAsync();

                return (await dbContext.Set<Menus>()
                        .FromSqlRaw(@"
                            WITH RECURSIVE Menus (ID, ParentMenuID) AS (
                                SELECT M.ID, M.ParentMenuID
                                FROM Seguranca_Menus M
                                WHERE
                                    M.IsDeleted = 0
                                    AND ({1} = 1
                                        OR EXISTS(
                                            SELECT 1
                                            FROM Seguranca_RolesMenus RM
                                                INNER JOIN Seguranca_RolesUsuarios RU ON RU.RoleID = RM.RoleID
                                                INNER JOIN Seguranca_Roles R ON R.ID = RU.RoleID AND R.IsDeleted = 0
                                            WHERE RM.MenuID = M.ID AND RU.UsuarioID = {0}
                                            LIMIT 1
                                        )
                                    )

                                UNION ALL

                                SELECT M1.ID, M1.ParentMenuID
                                FROM Seguranca_Menus M1
                                    INNER JOIN Menus M2 ON M2.ParentMenuID = M1.ID
                                WHERE M1.IsDeleted = 0
                            )
                            SELECT M.*
                            FROM Seguranca_Menus M
                                INNER JOIN (SELECT ID FROM Menus GROUP BY ID) M2 ON M2.ID = M.ID
                            WHERE
                                M.IsDeleted = 0
                            ", systemCurrentUserProvider.UsuarioID ?? 0, isAdministrator ? 1 : 0)
                        .IgnoreQueryFilters()
                        .ToListAsync()
                    )
                    .Where(x => x.ParentMenuID == parentMenuID)
                    .OrderBy(x => x.Order);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter os menus permitidos.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(parentMenuID), parentMenuID },
                    }
                );
                throw;
            }

        }

        /// <inheritdoc />
        public IQueryable<Menus> ObterTodosMenus()
        {
            try
            {
                return from m in dbContext.Set<Menus>()
                       select m;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter todos os menus.");
                throw;
            }
        }
        #endregion

        #region Private methods
        private async Task ValidarAsync(Menus menu)
        {
            ValidationResult result = new();

            // Label
            if (string.IsNullOrWhiteSpace(menu.Label))
            {
                result.SetError(nameof(Menus.Label), "required");
            }

            // Order
            if (menu.Order < 0)
            {
                result.SetError(nameof(Menus.Order), "min");
            }

            // URL
            if (!string.IsNullOrEmpty(menu.URL) && await dbContext.Set<Menus>().AnyAsync(x => x.URL == menu.URL && x.ID != menu.ID))
            {
                result.SetError(nameof(Menus.URL), "exists");
            }

            result.ValidateEntityErrors(menu);
        }
        #endregion
    }
}