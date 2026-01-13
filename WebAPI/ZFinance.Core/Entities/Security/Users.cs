using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;
using ZDatabase.Attributes;

namespace ZFinance.Core.Entities.Security
{
    /// <summary>
    /// Class for entity representation of the table <c>[Security].[Users]</c> from database.
    /// </summary>
    /// <seealso cref="AuditableEntity" />
    [Table(nameof(Users), Schema = nameof(Security))]
    public class Users : AuditableEntity
    {
        #region Properties
        /// <summary>
        /// Gets or sets the email of the user.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        [Column(TypeName = "VARCHAR(250)")]
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Users"/> is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the password hash.
        /// </summary>
        /// <value>
        /// The password hash.
        /// </value>
        public string? PasswordHash { get; set; }
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

        /// <summary>
        /// Gets or sets the refresh tokens.
        /// </summary>
        /// <value>
        /// The refresh tokens.
        /// </value>
        public virtual ICollection<RefreshTokens>? RefreshTokens { get; set; }
        #endregion
    }

    /// <summary>
    /// Entity Framework configuration for <see cref="Users"/>.
    /// Defines database mapping, relationships, and constraints for user records.
    /// </summary>
    /// <seealso cref="AuditableEntityConfiguration{TAuditableEntity}" />
    public class UsuariosConfiguration : AuditableEntityConfiguration<Users>
    {
        /// <inheritdoc />
        public override void Configure(EntityTypeBuilder<Users> builder)
        {
            base.Configure(builder);

            // RefreshTokens
            builder.HasMany(u => u.RefreshTokens)
                .WithOne(rt => rt.User)
                .HasForeignKey(rt => rt.UserID)
                .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.NoAction);
        }
    }
}