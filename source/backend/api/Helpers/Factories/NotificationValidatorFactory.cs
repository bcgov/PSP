using Pims.Dal.Entities;
using System.Collections.Generic;

namespace Pims.Api.Helpers.Validators
{
    public static class NotificationValidatorFactory
    {
        private static readonly Dictionary<string, INotificationSubtypeValidator> Validators =
            new()
            {
                { "Agreement", new AgreementValidator() },
                { "LeaseConsultation", new LeaseConsultationValidator() },
                { "LeaseRenewal", new LeaseRenewalValidator() },
                { "NoticeOfClaim", new NoticeOfClaimValidator() },
                { "Insurance", new InsuranceValidator() },
                { "ExpropOwnerHistory", new ExpropOwnerHistoryValidator() },
                { "Take", new TakeValidator() },
            };

        public static INotificationSubtypeValidator GetValidator(PimsNotification notification)
        {
            if (notification.AgreementId.HasValue)
            {
                return Validators["Agreement"];
            }

            if (notification.LeaseConsultationId.HasValue)
            {
                return Validators["LeaseConsultation"];
            }

            if (notification.LeaseRenewalId.HasValue)
            {
                return Validators["LeaseRenewal"];
            }

            if (notification.NoticeOfClaimId.HasValue)
            {
                return Validators["NoticeOfClaim"];
            }

            if (notification.InsuranceId.HasValue)
            {
                return Validators["Insurance"];
            }

            if (notification.ExpropOwnerHistoryId.HasValue)
            {
                return Validators["ExpropOwnerHistory"];
            }

            if (notification.TakeId.HasValue)
            {
                return Validators["Take"];
            }

            return null;
        }
    }
}
