using Microsoft.EntityFrameworkCore;
using ZDatabase;

namespace ZFinance.Core
{
    /// <inheritdoc />
    public class AppDbContext : ZDbContext<AppDbContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppDbContext"/> class.
        /// </summary>
        /// <param name="options">The <see cref="DbContextOptions{TDbContext}" /> instance.</param>
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}