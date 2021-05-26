using FluentAssertions;
using NetTopologySuite.Geometries;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Pims.Dal.Test.Entities
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("category", "entities")]
    [Trait("group", "entity")]
    [ExcludeFromCodeCoverage]
    public class AllPropertyFilterTest
    {
        #region Tests
        [Fact]
        public void AllPropertyFilter_Default_Constructor()
        {
            // Arrange
            // Act
            var filter = new AllPropertyFilter();

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
            filter.IncludeAllProperties.Should().BeFalse();
        }

        [Fact]
        public void AllPropertyFilter_Constructor_01()
        {
            // Arrange
            var nelat = 4.454;
            var nelng = 3.434;
            var swlat = 2.233;
            var swlng = 5.565;

            // Act
            var filter = new AllPropertyFilter(nelat, nelng, swlat, swlng);

            // Assert
            filter.NELatitude.Should().Be(nelat);
            filter.NELongitude.Should().Be(nelng);
            filter.SWLatitude.Should().Be(swlat);
            filter.SWLongitude.Should().Be(swlng);
        }

        [Fact]
        public void AllPropertyFilter_Constructor_02()
        {
            // Arrange
            var envelop = new Envelope(4.454, 3.434, 2.233, 5.565);

            // Act
            var filter = new AllPropertyFilter(envelop);

            // Assert
            filter.NELatitude.Should().Be(envelop.MaxY);
            filter.NELongitude.Should().Be(envelop.MaxX);
            filter.SWLatitude.Should().Be(envelop.MinY);
            filter.SWLongitude.Should().Be(envelop.MinX);
        }

        [Fact]
        public void AllPropertyFilter_Constructor_02_Null()
        {
            // Arrange
            // Act
            var filter = new AllPropertyFilter((Envelope)null);

            // Assert
            filter.NELatitude.Should().BeNull();
            filter.NELongitude.Should().BeNull();
            filter.SWLatitude.Should().BeNull();
            filter.SWLongitude.Should().BeNull();
        }

        [Fact]
        public void AllPropertyFilter_Constructor_03()
        {
            // Arrange
            var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery("?page=2&quantity=3&pid=323423&zoning=zone&zoningPotential=potential&minLandArea=343&maxLandArea=444&propertyType=Land&constructionTypeId=4&predominateUseId=4&floorCount=4&tenancy=ten&minRentableArea=343&maxRentableArea=444&includeAllProperties=true&sort=one,two");

            // Act
            var filter = new AllPropertyFilter(query);

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
            filter.IncludeAllProperties.Should().BeTrue();
            filter.Sort.Should().BeEquivalentTo(new[] { "one", "two" });
        }

        #region IsValid
        [Fact]
        public void AllPropertyFilter_IsValid()
        {
            // Arrange
            var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery("?pid=323423");
            var filter = new AllPropertyFilter(query);

            // Act
            var result = filter.IsValid();

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void AllPropertyFilter_Base_IsValid()
        {
            // Arrange
            var filter = new AllPropertyFilter();

            // Act
            var result = filter.IsValid();

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void AllPropertyFilter_False()
        {
            // Arrange
            var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery("?page=0");
            var filter = new AllPropertyFilter(query);

            // Act
            var result = filter.IsValid();

            // Assert
            result.Should().BeFalse();
        }
        #endregion
        #endregion
    }
}
