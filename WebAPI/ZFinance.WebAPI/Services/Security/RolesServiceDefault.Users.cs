using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore.Storage;
using ZDatabase.Exceptions;
using ZFinance.Core.Entities.Security;
using ZFinance.Core.Repositories.Security.Interfaces;
using ZFinance.WebAPI.Models;
using ZFinance.WebAPI.Models.Security.Role;
using ZSecurity.Attributes;
using ZWebAPI.ExtensionMethods;
using ZWebAPI.Interfaces;

namespace ZFinance.WebAPI.Services.Security
{
    public partial class RolesServiceDefault
    {
        #region Variables
        private readonly IUsersRepository usersRepository;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Public methods
        /// <inheritdoc />
        [ActionMethod]
        public async Task<IQueryable<RolesUsersListModel>> ListRoleUsersAsync(long roleID, IListParameters parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            try
            {
                await securityHandler.ValidateUserHasPermissionAsync();

                if (await rolesRepository.FindRoleByIDAsync(roleID) is Roles role)
                {
                    return (role.Users ?? Enumerable.Empty<Users>())
                        .AsQueryable()
                        .OrderBy(x => x.Email)
                        .GetRange(parameters)
                        .ProjectTo<RolesUsersListModel>(mapper.ConfigurationProvider);
                }
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in service when listing the users of the role.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(roleID), roleID },
                        { nameof(parameters), parameters },
                    }
                );
                throw;
            }

            throw new EntityNotFoundException<Roles>(roleID);
        }

        /// <inheritdoc />
        [ActionMethod]
        public async Task UpdateRelationshipRoleUsersAsync(long roleID, RelationshipUpdateModel<long> relationshipUpdateModel)
        {
            if (relationshipUpdateModel is null)
            {
                throw new ArgumentNullException(nameof(relationshipUpdateModel));
            }

            if (!relationshipUpdateModel.IDsToAdd.Any() && !relationshipUpdateModel.IDsToRemove.Any())
            {
                return;
            }

            try
            {
                await auditService.BeginNewServiceHistoryAsync();

                if (await rolesRepository.FindRoleByIDAsync(roleID) is not Roles role)
                {
                    throw new EntityNotFoundException<Roles>(roleID);
                }

                IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync();
                try
                {
                    role.Users ??= [];

                    foreach (long userID in relationshipUpdateModel.IDsToAdd)
                    {
                        if (await usersRepository.FindUserByIDAsync(userID) is Users user
                            && !role.Users.Contains(user))
                        {
                            role.Users.Add(user);
                        }
                    }

                    foreach (long userID in relationshipUpdateModel.IDsToRemove)
                    {
                        if (await usersRepository.FindUserByIDAsync(userID) is Users user
                            && role.Users.Contains(user))
                        {
                            role.Users.Remove(user);
                        }
                    }

                    await rolesRepository.UpdateRoleAsync(role);
                    await dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in service when updating the users of the role.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(roleID), roleID },
                        { nameof(relationshipUpdateModel), relationshipUpdateModel },
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