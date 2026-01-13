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
        private readonly IMenusRepository menusRepository;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Public methods
        /// <inheritdoc />
        [ActionMethod]
        public async Task<IQueryable<RolesMenusListModel>> ListRoleMenusAsync(long roleID, IListParameters parameters)
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
                    return (role.Menus ?? Enumerable.Empty<Menus>())
                        .AsQueryable()
                        .OrderBy(x => x.Label)
                        .GetRange(parameters)
                        .ProjectTo<RolesMenusListModel>(mapper.ConfigurationProvider);
                }
            }
            catch
            {
                exceptionHandler.AddBreadcrumb("Error in service when listing the menus of the role.",
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
        public async Task UpdateRelationshipRoleMenusAsync(long roleID, RelationshipUpdateModel<long> relationshipUpdateModel)
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
                    role.Menus ??= [];

                    foreach (long menuID in relationshipUpdateModel.IDsToAdd)
                    {
                        if (await menusRepository.FindMenuByIDAsync(menuID) is Menus menu
                            && !role.Menus.Contains(menu))
                        {
                            role.Menus.Add(menu);
                        }
                    }

                    foreach (long menuID in relationshipUpdateModel.IDsToRemove)
                    {
                        if (await menusRepository.FindMenuByIDAsync(menuID) is Menus menu
                            && role.Menus.Contains(menu))
                        {
                            role.Menus.Remove(menu);
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
                exceptionHandler.AddBreadcrumb("Error in service when updating the menus of the role.",
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