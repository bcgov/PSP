using System;
using System.Collections.Generic;
using System.Linq;
using Pims.Dal;
using Pims.Dal.Entities;

namespace Pims.Api.Helpers.Validators
{
    public class DispositionAgreementValidator : INotificationSubtypeValidator
    {
        private readonly PimsContext _context;

        public DispositionAgreementValidator(PimsContext context)
        {
            _context = context;
        }

        public void Validate(PimsNotification notification)
        {
            ArgumentNullException.ThrowIfNull(notification);

            var isValid = notification.DispositionAgreementId.HasValue
                && notification.DispositionFileId.HasValue;

            if (!isValid)
            {
                throw new InvalidOperationException(
                    "DISPOSITION_AGREEMENT_ID must be associated with DISPOSITION_FILE_ID.");
            }

            var agreement = _context.PimsDispositionAgreements
                .FirstOrDefault(a =>
                    a.DispositionAgreementId == notification.DispositionAgreementId.Value)
                ?? throw new KeyNotFoundException("Disposition agreement not found.");

            if (agreement.DispositionFileId != notification.DispositionFileId.Value)
            {
                throw new InvalidOperationException(
                    "Agreement's DispositionFileId does not match the notification's DispositionFileId.");
            }
        }
    }
}