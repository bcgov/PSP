using Pims.Dal.Entities;
using System;

namespace Pims.Api.Helpers.Validators
{
    public class LeaseRenewalValidator : INotificationSubtypeValidator
    {
        public void Validate(PimsNotification notification)
        {
            if (!notification.LeaseId.HasValue)
            {
                throw new InvalidOperationException("LEASE_RENEWAL_ID must be associated with LEASE_ID.");
            }
        }
    }
}
