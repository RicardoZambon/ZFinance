using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore.Storage;
using ZDatabase.Exceptions;
using ZFinance.Core.Entities.Security;
using ZFinance.Core.Repositories.Security.Interfaces;
using ZFinance.WebAPI.Models;
using ZFinance.WebAPI.Models.Security.User;
using ZSecurity.Attributes;
using ZWebAPI.ExtensionMethods;
using ZWebAPI.Interfaces;

namespace ZFinance.WebAPI.Services.Security
{
    public partial class UsersServiceDefault
    {
        #region Variables
        private readonly IRolesRepository rolesRepository;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Public methods
        /// <inheritdoc />
        [ActionMethod]
        public async Task<IQueryable<UsersRolesListModel>> ListUserRolesAsync(long userID, IListParameters parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            try
            {
                await securityHandler.ValidateUserHasPermissionAsync();

                if (await usersRepository.FindUserByIDAsync(userID) is Users user)
                {
                    return (user.Roles ?? Enumerable.Empty<Roles>())
                        .AsQueryable()
                        .OrderBy(x => x.Name)
                        .GetRange(parameters)
                        .ProjectTo<UsersRolesListModel>(mapper.ConfigurationProvider);
                }
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in service when listing the roles of the user.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(userID), userID },
                        { nameof(parameters), parameters },
                    }
                );
                throw;
            }

            throw new EntityNotFoundException<Users>(userID);
        }

        /// <inheritdoc />
        [ActionMethod]
        public async Task UpdateRelationshipUserRolesAsync(long userID, RelationshipUpdateModel<long> relationshipUpdateModel)
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

                if (await usersRepository.FindUserByIDAsync(userID) is not Users user)
                {
                    throw new EntityNotFoundException<Users>(userID);
                }

                IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync();
                try
                {
                    user.Roles ??= [];

                    foreach (long roleID in relationshipUpdateModel.IDsToAdd)
                    {
                        if (await rolesRepository.FindRoleByIDAsync(roleID) is Roles role
                            && !user.Roles.Contains(role))
                        {
                            user.Roles.Add(role);
                        }
                    }

                    foreach (long roleID in relationshipUpdateModel.IDsToRemove)
                    {
                        if (await rolesRepository.FindRoleByIDAsync(roleID) is Roles role
                            && user.Roles.Contains(role))
                        {
                            user.Roles.Remove(role);
                        }
                    }

                    await usersRepository.UpdateUserAsync(user);
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
                exceptionHandler.AddBreadcrumb("Error in service when updating the roles of the user.",
                    new Dictionary<string, object?>()
                    {
                        { nameof(userID), userID },
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