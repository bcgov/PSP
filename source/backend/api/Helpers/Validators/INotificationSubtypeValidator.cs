using Pims.Dal.Entities;

namespace Pims.Api.Helpers.Validators
{
    public interface INotificationSubtypeValidator
    {
        void Validate(PimsNotification notification);
    }
}
