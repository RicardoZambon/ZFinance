using Microsoft.EntityFrameworkCore;
using Niten.Core.Entities.Estoque;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.Estoque.Interfaces;
using ZDatabase.Interfaces;

namespace Niten.System.Core.Repositories.Estoque
{
    /// <inheritdoc />
    public class MateriaisRepository : IMateriaisRepository
    {
        #region Variables
        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MateriaisRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        public MateriaisRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task<Materiais?> EncontrarMaterialPorMaterialPaiIDExtraAsync(int materialPaiID, string extra)
        {
            try
            {
                return await ObterTodosMateriaisPorMaterialPaiID(materialPaiID).FirstOrDefaultAsync(x => EF.Functions.Like(x.Extra!, extra));
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro ao buscar por material pelo ID do material pai e pelo campo extra.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(materialPaiID), materialPaiID },
                        { nameof(extra), extra },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<Materiais?> EncontrarMaterialPorMaterialPaiIDNomeAsync(int materialPaiID, string nome)
        {
            try
            {
                return await ObterTodosMateriaisPorMaterialPaiID(materialPaiID).FirstOrDefaultAsync(x => EF.Functions.Like(x.Nome!, nome));
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro ao buscar por material pelo ID do material pai e pelo nome.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(materialPaiID), materialPaiID },
                        { nameof(nome), nome },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task InserirNovoMaterialAsync(Materiais material)
        {
            try
            {
                await dbContext.AddAsync(material);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro ao inserir novo material.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(material), material },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<Materiais> ObterTodosMateriaisPorMaterialPaiID(int materialPaiID)
        {
            try
            {
                return from m in dbContext.Set<Materiais>()
                       where m.MaterialPaiID == materialPaiID
                       select m;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro ao obter todos os materiais pelo ID do material pai.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(materialPaiID), materialPaiID },
                    }
                );
                throw;
            }
        }
        #endregion

        #region Private methods
        #endregion
    }
}