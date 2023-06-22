using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Security;

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
        /// <typeparam name="T">The entity type.</typeparam>
        /// <exception type="ArgumentNullException">Entity argument cannot be null.</exception>
        /// <exception type="ConcurrencyControlNumberMissingException">Entity.ConcurrencyControlNumber cannot be null.</exception>
        /// <exception type="NotAuthorizedException">User must have specified 'role'.</exception>
        /// <returns></returns>
        public static T ThrowIfNotAllowedToEdit<T>(this T entity, string paramName, ClaimsPrincipal user, string role, string message = null)
            where T : class, IBaseEntity
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
        /// <typeparam name="T">The entity type.</typeparam>
        /// <exception type="ArgumentNullException">Entity argument cannot be null.</exception>
        /// <exception type="ConcurrencyControlNumberMissingException">Entity.ConcurrencyControlNumber cannot be null.</exception>
        /// <exception type="NotAuthorizedException">User must have specified 'role'.</exception>
        /// <returns></returns>
        public static T ThrowIfNotAllowedToEdit<T>(this T entity, string paramName, ClaimsPrincipal user, Permissions permission, string message = null)
            where T : class, IBaseEntity
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
        /// <typeparam name="T">The entity type.</typeparam>
        /// <exception type="ArgumentNullException">Entity argument cannot be null.</exception>
        /// <exception type="ConcurrencyControlNumberMissingException">Entity.ConcurrencyControlNumber cannot be null.</exception>
        /// <exception type="NotAuthorizedException">User must have specified 'role'.</exception>
        /// <returns></returns>
        public static T ThrowIfNotAllowedToEdit<T>(this T entity, string paramName, ClaimsPrincipal user, Permissions[] permission, bool requireAll = false, string message = null)
            where T : class, IBaseEntity
        {
            entity.ThrowIfNull(paramName);
            if (requireAll)
            {
                user.ThrowIfNotAllAuthorized(permission);
            }
            else
            {
                user.ThrowIfNotAuthorized(permission, message);
            }

            return entity;
        }

        /// <summary>
        /// Update a single child navigation property on a parent entity specified by T and parentId.
        /// Expects to be passed a complete list of child entities for the targeted navigation property.
        /// This method will update the database such that the navigation property for the parent contains the exact list of children passed to this method.
        /// </summary>
        /// <typeparam name="T_ParentEntity">The parent entity type.</typeparam>
        /// <typeparam name="T_ParentId">The type of the parent id property.</typeparam>
        /// <typeparam name="T_ChildEntity">The type of the child navigation property being targeted for updates.</typeparam>
        /// <typeparam name="T_ChildId">The type of the child id property.</typeparam>
        /// <param name="context"></param>
        /// <param name="childNavigation"></param>
        /// <param name="parentId"></param>
        /// <param name="children"></param>
        public static void UpdateChild<T_ParentEntity, T_ParentId, T_ChildEntity, T_ChildId>(this PimsContext context, Expression<Func<T_ParentEntity, object>> childNavigation, T_ParentId parentId, T_ChildEntity[] children, bool updateChildValues = true)
            where T_ParentId : IComparable, IComparable<T_ParentId>, IEquatable<T_ParentId>
            where T_ChildId : IComparable, IComparable<T_ChildId>, IEquatable<T_ChildId>
            where T_ParentEntity : StandardIdentityBaseAppEntity<T_ParentId>
            where T_ChildEntity : StandardIdentityBaseAppEntity<T_ChildId>
        {
            var dbEntity = context.Find<T_ParentEntity>(parentId);

            var dbEntry = context.Entry(dbEntity);

            var propertyName = childNavigation.GetPropertyAccess().Name;
            var dbItemsEntry = dbEntry.Collection(propertyName);
            var accessor = dbItemsEntry.Metadata.GetCollectionAccessor();

            dbItemsEntry.Load();
            var dbItemsMap = dbItemsEntry.CurrentValue.Cast<StandardIdentityBaseAppEntity<T_ChildId>>()
                .ToDictionary(e => e.Internal_Id);

            foreach (var item in children)
            {
                if (item.Internal_Id == null || !dbItemsMap.TryGetValue(item.Internal_Id, out var oldItem))
                {
                    accessor.Add(dbEntity, item, false);
                }
                else
                {
                    if (updateChildValues)
                    {
                        context.Entry(oldItem).CurrentValues.SetValues(item);
                        dbItemsMap.Remove(item.Internal_Id);
                    }
                }
            }

            foreach (var oldItem in dbItemsMap.Values)
            {
                if (children.FirstOrDefault(x => x.Internal_Id.Equals(oldItem.Internal_Id)) == null)
                {
                    accessor.Remove(dbEntity, oldItem);
                    context.Remove(oldItem);
                }
            }
        }

        /// <summary>
        /// Update a single grandchild navigation property on a parent entity specified by T and parentId.
        /// Expects to be passed a complete list of child and grandchild entities for the targeted navigation property.
        /// This method will update the database such that the navigation property for the parent contains the exact list of children and grandchildren passed to this method.
        /// </summary>
        /// <typeparam name="T_Entity">The parent entity type.</typeparam>
        /// <typeparam name="T_Id">The type of the id property.</typeparam>
        /// <typeparam name="T_ChildEntity">The type of the child navigation property being targeted for updates.</typeparam>
        /// <param name="context"></param>
        /// <param name="childNavigation"></param>
        /// <param name="grandchildNavigation"></param>
        /// <param name="parentId"></param>
        /// <param name="childrenWithGrandchildren">The collection of children (and grandchildren) to update. Assumes grandchildren can be accessed via grandchildNavigation.</param>
        /// <param name="updateGrandChildValues">if false, do not update existing grandchild fields(add/remove only).</param>
        public static void UpdateGrandchild<T_Entity, T_Id, T_ChildEntity>(
            this PimsContext context,
            Expression<Func<T_Entity, object>> childNavigation,
            Expression<Func<T_ChildEntity, object>> grandchildNavigation,
            T_Id parentId,
            T_ChildEntity[] childrenWithGrandchildren,
            bool updateGrandChildValues = true)
            where T_Id : IComparable, IComparable<T_Id>, IEquatable<T_Id>
            where T_Entity : StandardIdentityBaseAppEntity<T_Id>
            where T_ChildEntity : StandardIdentityBaseAppEntity<T_Id>
        {
            UpdateGrandchild(context, childNavigation, grandchildNavigation, parentId, childrenWithGrandchildren, (context, x) => true, updateGrandChildValues);
        }

        /// <summary>
        /// Update a single grandchild navigation property on a parent entity specified by T and parentId.
        /// Expects to be passed a complete list of child and grandchild entities for the targeted navigation property.
        /// This method will update the database such that the navigation property for the parent contains the exact list of children and grandchildren passed to this method.
        /// </summary>
        /// <typeparam name="T_Entity">The parent entity type.</typeparam>
        /// <typeparam name="T_Id">The type of the id property.</typeparam>
        /// <typeparam name="T_ChildEntity">The type of the child navigation property being targeted for updates.</typeparam>
        /// <param name="context"></param>
        /// <param name="childNavigation"></param>
        /// <param name="grandchildNavigation"></param>
        /// <param name="parentId"></param>
        /// <param name="childrenWithGrandchildren">The collection of children (and grandchildren) to update. Assumes grandchildren can be accessed via grandchildNavigation.</param>
        /// <param name="canDeleteGrandchild">A callback to determine whether is safe to remove a grandchild entity.</param>
        /// <param name="updateGrandChildValues">if false, do not update existing grandchild fields(add/remove only).</param>
        public static void UpdateGrandchild<T_Entity, T_Id, T_ChildEntity>(
            this PimsContext context,
            Expression<Func<T_Entity, object>> childNavigation,
            Expression<Func<T_ChildEntity, object>> grandchildNavigation,
            T_Id parentId,
            T_ChildEntity[] childrenWithGrandchildren,
            Func<PimsContext, T_ChildEntity, bool> canDeleteGrandchild,
            bool updateGrandChildValues = true)
            where T_Id : IComparable, IComparable<T_Id>, IEquatable<T_Id>
            where T_Entity : StandardIdentityBaseAppEntity<T_Id>
            where T_ChildEntity : StandardIdentityBaseAppEntity<T_Id>
        {
            var dbEntity = context.Find<T_Entity>(parentId);
            var dbEntry = context.Entry(dbEntity);

            var childPropertyName = childNavigation.GetPropertyAccess().Name;
            var childCollection = dbEntry.Collection(childPropertyName);
            var childAccessor = childCollection.Metadata.GetCollectionAccessor();

            childCollection.Load();
            var existingChildren = childCollection.CurrentValue.Cast<StandardIdentityBaseAppEntity<T_Id>>().ToDictionary(e => e.Internal_Id);

            // Compile the grandchildNavigation lambda expression so we can extract the value from the passed in array of children
            var grandchildPropertyName = grandchildNavigation.GetPropertyAccess().Name;
            var grandchildFunc = grandchildNavigation.Compile();

            foreach (var child in childrenWithGrandchildren)
            {
                if (!existingChildren.TryGetValue(child.Internal_Id, out var existingChild))
                {
                    childAccessor.Add(dbEntity, child, false);
                }
                else
                {
                    var dbChildEntry = context.Entry(existingChild);
                    dbChildEntry.CurrentValues.SetValues(child);

                    var grandchildReference = dbChildEntry.Reference(grandchildPropertyName);

                    // load grandchild navigation property
                    grandchildReference.Load();

                    // Update grandchild navigation with values passed in the array
                    var grandchildValue = grandchildFunc(child);
                    if (updateGrandChildValues)
                    {
                        if (grandchildReference?.TargetEntry is null && grandchildValue is not null)
                        {
                            grandchildReference.CurrentValue = grandchildValue;
                        }
                        else
                        {
                            grandchildReference.TargetEntry?.CurrentValues.SetValues(grandchildValue);
                        }
                    }

                    existingChildren.Remove(child.Internal_Id);
                }
            }

            foreach (var existingChild in existingChildren.Values)
            {
                var dbChildEntry = context.Entry(existingChild);

                childAccessor.Remove(dbEntity, existingChild);
                context.Remove(existingChild);

                // Also remove the grandchild referenced by the child being removed
                if (canDeleteGrandchild(context, existingChild as T_ChildEntity))
                {
                    // load grandchild navigation property
                    var grandchildReference = dbChildEntry.Reference(grandchildPropertyName);
                    grandchildReference.Load();

                    var dbGrandchild = grandchildReference.TargetEntry?.Entity;
                    if (dbGrandchild != null)
                    {
                        context.Remove(dbGrandchild);
                    }
                }
            }
        }
    }
}
