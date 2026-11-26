using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Pims.Api.Helpers.Validators;
using Pims.Api.Models.CodeTypes;
using Pims.Dal.Entities;
using Xunit;

namespace Pims.Api.Test.Helpers
{
    public class NotificationValidatorFactoryTest
    {
        [Theory]
        [InlineData(nameof(NotificationTypes.DSP_AGMT_AGMTDT))]
        [InlineData(nameof(NotificationTypes.DSP_AGMT_COMPTDT))]
        [InlineData(nameof(NotificationTypes.DSP_AGMT_TERMDT))]
        [InlineData(nameof(NotificationTypes.DSP_AGMT_POSNDT))]
        public void GetValidator_DispositionAgreementTypes_ReturnsDispositionAgreementValidator(
            string notificationType)
        {
            // Arrange
            var services = new ServiceCollection();

            services.AddSingleton<DispositionAgreementValidator>(
                _ => new DispositionAgreementValidator(null));

            using var provider = services.BuildServiceProvider();

            var factory = new NotificationValidatorFactory(provider);

            var notification = new PimsNotification
            {
                NotificationTypeCode = notificationType,
            };

            // Act
            var validator = factory.GetValidator(notification);

            // Assert
            validator.Should().BeOfType<DispositionAgreementValidator>();
        }
    }
}