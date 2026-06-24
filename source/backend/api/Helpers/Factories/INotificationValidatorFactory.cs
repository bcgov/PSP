using Pims.Api.Helpers.Validators;
using Pims.Dal.Entities;

namespace Pims.Api.Helpers.Factories
{
    public interface INotificationValidatorFactory
    {
        INotificationSubtypeValidator GetValidator(PimsNotification notification);
    }
}