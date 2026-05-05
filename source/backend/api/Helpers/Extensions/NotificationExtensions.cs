using System;
using System.Linq;
using Pims.Api.Helpers.Factories;
using Pims.Dal.Entities;

namespace Pims.Api.Helpers.Extensions
{
    public static class NotificationExtensions
    {
        /// <summary>
        /// Validates that only one file id or sub-id is populated, and that sub-ids are only populated with their correct corresponding file id(s).
        /// Throws InvalidOperationException if invalid.
        /// </summary>
        public static void ThrowIfInvalidFileAndSubId(this PimsNotification notification, INotificationValidatorFactory validatorFactory)
        {
            ArgumentNullException.ThrowIfNull(notification);
            ArgumentNullException.ThrowIfNull(validatorFactory);

            var fileIds = new[]
            {
                notification.AcquisitionFileId,
                notification.DispositionFileId,
                notification.ResearchFileId,
                notification.ManagementFileId,
                notification.LeaseId,
            };

            var subIds = new[]
            {
                notification.TakeId,
                notification.AgreementId,
                notification.LeaseConsultationId,
                notification.LeaseRenewalId,
                notification.NoticeOfClaimId,
                notification.InsuranceId,
                notification.ExpropOwnerHistoryId,
            };

            int fileIdCount = fileIds.Count(id => id.HasValue);
            int subIdCount = subIds.Count(id => id.HasValue);

            // At least one file id or sub-id must be populated, but not more than one of each.
            if (fileIdCount == 0 && subIdCount == 0)
            {
                throw new InvalidOperationException("At least one file id or sub-id must be populated.");
            }

            if (fileIdCount > 1)
            {
                throw new InvalidOperationException("Only one file id can be populated.");
            }

            if (subIdCount > 1)
            {
                throw new InvalidOperationException("Only one sub-id can be populated.");
            }

            // If a sub-id is populated, its corresponding file id must also be populated. Additionally, only one file id and one sub-id can be populated.
            if (subIdCount > 0 && fileIdCount == 0)
            {
                throw new InvalidOperationException("If a sub-id is populated, its corresponding file id must also be populated.");
            }

            var validator = validatorFactory.GetValidator(notification);
            validator?.Validate(notification);
        }

        public static bool IsOwnedByUser(this PimsNotification notification, long userId)
        {
            ArgumentNullException.ThrowIfNull(notification);

            return notification.PimsNotificationUsers.Any(u => u.UserId == userId);
        }
    }
}
