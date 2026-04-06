using System;
using System.Linq;
using Pims.Dal;
using Pims.Dal.Entities;

namespace Pims.Api.Helpers.Validators
{
    public class LeaseRenewalValidator : INotificationSubtypeValidator
    {
        private readonly PimsContext _context;

        public LeaseRenewalValidator(PimsContext context)
        {
            _context = context;
        }

        public void Validate(PimsNotification notification)
        {
            if (!notification.LeaseId.HasValue)
            {
                throw new InvalidOperationException("LEASE_RENEWAL_ID must be associated with LEASE_ID.");
            }

            if (notification.LeaseRenewalId.HasValue && notification.LeaseId.HasValue)
            {
                var renewal = _context.PimsLeaseRenewals
                    .FirstOrDefault(lr => lr.LeaseRenewalId == notification.LeaseRenewalId.Value) ?? throw new InvalidOperationException("Lease Renewal not found.");
                if (renewal.LeaseId != notification.LeaseId)
                {
                    throw new InvalidOperationException("Lease Renewal's LeaseId does not match the notification's LeaseId.");
                }
            }
        }
    }
}
