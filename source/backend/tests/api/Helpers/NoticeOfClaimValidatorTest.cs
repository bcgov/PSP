using System;
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
    public class NoticeOfClaimValidatorTest
    {
        [Theory]
        [InlineData(31, null, true)]
        [InlineData(null, 42, true)]
        [InlineData(99, null, false)]
        [InlineData(null, 99, false)]
        public void Validate_ClaimLinkedToBothFiles_ChecksNotificationFileOnly(int? acquisitionFileId, int? managementFileId, bool valid)
        {
            var helper = new TestHelper();
            using var context = helper.CreatePimsContext(new ClaimsPrincipal());
            context.PimsNoticeOfClaims.Add(new PimsNoticeOfClaim
            {
                NoticeOfClaimId = 8,
                AcquisitionFileId = 31,
                ManagementFileId = 42,
                AppCreateUserid = "test",
                AppCreateUserDirectory = "test",
                AppLastUpdateUserid = "test",
                AppLastUpdateUserDirectory = "test",
                DbLastUpdateUserid = "test",
            });
            context.SaveChanges();

            var notification = new PimsNotification
            {
                NoticeOfClaimId = 8,
                AcquisitionFileId = acquisitionFileId,
                ManagementFileId = managementFileId,
            };
            var validator = new NoticeOfClaimValidator(context);
            Action act = () => validator.Validate(notification);

            if (valid)
            {
                act.Should().NotThrow();
            }
            else
            {
                var field = acquisitionFileId.HasValue ? "AcquisitionFileId" : "ManagementFileId";
                act.Should().Throw<InvalidOperationException>()
                    .WithMessage($"Notice of Claim's {field} does not match the notification's {field}.");
            }
        }
    }
}
