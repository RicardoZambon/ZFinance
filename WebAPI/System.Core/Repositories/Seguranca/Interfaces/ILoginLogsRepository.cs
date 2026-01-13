using Niten.Core.Entities.Seguranca;

namespace Niten.System.Core.Repositories.Seguranca.Interfaces
{
    public interface ILoginLogsRepository
    {
        /// <summary>
        /// Add a login successfull entry.
        /// </summary>
        /// <param name="loginLogs">The login log model to add successfull entry.</param>
        Task AddAsync(LoginLogs loginLogs);

        /// <summary>
        /// Return the amount of days since the user's last successfull login. If the user never logged in, returns "0".
        /// </summary>
        /// <param name="username">The username from the user.</param>
        /// <returns>The amount of days since last successfull login.</returns>
        Task<int> GetDaysSinceLastAccess(string username);
    }
}