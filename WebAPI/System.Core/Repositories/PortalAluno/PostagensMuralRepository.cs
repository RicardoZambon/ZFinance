using Niten.Core.Entities.PortalAluno;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.PortalAluno.Interfaces;
using ZDatabase.Exceptions;
using ZDatabase.Interfaces;
using ZDatabase.Validations;

namespace Niten.System.Core.Repositories.PortalAluno
{
    /// <inheritdoc />
    public class PostagensMuralRepository : IPostagensMuralRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PostagensMuralRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        public PostagensMuralRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task AtualizarPostagemMuralAsync(PostagensMural postagemMural)
        {
            try
            {
                await ValidarAsync(postagemMural);
                dbContext.Set<PostagensMural>().Update(postagemMural);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao atualizar a postagem.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(postagemMural), postagemMural },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<PostagensMural?> EncontrarPostagemMuralPorIDAsync(long postagemMuralID)
        {
            try
            {
                return await dbContext.FindAsync<PostagensMural>(postagemMuralID);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao encontrar a postagem pelo ID.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(postagemMuralID), postagemMuralID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task ExcluirPostagemMuralAsync(long postagemMuralID)
        {
            try
            {
                if (await EncontrarPostagemMuralPorIDAsync(postagemMuralID) is not PostagensMural postagemMural)
                {
                    throw new EntityNotFoundException<PostagensMural>(postagemMuralID);
                }

                postagemMural.IsDeleted = true;
                dbContext.Set<PostagensMural>().Update(postagemMural);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao excluir a postagem.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(postagemMuralID), postagemMuralID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task InserirNovaPostagemMuralAsync(PostagensMural postagemMural)
        {
            try
            {
                await ValidarAsync(postagemMural);
                await dbContext.Set<PostagensMural>().AddAsync(postagemMural);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao inserir nova postagem.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(postagemMural), postagemMural },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<PostagensMural> ObterTodasPostagensMural()
        {
            try
            {
                return from pm in dbContext.Set<PostagensMural>()
                       select pm;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter todas as postagens.");
                throw;
            }
        }
        #endregion

        #region Private methods
        private async Task ValidarAsync(PostagensMural postagemMural)
        {
            ValidationResult result = new();

            // Ordem
            if (postagemMural.Ordem < 0)
            {
                result.SetError(nameof(PostagensMural.Ordem), "min");
            }

            if (postagemMural.ID > 0)
            {
                // TraducaoPadraoID
                if (await dbContext.Set<PostagensMuralTraducoes>().FindAsync(postagemMural.TraducaoPadraoID) is null)
                {
                    result.SetError(nameof(PostagensMural.TraducaoPadraoID), "required");
                }
            }

            result.ValidateEntityErrors(postagemMural);
        }
        #endregion
    }
}