using Microsoft.EntityFrameworkCore;
using Niten.Core.Entities.Financeiro;
using Niten.Core.Entities.PortalAluno;
using Niten.Core.Entities.Seguranca;
using System.Diagnostics.CodeAnalysis;
using ZDatabase.Interfaces;
using ZTaskScheduler.Entities;

namespace Niten.System.Core.DataInitializers
{
    /// <summary>
    /// Data initializer for version 1.2.21.
    /// </summary>
    /// <seealso cref="BaseInitializer" />
    [ExcludeFromCodeCoverage]
    public class v1221Initializer : BaseInitializer
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="v1221Initializer"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        public v1221Initializer(IDbContext dbContext)
            : base(dbContext)
        {
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public override void Initialize()
        {
            // PortalAluno
            UpdateGrupos();
            InsertTiposPlanos();

            // Financeiro
            UpdatePlanos();

            // Segurança
            InsertActions();
            InsertMenus();
            UpdateActions();
            UpdateMenus();

            // TaskScheduler
            //InsertJobs();
            //InsertTriggers();
            //UpdateJobs();
            //UpdateTriggers();
        }
        #endregion

        #region Private methods
        private void InsertActions()
        {
            IEnumerable<Actions> actions = [
                new() { Entity = nameof(TiposPlanos), Name = "AtualizarTipoPlanoAsync", Description = "Atualiza um tipo de plano de forma assíncrona.", Code = "ITiposPlanosService.AtualizarTipoPlanoAsync" },
                new() { Entity = nameof(TiposPlanos), Name = "AuditarTipoPlanoHistoryOperationsAsync", Description = "Obtêm lista de auditoria com as operações realizadas no histórico de serviço do tipo de plano de forma assíncrona.", Code = "ITiposPlanosService.AuditarTipoPlanoHistoryOperationsAsync" },
                new() { Entity = nameof(TiposPlanos), Name = "AuditarTipoPlanoHistoryServicesAsync", Description = "Obtêm lista de auditoria com os históricos de serviços realizados no tipo de plano de forma assíncrona.", Code = "ITiposPlanosService.AuditarTipoPlanoHistoryServicesAsync" },
                new() { Entity = nameof(TiposPlanos), Name = "CatalogarTiposPlanosAsync", Description = "Cataloga todos os tipos de planos de forma assíncrona.", Code = "ITiposPlanosService.CatalogarTiposPlanosAsync" },
                new() { Entity = nameof(TiposPlanos), Name = "EncontrarTipoPlanoPorIDAsync", Description = "Encontra um tipo de plano pelo ID de forma assíncrona.", Code = "ITiposPlanosService.EncontrarTipoPlanoPorIDAsync" },
                new() { Entity = nameof(TiposPlanos), Name = "ExcluirTipoPlanoAsync", Description = "Exclui um tipo de plano pelo ID de forma assíncrona.", Code = "ITiposPlanosService.ExcluirTipoPlanoAsync" },
                new() { Entity = nameof(TiposPlanos), Name = "InserirNovoTipoPlanoAsync", Description = "Insere um novo tipo de plano de forma assíncrona.", Code = "ITiposPlanosService.InserirNovoTipoPlanoAsync" },
                new() { Entity = nameof(TiposPlanos), Name = "ListarTiposPlanosAsync", Description = "Obtêm lista com todos os tipos de planos de forma assíncrona.", Code = "ITiposPlanosService.ListarTiposPlanosAsync" },
            ];

            if (actions.Any())
            {
                SaveContext(actions, nameof(InsertActions), x => x.Code);
            }
        }

        private void InsertJobs()
        {
            IEnumerable<Jobs> jobs =
            [
                //// RefreshCacheTabelasValores
                //new() {
                //    Description = "Atualiza o cache com os valores de materaiais para as unidades de acordo com as tabelas de valores.",
                //    IsEnabled = true,
                //    LastEventDate = null,
                //    LastEventStatus = null,
                //    Name = "RefreshCacheTabelasValores",
                //},
            ];

            if (jobs.Any())
            {
                SaveContext(jobs, nameof(InsertJobs), x => x.Name);
            }
        }

        private void InsertMenus()
        {
            IList<Menus> menus = [];

            if (dbContext.Set<Menus>().FirstOrDefault(x => EF.Functions.Like(x.Label!, "Menu-PortalAluno")) is Menus portalAlunoMenu)
            {
                menus.Add(new Menus
                {
                    ID = 0,
                    Label = "Menu-PortalAluno-TiposPlanos",
                    Order = 4,
                    ParentMenu = portalAlunoMenu,
                    ParentMenuID = portalAlunoMenu.ID,
                    URL = "/portal-aluno/tipos-planos",
                });
            }

            if (menus.Any())
            {
                SaveContext(menus, nameof(InsertMenus), x => x.Label);
            }
        }

        private void InsertTiposPlanos()
        {
            IList<TiposPlanos> tiposPlanos = [
                new() { Nome = "Trimestral", NomeExibicao = "Planos-Trimestral", Quantidade = 3 },
                new() { Nome = "Semestral", NomeExibicao = "Planos-Semestral", Quantidade = 6 },
                new() { Nome = "Anual", NomeExibicao = "Planos-Anual", Quantidade = 12 },
            ];

            if (tiposPlanos.Any())
            {
                SaveContext(tiposPlanos, nameof(InsertTiposPlanos), EntityState.Added);
            }
        }

        private void InsertTriggers()
        {
            IList<Triggers> triggers = [];

            // RefreshCacheTabelasValores
            if (dbContext.Set<Jobs>().FirstOrDefault(x => x.Name == "RefreshCacheTabelasValores") is Jobs refreshTabelasValoresJob && (refreshTabelasValoresJob.Triggers?.Count ?? 0) == 0)
            {
                //triggers.Add(new Triggers
                //{
                //    ActivatesOn = DateTime.Today,
                //    DaysOfWeek = "1111111",
                //    IntervalQuantity = 1,
                //    IntervalType = IntervalTypes.Day,
                //    IsEnabled = true,
                //    Job = refreshTabelasValoresJob,
                //    JobID = refreshTabelasValoresJob.ID,
                //});
            }

            if (triggers.Any())
            {
                SaveContext(triggers, nameof(InsertTriggers), EntityState.Added);
            }
        }

        private void UpdateActions()
        {
            IList<Actions> actions = [];

            // IGruposPagamentosOnlineService.AtualizarGrupoPagamentosOnlineAsync
            if (dbContext.Set<Actions>().FirstOrDefault(x => EF.Functions.Like(x.Code!, "IGruposPagamentosOnlineService.AtualizarGrupoPagamentosOnlineAsync")) is Actions atualizarGrupoPagamentosOnline)
            {
                atualizarGrupoPagamentosOnline.Code = "IGruposService.AtualizarGrupoAsync";
                atualizarGrupoPagamentosOnline.Description = "Atualiza um grupo de forma assíncrona.";
                atualizarGrupoPagamentosOnline.Entity = "Grupos";
                atualizarGrupoPagamentosOnline.Name = "AtualizarGrupoAsync";

                actions.Add(atualizarGrupoPagamentosOnline);
            }

            // IGruposPagamentosOnlineService.AuditarGrupoPagamentosOnlineHistoryOperationsAsync
            if (dbContext.Set<Actions>().FirstOrDefault(x => EF.Functions.Like(x.Code!, "IGruposPagamentosOnlineService.AuditarGrupoPagamentosOnlineHistoryOperationsAsync")) is Actions auditarOperationsGrupoPagamentosOnline)
            {
                auditarOperationsGrupoPagamentosOnline.Code = "IGruposService.AuditarGrupoHistoryOperationsAsync";
                auditarOperationsGrupoPagamentosOnline.Description = "Obtêm lista de auditoria com operações realizadas no serviço do grupo de forma assíncrona.";
                auditarOperationsGrupoPagamentosOnline.Entity = "Grupos";
                auditarOperationsGrupoPagamentosOnline.Name = "AuditarGrupoHistoryOperationsAsync";

                actions.Add(auditarOperationsGrupoPagamentosOnline);
            }

            // IGruposPagamentosOnlineService.AuditarGrupoPagamentosOnlineHistoryServicesAsync
            if (dbContext.Set<Actions>().FirstOrDefault(x => EF.Functions.Like(x.Code!, "IGruposPagamentosOnlineService.AuditarGrupoPagamentosOnlineHistoryServicesAsync")) is Actions auditarServicesGrupoPagamentosOnline)
            {
                auditarServicesGrupoPagamentosOnline.Code = "IGruposService.AuditarGrupoHistoryServicesAsync";
                auditarServicesGrupoPagamentosOnline.Description = "Obtêm lista de auditoria com serviços realizados no grupo de forma assíncrona.";
                auditarServicesGrupoPagamentosOnline.Entity = "Grupos";
                auditarServicesGrupoPagamentosOnline.Name = "AuditarGrupoHistoryServicesAsync";

                actions.Add(auditarServicesGrupoPagamentosOnline);
            }

            // IGruposPagamentosOnlineService.CatalogarGruposPagamentosOnlineAsync
            if (dbContext.Set<Actions>().FirstOrDefault(x => EF.Functions.Like(x.Code!, "IGruposPagamentosOnlineService.CatalogarGruposPagamentosOnlineAsync")) is Actions catalogarGrupoPagamentosOnline)
            {
                catalogarGrupoPagamentosOnline.Code = "IGruposService.CatalogarGruposAsync";
                catalogarGrupoPagamentosOnline.Description = "Cataloga todos os grupos de forma assíncrona.";
                catalogarGrupoPagamentosOnline.Entity = "Grupos";
                catalogarGrupoPagamentosOnline.Name = "CatalogarGruposAsync";

                actions.Add(catalogarGrupoPagamentosOnline);
            }

            // IGruposPagamentosOnlineService.EncontrarGrupoPagamentosOnlinePorIDAsync
            if (dbContext.Set<Actions>().FirstOrDefault(x => EF.Functions.Like(x.Code!, "IGruposPagamentosOnlineService.EncontrarGrupoPagamentosOnlinePorIDAsync")) is Actions encontrarGrupoPagamentosOnline)
            {
                encontrarGrupoPagamentosOnline.Code = "IGruposService.EncontrarGrupoPorIDAsync";
                encontrarGrupoPagamentosOnline.Description = "Encontra um grupo pelo ID de forma assíncrona.";
                encontrarGrupoPagamentosOnline.Entity = "Grupos";
                encontrarGrupoPagamentosOnline.Name = "EncontrarGrupoPorIDAsync";

                actions.Add(encontrarGrupoPagamentosOnline);
            }

            // IGruposPagamentosOnlineService.ExcluirGrupoPagamentosOnlineAsync
            if (dbContext.Set<Actions>().FirstOrDefault(x => EF.Functions.Like(x.Code!, "IGruposPagamentosOnlineService.ExcluirGrupoPagamentosOnlineAsync")) is Actions excluirGrupoPagamentosOnline)
            {
                excluirGrupoPagamentosOnline.Code = "IGruposService.ExcluirGrupoAsync";
                excluirGrupoPagamentosOnline.Description = "Exclui um grupo pelo ID de forma assíncrona.";
                excluirGrupoPagamentosOnline.Entity = "Grupos";
                excluirGrupoPagamentosOnline.Name = "ExcluirGrupoAsync";

                actions.Add(excluirGrupoPagamentosOnline);
            }

            // IGruposPagamentosOnlineService.InserirNovoGrupoPagamentosOnlineAsync
            if (dbContext.Set<Actions>().FirstOrDefault(x => EF.Functions.Like(x.Code!, "IGruposPagamentosOnlineService.InserirNovoGrupoPagamentosOnlineAsync")) is Actions inserirGrupoPagamentosOnline)
            {
                inserirGrupoPagamentosOnline.Code = "IGruposService.InserirNovoGrupoAsync";
                inserirGrupoPagamentosOnline.Description = "Insere um novo grupo de forma assíncrona.";
                inserirGrupoPagamentosOnline.Entity = "Grupos";
                inserirGrupoPagamentosOnline.Name = "InserirNovoGrupoAsync";

                actions.Add(inserirGrupoPagamentosOnline);
            }

            // IGruposPagamentosOnlineService.ListarGruposPagamentosOnlineAsync
            if (dbContext.Set<Actions>().FirstOrDefault(x => EF.Functions.Like(x.Code!, "IGruposPagamentosOnlineService.ListarGruposPagamentosOnlineAsync")) is Actions listarGrupoPagamentosOnline)
            {
                listarGrupoPagamentosOnline.Code = "IGruposService.ListarGruposAsync";
                listarGrupoPagamentosOnline.Description = "Obtêm lista com todos os grupos de forma assíncrona.";
                listarGrupoPagamentosOnline.Entity = "Grupos";
                listarGrupoPagamentosOnline.Name = "ListarGruposAsync";

                actions.Add(listarGrupoPagamentosOnline);
            }

            if (actions.Any())
            {
                SaveContext(actions, nameof(UpdateActions), EntityState.Modified);
            }
        }

        private void UpdateGrupos()
        {
            IList<Grupos> grupos = [];

            // GruposPagamentosOnline-Certificados
            if (dbContext.Set<Grupos>().FirstOrDefault(x => EF.Functions.Like(x.NomeExibicao!, "GruposPagamentosOnline-Certificados")) is Grupos certificadosGrupo)
            {
                certificadosGrupo.NomeExibicao = "Grupos-Certificados";
                grupos.Add(certificadosGrupo);
            }

            // GruposPagamentosOnline-ContratosAluguel
            if (dbContext.Set<Grupos>().FirstOrDefault(x => EF.Functions.Like(x.NomeExibicao!, "GruposPagamentosOnline-ContratosAluguel")) is Grupos contratosAluguelGrupo)
            {
                contratosAluguelGrupo.NomeExibicao = "Grupos-ContratosAluguel";
                grupos.Add(contratosAluguelGrupo);
            }

            // GruposPagamentosOnline-Eventos
            if (dbContext.Set<Grupos>().FirstOrDefault(x => EF.Functions.Like(x.NomeExibicao!, "GruposPagamentosOnline-Eventos")) is Grupos eventosGrupo)
            {
                eventosGrupo.NomeExibicao = "Grupos-Eventos";
                grupos.Add(eventosGrupo);
            }

            // GruposPagamentosOnline-Mensalidades
            if (dbContext.Set<Grupos>().FirstOrDefault(x => EF.Functions.Like(x.NomeExibicao!, "GruposPagamentosOnline-Mensalidades")) is Grupos mensalidadesGrupo)
            {
                mensalidadesGrupo.NomeExibicao = "Grupos-Mensalidades";
                grupos.Add(mensalidadesGrupo);
            }

            if (grupos.Any())
            {
                SaveContext(grupos, nameof(UpdateGrupos), EntityState.Modified);
            }
        }

        private void UpdateJobs()
        {
            IList<Jobs> jobs = [];

            //if (dbContext.Set<Jobs>().FirstOrDefault(x => x.Name == "RefreshGlobalCache") is Jobs refreshGlobalCacheJob)
            //{
            //    refreshGlobalCacheJob.Name = "RefreshCachePagamentos";
            //    refreshGlobalCacheJob.Description = "Atualiza o cache com os valores de pagamentos online e planos.";
            //    jobs.Add(refreshGlobalCacheJob);
            //}

            if (jobs.Any())
            {
                SaveContext(jobs, nameof(UpdateJobs), EntityState.Modified);
            }
        }

        private void UpdatePlanos()
        {
            IList<Planos> planos = [];

            IEnumerable<TiposPlanos> tiposPlanos = [.. dbContext.Set<TiposPlanos>()];

            IEnumerable<Planos> planosToUpdate = dbContext.Set<Planos>()
                .ToList()
                .Where(p => p.TipoPlanoID == null || (p.Nome!.IndexOf('(') > 0 && p.Nome!.IndexOf(')') > 0));
            foreach (Planos plano in planosToUpdate)
            {
                if (plano.TipoPlanoID is null)
                {
                    TiposPlanos? tipoPlano = null;
                    if (plano.Nome?.Contains("Anual") ?? false)
                    {
                        tipoPlano = tiposPlanos
                            .FirstOrDefault(tp => tp.Nome == "Anual");
                    }
                    else if (plano.Nome?.Contains("Semestral") ?? false)
                    {
                        tipoPlano = tiposPlanos
                            .FirstOrDefault(tp => tp.Nome == "Semestral");
                    }
                    else if (plano.Nome?.Contains("Trimestral") ?? false)
                    {
                        tipoPlano = tiposPlanos
                            .FirstOrDefault(tp => tp.Nome == "Trimestral");
                    }

                    if (tipoPlano is not null)
                    {
                        plano.TipoPlanoID = tipoPlano.ID;
                    }
                }

                if ((plano.Nome?.Contains('(') ?? false) && (plano.Nome?.Contains(')') ?? false))
                {
                    if (plano.Nome?.Contains("Parcelado") ?? false)
                    {
                        plano.Nome = "Opcao-Parcelado";
                    } else
                    {
                        plano.Nome = "Opcao-AVista";
                    }
                }

                planos.Add(plano);
            }

            if (planos.Any())
            {
                SaveContext(planos, nameof(UpdatePlanos), EntityState.Modified);
            }
        }

        private void UpdateTriggers()
        {
            IList<Triggers> triggers = [];

            //if (dbContext.Set<Jobs>().FirstOrDefault(x => x.Name == "RefreshCachePagamentos") is Jobs refreshCachePagamentosJob)
            //{
            //    if (dbContext.Set<Triggers>().FirstOrDefault(x => x.JobID == refreshCachePagamentosJob.ID && x.ActivatesOn < DateTime.Today) is Triggers refreshCachePagamentosTrigger)
            //    {
            //        DateTime activatesOn = DateTime.Today.AddHours(1);
            //        refreshCachePagamentosTrigger.ActivatesOn = activatesOn;
            //        refreshCachePagamentosTrigger.NextExecution = activatesOn.AddDays(1);
            //        triggers.Add(refreshCachePagamentosTrigger);
            //    }
            //}

            if (triggers.Any())
            {
                SaveContext(triggers, nameof(UpdateTriggers), EntityState.Modified);
            }
        }

        private void UpdateMenus()
        {
            IList<Menus> menus = [];

            // Menu-PortalAluno-GruposPagamentosOnline
            if (dbContext.Set<Menus>().FirstOrDefault(x => EF.Functions.Like(x.Label!, "Menu-PortalAluno-GruposPagamentosOnline")) is Menus gruposPagamentosOnlineMenu)
            {
                gruposPagamentosOnlineMenu.Label = "Menu-PortalAluno-Grupos";
                gruposPagamentosOnlineMenu.URL = "/portal-aluno/grupos";
                menus.Add(gruposPagamentosOnlineMenu);
            }

            if (menus.Any())
            {
                SaveContext(menus, nameof(UpdateMenus), EntityState.Modified);
            }
        }
        #endregion
    }
}