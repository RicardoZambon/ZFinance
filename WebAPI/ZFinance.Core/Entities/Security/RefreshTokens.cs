using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZDatabase.Entities;
using ZDatabase.ValueGenerators;

namespace ZFinance.Core.Entities.Security
{
    /// <summary>
    /// Class for entity representation of the table <c>[Security].[RefreshTokens]</c> from database.
    /// </summary>
    /// <seealso cref="AuditableEntity" />
    [Table(nameof(RefreshTokens), Schema = nameof(Security))]
    public class RefreshTokens : BaseEntity
    {
        #region Properties
        /// <summary>
        /// Gets or sets the created on.
        /// </summary>
        /// <value>
        /// Tthe created on.
        /// </value>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the expiration.
        /// </summary>
        /// <value>
        /// The expiration.
        /// </value>
        public DateTime Expiration { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [Key]
        public long ID { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive => RevokedOn == null && !IsExpired;

        /// <summary>
        /// Gets a value indicating whether this instance is expired.
        /// </summary>
        /// <value>
        ///   <c>true</c> if is expired; otherwise, <c>false</c>.
        /// </value>
        public bool IsExpired => DateTime.UtcNow >= Expiration;

        /// <summary>
        /// Gets or sets the revoked on.
        /// </summary>
        /// <value>
        /// The revoked on.
        /// </value>
        public DateTime? RevokedOn { get; set; }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        [Column(TypeName = "VARCHAR(50)")]
        public string? Token { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public virtual Users? User { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public long UserID { get; set; }
        #endregion Properties

        #region Relationships
        #endregion
    }

    /// <summary>
    /// Entity Framework configuration for <see cref="RefreshTokens"/>.
    /// Defines database mapping, relationships, and constraints for refresh token records.
    /// </summary>
    /// <seealso cref="BaseEntityConfiguration{TAuditableEntity}" />
    public class RefreshTokensConfiguration : BaseEntityConfiguration<RefreshTokens>
    {
        /// <inheritdoc />
        public override void Configure(EntityTypeBuilder<RefreshTokens> builder)
        {
            base.Configure(builder);

            // CreatedOn
            builder.Property(x => x.CreatedOn)
                .ValueGeneratedOnAdd()
                .HasValueGenerator<DateTimeUtcGenerator>();

            // User
            builder.HasOne(x => x.User)
                .WithMany(x => x.RefreshTokens)
                .HasForeignKey(x => x.UserID)
                .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.NoAction);
        }
    }
}