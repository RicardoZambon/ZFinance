using Microsoft.EntityFrameworkCore;
using ZDatabase.Exceptions;
using ZDatabase.Interfaces;
using ZFinance.Core.Entities.Security;
using ZFinance.Core.Repositories.Security.Interfaces;
using ZFinance.Core.Services.Interfaces;
using ZSecurity.Repositories;

namespace ZFinance.Core.Services
{
    /// <inheritdoc />
    public class InformationProvider : BaseUsersRepository<Actions, long>, IInformationProvider
    {
        #region Variables
        private readonly IExceptionHandler exceptionHandler;
        private readonly IUsersRepository usersRepository;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="InformationProvider"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext" /> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler" /> instance.</param>
        /// <param name="usersRepository">The <see cref="IUsersRepository" /> instance.</param>
        public InformationProvider(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler,
            IUsersRepository usersRepository)
            : base(dbContext)
        {
            this.exceptionHandler = exceptionHandler;
            this.usersRepository = usersRepository;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task<Menus?> GetMenuByUrlAsync(long userID, string url)
        {
            try
            {
                return (await ListAllMenusAsync(userID))
                    .FirstOrDefault(x => x.URL == url);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in information provider when getting the menu by url from the user.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(userID), userID },
                        { nameof(url), url },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public override async Task<IEnumerable<Actions>> ListAllActionsAsync(long userID)
        {
            try
            {
                return (await GetAssignedRolesAsync(userID))
                    .SelectMany(x => x.Actions!);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in information provider when listing all actions from the user.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(userID), userID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Menus>> ListAllowedMenusAsync(long userID, long? parentMenuID)
        {
            try
            {
                return (await ListAllMenusAsync(userID))
                    .Where(x => x.ParentMenuID == parentMenuID)
                    .OrderBy(x => x.Order);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in information provider when listing allowed menus from the user.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(userID), userID },
                        { nameof(parentMenuID), parentMenuID },
                    }
                );
                throw;
            }
        }
        #endregion

        #region Private methods
        private async Task<IQueryable<Roles>> GetAssignedRolesAsync(long userID)
        {
            if (await usersRepository.FindUserByIDAsync(userID) is not Users user)
            {
                throw new EntityNotFoundException<Users>(userID);
            }

            List<long> rolesIDs = user.Roles?.Select(x => x.ID)?.ToList() ?? [];

            return from r in dbContext.Set<Roles>()
                   where rolesIDs.Contains(r.ID)
                   select r;
        }

        private async Task<bool> IsAdministrator(long userID)
        {
            return await HasAnyActionAsync(userID, "AdministrativeMaster");
        }

        private async Task<IEnumerable<Menus>> ListAllMenusAsync(long userID)
        {
            bool isAdministrator = await IsAdministrator(userID);

            string menuIDs = string.Join(" UNION ALL ", (await GetAssignedRolesAsync(userID))
                .SelectMany(x => x.Menus!)
                .Select(x => x.ID)
                .Distinct()
                .Select(x => $"SELECT CAST({x} AS BIGINT) AS ID"));

            if (string.IsNullOrEmpty(menuIDs))
            {
                menuIDs = "SELECT CAST(0 AS BIGINT) AS ID";
            }

            string query = $@"
                WITH AllowedMenus AS (
                    {menuIDs}
                ),
                Menus AS (
	                SELECT M.ID, M.ParentMenuID
	                FROM [Security].[Menus] M
	                WHERE
                        M.IsDeleted = 0
                        AND (
                            M.ID IN (SELECT ID FROM AllowedMenus)
                            OR {(isAdministrator ? 1 : 0)} = 1
                        )

	                UNION ALL

	                SELECT M1.ID, M1.ParentMenuID
	                FROM [Security].[Menus] M1
		                INNER JOIN Menus M2 ON M2.ParentMenuID = M1.ID
                    WHERE M1.IsDeleted = 0
                )
                SELECT M.*
                FROM [Security].[Menus] M
                    INNER JOIN (SELECT ID FROM Menus GROUP BY ID) M2 ON M2.ID = M.ID
                WHERE
                    M.IsDeleted = 0
                ";

            return await dbContext.Set<Menus>()
                .FromSqlRaw(query)
                .IgnoreQueryFilters()
                .ToListAsync();
        }
        #endregion
    }
}
