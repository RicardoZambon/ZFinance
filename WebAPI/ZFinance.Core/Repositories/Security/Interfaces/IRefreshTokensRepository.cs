using ZFinance.Core.Entities.Security;

namespace ZFinance.Core.Repositories.Security.Interfaces
{
    /// <summary>
    /// Repository for <see cref="RefreshTokens"/>.
    /// </summary>
    public interface IRefreshTokensRepository
    {
        /// <summary>
        /// Finds the refresh token by user and token asynchronous.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="token">The token.</param>
        /// <returns>The refresh token, if found; otherwise, <c>null</c>.</returns>
        Task<RefreshTokens?> FindRefreshTokenByUserAndTokenAsync(string email, string token);

        /// <summary>
        /// Adds the refresh token asynchronous.
        /// </summary>
        /// <param name="refreshToken">The refresh token.</param>
        Task InsertRefreshTokenAsync(RefreshTokens refreshToken);

        /// <summary>
        /// Revokes the refresh token.
        /// </summary>
        /// <param name="refreshToken">The refresh token.</param>
        void RevokeRefreshToken(RefreshTokens refreshToken);
    }
}