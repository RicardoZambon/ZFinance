using Microsoft.Extensions.DependencyInjection;
using ZDatabase.ExtensionMethods;
using ZDatabase.Services.Interfaces;
using ZFinance.Core.Entities.Audit;
using ZFinance.Core.Entities.Security;
using ZFinance.Core.Repositories.Security;
using ZFinance.Core.Repositories.Security.Interfaces;
using ZFinance.Core.Services;
using ZFinance.Core.Services.Interfaces;
using ZSecurity.ExtensionMethods;
using ZSecurity.Services;

namespace ZFinance.Core.ExtensionMethods
{
    /// <summary>
    /// Extension methods for DI.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Adds the repositories.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                // Audit
                .AddAuditRepositories<ServicesHistory, OperationsHistory, Users, long>()

                // Configs

                // Security
                .AddScoped<IActionsRepository, ActionsRepository>()
                .AddScoped<IMenusRepository, MenusRepository>()
                .AddScoped<IRefreshTokensRepository, RefreshTokensRepository>()
                .AddScoped<IRolesRepository, RolesRepository>()
                .AddScoped<IUsersRepository, UsersRepository>()
                ;
        }

        /// <summary>
        /// Adds the core services.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            return services
                .AddScoped<IAuditHandler, CoreAuditHandler>()
                .AddScoped<IExceptionHandler, ExceptionHandler>()
                .AddScoped<IInformationProvider, InformationProvider>()
                .AddSecurityHandlerService<SecurityHandler<IInformationProvider, Actions, long>>()
                ;
        }
    }
}
