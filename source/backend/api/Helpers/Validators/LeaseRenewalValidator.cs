using System;
using System.Collections.Generic;
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
            ArgumentNullException.ThrowIfNull(notification);
            var isValid = notification.LeaseId.HasValue && notification.LeaseRenewalId.HasValue;
            if (!isValid)
            {
                throw new InvalidOperationException("Lease and Licence renewal notification requires LEASE_ID and LEASE_RENEWAL_ID to be set.");
            }

            var renewal = _context.PimsLeaseRenewals
                .FirstOrDefault(lr => lr.LeaseRenewalId == notification.LeaseRenewalId.Value) ?? throw new KeyNotFoundException("Lease Renewal not found.");
            if (renewal.LeaseId != notification.LeaseId)
            {
                throw new InvalidOperationException("Lease Renewal's LeaseId does not match the notification's LeaseId.");
            }
        }
    }
}
