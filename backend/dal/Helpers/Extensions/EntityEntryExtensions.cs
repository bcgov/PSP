using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Pims.Dal.Entities;

namespace Pims.Dal.Extensions
{
    /// <summary>
    /// EntityEntryExtensions static class, provides extension methods for EntityEntry objects.
    /// </summary>
    public static class EntityEntryExtensions
    {
        /// <summary>
        /// Do not allow editing the App created properties.
        /// Reset them to their original values.
        /// </summary>
        /// <param name="entry"></param>
        public static void UndoAppCreatedEdits(this EntityEntry entry)
        {
            if (entry.Entity is IDisableBaseAppEntity entity)
            {
                var oCreatedOn = (DateTime)entry.OriginalValues[nameof(entity.AppCreateTimestamp)];
                if (!oCreatedOn.Equals(entity.AppCreateTimestamp))
                    entity.AppCreateTimestamp = oCreatedOn;

                var oCreatedBy = (string)entry.OriginalValues[nameof(entity.AppCreateUserid)];
                if (String.IsNullOrWhiteSpace(entity.AppCreateUserid) || !oCreatedBy.Equals(entity.AppCreateUserid))
                    entity.AppCreateUserid = oCreatedBy;

                var oCreatedByKey = (Guid?)entry.OriginalValues[nameof(entity.AppCreateUserGuid)];
                if (!entity.AppCreateUserGuid.HasValue || !oCreatedByKey.Equals(entity.AppCreateUserGuid))
                    entity.AppCreateUserGuid = oCreatedByKey;

                var oCreatedByDirectory = (string)entry.OriginalValues[nameof(entity.AppCreateUserDirectory)];
                if (String.IsNullOrWhiteSpace(entity.AppCreateUserDirectory) || !oCreatedByDirectory.Equals(entity.AppCreateUserDirectory))
                    entity.AppCreateUserDirectory = oCreatedByDirectory;
            }
        }

        /// <summary>
        /// Do not allow editing the App created properties.
        /// Reset them to their original values.
        /// </summary>
        /// <param name="entry"></param>
        public static void UndoDbCreatedEdits(this EntityEntry entry)
        {
            if (entry.Entity is IDisableBaseAppEntity entity)
            {
                var oCreatedOn = (DateTime)entry.OriginalValues[nameof(entity.DbCreateTimestamp)];
                if (!oCreatedOn.Equals(entity.DbCreateTimestamp))
                    entity.DbCreateTimestamp = oCreatedOn;

                var oCreatedBy = (string)entry.OriginalValues[nameof(entity.DbCreateUserid)];
                if (String.IsNullOrWhiteSpace(entity.DbCreateUserid) || !oCreatedBy.Equals(entity.DbCreateUserid))
                    entity.DbCreateUserid = oCreatedBy;
            }
        }

        /// <summary>
        /// Update the rowversion by incrementing it.
        /// </summary>
        /// <param name="entry"></param>
        public static void UpdateRowversion(this EntityEntry entry)
        {
            if (entry.Entity is IBaseEntity entity)
            {
                entity.ConcurrencyControlNumber += 1;
            }
        }

        /// <summary>
        /// Auto update the application audit properties.
        /// This handles both added and updated entities.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="username"></param>
        /// <param name="userKey"></param>
        /// <param name="userDirectory"></param>
        public static void UpdateAppAuditProperties(this EntityEntry entry, string username, Guid userKey, string userDirectory)
        {
            if (entry.Entity is IDisableBaseAppEntity disableEntity)
            {
                disableEntity.IsDisabled ??= false;
            }
            if (entry.Entity is IBaseAppEntity entity)
            {
                var date = DateTime.UtcNow;
                if (entry.State == EntityState.Added)
                {
                    entity.AppCreateTimestamp = date;
                    entity.AppCreateUserid = username;
                    entity.AppCreateUserGuid = userKey;
                    entity.AppCreateUserDirectory = userDirectory;
                }
                else
                {
                    entry.UndoAppCreatedEdits();
                }
                entity.AppLastUpdateTimestamp = date;
                entity.AppLastUpdateUserid = username;
                entity.AppLastUpdateUserGuid = userKey;
                entity.AppLastUpdateUserDirectory = userDirectory;
            }
            UpdateDbAuditProperties(entry, username);
            entry.UpdateRowversion();
        }

        /// <summary>
        /// Auto update the application audit properties.
        /// This handles both added and updated entities.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="username"></param>
        /// <param name="userKey"></param>
        /// <param name="userDirectory"></param>
        public static void UpdateDbAuditProperties(this EntityEntry entry, string username)
        {
            if (entry.Entity is IDisableBaseAppEntity entity)
            {
                var date = DateTime.UtcNow;
                if (entry.State == EntityState.Added)
                {
                    entity.DbCreateTimestamp = date;
                    entity.DbCreateUserid = username;
                }
                else
                {
                    entry.UndoDbCreatedEdits();
                }
                entity.DbLastUpdateTimestamp = date;
                entity.DbLastUpdateUserid = username;
            }
        }
    }
}
