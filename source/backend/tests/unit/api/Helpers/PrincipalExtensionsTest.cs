using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Pims.Core.Exceptions;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;
using Xunit;

namespace Pims.Api.Test.Helpers
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("area", "helpers")]
    [Trait("group", "permissons")]
    [ExcludeFromCodeCoverage]
    public class PrincipalExtensionsTest
    {
        private TestHelper _helper;

        public PrincipalExtensionsTest()
        {
            this._helper = new TestHelper();
        }

        #region Tests
        [Fact]
        public void LeaseRegionUserAccess_NoRegion_Success()
        {
            var pimsUser = EntityHelper.CreateUser("testuser");
            pimsUser.ThrowInvalidAccessToLeaseFile(null);
        }

        [Fact]
        public void LeaseRegionUserAccess_HasRegion_Failure()
        {
            var pimsUser = EntityHelper.CreateUser(1, Guid.NewGuid(), "testuser", regionCode: 1);
            Action act = () => pimsUser.ThrowInvalidAccessToLeaseFile(2);
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void LeaseRegionUserAccess_HasRegion_Success()
        {
            var pimsUser = EntityHelper.CreateUser(1, Guid.NewGuid(), "testuser", regionCode: 1);
            Action act = () => pimsUser.ThrowInvalidAccessToLeaseFile(1);
            act.Should().NotThrow<NotAuthorizedException>();
        }
        #endregion
    }
}
