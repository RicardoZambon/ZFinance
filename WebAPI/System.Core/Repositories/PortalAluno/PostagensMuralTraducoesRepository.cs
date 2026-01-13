using Microsoft.EntityFrameworkCore;
using Niten.Core.Entities.Configs;
using Niten.Core.Entities.PortalAluno;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.PortalAluno.Interfaces;
using ZDatabase.Exceptions;
using ZDatabase.Interfaces;
using ZDatabase.Validations;

namespace Niten.System.Core.Repositories.PortalAluno
{
    /// <inheritdoc />
    public class PostagensMuralTraducoesRepository : IPostagensMuralTraducoesRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PostagensMuralTraducoesRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        public PostagensMuralTraducoesRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task AtualizarPostagemMuralTraducaoAsync(PostagensMuralTraducoes postagemMuralTraducao)
        {
            try
            {
                await ValidarAsync(postagemMuralTraducao);
                dbContext.Set<PostagensMuralTraducoes>().Update(postagemMuralTraducao);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao atualizar a tradução da postagem.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(postagemMuralTraducao), postagemMuralTraducao },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<PostagensMuralTraducoes?> EncontrarPostagemMuralTraducaoPorIDAsync(long postagemMuralTraducaoID)
        {
            try
            {
                return await dbContext.FindAsync<PostagensMuralTraducoes>(postagemMuralTraducaoID);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao encontrar a tradução da postagem pelo ID.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(postagemMuralTraducaoID), postagemMuralTraducaoID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task ExcluirPostagemMuralTraducaoAsync(long postagemMuralTraducaoID)
        {
            try
            {
                if (await EncontrarPostagemMuralTraducaoPorIDAsync(postagemMuralTraducaoID) is not PostagensMuralTraducoes postagemMuralTraducao)
                {
                    throw new EntityNotFoundException<PostagensMuralTraducoes>(postagemMuralTraducaoID);
                }

                postagemMuralTraducao.IsDeleted = true;
                dbContext.Set<PostagensMuralTraducoes>().Update(postagemMuralTraducao);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao excluir a tradução da postagem.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(postagemMuralTraducaoID), postagemMuralTraducaoID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task InserirNovaPostagemMuralTraducaoAsync(PostagensMuralTraducoes postagemMuralTraducao)
        {
            try
            {
                await ValidarAsync(postagemMuralTraducao);
                await dbContext.Set<PostagensMuralTraducoes>().AddAsync(postagemMuralTraducao);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao inserir nova tradução da postagem.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(postagemMuralTraducao), postagemMuralTraducao },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<PostagensMuralTraducoes> ObterTodasTraducoesPorPostagemMural(long postagemMuralID)
        {
            try
            {
                return from m in dbContext.Set<PostagensMuralTraducoes>()
                       select m;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter todas as traduções da postagem.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(postagemMuralID), postagemMuralID },
                    }
                );
                throw;
            }
        }
        #endregion

        #region Private methods
        private async Task ValidarAsync(PostagensMuralTraducoes postagemMuralTraducao)
        {
            ValidationResult result = new();

            // Conteudo
            if (string.IsNullOrWhiteSpace(postagemMuralTraducao.Conteudo))
            {
                result.SetError(nameof(PostagensMuralTraducoes.Conteudo), "required");
            }

            // IdiomaID
            if (await dbContext.FindAsync<Idiomas>(postagemMuralTraducao.IdiomaID) is null)
            {
                result.SetError(nameof(PostagensMuralTraducoes.IdiomaID), "required");
            }
            else if (await dbContext.Set<PostagensMuralTraducoes>().AnyAsync(x => x.IdiomaID == postagemMuralTraducao.IdiomaID && x.PostagemID == postagemMuralTraducao.PostagemID && x.ID != postagemMuralTraducao.ID))
            {
                result.SetError(nameof(PostagensMuralTraducoes.IdiomaID), "exists");
            }

            // PostagemID
            if (await dbContext.FindAsync<Idiomas>(postagemMuralTraducao.PostagemID) is null)
            {
                result.SetError(nameof(PostagensMuralTraducoes.PostagemID), "required");
            }

            // Slug
            if (string.IsNullOrWhiteSpace(postagemMuralTraducao.Slug))
            {
                result.SetError(nameof(PostagensMuralTraducoes.Slug), "required");
            }
            else if (await dbContext.Set<PostagensMuralTraducoes>().AnyAsync(x => EF.Functions.Like(x.Slug!, postagemMuralTraducao.Slug) && x.ID != postagemMuralTraducao.ID))
            {
                result.SetError(nameof(PostagensMuralTraducoes.Slug), "exists");
            }

            // Titulo
            if (string.IsNullOrWhiteSpace(postagemMuralTraducao.Titulo))
            {
                result.SetError(nameof(PostagensMuralTraducoes.Titulo), "required");
            }
            else if (await dbContext.Set<PostagensMuralTraducoes>().AnyAsync(x => EF.Functions.Like(x.Titulo!, postagemMuralTraducao.Titulo) && x.ID != postagemMuralTraducao.ID))
            {
                result.SetError(nameof(PostagensMuralTraducoes.Titulo), "exists");
            }

            result.ValidateEntityErrors(postagemMuralTraducao);
        }
        #endregion
    }
}