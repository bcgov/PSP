using FluentAssertions;
using NetTopologySuite.Geometries;
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
    public class PropertyFilterTest
    {
        #region Tests
        [Fact]
        public void PropertyFilter_Default_Constructor()
        {
            // Arrange
            // Act
            var filter = new PropertyFilter();

            // Assert
            filter.Page.Should().Be(1);
            filter.Quantity.Should().Be(10);
            filter.PID.Should().BeNull();
        }

        [Fact]
        public void PropertyFilter_Constructor_01()
        {
            // Arrange
            var nelat = 4.454;
            var nelng = 3.434;
            var swlat = 2.233;
            var swlng = 5.565;

            // Act
            var filter = new PropertyFilter(nelat, nelng, swlat, swlng);

            // Assert
            filter.NELatitude.Should().Be(nelat);
            filter.NELongitude.Should().Be(nelng);
            filter.SWLatitude.Should().Be(swlat);
            filter.SWLongitude.Should().Be(swlng);
        }

        [Fact]
        public void PropertyFilter_Constructor_02()
        {
            // Arrange
            var envelop = new Envelope(4.454, 3.434, 2.233, 5.565);

            // Act
            var filter = new PropertyFilter(envelop);

            // Assert
            filter.NELatitude.Should().Be(envelop.MaxY);
            filter.NELongitude.Should().Be(envelop.MaxX);
            filter.SWLatitude.Should().Be(envelop.MinY);
            filter.SWLongitude.Should().Be(envelop.MinX);
        }

        [Fact]
        public void PropertyFilter_Constructor_02_Null()
        {
            // Arrange
            // Act
            var filter = new PropertyFilter((Envelope)null);

            // Assert
            filter.NELatitude.Should().BeNull();
            filter.NELongitude.Should().BeNull();
            filter.SWLatitude.Should().BeNull();
            filter.SWLongitude.Should().BeNull();
        }

        [Fact]
        public void PropertyFilter_Constructor_03()
        {
            // Arrange
            var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery("?page=2&quantity=3&pid=323423&zoning=zone&zoningPotential=potential&minLandArea=343&maxLandArea=444&propertyType=Land&constructionTypeId=4&predominateUseId=4&floorCount=4&tenancy=ten&minRentableArea=343&maxRentableArea=444&includeAllProperties=true&sort=one,two");

            // Act
            var filter = new PropertyFilter(query);

            // Assert
            filter.Page.Should().Be(2);
            filter.Quantity.Should().Be(3);
            filter.PID.Should().Be("323423");
            filter.Sort.Should().BeEquivalentTo(new[] { "one", "two" });
        }

        #region IsValid
        [Fact]
        public void PropertyFilter_IsValid()
        {
            // Arrange
            var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery("?pid=323423");
            var filter = new PropertyFilter(query);

            // Act
            var result = filter.IsValid();

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void PropertyFilter_Base_IsValid()
        {
            // Arrange
            var filter = new PropertyFilter();

            // Act
            var result = filter.IsValid();

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void PropertyFilter_False()
        {
            // Arrange
            var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery("?page=0");
            var filter = new PropertyFilter(query);

            // Act
            var result = filter.IsValid();

            // Assert
            result.Should().BeFalse();
        }
        #endregion
        #endregion
    }
}
