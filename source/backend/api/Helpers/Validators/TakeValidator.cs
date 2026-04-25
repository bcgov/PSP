using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            ArgumentNullException.ThrowIfNull(notification);
            var isValid = notification.TakeId.HasValue && notification.AcquisitionFileId.HasValue;
            if (!isValid)
            {
                throw new InvalidOperationException("TAKE_ID must be associated with ACQUISITION_FILE_ID.");
            }

            var take = _context.PimsTakes
                .Include(t => t.PropertyAcquisitionFile)
                .Where(t => t.TakeId == notification.TakeId.Value)
                .FirstOrDefault() ?? throw new KeyNotFoundException("Take not found.");

            if (take.PropertyAcquisitionFile == null || take.PropertyAcquisitionFile.AcquisitionFileId != notification.AcquisitionFileId)
            {
                throw new InvalidOperationException("Take's AcquisitionFileId does not match the notification's AcquisitionFileId.");
            }
        }
    }
}
