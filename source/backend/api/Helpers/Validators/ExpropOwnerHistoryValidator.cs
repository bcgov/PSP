using Pims.Dal.Entities;
using System;

namespace Pims.Api.Helpers.Validators
{
    public class ExpropOwnerHistoryValidator : INotificationSubtypeValidator
    {
        public void Validate(PimsNotification notification)
        {
            if (!notification.AcquisitionFileId.HasValue)
            {
                throw new InvalidOperationException("EXPROP_OWNER_HISTORY_ID must be associated with ACQUISITION_FILE_ID.");
            }
        }
    }
}
