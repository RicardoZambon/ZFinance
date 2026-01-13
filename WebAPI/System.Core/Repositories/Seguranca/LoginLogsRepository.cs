using Microsoft.EntityFrameworkCore;
using Niten.Core.Entities.Seguranca;
using Niten.System.Core.Repositories.Seguranca.Interfaces;
using ZDatabase.Interfaces;

namespace Niten.System.Core.Repositories.Seguranca
{
    public class LoginLogsRepository : ILoginLogsRepository
    {
        private readonly IDbContext dbContext;
        private readonly IUsuariosRepository usuariosRepository;

        public LoginLogsRepository(IDbContext dbContext, IUsuariosRepository usuariosRepository)
        {
            this.dbContext = dbContext;
            this.usuariosRepository = usuariosRepository;
        }


        public async Task AddAsync(LoginLogs loginLogs)
        {
            loginLogs.Data = DateTime.Now;

            await dbContext.Set<LoginLogs>().AddAsync(loginLogs);
        }

        public async Task<int> GetDaysSinceLastAccess(string username)
        {
            var lastAccess = await dbContext.Set<LoginLogs>()
                .Where(x => x.Usuario == username)
                .OrderByDescending(x => x.Data)
                .FirstOrDefaultAsync();
            if (lastAccess != null)
            {
                return (DateTime.Now - Convert.ToDateTime(lastAccess.Data)).Days;
            }
            return 0;
        }
    }
}