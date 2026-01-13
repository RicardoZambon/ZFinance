using Niten.Core.Entities.Geral;
using Niten.Core.Entities.Seguranca;
using Niten.Core.Helpers.Constants;
using Niten.Core.Helpers.Validators;
using Niten.Core.Repositories.Geral;
using Niten.Core.Services.Interfaces;
using Niten.System.Core.Repositories.Geral.Interfaces;
using Niten.System.Core.Repositories.Historico.Interfaces;
using Niten.System.Core.Repositories.Seguranca.Interfaces;
using Niten.System.Core.Services.Interfaces;
using ZDatabase.Exceptions;
using ZDatabase.Interfaces;
using ZDatabase.Validations;
using ZSecurity.Services;

namespace Niten.System.Core.Repositories.Geral
{
    /// <inheritdoc />
    public class CadastrosRepository : CadastrosRepositoryCore, ICadastrosRepository
    {
        #region Constants
        private const string VIEW_ALL_ACTION_CODE = "AccessAllowedToAllCadastros";
        #endregion

        #region Variables
        private readonly ISecurityHandler securityHandler;
        private readonly ISystemCurrentUserProvider systemCurrentUserProvider;
        private readonly IUsuariosRepository usuariosRepository;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CadastrosRepository"/> class.
        /// </summary>
        /// <param name="cadastrosHistoricoRepository">The <see cref="ICadastrosHistoricoRepository"/> instance.</param>
        /// <param name="dbContext">The <see cref="IDbContext"/> instance.</param>
        /// <param name="exceptionHandler">The <see cref="IExceptionHandler"/> instance.</param>
        /// <param name="securityHandler">The <see cref="ISecurityHandler"/> instance.</param>
        /// <param name="systemCurrentUserProvider">The <see cref="ISystemCurrentUserProvider"/> instance.</param>
        /// <param name="usuariosRepository">The <see cref="IUsuariosRepository"/> instance.</param>
        public CadastrosRepository(
            ICadastrosHistoricoRepository cadastrosHistoricoRepository,
            IDbContext dbContext,
            IExceptionHandler exceptionHandler,
            ISecurityHandler securityHandler,
            ISystemCurrentUserProvider systemCurrentUserProvider,
            IUsuariosRepository usuariosRepository)
            : base(cadastrosHistoricoRepository, dbContext, exceptionHandler)
        {
            this.securityHandler = securityHandler;
            this.systemCurrentUserProvider = systemCurrentUserProvider;
            this.usuariosRepository = usuariosRepository;
        }
        #endregion

        #region Public Methods
        /// <inheritdoc />
        public IQueryable<Cadastros> ObterTodosCadastros()
        {
            try
            {
                return from c in dbContext.Set<Cadastros>()
                       select c;
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Erro ao obter todos os cadastros.");
                throw;
            }
        }
        #endregion

        #region Private Methods

        private async Task<IQueryable<Cadastros>> GetAlunosPermitidosAsync()
            => from c in await GetCadastrosPermitidosAsync()
               where
                c.Situacao == "Aluno"
                || c.Situacao == "Coordenador"
                || c.Situacao == "Monitor"
                || c.Situacao == "Ex-Aluno"
               select c;

        private async Task<IQueryable<Cadastros>> GetCadastrosPermitidosAsync()
        {
            bool viewAll = await securityHandler.CheckCurrentUserHasPermissionOrIsAdministratorAsync(VIEW_ALL_ACTION_CODE);

            Usuarios? usuario = null;
            if (systemCurrentUserProvider.UsuarioID is uint usuarioID)
            {
                usuario = await usuariosRepository.EncontrarUsuarioPorIDAsync(usuarioID);
            }

            //IEnumerable<string> unidades = usuario?.Unidades?.Split(";") ?? Enumerable.Empty<string>();
            //return from c in dbContext.Set<Cadastros>()
            //       where viewAll || unidades.Contains(c.Unidade)
            //       select c;

            // TODO: Criar relacionamento novo de Cadastros X Unidades.
            return Enumerable.Empty<Cadastros>().AsQueryable();
        }

        /// <inheritdoc />
        protected override void Validate(Cadastros cadastro)
        {
            base.Validate(cadastro);

            if (!string.Equals(dbContext.Entry(cadastro).OriginalValues[nameof(Cadastros.Situacao)]?.ToString() ?? string.Empty, cadastro.Situacao, StringComparison.OrdinalIgnoreCase))
            {
                // Caso a situação esteja sendo alterada, não fazemos validações no cadastro.
                return;
            }

            switch (cadastro.Situacao)
            {
                case CadastrosConstants.CADASTROS_SITUACAO_ALUNO:
                case CadastrosConstants.CADASTROS_SITUACAO_MONITOR:
                case CadastrosConstants.CADASTROS_SITUACAO_COORDENADOR:
                case CadastrosConstants.CADASTROS_SITUACAO_EXALUNO:
                    ValidateAlunos(cadastro);
                    break;
                default:
                    ValidateInteressados(cadastro);
                    break;
            }
        }

