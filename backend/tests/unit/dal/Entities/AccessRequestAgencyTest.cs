using FluentAssertions;
using Pims.Core.Test;
using Pims.Dal.Entities;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Pims.Dal.Test.Entities
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("category", "entities")]
    [Trait("group", "entity")]
    [ExcludeFromCodeCoverage]
    public class AccessRequestAgencyTest
    {
        #region Tests
        [Fact]
        public void AccessRequestAgency_Id_Constructor()
        {
            // Arrange
            var accessRequest = EntityHelper.CreateAccessRequest(5);
            var agency = EntityHelper.CreateAgency(2);

            // Act
            var accessRequestAgency = new AccessRequestAgency(accessRequest.Id, agency.Id);

            // Assert
            accessRequestAgency.AccessRequestId.Should().Be(accessRequest.Id);
            accessRequestAgency.AgencyId.Should().Be(agency.Id);
        }

        [Fact]
        public void AccessRequestAgency_Object_Constructor()
        {
            // Arrange
            var accessRequest = EntityHelper.CreateAccessRequest(5);
            var agency = EntityHelper.CreateAgency(2);

            // Act
            var accessRequestAgency = new AccessRequestAgency(accessRequest, agency);

            // Assert
            accessRequestAgency.AccessRequestId.Should().Be(accessRequest.Id);
            accessRequestAgency.AccessRequest.Should().Be(accessRequest);
            accessRequestAgency.AgencyId.Should().Be(agency.Id);
            accessRequestAgency.Agency.Should().Be(agency);
        }

        [Fact]
        public void AccessRequestAgency_AccessRequest_Constructor_ArgumentNullException()
        {
            // Arrange
            var agency = EntityHelper.CreateAgency(5);

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new AccessRequestAgency(null, agency));
        }

        [Fact]
        public void AccessRequestAgency_Agency_Constructor_ArgumentNullException()
        {
            // Arrange
            var accessRequest = EntityHelper.CreateAccessRequest(5);

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new AccessRequestAgency(accessRequest, null));
        }
        #endregion
    }
}
