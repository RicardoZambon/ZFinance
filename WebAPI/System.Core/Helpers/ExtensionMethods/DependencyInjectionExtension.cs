using Microsoft.Extensions.DependencyInjection;
using Niten.Core.Entities.Audit;
using Niten.Core.Entities.Geral;
using Niten.Core.Entities.Seguranca;
using Niten.Core.Helpers.ExtensionMethods;
using Niten.Core.Repositories.Geral.Interfaces;
using Niten.Core.Repositories.Seguranca;
using Niten.Core.Repositories.Seguranca.Interfaces;
using Niten.System.Core.Repositories.Cache;
using Niten.System.Core.Repositories.Cache.Interfaces;
using Niten.System.Core.Repositories.Configs;
using Niten.System.Core.Repositories.Configs.Interfaces;
using Niten.System.Core.Repositories.Estoque;
using Niten.System.Core.Repositories.Estoque.Interfaces;
using Niten.System.Core.Repositories.Financeiro;
using Niten.System.Core.Repositories.Financeiro.Interfaces;
using Niten.System.Core.Repositories.Geral;
using Niten.System.Core.Repositories.Geral.Interfaces;
using Niten.System.Core.Repositories.Historico;
using Niten.System.Core.Repositories.Historico.Interfaces;
using Niten.System.Core.Repositories.Integracoes;
using Niten.System.Core.Repositories.Integracoes.Interfaces;
using Niten.System.Core.Repositories.PortalAluno;
using Niten.System.Core.Repositories.PortalAluno.Interfaces;
using Niten.System.Core.Repositories.Relatorios;
using Niten.System.Core.Repositories.Relatorios.Interfaces;
using Niten.System.Core.Repositories.Seguranca;
using Niten.System.Core.Repositories.Seguranca.Interfaces;
using Niten.System.Core.Repositories.TaskScheduler;
using Niten.System.Core.Repositories.TaskScheduler.Interfaces;
using Niten.System.Core.Repositories.Views;
using Niten.System.Core.Repositories.Views.Interfaces;
using ZDatabase.ExtensionMethods;
using ZSecurity.ExtensionMethods;
using ZSecurity.Services;
using ZTaskScheduler.Repositories;

