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
                return $"Acquisition File #: {notification.AcquisitionFile.FileNumberFormatted}";
            }
            if (notification.DispositionFileId.HasValue && notification.DispositionFile != null)
            {
                return $"Disposition File #: D-{notification.DispositionFile.FileNumber}";
            }
            if (notification.ResearchFileId.HasValue && notification.ResearchFile != null)
            {
                return $"Research File #: {notification.ResearchFile.RfileNumber}";
            }
            if (notification.ManagementFileId.HasValue && notification.ManagementFile != null)
            {
                return $"Management File #: M-{notification.ManagementFile.ManagementFileId}";
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
    }
}
