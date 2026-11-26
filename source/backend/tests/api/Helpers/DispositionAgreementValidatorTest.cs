using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using FluentAssertions;
using Pims.Api.Helpers.Validators;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Xunit;

namespace Pims.Api.Test.Helpers
{
    [ExcludeFromCodeCoverage]
    [Trait("category", "unit")]
    [Trait("category", "api")]
    public class DispositionAgreementValidatorTest
    {
        [Fact]
        public void Validate_ValidDispositionAgreement()
        {
            // Arrange
            var helper = new TestHelper();
            using var context = helper.CreatePimsContext(new ClaimsPrincipal());

            var dispositionFile = EntityHelper.CreateDispositionFile();

            var agreement = new PimsDispositionAgreement
            {
                DispositionFile = dispositionFile,
                DispositionAgreementTypeCode = "test",
                DspAgreementStatusTypeCode = "draft",
            };

            context.PimsDispositionAgreements.Add(agreement);
            context.SaveChanges();

            var notification = new PimsNotification
            {
                DispositionAgreementId = agreement.DispositionAgreementId,
                DispositionFileId = agreement.DispositionFileId,
            };

            var validator = new DispositionAgreementValidator(context);

            // Act
            Action act = () => validator.Validate(notification);

            // Assert
            act.Should().NotThrow();
        }

        [Theory]
        [InlineData(null, 10L)]
        [InlineData(10L, null)]
        public void Validate_MissingRequiredId_ThrowsException(
            long? dispositionAgreementId,
            long? dispositionFileId)
        {
            // Arrange
            var helper = new TestHelper();
            using var context = helper.CreatePimsContext(new ClaimsPrincipal());

            var notification = new PimsNotification
            {
                DispositionAgreementId = dispositionAgreementId,
                DispositionFileId = dispositionFileId,
            };

            var validator = new DispositionAgreementValidator(context);

            // Act
            Action act = () => validator.Validate(notification);

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("DISPOSITION_AGREEMENT_ID must be associated with DISPOSITION_FILE_ID.");
        }

        [Fact]
        public void Validate_DispositionAgreementNotFound_ThrowsException()
        {
            // Arrange
            var helper = new TestHelper();
            using var context = helper.CreatePimsContext(new ClaimsPrincipal());

            var notification = new PimsNotification
            {
                DispositionAgreementId = 999L,
                DispositionFileId = 10L,
            };

            var validator = new DispositionAgreementValidator(context);

            // Act
            Action act = () => validator.Validate(notification);

            // Assert
            act.Should().Throw<KeyNotFoundException>()
                .WithMessage("Disposition agreement not found.");
        }

        [Fact]
        public void Validate_DispositionAgreementBelongsToDifferentFile_ThrowsException()
        {
            // Arrange
            var helper = new TestHelper();
            using var context = helper.CreatePimsContext(new ClaimsPrincipal());

            var dispositionFile = EntityHelper.CreateDispositionFile();

            var agreement = new PimsDispositionAgreement
            {
                DispositionFile = dispositionFile,
                DispositionAgreementTypeCode = "test",
                DspAgreementStatusTypeCode = "draft",
            };

            context.PimsDispositionAgreements.Add(agreement);
            context.SaveChanges();

            var notification = new PimsNotification
            {
                DispositionAgreementId = agreement.DispositionAgreementId,
                DispositionFileId = agreement.DispositionFileId + 1,
            };

            var validator = new DispositionAgreementValidator(context);

            // Act
            Action act = () => validator.Validate(notification);

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .WithMessage(
                    "Agreement's DispositionFileId does not match the notification's DispositionFileId.");
        }
    }
}