using Pims.Dal.Entities;
using System;

namespace Pims.Api.Helpers.Validators
{
    public class AgreementValidator : INotificationSubtypeValidator
    {
        public void Validate(PimsNotification notification)
        {
            if (!(notification.AcquisitionFileId.HasValue || notification.DispositionFileId.HasValue))
            {
                throw new InvalidOperationException("AGREEMENT_ID must be associated with ACQUISITION_FILE_ID or DISPOSITION_FILE_ID.");
            }
        }
    }
}
