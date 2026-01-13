using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;

namespace Niten.System.Core.Helpers.MySql
{
    /// <inheritdoc />
    public class MySqlHistoryRepository : HistoryRepository
    {
        #region Variables
        private static readonly TimeSpan _retryDelay = TimeSpan.FromSeconds(1);

        /// <inheritdoc />
        protected override string ExistsSql
            => CreateExistsSql(TableName);

        /// <inheritdoc />
        public override LockReleaseBehavior LockReleaseBehavior => LockReleaseBehavior.Explicit;

        /// <inheritdoc />
        protected virtual string LockTableName { get; } = "__EFMigrationsLock";
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlHistoryRepository"/> class.
        /// </summary>
        /// <param name="dependencies">Parameter object containing dependencies for this service.</param>
        public MySqlHistoryRepository(HistoryRepositoryDependencies dependencies)
            : base(dependencies)
        {
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public override IMigrationsDatabaseLock AcquireDatabaseLock()
        {
            Dependencies.MigrationsLogger.AcquiringMigrationLock();

            if (!InterpretExistsResult(
                Dependencies.RawSqlCommandBuilder.Build(CreateExistsSql(LockTableName))
                    .ExecuteScalar(CreateRelationalCommandParameters())))
            {
                CreateLockTableCommand().ExecuteNonQuery(CreateRelationalCommandParameters());
            }

            TimeSpan retryDelay = _retryDelay;
            while (true)
            {
                MySqlMigrationDatabaseLock dbLock = CreateMigrationDatabaseLock();
                object? insertCount = CreateInsertLockCommand(DateTimeOffset.UtcNow)
                    .ExecuteScalar(CreateRelationalCommandParameters());
                if ((long)insertCount! == 1)
                {
                    return dbLock;
                }

                Thread.Sleep(retryDelay);
                if (retryDelay < TimeSpan.FromMinutes(1))
                {
                    retryDelay = retryDelay.Add(retryDelay);
                }
            }
        }

        /// <inheritdoc />
        public override async Task<IMigrationsDatabaseLock> AcquireDatabaseLockAsync(
            CancellationToken cancellationToken = default)
        {
            Dependencies.MigrationsLogger.AcquiringMigrationLock();

            if (!InterpretExistsResult(
                await Dependencies.RawSqlCommandBuilder.Build(CreateExistsSql(LockTableName))
                    .ExecuteScalarAsync(CreateRelationalCommandParameters(), cancellationToken).ConfigureAwait(false)))
            {
                await CreateLockTableCommand().ExecuteNonQueryAsync(CreateRelationalCommandParameters(), cancellationToken)
                    .ConfigureAwait(false);
            }

            TimeSpan retryDelay = _retryDelay;
            while (true)
            {
                MySqlMigrationDatabaseLock dbLock = CreateMigrationDatabaseLock();
                object? insertCount = await CreateInsertLockCommand(DateTimeOffset.UtcNow)
                    .ExecuteScalarAsync(CreateRelationalCommandParameters(), cancellationToken)
                    .ConfigureAwait(false);
                if ((long)insertCount! == 1)
                {
                    return dbLock;
                }

                await Task.Delay(_retryDelay, cancellationToken).ConfigureAwait(true);
                if (retryDelay < TimeSpan.FromMinutes(1))
                {
                    retryDelay = retryDelay.Add(retryDelay);
                }
            }
        }

        /// <inheritdoc />
        public override string GetBeginIfExistsScript(string migrationId)
            => throw new NotSupportedException("MigrationScriptGenerationNotSupported");

        /// <inheritdoc />
        public override string GetBeginIfNotExistsScript(string migrationId)
            => throw new NotSupportedException("MigrationScriptGenerationNotSupported");

        /// <inheritdoc />
        public override string GetCreateIfNotExistsScript()
        {
            string script = GetCreateScript();
            return script.Insert(script.IndexOf("CREATE TABLE", StringComparison.Ordinal) + 12, " IF NOT EXISTS");
        }

        /// <inheritdoc />
        public override string GetEndIfScript()
            => throw new NotSupportedException("MigrationScriptGenerationNotSupported");
        #endregion

        #region Protected methods
        /// <inheritdoc />
        protected override bool InterpretExistsResult(object? value)
            => (long)value! != 0L;
        #endregion

        #region Private methods
        private IRelationalCommand CreateDeleteLockCommand(int? id = null)
        {
            string sql = $"""
            DELETE FROM `{LockTableName}`
            """;

            if (id != null)
            {
                sql += $""" WHERE `Id` = {id}""";
            }

            sql += ";";
            return Dependencies.RawSqlCommandBuilder.Build(sql);
        }

        private string CreateExistsSql(string tableName)
        {
            RelationalTypeMapping stringTypeMapping = Dependencies.TypeMappingSource.GetMapping(typeof(string));

            return $"""
            SELECT COUNT(*)
            FROM information_schema.tables
            WHERE table_schema = {stringTypeMapping.GenerateSqlLiteral(Dependencies.Connection.DbConnection.Database)}
            AND table_name = {stringTypeMapping.GenerateSqlLiteral(tableName)}
            LIMIT 1;
            """;
        }

        private IRelationalCommand CreateLockTableCommand()
            => Dependencies.RawSqlCommandBuilder.Build(
                $"""
                CREATE TABLE IF NOT EXISTS `{LockTableName}` (
                    `Id` INT NOT NULL,
                    `Timestamp` TEXT NOT NULL,
                    CONSTRAINT `PK_{LockTableName}` PRIMARY KEY (`Id`)
                );
                """);

        private IRelationalCommand CreateInsertLockCommand(DateTimeOffset timestamp)
        {
            string timestampLiteral = Dependencies.TypeMappingSource.GetMapping(typeof(DateTimeOffset)).GenerateSqlLiteral(timestamp);

            return Dependencies.RawSqlCommandBuilder.Build(
                $"""
                INSERT IGNORE INTO `{LockTableName}` (`Id`, `Timestamp`) VALUES(1, {timestampLiteral});
                SELECT ROW_COUNT();
                """);
        }

        private MySqlMigrationDatabaseLock CreateMigrationDatabaseLock()
            => new(CreateDeleteLockCommand(), CreateRelationalCommandParameters(), this);

        private RelationalCommandParameterObject CreateRelationalCommandParameters()
            => new(
                Dependencies.Connection,
                null,
                null,
                Dependencies.CurrentContext.Context,
                Dependencies.CommandLogger, CommandSource.Migrations);
        #endregion
    }
}