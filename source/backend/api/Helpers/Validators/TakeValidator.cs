using Pims.Dal.Entities;
using System;

namespace Pims.Api.Helpers.Validators
{
    public class TakeValidator : INotificationSubtypeValidator
    {
        public void Validate(PimsNotification notification)
        {
            if (!notification.AcquisitionFileId.HasValue)
            {
                throw new InvalidOperationException("TAKE_ID must be associated with ACQUISITION_FILE_ID.");
            }
        }
    }
}
