using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;
using ZDatabase.Attributes;

namespace ZFinance.Core.Entities.Security
{
    /// <summary>
    /// Class for entity representation of the table <c>[Security].[Roles]</c> from database.
    /// </summary>
    /// <seealso cref="AuditableEntity" />
    [Table(nameof(Roles), Schema = nameof(Security))]
    public class Roles : AuditableEntity
    {
        #region Properties
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Column(TypeName = "VARCHAR(200)")]
        public string? Name { get; set; }
        #endregion

        #region Relationships
        /// <summary>
        /// Gets or sets the actions.
        /// </summary>
        /// <value>
        /// As actions.
        /// </value>
        [AuditableRelation]
        public virtual ICollection<Actions>? Actions { get; set; }

        /// <summary>
        /// Gets or sets the menus.
        /// </summary>
        /// <value>
        /// The menus.
        /// </value>
        [AuditableRelation]
        public virtual ICollection<Menus>? Menus { get; set; }

        /// <summary>
        /// Gets or sets the users.
        /// </summary>
        /// <value>
        /// The users.
        /// </value>
        [AuditableRelation]
        public virtual ICollection<Users>? Users { get; set; }
        #endregion
    }

    /// <summary>
    /// Entity Framework configuration for <see cref="Roles"/>.
    /// Defines database mapping, relationships, and constraints for role records.
    /// </summary>
    /// <seealso cref="AuditableEntityConfiguration{TAuditableEntity}" />
    public class RolesConfiguration : AuditableEntityConfiguration<Roles>
    {
        /// <inheritdoc />
        public override void Configure(EntityTypeBuilder<Roles> builder)
        {
            base.Configure(builder);

            // Actions
            builder.HasMany(x => x.Actions)
                .WithMany(x => x.Roles)
                .UsingEntity<Dictionary<string, object>>(

                    r => r.HasOne<Actions>()
                            .WithMany()
                            .HasForeignKey("ActionID")
                            .OnDelete(DeleteBehavior.NoAction),

                    l => l.HasOne<Roles>()
                            .WithMany()
                            .HasForeignKey("RoleID")
                            .OnDelete(DeleteBehavior.NoAction),

                    x =>
                    {
                        x.ToTable($"{nameof(Roles)}{nameof(Actions)}", schema: nameof(Security));
                        x.HasKey("ActionID", "RoleID");
                    }
                );

            // Menus
            builder.HasMany(x => x.Menus)
                .WithMany(x => x.Roles)
                .UsingEntity<Dictionary<string, object>>(

                    r => r.HasOne<Menus>()
                            .WithMany()
                            .HasForeignKey("MenuID")
                            .OnDelete(DeleteBehavior.NoAction),

                    l => l.HasOne<Roles>()
                            .WithMany()
                            .HasForeignKey("RoleID")
                            .OnDelete(DeleteBehavior.NoAction),

                    x =>
                    {
                        x.ToTable($"{nameof(Roles)}{nameof(Menus)}", schema: nameof(Security));
                        x.HasKey("MenuID", "RoleID");
                    }
                );

            // Users
            builder.HasMany(x => x.Users)
                .WithMany(x => x.Roles)
                .UsingEntity<Dictionary<string, object>>(

                    r => r.HasOne<Users>()
                            .WithMany()
                            .HasForeignKey("UserID")
                            .OnDelete(DeleteBehavior.NoAction),

                    l => l.HasOne<Roles>()
                            .WithMany()
                            .HasForeignKey("RoleID")
                            .OnDelete(DeleteBehavior.NoAction),

                    x =>
                    {
                        x.ToTable($"{nameof(Roles)}{nameof(Users)}", schema: nameof(Security));
                        x.HasKey("RoleID", "UserID");
                    }
                );
        }
    }
}