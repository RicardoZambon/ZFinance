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
    public class UsersRepository : IUsersRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext" /> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler" /> instance.</param>
        public UsersRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task DisableUserAsync(long userID)
        {
            try
            {
                if (await FindUserByIDAsync(userID) is not Users user)
                {
                    throw new EntityNotFoundException<Users>(userID);
                }
                else if ((user.Roles?.Count ?? 0) > 0)
                {
                    throw new InvalidOperationException("Users-Button-Delete-Roles-Assigned");
                }

                user.IsActive = false;
                dbContext.Set<Users>().Update(user);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in repository when deactivating a user.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(userID), userID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<Users?> FindUserByIDAsync(long userID)
        {
            try
            {
                return await dbContext.FindAsync<Users>(userID);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in repository when finding a user by ID.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(userID), userID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<Users?> FindUserByEmailAsync(string email)
        {
            try
            {
                return await dbContext.Set<Users>()
                    .FirstOrDefaultAsync(u => EF.Functions.Like(u.Email ?? string.Empty, email));
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in repository when finding a user by email.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(email), email },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task InsertUserAsync(Users user)
        {
            try
            {
                await ValidateAsync(user);
                user.IsActive = true;

                await dbContext.Set<Users>().AddAsync(user);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in repository when inserting a user.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(user), user },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<Users> ListUsers()
        {
            try
            {
                return from u in dbContext.Set<Users>()
                       select u;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in repository when listing users.");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task UpdateUserAsync(Users user)
        {
            try
            {
                await ValidateAsync(user);
                dbContext.Set<Users>().Update(user);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in repository when updating a user.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(user), user },
                    }
                );
                throw;
            }
        }
        #endregion

        #region Private methods
        private async Task ValidateAsync(Users user)
        {
            ValidationResult result = new();

            // Email
            if (string.IsNullOrWhiteSpace(user.Email))
            {
                result.SetError(nameof(Users.Email), "required");
            }
            else if (await dbContext.Set<Users>().AnyAsync(x => EF.Functions.Like(x.Email!, user.Email) && x.ID != user.ID))
            {
                result.SetError(nameof(Users.Email), "exists");
            }

            result.ValidateEntityErrors(user);
        }
        #endregion
    }
}