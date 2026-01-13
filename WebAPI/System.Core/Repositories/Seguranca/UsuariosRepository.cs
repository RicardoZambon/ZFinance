using Microsoft.EntityFrameworkCore;
using Niten.Core.Entities.Geral;
using Niten.Core.Entities.Seguranca;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.Seguranca.Interfaces;
using Niten.System.Core.Services.Interfaces;
using ZDatabase.Exceptions;
using ZDatabase.Interfaces;
using ZDatabase.Validations;
using ZSecurity.Repositories;

namespace Niten.System.Core.Repositories.Seguranca
{
    /// <inheritdoc />
    public class UsuariosRepository : BaseUsersRepository<Actions, int>, IUsuariosRepository
    {
        #region Variables
        private readonly IExceptionHandler exceptionHandler;
        private readonly ISystemCurrentUserProvider systemCurrentUserProvider;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="UsuariosRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="IDbContext" /> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler" /> instance.</param>
        /// <param name="systemCurrentUserProvider">The <see cref="ISystemCurrentUserProvider" /> instance.</param>
        public UsuariosRepository(
            IDbContext dbContext,
            IExceptionHandler exceptionHandler,
            ISystemCurrentUserProvider systemCurrentUserProvider)
            : base(dbContext)
        {
            this.exceptionHandler = exceptionHandler;
            this.systemCurrentUserProvider = systemCurrentUserProvider;
        }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public async Task AtualizarUsuarioAsync(Usuarios usuario)
        {
            try
            {
                await ValidarAsync(usuario);
                dbContext.Set<Usuarios>().Update(usuario);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao atualizar o usuário.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(usuario), usuario },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<Usuarios?> EncontrarUsuarioPorIDAsync(uint usuarioID)
        {
            try
            {
                return await dbContext.FindAsync<Usuarios>(usuarioID);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro ao buscar usuário pelo ID.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(usuarioID), usuarioID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<Usuarios?> EncontrarUsuarioPorUsuarioAsync(string usuario)
        {
            try
            {
                return await dbContext.Set<Usuarios>().FirstOrDefaultAsync(u => EF.Functions.Like(u.Usuario, usuario));
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro ao buscar usuário pelo usuário.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(usuario), usuario },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task ExcluirUsuarioAsync(uint usuarioID)
        {
            try
            {
                if (await EncontrarUsuarioPorIDAsync(usuarioID) is not Usuarios usuario)
                {
                    throw new EntityNotFoundException<Usuarios>(usuarioID);
                }

                usuario.Excluido = true;
                dbContext.Set<Usuarios>().Update(usuario);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao excluir o usuario.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(usuarioID), usuarioID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async Task InserirNovoUsuarioAsync(Usuarios usuario)
        {
            try
            {
                await ValidarAsync(usuario);
                await dbContext.Set<Usuarios>().AddAsync(usuario);
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao inserir novo usuário.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(usuario), usuario },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public async override Task<IEnumerable<Actions>> ListAllActionsAsync(int cadastroID)
        {
            try
            {
                if (systemCurrentUserProvider.UsuarioID is not uint usuarioID
                || await EncontrarUsuarioPorIDAsync(usuarioID) is not Usuarios usuario)
                {
                    throw new EntityNotFoundException<Usuarios>(systemCurrentUserProvider.UsuarioID ?? 0);
                }

                if (usuario.CadastroID == cadastroID)
                {
                    return usuario.Roles
                        ?.SelectMany(x => x.Actions ?? Array.Empty<Actions>())
                        ?? Enumerable.Empty<Actions>();
                }

                return Enumerable.Empty<Actions>();
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro no repositório ao listas todas as actions do usuário.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(cadastroID), cadastroID },
                    }
                );
                throw;
            }
        }

        /// <inheritdoc />
        public IQueryable<Usuarios> ObterTodosUsuarios()
        {
            try
            {
                return from u in dbContext.Set<Usuarios>()
                       select u;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro ao obter todos os usuários.");
                throw;
            }
        }
        #endregion

        #region Private methods
        private async Task ValidarAsync(Usuarios usuario)
        {
            ValidationResult result = new();

            // Cadastro
            if (await dbContext.Set<Cadastros>().FindAsync(usuario.CadastroID) is null)
            {
                result.SetError(nameof(Usuarios.CadastroID), "required");
            }

            // Email
            if (string.IsNullOrWhiteSpace(usuario.Email))
            {
                result.SetError(nameof(Usuarios.Email), "required");
            }

            // Senha
            if (usuario.ID <= 0 && string.IsNullOrEmpty(usuario.Senha))
            {
                result.SetError(nameof(Usuarios.Senha), "required");
            }

            // Usuario
            if (string.IsNullOrWhiteSpace(usuario.Usuario))
            {
                result.SetError(nameof(Usuarios.Usuario), "required");
            }
            else if (await dbContext.Set<Usuarios>().AnyAsync(x => EF.Functions.Like(x.Usuario, usuario.Usuario) && x.ID != usuario.ID))
            {
                result.SetError(nameof(Usuarios.Usuario), "exists");
            }

            if (result.HasErrors)
            {
                throw new EntityValidationFailureException<uint>(nameof(Usuarios), usuario.ID, result);
            }
        }
        #endregion
    }
}