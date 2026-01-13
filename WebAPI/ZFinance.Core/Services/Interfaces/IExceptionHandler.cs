namespace ZFinance.Core.Services.Interfaces
{
    /// <summary>
    /// Service for handling exceptions into Sentry.
    /// </summary>
    public interface IExceptionHandler
    {
        /// <summary>
        /// Adds the breadcrumb.
        /// </summary>
        void AddBreadcrumb();

        /// <summary>
        /// Adds the breadcrumb.
        /// </summary>
        /// <param name="description">The description.</param>
        void AddBreadcrumb(string description);

        /// <summary>
        /// Adds the breadcrumb.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        void AddBreadcrumb(IDictionary<string, object?> parameters);

        /// <summary>
        /// Adds the breadcrumb.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="parameters">The parameters.</param>
        void AddBreadcrumb(string description, IDictionary<string, object?> parameters);

        /// <summary>
        /// Captures the exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>The error identifier.</returns>
        Guid? CaptureException(Exception exception);
    }
}