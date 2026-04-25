using System;
using System.Collections.Generic;
using System.Linq;
using Pims.Dal;
using Pims.Dal.Entities;

namespace Pims.Api.Helpers.Validators
{
    public class ExpropOwnerHistoryValidator : INotificationSubtypeValidator
    {
        private readonly PimsContext _context;

        public ExpropOwnerHistoryValidator(PimsContext context)
        {
            _context = context;
        }

        public void Validate(PimsNotification notification)
        {
            ArgumentNullException.ThrowIfNull(notification);
            var isValid = notification.ExpropOwnerHistoryId.HasValue && notification.AcquisitionFileId.HasValue;
            if (!isValid)
            {
                throw new InvalidOperationException("EXPROP_OWNER_HISTORY_ID must be associated with ACQUISITION_FILE_ID.");
            }

            var history = _context.PimsExpropOwnerHistories
                .FirstOrDefault(eoh => eoh.ExpropOwnerHistoryId == notification.ExpropOwnerHistoryId.Value) ?? throw new KeyNotFoundException("Expropriation Owner History not found.");
            if (history.AcquisitionFileId != notification.AcquisitionFileId)
            {
                throw new InvalidOperationException("Expropriation Owner History's AcquisitionFileId does not match the notification's AcquisitionFileId.");
            }
        }
    }
}
