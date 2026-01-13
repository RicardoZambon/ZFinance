using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using ZDatabase.Interfaces;
using ZDatabase.Services.Interfaces;
using ZFinance.Core.Entities.Security;
using ZFinance.Core.Services.Interfaces;

namespace ZFinance.Core.Services
{
    /// <inheritdoc />
    public class ExceptionHandler : IExceptionHandler
    {
        #region Variables
        private readonly ICurrentUserProvider<long> currentUserProvider;
        private readonly IDbContext dbContext;
        private readonly IHub hub;
        #endregion

        #region Properties
        private readonly IList<Breadcrumb> breadcrumbs = [];
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandler"/> class.
        /// </summary>
        /// <param name="currentUserProvider">The <see cref="ICurrentUserProvider{TUsersKey}"/> instance.</param>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="hub">The <see cref="IHub"/> instance.</param>
        public ExceptionHandler(
            ICurrentUserProvider<long> currentUserProvider,
            IDbContext dbContext,
            IHub hub)
        {
            this.currentUserProvider = currentUserProvider;
            this.dbContext = dbContext;
            this.hub = hub;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public void AddBreadcrumb()
        {
            AddBreadcrumb(string.Empty, new Dictionary<string, object?>() { });
        }

        /// <inheritdoc />
        public void AddBreadcrumb(string description)
        {
            AddBreadcrumb(description, new Dictionary<string, object?>() { });
        }

        /// <inheritdoc />
        public void AddBreadcrumb(IDictionary<string, object?> parameters)
        {
            AddBreadcrumb(string.Empty, parameters);
        }

        /// <inheritdoc />
        public void AddBreadcrumb(string description, IDictionary<string, object?> parameters)
        {
            IDictionary<string, string> data = new Dictionary<string, string>();

            foreach (KeyValuePair<string, object?> parameter in parameters)
            {
                data.Add(parameter.Key, Serialize(parameter.Value));
            }

            KeyValuePair<string, string> methodAndClassNames = GetMethodAndClassCallerName();

            string className = methodAndClassNames.Key;
            string methodName = methodAndClassNames.Value;

            string message = $"{methodName}:";
            if (!string.IsNullOrWhiteSpace(description))
            {
                message += $"\n{description}";
            }

            if (data.Any())
            {
                breadcrumbs.Add(new(message, string.Empty, new ReadOnlyDictionary<string, string>(data), className));
            }
            else
            {
                breadcrumbs.Add(new(message, string.Empty, null, className));
            }
        }

        /// <inheritdoc />
        public Guid? CaptureException(Exception exception)
        {
            foreach (Breadcrumb breadcrumb in breadcrumbs.Reverse())
            {
                hub.AddBreadcrumb(breadcrumb);
            }

            Guid errorId = Guid.NewGuid();
            hub.ConfigureScope(async scope =>
            {
                scope.SetTag("ErrorId", errorId.ToString());
                scope.UnsetTag("ActionId");
                scope.UnsetTag("ConnectionId");
                scope.UnsetTag("RequestId");

                if (currentUserProvider.CurrentUserID is long currentUserID
                    && await dbContext.FindAsync<Users>(currentUserID) is Users user)
                {
                    scope.User = new SentryUser()
                    {
                        Id = currentUserID.ToString(),
                        Username = user.Email,
                    };
                }
            });

            hub.CaptureException(exception);
            hub.Flush();

            return errorId;
        }
        #endregion

        #region Private methods
        private static KeyValuePair<string, string> GetMethodAndClassCallerName()
        {
            string[] ignoreMethodNames = new string[] {
                "MoveNext",
                "Start",
                "GetMethodAndClassCallerName",
                "AddBreadcrumb"
            };

            StackTrace stackTrace = new();
            foreach (StackFrame frame in stackTrace.GetFrames())
            {
                MethodBase? method = frame.GetMethod();
                if (method is not null && method.DeclaringType is not null
                    && !ignoreMethodNames.Contains(method.Name))
                {
                    return new KeyValuePair<string, string>(method.DeclaringType.Name, method.Name);
                }
            }
            return new KeyValuePair<string, string>(string.Empty, string.Empty);
        }

        private static IDictionary<string, object> GetPrimitives(object obj)
        {
            IDictionary<string, object> primitiveProperties = new Dictionary<string, object>();

            foreach (PropertyInfo property in obj.GetType().GetProperties())
            {
                if (IsPrimitive(property.PropertyType))
                {
                    object? value = property.GetValue(obj);
                    if (value is not null)
                    {
                        primitiveProperties[property.Name] = value;
                    }
                }
            }

            return primitiveProperties;
        }

        private static bool IsList(Type type)
            => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>) || type.IsArray;

        private static bool IsPrimitive(Type type)
            => type.IsPrimitive || type == typeof(string);

        private static string Serialize(object? obj)
        {
            if (obj is null)
            {
                return "null";
            }
            else if (IsPrimitive(obj.GetType()))
            {
                return obj.ToString() ?? "null";
            }
            else if (IsList(obj.GetType()))
            {
                return SerializeList((IEnumerable)obj);
            }
            else
            {
                return SerializePrimitives(obj);
            }
        }

        private static string SerializeList(IEnumerable list)
        {
            List<object> listContent = new();

            foreach (object item in list)
            {
                object value;
                if (item is null)
                {
                    value = "null";
                }
                else if (IsPrimitive(item.GetType()))
                {
                    value = item.ToString() ?? "null";
                }
                else if (IsList(item.GetType()))
                {
                    value = SerializeList((IEnumerable)item);
                }
                else
                {
                    value = GetPrimitives(item);
                }

                listContent.Add(value);
            }
            return JsonSerializer.Serialize(listContent, new JsonSerializerOptions() { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
        }

        private static string SerializePrimitives(object obj)
            => JsonSerializer.Serialize(GetPrimitives(obj), new JsonSerializerOptions() { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
        #endregion
    }
}