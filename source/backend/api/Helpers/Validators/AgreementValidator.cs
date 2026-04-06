using System;
using System.Linq;
using Pims.Dal;
using Pims.Dal.Entities;

namespace Pims.Api.Helpers.Validators
{
    public class AgreementValidator : INotificationSubtypeValidator
    {
        private readonly PimsContext _context;

        public AgreementValidator(PimsContext context)
        {
            _context = context;
        }

        public void Validate(PimsNotification notification)
        {
            if (!(notification.AcquisitionFileId.HasValue || notification.DispositionFileId.HasValue))
            {
                throw new InvalidOperationException("AGREEMENT_ID must be associated with ACQUISITION_FILE_ID or DISPOSITION_FILE_ID.");
            }

            if (notification.AgreementId.HasValue && notification.AcquisitionFileId.HasValue)
            {
                var agreement = _context.PimsAgreements
                    .FirstOrDefault(a => a.AgreementId == notification.AgreementId.Value) ?? throw new InvalidOperationException("Agreement not found.");
                if (agreement.AcquisitionFileId != notification.AcquisitionFileId)
                {
                    throw new InvalidOperationException("Agreement's AcquisitionFileId does not match the notification's AcquisitionFileId.");
                }
            }
        }
    }
}
