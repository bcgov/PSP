using System;
using Pims.Api.Models.CodeTypes;
using Pims.Core.Extensions;
using Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Notification
{
    /// <summary>
    /// Resolves the derived inbox-item fields that aren't a direct column on the delivery
    /// (output) record from the materialized notification graph.
    ///
    /// These rely on the deep include chain in NotificationInboxRepository.BuildUserNotificationsQuery
    /// (deep: true), which loads each of the notification's FK navigation properties. Each
    /// resolver dispatches on whichever FK is set and reads from the corresponding loaded
    /// navigation property. All access is null-guarded so a shallow or partially-loaded
    /// entity yields an empty/None result rather than throwing.
    /// </summary>
    public static class NotificationInboxItemResolver
    {
        public static PimsNotificationType GetNotificationType(PimsNotificationUserOutput output)
        {
            var notification = output?.NotificationUser?.Notification;
            return notification?.NotificationTypeCodeNavigation;
        }

        public static string GetSubject(PimsNotificationUserOutput output)
        {
            var notification = output?.NotificationUser?.Notification;
            if (notification is null)
            {
                return string.Empty;
            }

            if (notification.AcquisitionFileId.HasValue && notification.AcquisitionFile != null)
            {
                return $"Acquisition File #: {GetFileNameOrNumber(notification.AcquisitionFile)}";
            }
            if (notification.DispositionFileId.HasValue && notification.DispositionFile != null)
            {
                return $"Disposition File #: {GetFileNameOrNumber(notification.DispositionFile)}";
            }
            if (notification.ResearchFileId.HasValue && notification.ResearchFile != null)
            {
                return $"Research File #: {GetFileNameOrNumber(notification.ResearchFile)}";
            }
            if (notification.ManagementFileId.HasValue && notification.ManagementFile != null)
            {
                return $"Management File #: {GetFileNameOrNumber(notification.ManagementFile)}";
            }
            if (notification.LeaseId.HasValue && notification.Lease != null)
            {
                return $"Lease #: {notification.Lease.LFileNo}";
            }

            return string.Empty;
        }

        public static DateTime? GetTrackedDate(PimsNotificationUserOutput src)
        {
            var notification = src?.NotificationUser?.Notification;
            if (notification is null)
            {
                return null;
            }

            return notification.NotificationTypeCode switch
            {
                // Acquisition sub-entities
                nameof(NotificationTypes.TAKE_SRW) => notification.Take?.SrwEndDt.ToNullableDateTime() ?? null,
                nameof(NotificationTypes.TAKE_LAT) => notification.Take?.LandActEndDt.ToNullableDateTime() ?? null,
                nameof(NotificationTypes.TAKE_LTC) => notification.Take?.LtcEndDt.ToNullableDateTime() ?? null,
                nameof(NotificationTypes.TAKE_LPYBLE) => notification.Take?.ActiveLeaseEndDt.ToNullableDateTime() ?? null,
                nameof(NotificationTypes.AGMT_SIGND) => null, // TODO: confirm column
                nameof(NotificationTypes.EXPROPH_APPEFFDT) => notification.ExpropOwnerHistory?.EventDt ?? null,

                // Lease sub-entities
                nameof(NotificationTypes.L_RENEWAL) => notification.LeaseRenewal?.ExpiryDt ?? null,
                nameof(NotificationTypes.L_INSURANCE) => notification.Insurance?.ExpiryDate.ToNullableDateTime() ?? null,
                nameof(NotificationTypes.L_CONSULTFN) => notification.LeaseConsultation?.RequestedOn ?? null,

                // Notice of Claim sub-entity (can be in either Acquisition of Management files)
                nameof(NotificationTypes.NOC) => notification.NoticeOfClaim?.ReceivedDt.ToNullableDateTime() ?? null,
                _ => null,
            };
        }

        // Display file name if available/non-empty, otherwise display file number in same format as list view screen.
        private static string GetFileNameOrNumber(PimsAcquisitionFile file)
        {
            if (file == null)
            {
                return string.Empty;
            }

            if (!string.IsNullOrWhiteSpace(file.FileName))
            {
                return file.FileName;
            }

            if (!string.IsNullOrWhiteSpace(file.LegacyFileNumber))
            {
                return file.LegacyFileNumber;
            }

            if (!string.IsNullOrWhiteSpace(file.FileNumberFormatted))
            {
                return file.FileNumberFormatted;
            }

            return string.Empty;
        }

        private static string GetFileNameOrNumber(PimsDispositionFile file)
        {
            if (file == null)
            {
                return string.Empty;
            }

            if (!string.IsNullOrWhiteSpace(file.FileName))
            {
                return file.FileName;
            }

            if (!string.IsNullOrWhiteSpace(file.FileReference))
            {
                return file.FileReference;
            }

            if (!string.IsNullOrWhiteSpace(file.FileNumber))
            {
                return $"D-{file.FileNumber}";
            }

            return string.Empty;
        }

        private static string GetFileNameOrNumber(PimsResearchFile file)
        {
            if (file == null)
            {
                return string.Empty;
            }

            if (!string.IsNullOrWhiteSpace(file.Name))
            {
                return file.Name;
            }

            if (!string.IsNullOrWhiteSpace(file.RfileNumber))
            {
                return file.RfileNumber;
            }

            return string.Empty;
        }

        private static string GetFileNameOrNumber(PimsManagementFile file)
        {
            if (file == null)
            {
                return string.Empty;
            }

            if (!string.IsNullOrWhiteSpace(file.FileName))
            {
                return file.FileName;
            }

            if (!string.IsNullOrWhiteSpace(file.LegacyFileNum))
            {
                return file.LegacyFileNum;
            }

            if (file.ManagementFileId > 0)
            {
                return $"M-{file.ManagementFileId}";
            }

            return string.Empty;
        }

    }
}
