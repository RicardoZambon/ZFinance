using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;
using ZDatabase.Attributes;

namespace ZFinance.Core.Entities.Security
{
    /// <summary>
    /// Class for entity representation of the table <c>[Security].[Menus]</c> from database.
    /// </summary>
    /// <seealso cref="AuditableEntity" />
    [Table(nameof(Menus), Schema = nameof(Security))]
    public class Menus : AuditableEntity
    {
        #region Properties
        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        /// <value>
        /// The icon.
        /// </value>
        [Column(TypeName = "VARCHAR(50)")]
        public string? Icon { get; set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        [Column(TypeName = "VARCHAR(100)")]
        public string? Label { get; set; }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the parent menu.
        /// </summary>
        /// <value>
        /// The parent menu.
        /// </value>
        public virtual Menus? ParentMenu { get; set; }

        /// <summary>
        /// Gets or sets the parent menu identifier.
        /// </summary>
        /// <value>
        /// The parent menu identifier.
        /// </value>
        public long? ParentMenuID { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        [Column(TypeName = "VARCHAR(200)")]
        public string? URL { get; set; }
        #endregion

        #region Relationships
        /// <summary>
        /// Gets or sets the child menus.
        /// </summary>
        /// <value>
        /// The child menus.
        /// </value>
        [AuditableRelation]
        public virtual ICollection<Menus>? ChildMenus { get; set; }

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
    /// Entity Framework configuration for <see cref="Menus"/>.
    /// Defines database mapping, relationships, and constraints for menu records.
    /// </summary>
    /// <seealso cref="AuditableEntityConfiguration{TAuditableEntity}" />
    public sealed class MenusConfiguration : AuditableEntityConfiguration<Menus>
    {
        /// <inheritdoc />
        public override void Configure(EntityTypeBuilder<Menus> builder)
        {
            base.Configure(builder);

            // ChildMenus
            builder.HasMany(x => x.ChildMenus)
                .WithOne(x => x.ParentMenu)
                .HasForeignKey(x => x.ParentMenuID)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}