using System;
using Microsoft.Extensions.DependencyInjection;
using Pims.Api.Helpers.Factories;
using Pims.Api.Models.CodeTypes;
using Pims.Dal.Entities;

namespace Pims.Api.Helpers.Validators
{
    public class NotificationValidatorFactory : INotificationValidatorFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public NotificationValidatorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public INotificationSubtypeValidator GetValidator(PimsNotification notification)
        {
            ArgumentNullException.ThrowIfNull(notification);

            return notification.NotificationTypeCode switch
            {
                nameof(NotificationTypes.TAKE_SRW) => _serviceProvider.GetRequiredService<TakeValidator>(),
                nameof(NotificationTypes.TAKE_LAT) => _serviceProvider.GetRequiredService<TakeValidator>(),
                nameof(NotificationTypes.TAKE_LTC) => _serviceProvider.GetRequiredService<TakeValidator>(),
                nameof(NotificationTypes.TAKE_LPYBLE) => _serviceProvider.GetRequiredService<TakeValidator>(),
                nameof(NotificationTypes.L_RENEWAL) => _serviceProvider.GetRequiredService<LeaseRenewalValidator>(),
                nameof(NotificationTypes.L_INSURANCE) => _serviceProvider.GetRequiredService<InsuranceValidator>(),
                nameof(NotificationTypes.L_CONSULTFN) => _serviceProvider.GetRequiredService<LeaseConsultationValidator>(),
                nameof(NotificationTypes.NOC) => _serviceProvider.GetRequiredService<NoticeOfClaimValidator>(),
                nameof(NotificationTypes.AGMT_SIGND) => _serviceProvider.GetRequiredService<AgreementValidator>(),
                nameof(NotificationTypes.EXPROPH_APPEFFDT) => _serviceProvider.GetRequiredService<ExpropOwnerHistoryValidator>(),
                _ => null,
            };
        }
    }
}
