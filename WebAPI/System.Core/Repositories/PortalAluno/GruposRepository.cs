using Microsoft.EntityFrameworkCore;
using Niten.Core.Entities.PortalAluno;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.PortalAluno.Interfaces;
using ZDatabase.Exceptions;
using ZDatabase.Interfaces;
using ZDatabase.Validations;

namespace Niten.System.Core.Repositories.PortalAluno
{
    /// <inheritdoc />
    public class GruposRepository : IGruposRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="GruposRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        public GruposRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task AtualizarGrupoAsync(Grupos grupo)
        {
            try
            {
                await ValidarAsync(grupo);
                dbContext.Set<Grupos>().Update(grupo);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao atualizar a grupo.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(grupo), grupo },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<Grupos?> EncontrarGrupoPorIDAsync(long grupoID)
        {
            try
            {
                return await dbContext.FindAsync<Grupos>(grupoID);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao encontrar a grupo pelo ID.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(grupoID), grupoID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task ExcluirGrupoAsync(long grupoID)
        {
            try
            {
                if (await EncontrarGrupoPorIDAsync(grupoID) is not Grupos grupo)
                {
                    throw new EntityNotFoundException<Grupos>(grupoID);
                }

                grupo.IsDeleted = true;
                dbContext.Set<Grupos>().Update(grupo);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao excluir a grupo.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(grupoID), grupoID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task InserirNovoGrupoAsync(Grupos grupo)
        {
            try
            {
                await ValidarAsync(grupo);
                await dbContext.Set<Grupos>().AddAsync(grupo);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao inserir novo grupo.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(grupo), grupo },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<Grupos> ObterTodosGrupos()
        {
            try
            {
                return from g in dbContext.Set<Grupos>()
                       select g;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter todos os grupos.");
                throw;
            }
        }

        #endregion

        #region Private methods
        private async Task ValidarAsync(Grupos grupo)
        {
            ValidationResult result = new();

            // Nome
            if (string.IsNullOrWhiteSpace(grupo.Nome))
            {
                result.SetError(nameof(Grupos.Nome), "required");
            }
            else if (await dbContext.Set<Grupos>().AnyAsync(x => EF.Functions.Like(x.Nome, grupo.Nome) && x.Tipo == grupo.Tipo && x.ID != grupo.ID))
            {
                result.SetError(nameof(Grupos.Nome), "exists");
            }

            // NomeExibicao
            if (string.IsNullOrWhiteSpace(grupo.NomeExibicao))
            {
                result.SetError(nameof(Grupos.NomeExibicao), "required");
            }
            else if (await dbContext.Set<Grupos>().AnyAsync(x => EF.Functions.Like(x.NomeExibicao, grupo.NomeExibicao) && x.Tipo == grupo.Tipo && x.ID != grupo.ID))
            {
                result.SetError(nameof(Grupos.NomeExibicao), "exists");
            }

            result.ValidateEntityErrors(grupo);
        }
        #endregion
    }
}