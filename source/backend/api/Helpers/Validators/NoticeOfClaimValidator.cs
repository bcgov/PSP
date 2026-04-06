using System;
using System.Linq;
using Pims.Dal;
using Pims.Dal.Entities;

namespace Pims.Api.Helpers.Validators
{
    public class NoticeOfClaimValidator : INotificationSubtypeValidator
    {
        private readonly PimsContext _context;

        public NoticeOfClaimValidator(PimsContext context)
        {
            _context = context;
        }

        public void Validate(PimsNotification notification)
        {
            if (!(notification.AcquisitionFileId.HasValue || notification.ManagementFileId.HasValue))
            {
                throw new InvalidOperationException("NOTICE_OF_CLAIM_ID must be associated with ACQUISITION_FILE_ID or MANAGEMENT_FILE_ID.");
            }

            if (notification.NoticeOfClaimId.HasValue && notification.AcquisitionFileId.HasValue)
            {
                var claim = _context.PimsNoticeOfClaims
                    .FirstOrDefault(noc => noc.NoticeOfClaimId == notification.NoticeOfClaimId.Value) ?? throw new InvalidOperationException("Notice of Claim not found.");
                if (claim.AcquisitionFileId != notification.AcquisitionFileId)
                {
                    throw new InvalidOperationException("Notice of Claim's AcquisitionFileId does not match the notification's AcquisitionFileId.");
                }
            }
        }
    }
}
