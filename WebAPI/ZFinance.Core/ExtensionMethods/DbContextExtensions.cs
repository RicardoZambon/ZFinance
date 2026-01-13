using Microsoft.EntityFrameworkCore;
using ZDatabase.Interfaces;
using ZFinance.Core.Initializers;

namespace ZFinance.Core.ExtensionMethods
{
    /// <summary>
    /// Extension methods for database context.
    /// </summary>
    public static class DbContextExtensions
    {
        /// <summary>
        /// Applies the database initializations.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public static void ApplyDatabaseInitializations(this IDbContext dbContext)
        {
            IEnumerable<string> pendingMigrations = dbContext.Database.GetPendingMigrations();

            dbContext.Database.Migrate();

            foreach (string migration in pendingMigrations)
            {
                BaseInitializer? initializer = null;

                if (migration.EndsWith("_v1.0.0"))
                {
                    initializer = new V100Initializer(dbContext);
                }

                initializer?.Initialize();
            }
        }
    }
}