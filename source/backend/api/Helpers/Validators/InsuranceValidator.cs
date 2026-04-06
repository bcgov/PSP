using System;
using System.Linq;
using Pims.Dal;
using Pims.Dal.Entities;

namespace Pims.Api.Helpers.Validators
{
    public class InsuranceValidator : INotificationSubtypeValidator
    {
        private readonly PimsContext _context;

        public InsuranceValidator(PimsContext context)
        {
            _context = context;
        }

        public void Validate(PimsNotification notification)
        {
            if (!notification.LeaseId.HasValue)
            {
                throw new InvalidOperationException("INSURANCE_ID must be associated with LEASE_ID.");
            }

            if (notification.InsuranceId.HasValue && notification.LeaseId.HasValue)
            {
                var insurance = _context.PimsInsurances
                    .FirstOrDefault(i => i.InsuranceId == notification.InsuranceId.Value) ?? throw new InvalidOperationException("Insurance not found.");
                if (insurance.LeaseId != notification.LeaseId)
                {
                    throw new InvalidOperationException("Insurance's LeaseId does not match the notification's LeaseId.");
                }
            }
        }
    }
}
