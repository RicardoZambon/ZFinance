using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZDatabase.Attributes;
using ZSecurity.Interfaces;

namespace ZFinance.Core.Entities.Security
{
    /// <summary>
    /// Class for entity representation of the table <c>[Security].[Actions]</c> from database.
    /// </summary>
    /// <seealso cref="AuditableEntity" />
    /// <seealso cref="IActionEntity" />
    [Table(nameof(Actions), Schema = nameof(Security))]
    public class Actions : AuditableEntity, IActionEntity
    {
        #region Properties
        /// <inheritdoc />
        [Column(TypeName = "VARCHAR(150)")]
        public string? Code { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [StringLength(300)]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the entity.
        /// </summary>
        /// <value>
        /// The entity.
        /// </value>
        [Column(TypeName = "VARCHAR(100)")]
        public string? Entity { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Column(TypeName = "VARCHAR(150)")]
        public string? Name { get; set; }
        #endregion

        #region Relationships
        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        [AuditableRelation]
        public virtual ICollection<Roles>? Roles { get; set; }
        #endregion
    }

    /// <summary>
    /// Entity Framework configuration for <see cref="Actions"/>.
    /// Defines database mapping, relationships, and constraints for action records.
    /// </summary>
    /// <seealso cref="AuditableEntityConfiguration{TAuditableEntity}" />
    public class ActionsConfiguration : AuditableEntityConfiguration<Actions>
    {
        /// <inheritdoc />
        public override void Configure(EntityTypeBuilder<Actions> builder)
        {
            base.Configure(builder);
        }
    }
}