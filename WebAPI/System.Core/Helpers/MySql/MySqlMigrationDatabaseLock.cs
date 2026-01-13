using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;

namespace Niten.System.Core.Helpers.MySql
{
    /// <inheritdoc />
    public class MySqlMigrationDatabaseLock(
        IRelationalCommand releaseLockCommand,
        RelationalCommandParameterObject relationalCommandParameters,
        IHistoryRepository historyRepository,
        CancellationToken cancellationToken = default)
    : IMigrationsDatabaseLock
    {
        /// <inheritdoc />
        public virtual IHistoryRepository HistoryRepository => historyRepository;

        /// <inheritdoc />
        public void Dispose()
            => releaseLockCommand.ExecuteScalar(relationalCommandParameters);

        /// <inheritdoc />
        public async ValueTask DisposeAsync()
            => await releaseLockCommand.ExecuteScalarAsync(relationalCommandParameters, cancellationToken).ConfigureAwait(false);
    }
}