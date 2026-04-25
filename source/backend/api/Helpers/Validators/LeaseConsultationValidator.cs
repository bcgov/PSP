using System;
using System.Collections.Generic;
using System.Linq;
using Pims.Dal;
using Pims.Dal.Entities;

namespace Pims.Api.Helpers.Validators
{
    public class LeaseConsultationValidator : INotificationSubtypeValidator
    {
        private readonly PimsContext _context;

        public LeaseConsultationValidator(PimsContext context)
        {
            _context = context;
        }

        public void Validate(PimsNotification notification)
        {
            ArgumentNullException.ThrowIfNull(notification);
            var isValid = notification.LeaseConsultationId.HasValue && notification.LeaseId.HasValue;
            if (!isValid)
            {
                throw new InvalidOperationException("LEASE_CONSULTATION_ID must be associated with LEASE_ID.");
            }

            var consultation = _context.PimsLeaseConsultations
                .FirstOrDefault(lc => lc.LeaseConsultationId == notification.LeaseConsultationId.Value) ?? throw new KeyNotFoundException("Lease Consultation not found.");
            if (consultation.LeaseId != notification.LeaseId)
            {
                throw new InvalidOperationException("Lease Consultation's LeaseId does not match the notification's LeaseId.");
            }
        }
    }
}
