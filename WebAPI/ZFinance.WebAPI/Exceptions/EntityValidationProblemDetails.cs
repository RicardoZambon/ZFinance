using Microsoft.AspNetCore.Mvc;
using ZDatabase.Exceptions;

namespace ZFinance.WebAPI.Exceptions
{
    /// <inheritdoc />
    public class EntityValidationProblemDetails<TKey> : ValidationProblemDetails
    where TKey : struct
    {
        /// <summary>
        /// Gets or sets the entity key.
        /// </summary>
        /// <value>
        /// The entity key.
        /// </value>
        public TKey EntityKey { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityValidationProblemDetails{TKey}"/> class.
        /// </summary>
        /// <param name="validationEx">The validation exception.</param>
        public EntityValidationProblemDetails(EntityValidationFailureException<TKey> validationEx)
            : base(validationEx.ValidationResult.Errors)
        {
            EntityKey = validationEx.EntityKey;
        }
    }
}