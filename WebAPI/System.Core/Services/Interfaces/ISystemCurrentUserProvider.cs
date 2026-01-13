using ZDatabase.Services.Interfaces;

namespace Niten.System.Core.Services.Interfaces
{
    /// <summary>
    /// Serviço para prover o usuário atual logado na aplicação (apenas System).
    /// </summary>
    /// <seealso cref="ZDatabase.Services.Interfaces.ICurrentUserProvider{TUserKey}" />
    public interface ISystemCurrentUserProvider : ICurrentUserProvider<int>
    {
        /// <summary>
        /// Obtém ou define o ID do usuário.
        /// </summary>
        /// <value>
        /// O ID do usuário.
        /// </value>
        uint? UsuarioID { get; }
    }
}