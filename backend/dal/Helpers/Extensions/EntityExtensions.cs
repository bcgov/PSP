using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Pims.Dal.Helpers.Extensions
{
    /// <summary>
    /// EntityExtensions static class, provides extensions methods for entities.
    /// </summary>
    public static class EntityExtensions
    {
        /// <summary>
        /// Throw exception if the user is not allowed to edit the specified entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="paramName"></param>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <param name="message"></param>
        /// <typeparam name="T"></typeparam>
        /// <exception type="ArgumentNullException">Entity argument cannot be null.</exception>
        /// <exception type="ConcurrencyControlNumberMissingException">Entity.ConcurrencyControlNumber cannot be null.</exception>
        /// <exception type="NotAuthorizedException">User must have specified 'role'.</exception>
        /// <returns></returns>
        public static T ThrowIfNotAllowedToEdit<T>(this T entity, string paramName, ClaimsPrincipal user, string role, string message = null) where T : class, IBaseEntity
        {
            entity.ThrowIfNull(paramName);
            user.ThrowIfNotAuthorized(role, message);

            return entity;
        }

        /// <summary>
        /// Throw exception if the 'user' is not allowed to edit the specified 'entity'.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="paramName"></param>
        /// <param name="user"></param>
        /// <param name="permission"></param>
        /// <param name="message"></param>
        /// <typeparam name="T"></typeparam>
        /// <exception type="ArgumentNullException">Entity argument cannot be null.</exception>
        /// <exception type="ConcurrencyControlNumberMissingException">Entity.ConcurrencyControlNumber cannot be null.</exception>
        /// <exception type="NotAuthorizedException">User must have specified 'role'.</exception>
        /// <returns></returns>
        public static T ThrowIfNotAllowedToEdit<T>(this T entity, string paramName, ClaimsPrincipal user, Permissions permission, string message = null) where T : class, IBaseEntity
        {
            entity.ThrowIfNull(paramName);
            user.ThrowIfNotAuthorized(permission, message);

            return entity;
        }

        /// <summary>
        /// Throw exception if the 'user' is not allowed to edit the specified 'entity'.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="paramName"></param>
        /// <param name="user"></param>
        /// <param name="permission"></param>
        /// <param name="requireAll"></param>
        /// <param name="message"></param>
        /// <typeparam name="T"></typeparam>
        /// <exception type="ArgumentNullException">Entity argument cannot be null.</exception>
        /// <exception type="ConcurrencyControlNumberMissingException">Entity.ConcurrencyControlNumber cannot be null.</exception>
        /// <exception type="NotAuthorizedException">User must have specified 'role'.</exception>
        /// <returns></returns>
        public static T ThrowIfNotAllowedToEdit<T>(this T entity, string paramName, ClaimsPrincipal user, Permissions[] permission, bool requireAll = false, string message = null) where T : class, IBaseEntity
        {
            entity.ThrowIfNull(paramName);
            if (requireAll) user.ThrowIfNotAllAuthorized(permission);
            else user.ThrowIfNotAuthorized(permission, message);

            return entity;
        }

        /// <summary>
        /// When manipulating entities it is necessary to reset the original value for 'ConcurrencyControlNumber' so that concurrency checking can occur.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="context"></param>
        public static void SetOriginalConcurrencyControlNumber<T>(this T source, DbContext context) where T : class, IBaseEntity
        {
            context.SetOriginalConcurrencyControlNumber(source);
        }

        /// <summary>
        /// Update a single child navigation property on a parent entity specified by T and parentId.
        /// Expects to be passed a complete list of child entities for the targeted navigation property.
        /// This method will update the database such that the navigation property for the parent contains the exact list of children passed to this method.
        /// </summary>
        /// <typeparam name="T">The parent entity type</typeparam>
        /// <typeparam name="I">The type of the id property</typeparam>
        /// <typeparam name="C">The type of the child navigation property being targeted for updates.</typeparam>
        /// <param name="context"></param>
        /// <param name="childNavigation"></param>
        /// <param name="parentId"></param>
        /// <param name="children"></param>
        public static void UpdateChild<T, I, C>(this PimsContext context, Expression<Func<T, object>> childNavigation, I parentId, C[] children) where T : IdentityBaseAppEntity<I> where C : IdentityBaseAppEntity<I>
        {
            var dbEntity = context.Find<T>(parentId);

            var dbEntry = context.Entry(dbEntity);

            var propertyName = childNavigation.GetPropertyAccess().Name;
            var dbItemsEntry = dbEntry.Collection(propertyName);
            var accessor = dbItemsEntry.Metadata.GetCollectionAccessor();

            dbItemsEntry.Load();
            var dbItemsMap = dbItemsEntry.CurrentValue.Cast<IdentityBaseAppEntity<I>>()
                .ToDictionary(e => e.Id);

            foreach (var item in children)
            {
                if (!dbItemsMap.TryGetValue(item.Id, out var oldItem))
                    accessor.Add(dbEntity, item, false);
                else
                {
                    context.Entry(oldItem).CurrentValues.SetValues(item);
                    dbItemsMap.Remove(item.Id);
                }
            }

            foreach (var oldItem in dbItemsMap.Values)
            {
                accessor.Remove(dbEntity, oldItem);
                context.Remove(oldItem);
            }
        }
    }
}