        private void ValidateAlunos(Cadastros cadastro)
        {
            ValidationResult result = new();

            // DataMatricula
            if (cadastro.DataMatricula == DateTime.MinValue)
            {
                result.SetError(nameof(Cadastros.DataMatricula), "required");
            }

            #region Cadastro

            // CPF
            if (string.IsNullOrWhiteSpace(cadastro.CPF))
            {
                result.SetError(nameof(Cadastros.CPF), "required");
            }

            // DataNascimento
            if (cadastro.DataNascimento is null)
            {
                result.SetError(nameof(Cadastros.DataNascimento), "required");
            }

            // NomeCompleto
            if (string.IsNullOrWhiteSpace(cadastro.NomeCompleto))
            {
                result.SetError(nameof(Cadastros.NomeCompleto), "required");
            }

            #endregion

            #region Contato

            // TelefoneCelular / TelefoneComercial / TelefoneResidencial
            if (string.IsNullOrWhiteSpace(cadastro.TelefoneCelular) && string.IsNullOrWhiteSpace(cadastro.TelefoneComercial) && string.IsNullOrWhiteSpace(cadastro.TelefoneResidencial))
            {
                result.SetError(nameof(Cadastros.TelefoneCelular), "required");
                result.SetError(nameof(Cadastros.TelefoneComercial), "required");
                result.SetError(nameof(Cadastros.TelefoneResidencial), "required");
            }

            // Email1 / Email2 / Email3
            if (string.IsNullOrWhiteSpace(cadastro.Email1) && string.IsNullOrWhiteSpace(cadastro.Email2) && string.IsNullOrWhiteSpace(cadastro.Email3))
            {
                result.SetError(nameof(Cadastros.Email1), "required");
                result.SetError(nameof(Cadastros.Email2), "required");
                result.SetError(nameof(Cadastros.Email3), "required");
            }
            else
            {
                // Email1
                if (!string.IsNullOrWhiteSpace(cadastro.Email1) && !EmailValidator.IsValid(cadastro.Email1))
                {
                    result.SetError(nameof(Cadastros.Email1), "email");
                }

                // Email2
                if (!string.IsNullOrWhiteSpace(cadastro.Email2) && !EmailValidator.IsValid(cadastro.Email2))
                {
                    result.SetError(nameof(Cadastros.Email2), "email");
                }

                // Email3
                if (!string.IsNullOrWhiteSpace(cadastro.Email3) && !EmailValidator.IsValid(cadastro.Email3))
                {
                    result.SetError(nameof(Cadastros.Email3), "email");
                }
            }

            #endregion

            #region Endereço

            // CEP
            if (!string.IsNullOrWhiteSpace(cadastro.CEP))
            {
                // Endereco
                if (string.IsNullOrWhiteSpace(cadastro.Endereco))
                {
                    result.SetError(nameof(Cadastros.Endereco), "required");
                }

                // EnderecoNumero
                if (string.IsNullOrWhiteSpace(cadastro.EnderecoNumero))
                {
                    result.SetError(nameof(Cadastros.EnderecoNumero), "required");
                }

                // Bairro
                if (string.IsNullOrWhiteSpace(cadastro.Bairro))
                {
                    result.SetError(nameof(Cadastros.Bairro), "required");
                }

                // Cidade
                if (string.IsNullOrWhiteSpace(cadastro.Cidade))
                {
                    result.SetError(nameof(Cadastros.Cidade), "required");
                }

                // UF
                if (string.IsNullOrWhiteSpace(cadastro.UF))
                {
                    result.SetError(nameof(Cadastros.UF), "required");
                }
            }

            #endregion

            if (result.HasErrors)
            {
                throw new EntityValidationFailureException<int>(nameof(Cadastros), cadastro.ID, result);
            }
        }

        private void ValidateInteressados(Cadastros cadastro)
        {
            ValidationResult result = new();

            #region Cadastro

            // NomeCompleto
            if (string.IsNullOrWhiteSpace(cadastro.NomeCompleto))
            {
                result.SetError(nameof(Cadastros.NomeCompleto), "required");
            }

            #endregion

            #region Contato

            // TelefoneCelular / TelefoneComercial / TelefoneResidencial
            // Email1 / Email2 / Email3
            if (string.IsNullOrWhiteSpace(cadastro.TelefoneCelular) && string.IsNullOrWhiteSpace(cadastro.TelefoneComercial) && string.IsNullOrWhiteSpace(cadastro.TelefoneResidencial)
                && string.IsNullOrWhiteSpace(cadastro.Email1) && string.IsNullOrWhiteSpace(cadastro.Email2) && string.IsNullOrWhiteSpace(cadastro.Email3))
            {
                result.SetError(nameof(Cadastros.TelefoneCelular), "required");
                result.SetError(nameof(Cadastros.Email1), "required");
            }

            // Email1
            if (!string.IsNullOrWhiteSpace(cadastro.Email1) && !EmailValidator.IsValid(cadastro.Email1))
            {
                result.SetError(nameof(Cadastros.Email1), "email");
            }

            // Email2
            if (!string.IsNullOrWhiteSpace(cadastro.Email2) && !EmailValidator.IsValid(cadastro.Email2))
            {
                result.SetError(nameof(Cadastros.Email2), "email");
            }

            // Email3
            if (!string.IsNullOrWhiteSpace(cadastro.Email3) && !EmailValidator.IsValid(cadastro.Email3))
            {
                result.SetError(nameof(Cadastros.Email3), "email");
            }

            #endregion

            if (result.HasErrors)
            {
                throw new EntityValidationFailureException<int>(nameof(Cadastros), cadastro.ID, result);
            }
        }

        #endregion
    }
}