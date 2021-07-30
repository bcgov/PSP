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
            if (entry.Entity is BaseAppEntity entity)
            {
                var oCreatedOn = (DateTime)entry.OriginalValues[nameof(entity.CreatedOn)];
                if (!oCreatedOn.Equals(entity.CreatedOn))
                    entity.CreatedOn = oCreatedOn;

                var oCreatedBy = (string)entry.OriginalValues[nameof(entity.CreatedBy)];
                if (String.IsNullOrWhiteSpace(entity.CreatedBy) || !oCreatedBy.Equals(entity.CreatedBy))
                    entity.CreatedBy = oCreatedBy;

                var oCreatedByKey = (Guid?)entry.OriginalValues[nameof(entity.CreatedByKey)];
                if (!entity.CreatedByKey.HasValue || !oCreatedByKey.Equals(entity.CreatedByKey))
                    entity.CreatedByKey = oCreatedByKey;

                var oCreatedByDirectory = (string)entry.OriginalValues[nameof(entity.CreatedByDirectory)];
                if (String.IsNullOrWhiteSpace(entity.CreatedByDirectory) || !oCreatedByDirectory.Equals(entity.CreatedByDirectory))
                    entity.CreatedByDirectory = oCreatedByDirectory;
            }
        }

        public static void UpdateRowversion(this EntityEntry entry)
        {
            if (entry.Entity is BaseEntity entity)
            {
                entity.RowVersion += 1;
            }
        }

        public static void UpdateAppAuditProperties(this EntityEntry entry, string username, Guid userKey, string userDirectory)
        {
            if (entry.Entity is BaseAppEntity entity)
            {
                var date = DateTime.UtcNow;
                if (entry.State == EntityState.Added)
                {
                    entity.CreatedOn = date;
                    entity.CreatedBy = username;
                    entity.CreatedByKey = userKey;
                    entity.CreatedByDirectory = userDirectory;
                }
                else
                {
                    entry.UndoAppCreatedEdits();
                }
                entity.UpdatedOn = date;
                entity.UpdatedBy = username;
                entity.UpdatedByKey = userKey;
                entity.UpdatedByDirectory = userDirectory;
            }

            entry.UpdateRowversion();
        }
    }
}
