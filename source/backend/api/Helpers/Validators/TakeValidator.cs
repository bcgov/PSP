using System;
using System.Linq;
using Pims.Dal;
using Pims.Dal.Entities;

namespace Pims.Api.Helpers.Validators
{
    public class TakeValidator : INotificationSubtypeValidator
    {
        private readonly PimsContext _context;

        public TakeValidator(PimsContext context)
        {
            _context = context;
        }

        public void Validate(PimsNotification notification)
        {
            if (!notification.AcquisitionFileId.HasValue)
            {
                throw new InvalidOperationException("TAKE_ID must be associated with ACQUISITION_FILE_ID.");
            }
            if (notification.TakeId.HasValue && notification.AcquisitionFileId.HasValue)
            {
                var take = _context.PimsTakes
                    .Where(t => t.TakeId == notification.TakeId.Value)
                    .Select(t => new
                    {
                        Take = t,
                        AcquisitionFileId = t.PropertyAcquisitionFile.AcquisitionFileId,
                    })
                    .FirstOrDefault() ?? throw new InvalidOperationException("Take not found.");

                if (take.AcquisitionFileId != notification.AcquisitionFileId)
                {
                    throw new InvalidOperationException("Take's AcquisitionFileId does not match the notification's AcquisitionFileId.");
                }
            }
        }
    }
}
