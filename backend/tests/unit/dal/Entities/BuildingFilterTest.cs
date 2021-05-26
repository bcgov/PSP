using FluentAssertions;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;

namespace Pims.Dal.Test.Entities
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("category", "entities")]
    [Trait("group", "entity")]
    [ExcludeFromCodeCoverage]
    public class BuildingFilterTest
    {
        #region Tests
        [Fact]
        public void BuildingFilter_Default_Constructor()
        {
            // Arrange
            // Act
            var filter = new BuildingFilter();

            // Assert
            filter.Page.Should().Be(1);
            filter.Quantity.Should().Be(10);
            filter.PID.Should().BeNull();
            filter.Zoning.Should().BeNull();
            filter.ZoningPotential.Should().BeNull();
            filter.MinLandArea.Should().BeNull();
            filter.MaxLandArea.Should().BeNull();
            filter.ConstructionTypeId.Should().BeNull();
            filter.PredominateUseId.Should().BeNull();
            filter.FloorCount.Should().BeNull();
            filter.Tenancy.Should().BeNull();
            filter.MinRentableArea.Should().BeNull();
            filter.MaxRentableArea.Should().BeNull();
        }

        [Fact]
        public void BuildingFilter_Constructor_01()
        {
            // Arrange
            var nelat = 4.454;
            var nelng = 3.434;
            var swlat = 2.233;
            var swlng = 5.565;

            // Act
            var filter = new BuildingFilter(nelat, nelng, swlat, swlng);

            // Assert
            filter.NELatitude.Should().Be(nelat);
            filter.NELongitude.Should().Be(nelng);
            filter.SWLatitude.Should().Be(swlat);
            filter.SWLongitude.Should().Be(swlng);
        }

        [Fact]
        public void BuildingFilter_Constructor_02()
        {
            // Arrange
            var address = "address";
            var agencyId = 1;
            var constructionTypeId = 2;
            var predominateUseId = 3;
            var floorCount = 4;
            var tenancy = "tenancy";
            var minRentableArea = 5.12f;
            var maxRentableArea = 6.55f;
            var minMarketValue = 7.44m;
            var maxMarketValue = 8.23m;
            var minAssessedValue = 9.1m;
            var maxAssessedValue = 10.2m;
            var sort = new[] { "one", "two " };

            // Act
            var filter = new BuildingFilter(address, agencyId, constructionTypeId, predominateUseId, floorCount, tenancy, minRentableArea, maxRentableArea, minMarketValue, maxMarketValue, minAssessedValue, maxAssessedValue, sort);

            // Assert
            filter.Address.Should().Be(address);
            filter.Agencies.First().Should().Be(agencyId);
            filter.ConstructionTypeId.Should().Be(constructionTypeId);
            filter.PredominateUseId.Should().Be(predominateUseId);
            filter.FloorCount.Should().Be(floorCount);
            filter.Tenancy.Should().Be(tenancy);
            filter.MinRentableArea.Should().Be(minRentableArea);
            filter.MaxRentableArea.Should().Be(maxRentableArea);
            filter.MinMarketValue.Should().Be(minMarketValue);
            filter.MaxMarketValue.Should().Be(maxMarketValue);
            filter.MinAssessedValue.Should().Be(minAssessedValue);
            filter.MaxAssessedValue.Should().Be(maxAssessedValue);
            filter.Sort.Should().BeEquivalentTo(sort);
        }

        [Fact]
        public void BuildingFilter_Constructor_02_NoAgency()
        {
            // Arrange
            var address = "address";
            var constructionTypeId = 2;
            var predominateUseId = 3;
            var floorCount = 4;
            var tenancy = "tenancy";
            var minRentableArea = 5.12f;
            var maxRentableArea = 6.55f;
            var minMarketValue = 7.44m;
            var maxMarketValue = 8.23m;
            var minAssessedValue = 9.1m;
            var maxAssessedValue = 10.2m;
            var sort = new[] { "one", "two " };

            // Act
            var filter = new BuildingFilter(address, (int?)null, constructionTypeId, predominateUseId, floorCount, tenancy, minRentableArea, maxRentableArea, minMarketValue, maxMarketValue, minAssessedValue, maxAssessedValue, sort);

            // Assert
            filter.Address.Should().Be(address);
            filter.Agencies.Should().BeNull();
            filter.ConstructionTypeId.Should().Be(constructionTypeId);
            filter.PredominateUseId.Should().Be(predominateUseId);
            filter.FloorCount.Should().Be(floorCount);
            filter.Tenancy.Should().Be(tenancy);
            filter.MinRentableArea.Should().Be(minRentableArea);
            filter.MaxRentableArea.Should().Be(maxRentableArea);
            filter.MinMarketValue.Should().Be(minMarketValue);
            filter.MaxMarketValue.Should().Be(maxMarketValue);
            filter.MinAssessedValue.Should().Be(minAssessedValue);
            filter.MaxAssessedValue.Should().Be(maxAssessedValue);
            filter.Sort.Should().BeEquivalentTo(sort);
        }

        [Fact]
        public void BuildingFilter_Constructor_03()
        {
            // Arrange
            var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery("?page=2&quantity=3&pid=323423&zoning=zone&zoningPotential=potential&minLandArea=343&maxLandArea=444&propertyType=Land&constructionTypeId=4&predominateUseId=4&floorCount=4&tenancy=ten&minRentableArea=343&maxRentableArea=444&sort=one,two");

            // Act
            var filter = new BuildingFilter(query);

            // Assert
            filter.Page.Should().Be(2);
            filter.Quantity.Should().Be(3);
            filter.PID.Should().Be("323423");
            filter.Zoning.Should().Be("zone");
            filter.ZoningPotential.Should().Be("potential");
            filter.MinLandArea.Should().Be(343);
            filter.MaxLandArea.Should().Be(444);
            filter.PropertyType.Should().Be(PropertyTypes.Land);
            filter.ConstructionTypeId.Should().Be(4);
            filter.PredominateUseId.Should().Be(4);
            filter.FloorCount.Should().Be(4);
            filter.Tenancy.Should().Be("ten");
            filter.MinRentableArea.Should().Be(343);
            filter.MaxRentableArea.Should().Be(444);
            filter.Sort.Should().BeEquivalentTo(new[] { "one", "two" });
        }

        #region IsValid
        [Fact]
        public void BuildingFilter_IsValid()
        {
            // Arrange
            var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery("?constructionTypeId=3");
            var filter = new BuildingFilter(query);

            // Act
            var result = filter.IsValid();

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void BuildingFilter_Base_IsValid()
        {
            // Arrange
            var filter = new BuildingFilter();

            // Act
            var result = filter.IsValid();

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void BuildingFilter_False()
        {
            // Arrange
            var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery("?page=0");
            var filter = new BuildingFilter(query);

            // Act
            var result = filter.IsValid();

            // Assert
            result.Should().BeFalse();
        }
        #endregion
        #endregion
    }
}
