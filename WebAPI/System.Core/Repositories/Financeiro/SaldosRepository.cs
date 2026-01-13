using Microsoft.EntityFrameworkCore;
using Niten.Core.Entities.Financeiro;
using Niten.Core.Services;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.Financeiro.Interfaces;
using ZDatabase.Interfaces;

namespace Niten.System.Core.Repositories.Financeiro
{
    /// <inheritdoc />
    public class SaldosRepository : ISaldosRepository
    {
        #region Variables
        private readonly static LockProvider<string> lockProvider = new();

        private readonly IDbContext dbContext;
        private readonly IExceptionHandler exceptionHandler;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SaldosRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        public SaldosRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler)
        {
            this.dbContext = dbContext;
            this.exceptionHandler = exceptionHandler;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task<Saldos?> EncontrarUltimoSaldoDoCadastro(int cadastroID)
        {
            try
            {
                return await (from s in dbContext.Set<Saldos>()
                              where s.UnidadeID == null
                                    && s.CadastroID == cadastroID
                              orderby s.ID descending
                              select s).FirstOrDefaultAsync();
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório encontrar o último saldo do cadastro.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(cadastroID), cadastroID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<Saldos?> EncontrarUltimoSaldoDaUnidade(int unidadeID)
        {
            try
            {
                return await (from s in dbContext.Set<Saldos>()
                              where s.UnidadeID == unidadeID
                                    && s.CadastroID == null
                              orderby s.ID descending
                              select s).FirstOrDefaultAsync();
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório encontrar o último saldo da unidade.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(unidadeID), unidadeID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<string> InserirNovoSaldoAsync(Saldos saldo)
        {
            string chaveLock = ObtemChaveLock(saldo);
            try
            {
                await lockProvider.WaitAsync(chaveLock);

                Saldos? saldoAnterior = null;
                if (saldo.CadastroID is int cadastroID)
                {
                    saldoAnterior = await EncontrarUltimoSaldoDoCadastro(cadastroID);
                }
                else if (saldo.UnidadeID is int unidadeID)
                {
                    saldoAnterior = await EncontrarUltimoSaldoDaUnidade(unidadeID);
                }

                saldo.SaldoAnterior = saldoAnterior?.SaldoAtual ?? 0;

                saldo.SaldoAtual = saldo.SaldoAnterior + saldo.Valor;

                await dbContext.Set<Saldos>().AddAsync(saldo);
                return chaveLock;
            }
            catch
            {
                LiberarLock(chaveLock);

                exceptionHandler.AddBreadcrumb("Erro no repositório ao inserir novo saldo.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(saldo), saldo },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public void LiberarLock(string chaveLock)
        {
            try
            {
                lockProvider.Release(chaveLock);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao liberar o lock do saldo.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(chaveLock), chaveLock },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<Saldos> ObterSaldosPorCadastroID(int cadastroID)
        {
            try
            {
                return from c in dbContext.Set<Saldos>()
                       where c.CadastroID == cadastroID
                       select c;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter todos os saldos do cadastro.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(cadastroID), cadastroID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<Saldos> ObterSaldosPorUnidadeID(int unidadeID)
        {
            try
            {
                return from c in dbContext.Set<Saldos>()
                       where c.UnidadeID == unidadeID
                             && c.CadastroID == null
                       select c;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao obter todos os saldos da unidade.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(unidadeID), unidadeID },
                    }
                );
                throw;
            }
        }
        #endregion

        #region Private methods
        private string ObtemChaveLock(Saldos saldo)
        {
            if (saldo.CadastroID != null)
            {
                return $"Cadastro#{saldo.CadastroID}";
            }
            else if (saldo.UnidadeID != null)
            {
                return $"Unidade#{saldo.UnidadeID}";
            }
            throw new InvalidOperationException("Saldo não possui cadastro ou unidade.");
        }
        #endregion
    }
}