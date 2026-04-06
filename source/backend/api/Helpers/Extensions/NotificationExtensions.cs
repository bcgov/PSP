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

            if (fileIdCount != 1 || subIdCount != 1)
            {
                throw new InvalidOperationException("Exactly one file id and one sub-id must be populated.");
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