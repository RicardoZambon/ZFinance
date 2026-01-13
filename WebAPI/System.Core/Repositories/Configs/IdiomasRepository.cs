using Microsoft.EntityFrameworkCore;
using Niten.Core.Entities.Configs;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.Configs.Interfaces;
using ZDatabase.Exceptions;
using ZDatabase.Interfaces;
using ZDatabase.Validations;

namespace Niten.System.Core.Repositories.Configs
{
    /// <inheritdoc />
    public class IdiomasRepository : IIdiomasRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="IdiomasRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        public IdiomasRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task AtualizarIdiomaAsync(Idiomas idioma)
        {
            try
            {
                await ValidarAsync(idioma);
                dbContext.Set<Idiomas>().Update(idioma);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao atualizar o idioma.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(idioma), idioma },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<Idiomas?> EncontrarIdiomaPorIDAsync(long idiomaID)
        {
            try
            {
                return await dbContext.FindAsync<Idiomas>(idiomaID);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao encontrar o idioma pelo ID.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(idiomaID), idiomaID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task ExcluirIdiomaAsync(long idiomaID)
        {
            try
            {
                if (await EncontrarIdiomaPorIDAsync(idiomaID) is not Idiomas idioma)
                {
                    throw new EntityNotFoundException<Idiomas>(idiomaID);
                }

                idioma.IsDeleted = true;
                dbContext.Set<Idiomas>().Update(idioma);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao excluir o idioma.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(idiomaID), idiomaID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task InserirNovoIdiomaAsync(Idiomas idioma)
        {
            try
            {
                await ValidarAsync(idioma);
                await dbContext.Set<Idiomas>().AddAsync(idioma);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao inserir novo idioma.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(idioma), idioma },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<Idiomas> ObterTodosIdiomas()
        {
            try
            {
                return from m in dbContext.Set<Idiomas>()
                       select m;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter todos os idiomas.");
                throw;
            }
        }
        #endregion

        #region Private methods
        private async Task ValidarAsync(Idiomas idioma)
        {
            ValidationResult result = new();

            // Codigo
            if (string.IsNullOrWhiteSpace(idioma.Codigo))
            {
                result.SetError(nameof(Idiomas.Codigo), "required");
            }
            else if (await dbContext.Set<Idiomas>().AnyAsync(x => EF.Functions.Like(x.Codigo!, idioma.Codigo) && x.ID != idioma.ID))
            {
                result.SetError(nameof(Idiomas.Codigo), "exists");
            }

            // Nome
            if (string.IsNullOrWhiteSpace(idioma.Nome))
            {
                result.SetError(nameof(Idiomas.Nome), "required");
            }
            else if (await dbContext.Set<Idiomas>().AnyAsync(x => EF.Functions.Like(x.Nome!, idioma.Nome) && x.ID != idioma.ID))
            {
                result.SetError(nameof(Idiomas.Nome), "exists");
            }

            // NomeExibicao
            if (string.IsNullOrWhiteSpace(idioma.NomeExibicao))
            {
                result.SetError(nameof(Idiomas.NomeExibicao), "required");
            }
            else if (await dbContext.Set<Idiomas>().AnyAsync(x => EF.Functions.Like(x.NomeExibicao!, idioma.NomeExibicao) && x.ID != idioma.ID))
            {
                result.SetError(nameof(Idiomas.NomeExibicao), "exists");
            }

            // Ordem
            if (idioma.Ordem < 0)
            {
                result.SetError(nameof(Idiomas.Ordem), "min");
            }

            result.ValidateEntityErrors(idioma);
        }
        #endregion
    }
}