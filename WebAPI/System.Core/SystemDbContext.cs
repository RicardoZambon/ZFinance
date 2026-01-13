using Microsoft.EntityFrameworkCore;
using Niten.Core;

namespace Niten.System.Core
{
    /// <inheritdoc />
    public class SystemDbContext : AppDbContext<SystemDbContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemDbContext"/> class.
        /// </summary>
        /// <param name="options">The <see cref="DbContextOptions{TDbContext}" /> instance.</param>
        public SystemDbContext(DbContextOptions<SystemDbContext> options)
            : base(options)
        {
        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ZTaskScheduler.Tasks.BaseTask).Assembly);
        }
    }
}
