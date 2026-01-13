using ZDatabase.Services.Interfaces;
using ZFinance.Core.Entities.Audit;
using ZFinance.Core.Entities.Security;
using ZFinance.WebAPI.Services;
using ZFinance.WebAPI.Services.Interfaces;
using ZFinance.WebAPI.Services.Security;
using ZFinance.WebAPI.Services.Security.Interfaces;
using ZWebAPI.Services;
using ZWebAPI.Services.Interfaces;

namespace ZFinance.WebAPI.ExtensionMethods
{
    /// <summary>
    /// Provides extension methods for configuring dependency injection.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Registers all Web API services from System in the service collection.
        /// </summary>
        /// <param name="services">The service collection to which repositories will be added.</param>
        /// <returns>An updated service collection with all repositories registered.</returns>
        public static IServiceCollection AddWebAPIServices(this IServiceCollection services)
        {
            return services
                .AddScoped<IAuthenticationService, AuthenticationServiceDefault>()
                .AddScoped<ICurrentUserProvider<long>, CurrentUserProviderDefault>()

                // Audit
                .AddScoped<IAuditService<Users, long>, AuditServiceDefault<ServicesHistory, OperationsHistory, Users, long>>()

                // Security
                .AddScoped<IActionsService, ActionsServiceDefault>()
                .AddScoped<IMenusService, MenusServiceDefault>()
                .AddScoped<IRolesService, RolesServiceDefault>()
                .AddScoped<IUsersService, UsersServiceDefault>()
                ;
        }
    }
}