using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZFinance.Core.Entities.Security;
using ZDatabase.Entities;

namespace ZFinance.Core.Entities
{
    /// <summary>
    /// Abstract base class for auditable entities, tracking creation and modification details.
    /// </summary>
    /// <seealso cref="AuditableEntity{TUsers, TUsersKey}" />
    public abstract class AuditableEntity : AuditableEntity<Users, long>
    {
        #region Properties
        #endregion

        #region Relationships
        #endregion
    }

    /// <summary>
    /// Entity Framework configuration for <see cref="AuditableEntity"/>.
    /// Defines database mapping, relationships, and constraints for operation audit records.
    /// </summary>
    /// <typeparam name="TAuditableEntity">The type of the auditable entity.</typeparam>
    /// <seealso cref="AuditableEntityConfiguration{TAuditableEntity, TUsers, TUsersKey}" />
    public class AuditableEntityConfiguration<TAuditableEntity> : AuditableEntityConfiguration<TAuditableEntity, Users, long>
        where TAuditableEntity : AuditableEntity
    {
        /// <inheritdoc />
        public override void Configure(EntityTypeBuilder<TAuditableEntity> builder)
        {
            base.Configure(builder);
        }
    }
}