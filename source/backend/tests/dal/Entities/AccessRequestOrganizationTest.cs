using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Xunit;

namespace Pims.Dal.Test.Entities
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("category", "entities")]
    [Trait("group", "entity")]
    [ExcludeFromCodeCoverage]
    public class AccessRequestOrganizationTest
    {
        #region Tests
        [Fact]
        public void AccessRequestOrganization_Id_Constructor()
        {
            // Arrange
            var accessRequest = EntityHelper.CreateAccessRequest(5);
            var organization = EntityHelper.CreateOrganization(2, "ORG");

            // Act
            var accessRequestOrganization = new PimsAccessRequestOrganization(accessRequest.AccessRequestId, organization.Internal_Id);

            // Assert
            accessRequestOrganization.AccessRequestId.Should().Be(accessRequest.AccessRequestId);
            accessRequestOrganization.OrganizationId.Should().Be(organization.Internal_Id);
        }

        [Fact]
        public void AccessRequestOrganization_Object_Constructor()
        {
            // Arrange
            var accessRequest = EntityHelper.CreateAccessRequest(5);
            var organization = EntityHelper.CreateOrganization(2, "ORG");

            // Act
            var accessRequestOrganization = new PimsAccessRequestOrganization(accessRequest, organization);

            // Assert
            accessRequestOrganization.AccessRequestId.Should().Be(accessRequest.AccessRequestId);
            accessRequestOrganization.AccessRequest.Should().Be(accessRequest);
            accessRequestOrganization.OrganizationId.Should().Be(organization.Internal_Id);
            accessRequestOrganization.Organization.Should().Be(organization);
        }

        [Fact]
        public void AccessRequestOrganization_AccessRequest_Constructor_ArgumentNullException()
        {
            // Arrange
            var organization = EntityHelper.CreateOrganization(5, "ORG");

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new PimsAccessRequestOrganization(null, organization));
        }

        [Fact]
        public void AccessRequestOrganization_Organization_Constructor_ArgumentNullException()
        {
            // Arrange
            var accessRequest = EntityHelper.CreateAccessRequest(5);

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new PimsAccessRequestOrganization(accessRequest, null));
        }
        #endregion
    }
}
