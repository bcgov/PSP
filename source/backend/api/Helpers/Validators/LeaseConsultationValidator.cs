using Pims.Dal;
using Pims.Dal.Entities;
using System;
using System.Linq;

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
            if (!notification.LeaseId.HasValue)
            {
                throw new InvalidOperationException("LEASE_CONSULTATION_ID must be associated with LEASE_ID.");
            }
            if (notification.LeaseConsultationId.HasValue && notification.LeaseId.HasValue)
            {
                var consultation = _context.PimsLeaseConsultations
                    .FirstOrDefault(lc => lc.LeaseConsultationId == notification.LeaseConsultationId.Value) ?? throw new InvalidOperationException("Lease Consultation not found.");
                if (consultation.LeaseId != notification.LeaseId)
                {
                    throw new InvalidOperationException("Lease Consultation's LeaseId does not match the notification's LeaseId.");
                }
            }
        }
    }
}
