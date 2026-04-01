using Pims.Dal.Entities;
using System;

namespace Pims.Api.Helpers.Validators
{
    public class InsuranceValidator : INotificationSubtypeValidator
    {
        public void Validate(PimsNotification notification)
        {
            if (!notification.LeaseId.HasValue)
            {
                throw new InvalidOperationException("INSURANCE_ID must be associated with LEASE_ID.");
            }
        }
    }
}