namespace Niten.System.Core.Helpers.ExtensionMethods
{
    /// <summary>
    /// Provides extension methods for configuring dependency injection.
    /// </summary>
    public static class DependencyInjectionExtension
    {
        /// <summary>
        /// Registers all repositories from System Core in the service collection.
        /// </summary>
        /// <param name="services">The service collection to which repositories will be added.</param>
        /// <returns>An updated service collection with all repositories registered.</returns>
        public static IServiceCollection AddSystemCoreRepositories(this IServiceCollection services)
        {
            return services.AddCoreRepositories()

                // Audit
                .AddAuditRepositories<ServicesHistory, OperationsHistory, Cadastros, int>()

                // Cache
                .AddScoped<ICacheRepository, CacheRepository>()

                // Config
                .AddScoped<ICamposFiltrosOpcoesRepository, CamposFiltrosOpcoesRepository>()
                .AddScoped<ICamposFiltrosRepository, CamposFiltrosRepository>()
                .AddScoped<IDefinicaoComissoesComissionadosRepository, DefinicaoComissoesComissionadosRepository>()
                .AddScoped<IDefinicaoComissoesRepository, DefinicaoComissoesRepository>()
                .AddScoped<IDefinicaoComissoesUnidadesRepository, DefinicaoComissoesUnidadesRepository>()
                .AddScoped<IIdiomasRepository, IdiomasRepository>()
                .AddScoped<IMoedasRepository, MoedasRepository>()
                .AddScoped<IRecorrenciasRepository, RecorrenciasRepository>()
                .AddScoped<ITiposContratacoesRepository, TiposContratacoesRepository>()

                // Estoque
                .AddScoped<IMateriaisRepository, MateriaisRepository>()

                // Financeiro
                .AddScoped<ICobrancasRepository, CobrancasRepository>()
                .AddScoped<IComissoesRepository, ComissoesRepository>()
                .AddScoped<IComissoesTransacoesRepository, ComissoesTransacoesRepository>()
                .AddScoped<IContratacoesAnexosRepository, ContratacoesAnexosRepository>()
                .AddScoped<IContratacoesRepository, ContratacoesRepository>()
                .AddScoped<ISaldosRepository, SaldosRepository>()
                .AddScoped<INotasFiscaisRepository, NotasFiscaisRepository>()
                .AddScoped<IPagamentosOnlineFiltrosRepository, PagamentosOnlineFiltrosRepository>()
                .AddScoped<IPagamentosOnlineRepository, PagamentosOnlineRepository>()
                .AddScoped<IPlanosRepository, PlanosRepository>()
                .AddScoped<IPlanosFiltrosRepository, PlanosFiltrosRepository>()
                .AddScoped<ITabelasValoresMateriaisRepository, TabelasValoresMateriaisRepository>()
                .AddScoped<ITabelasValoresModificadoresRepository, TabelasValoresModificadoresRepository>()
                .AddScoped<ITabelasValoresRepository, TabelasValoresRepository>()
                .AddScoped<ITransacoesRepository, TransacoesRepository>()

                // Geral
                .AddScoped<ICadastrosRepository, CadastrosRepository>()
                .AddScoped<ICadastrosRepositoryCore, CadastrosRepository>()
                .AddScoped<IComissionadosRepository, ComissionadosRepository>()
                .AddScoped<IGraduacoesRepository, GraduacoesRepository>()
                .AddScoped<IModalidadesRepository, ModalidadesRepository>()
                .AddScoped<IUnidadesCoordenadoresRepository, UnidadesCoordenadoresRepository>()
                .AddScoped<IUnidadesRepository, UnidadesRepository>()
                .AddScoped<IUnidadesTabelasValoresRepository, UnidadesTabelasValoresRepository>()

                // Historico
                .AddScoped<ICadastrosHistoricoRepository, CadastrosHistoricoRepository>()

                // Integrações
                .AddScoped<ICheckoutsItensRepository, CheckoutsItensRepository>()
                .AddScoped<ICheckoutsPagamentosRepository, CheckoutsPagamentosRepository>()
                .AddScoped<ICheckoutsRepository, CheckoutsRepository>()
                .AddScoped<IPerfisPagSeguroRepository, PerfisPagSeguroRepository>()
                .AddScoped<IPerfisSafraPayRepository, PerfisSafraPayRepository>()
                .AddScoped<IPerfisRepository, PerfisRepository>()

                // PortalAluno
                .AddScoped<IGruposRepository, GruposRepository>()
                .AddScoped<IMatriculasEnviosFormularioRepository, MatriculasEnviosFormularioRepository>()
                .AddScoped<IMatriculasRepository, MatriculasRepository>()
                .AddScoped<IPostagensMuralRepository, PostagensMuralRepository>()
                .AddScoped<IPostagensMuralTraducoesRepository, PostagensMuralTraducoesRepository>()
                .AddScoped<ITiposPlanosRepository, TiposPlanosRepository>()

                // Relatórios
                .AddScoped<IPresencasSemMatriculasRepository, PresencasSemMatriculasRepository>()

                // Seguranca
                .AddScoped<IActionsRepository, ActionsRepository>()
                .AddScoped<ILoginLogsRepository, LoginLogsRepository>()
                .AddScoped<IMenusRepository, MenusRepository>()
                .AddScoped<IRefreshTokensRepository, RefreshTokensRepository>()
                .AddScoped<IRolesRepository, RolesRepository>()
                .AddScoped<IUsuariosRepository, UsuariosRepository>()

                // TaskScheduler
                .AddZTaskScheduler()
                .AddScoped<IEventsRepository, EventsRepository>()
                .AddScoped<IJobsRepository, JobsRepository>()
                .AddScoped<ITriggersRepository, TriggersRepository>()

                // Views
                .AddScoped<IViewCobrancasRepository, ViewCobrancasRepository>()
                .AddScoped<IViewContratacoesRepository, ViewContratacoesRepository>()
                .AddScoped<IViewGraduacoesPendentesEstornoRepository, ViewGraduacoesPendentesEstornoRepository>()
                .AddScoped<IViewGraduacoesPendentesGerarCobrancasRepository, ViewGraduacoesPendentesGerarCobrancasRepository>()
                .AddScoped<IViewMatriculasRepository, ViewMatriculasRepository>()
                .AddScoped<IViewNotasFiscaisRepository, ViewNotasFiscaisRepository>()
                .AddScoped<IViewPagamentosOnlineRepository, ViewPagamentosOnlineRepository>()
                .AddScoped<IViewTransacoesSemNotasFiscaisRepository, ViewTransacoesSemNotasFiscaisRepository>()
                ;
        }

        /// <summary>
        /// Registers all services from System Core in the service collection.
        /// </summary>
        /// <param name="services">The service collection to which services will be added.</param>
        /// <returns>An updated service collection with all services registered.</returns>
        public static IServiceCollection AddSystemCoreServices(this IServiceCollection services)
        {
            return Niten.Core.Helpers.ExtensionMethods.DependencyInjectionExtension.AddCoreServices(services)
                .AddSecurityHandlerService<SecurityHandler<IUsuariosRepository, Actions, int>>()
                ;
        }
    }
}