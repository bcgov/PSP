using Pims.Dal.Entities;
using System;

namespace Pims.Api.Helpers.Validators
{
    public class NoticeOfClaimValidator : INotificationSubtypeValidator
    {
        public void Validate(PimsNotification notification)
        {
            if (!(notification.AcquisitionFileId.HasValue || notification.ManagementFileId.HasValue))
            {
                throw new InvalidOperationException("NOTICE_OF_CLAIM_ID must be associated with ACQUISITION_FILE_ID or MANAGEMENT_FILE_ID.");
            }
        }
    }
}
