using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZFinance.Core.Entities.Security;
using System.ComponentModel.DataAnnotations.Schema;
using ZDatabase.Entities.Audit;

namespace ZFinance.Core.Entities.Audit
{
    /// <summary>
    /// Class for entity representation of the table <c>[Audit].[ServicesHistory]</c> from database.
    /// </summary>
    /// <seealso cref="ServicesHistory{TServicesHistory, TOperationsHistory, TUsers, TUsersKey}" />
    /// <remarks>
    /// This entity is part of the audit trail system, where:
    /// - <see cref="ServicesHistory"/> tracks high-level service executions
    /// - <see cref="OperationsHistory"/> tracks individual operations within those services
    /// Each operation is linked to a user (<see cref="Users"/>) who performed it.
    /// </remarks>
    [Table(nameof(ServicesHistory), Schema = nameof(Audit))]
    public class ServicesHistory : ServicesHistory<ServicesHistory, OperationsHistory, Users, long>
    {
        #region Properties
        #endregion

        #region Relationships
        #endregion
    }

    /// <summary>
    /// Entity Framework configuration for <see cref="ServicesHistory"/>.
    /// Defines database mapping, relationships, and constraints for services audit records.
    /// </summary>
    /// <seealso cref="ServicesHistoryConfiguration{TOperationsHistory, TServicesHistory, TUsers, TUsersKey}" />
    public class ServicesHistoryConfiguration : ServicesHistoryConfiguration<ServicesHistory, OperationsHistory, Users, long>
    {
        /// <inheritdoc />
        public override void Configure(EntityTypeBuilder<ServicesHistory> builder)
        {
            base.Configure(builder);
        }
    }
}