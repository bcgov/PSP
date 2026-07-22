using System;
using System.Collections.Generic;
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
            ArgumentNullException.ThrowIfNull(notification);
            var isValid = notification.AgreementId.HasValue && notification.AcquisitionFileId.HasValue;
            if (!isValid)
            {
                throw new InvalidOperationException("AGREEMENT_ID must be associated with ACQUISITION_FILE_ID.");
            }

            var agreement = _context.PimsAgreements
                .FirstOrDefault(a => a.AgreementId == notification.AgreementId.Value) ?? throw new KeyNotFoundException("Agreement not found.");
            if (agreement.AcquisitionFileId != notification.AcquisitionFileId)
            {
                throw new InvalidOperationException("Agreement's AcquisitionFileId does not match the notification's AcquisitionFileId.");
            }
        }
    }
}
