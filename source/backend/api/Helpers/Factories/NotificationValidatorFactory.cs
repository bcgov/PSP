using System;
using Microsoft.Extensions.DependencyInjection;
using Pims.Api.Helpers.Factories;
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
            if (notification.AgreementId.HasValue)
            {
                return _serviceProvider.GetRequiredService<AgreementValidator>();
            }

            if (notification.LeaseConsultationId.HasValue)
            {
                return _serviceProvider.GetRequiredService<LeaseConsultationValidator>();
            }

            if (notification.LeaseRenewalId.HasValue)
            {
                return _serviceProvider.GetRequiredService<LeaseRenewalValidator>();
            }

            if (notification.NoticeOfClaimId.HasValue)
            {
                return _serviceProvider.GetRequiredService<NoticeOfClaimValidator>();
            }

            if (notification.InsuranceId.HasValue)
            {
                return _serviceProvider.GetRequiredService<InsuranceValidator>();
            }

            if (notification.ExpropOwnerHistoryId.HasValue)
            {
                return _serviceProvider.GetRequiredService<ExpropOwnerHistoryValidator>();
            }

            if (notification.TakeId.HasValue)
            {
                return _serviceProvider.GetRequiredService<TakeValidator>();
            }

            return null;
        }
    }
}
