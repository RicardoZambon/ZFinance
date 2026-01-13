using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Niten.Core.Entities.Audit;
using ZDatabase.Entities;
using ZDatabase.Interfaces;
using ZDatabase.Services.Interfaces;

namespace Niten.System.Core.DataInitializers
{
    /// <summary>
    /// Classe abstrata para inicialização da base de dados.
    /// </summary>
    public abstract class BaseInitializer
    {
        #region Variables
        /// <summary>
        /// O contexto da base de dados.
        /// </summary>
        protected readonly IDbContext dbContext;
        #endregion

        #region Properties
        /// <summary>
        /// Obtém o serviço de auditoria.
        /// </summary>
        /// <value>
        /// O serviço de auditoria.
        /// </value>
        protected IAuditHandler? AuditHandler
        {
            get => (dbContext as DbContext)?.GetService<IAuditHandler>();
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseInitializer"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        public BaseInitializer(IDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        #endregion

        #region Public methods
        #endregion

        #region Private methods
        /// <summary>
        /// Inicia a transação.
        /// </summary>
        /// <param name="methodName">O nome do método..</param>
        /// <returns>Retorna a transação.</returns>
        protected IDbContextTransaction BeginTransaction(string methodName)
        {
            IDbContextTransaction transaction = dbContext.Database.BeginTransaction();
            dbContext.Set<ServicesHistory>().Add(new ServicesHistory { Name = $"{GetType().Name}\\{methodName}" });
            return transaction;
        }

        /// <summary>
        /// Salva o contexto.
        /// </summary>
        /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
        /// <typeparam name="TValue">O tipo do valor.</typeparam>
        /// <param name="entities">As entidades.</param>
        /// <param name="methodName">O nome do método.</param>
        /// <param name="propertyCheck">A propriedade para checar.</param>
        protected void SaveContext<TEntity, TValue>(IEnumerable<TEntity> entities, string methodName, Func<TEntity, TValue> propertyCheck)
            where TEntity : Entity
        {
            long maxID = 1L;
            if (dbContext.Set<TEntity>().Any())
            {
                maxID = dbContext.Set<TEntity>().IgnoreQueryFilters().Max(x => x.ID) + 1;
            }

            if (entities.Max(x => x.ID) is long entitiesMaxID && entitiesMaxID > maxID)
            {
                maxID = entitiesMaxID + 1;
            }

            foreach (TEntity entity in entities)
            {
                TValue? propertyValue = new TEntity[] { entity }.Select(propertyCheck).FirstOrDefault();

                if (!dbContext.Set<TEntity>().Select(propertyCheck).Any(x => EqualityComparer<TValue>.Default.Equals(x, propertyValue)) && entity.ID <= 0)
                {
                    entity.ID = maxID;
                    maxID++;
                }
                else if (dbContext.Set<TEntity>().Select(propertyCheck).Any(x => EqualityComparer<TValue>.Default.Equals(x, propertyValue)) && entity.ID > 0)
                {
                    entity.ID = 0;
                }
            }

            SaveContext(entities.Where(x => x.ID > 0), methodName, entriesState: EntityState.Added);
        }

        /// <summary>
        /// Salva o contexto.
        /// </summary>
        /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
        /// <param name="entities">As entidades.</param>
        /// <param name="methodName">O nome do método.</param>
        /// <param name="entriesState">O estado das entradas.</param>
        protected void SaveContext<TEntity>(IEnumerable<TEntity> entities, string methodName, EntityState entriesState = EntityState.Added)
            where TEntity : class
        {
            using IDbContextTransaction transaction = BeginTransaction(methodName);
            try
            {
                foreach (TEntity entity in entities)
                {
                    switch (entriesState)
                    {
                        case EntityState.Added:
                            dbContext.Set<TEntity>().Add(entity);
                            break;
                        case EntityState.Modified:
                            dbContext.Set<TEntity>().Update(entity);
                            break;
                        case EntityState.Deleted:
                            dbContext.Set<TEntity>().Remove(entity);
                            break;
                    }
                }
                dbContext.SaveChanges();
                transaction.Commit();

                AuditHandler?.ClearServiceHistory();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
            dbContext.ChangeTracker.Clear();
        }
        #endregion

        #region Abstract methods
        /// <summary>
        /// Inicializa a instância.
        /// </summary>
        public abstract void Initialize();
        #endregion
    }
}