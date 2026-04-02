using Pims.Dal;
using Pims.Dal.Entities;
using System;
using System.Linq;

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
            if (!notification.AcquisitionFileId.HasValue)
            {
                throw new InvalidOperationException("EXPROP_OWNER_HISTORY_ID must be associated with ACQUISITION_FILE_ID.");
            }

            if (notification.ExpropOwnerHistoryId.HasValue && notification.AcquisitionFileId.HasValue)
            {
                var history = _context.PimsExpropOwnerHistories
                    .FirstOrDefault(eoh => eoh.ExpropOwnerHistoryId == notification.ExpropOwnerHistoryId.Value) ?? throw new InvalidOperationException("Expropriation Owner History not found.");
                if (history.AcquisitionFileId != notification.AcquisitionFileId)
                {
                    throw new InvalidOperationException("Expropriation Owner History's AcquisitionFileId does not match the notification's AcquisitionFileId.");
                }
            }
        }
    }
}
