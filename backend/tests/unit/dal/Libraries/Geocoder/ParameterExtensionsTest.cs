using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Pims.Core.Extensions;
using Pims.Geocoder.Extensions;
using Pims.Geocoder.Parameters;
using Xunit;

namespace Pims.Dal.Test.Libraries.Geocoder
{
    [Trait("category", "unit")]
    [Trait("category", "geocoder")]
    [Trait("group", "geocoder")]
    [ExcludeFromCodeCoverage]
    public class ParameterExtensionsTest
    {
        #region Variables
        public static IEnumerable<object[]> Addresses =>
            new List<object[]>
            {
                new object[] { new AddressesParameters() },
                new object[] { new AddressesParameters() { Brief = true } },
                new object[] { new AddressesParameters() { MinScore = 34 } },
                new object[] { new AddressesParameters() { AddressString = "test" } },
            };
        #endregion

        #region Methods
        [Theory]
        [MemberData(nameof(Addresses))]
        public void ToQueryStringDictionary_Success(AddressesParameters address)
        {
            // Arrange
            // Act
            var result = address.ToQueryStringDictionary();

            // Assert
            result.Should().NotBeNull();
            result[nameof(address.MaxResults).LowercaseFirstCharacter()].Should().Be($"{address.MaxResults}");
            result[nameof(address.Echo).LowercaseFirstCharacter()].Should().Be($"{address.Echo}".ToLower());
            result[nameof(address.Brief).LowercaseFirstCharacter()].Should().Be($"{address.Brief}".ToLower());
            result[nameof(address.AutoComplete).LowercaseFirstCharacter()].Should().Be($"{address.AutoComplete}".ToLower());
            result[nameof(address.SetBack).LowercaseFirstCharacter()].Should().Be($"{address.SetBack}");
            result[nameof(address.OutputSRS).LowercaseFirstCharacter()].Should().Be($"{address.OutputSRS}");
            result[nameof(address.MinScore).LowercaseFirstCharacter()].Should().Be($"{address.MinScore}");
            result[nameof(address.MaxDistance).LowercaseFirstCharacter()].Should().Be($"{address.MaxDistance}");
            result[nameof(address.Extrapolate).LowercaseFirstCharacter()].Should().Be($"{address.Extrapolate}".ToLower());
            if (!string.IsNullOrWhiteSpace(address.Interpolation)) result[nameof(address.Interpolation).LowercaseFirstCharacter()].Should().Be(address.Interpolation);
            if (!string.IsNullOrWhiteSpace(address.Ver)) result[nameof(address.Ver).LowercaseFirstCharacter()].Should().Be(address.Ver);
            if (!string.IsNullOrWhiteSpace(address.AddressString)) result[nameof(address.AddressString).LowercaseFirstCharacter()].Should().Be(address.AddressString);
            if (!string.IsNullOrWhiteSpace(address.LocationDescriptor)) result[nameof(address.LocationDescriptor).LowercaseFirstCharacter()].Should().Be(address.LocationDescriptor);
            if (!string.IsNullOrWhiteSpace(address.MatchPrecision)) result[nameof(address.MatchPrecision).LowercaseFirstCharacter()].Should().Be(address.MatchPrecision);
            if (!string.IsNullOrWhiteSpace(address.MatchPrecisionNot)) result[nameof(address.MatchPrecisionNot).LowercaseFirstCharacter()].Should().Be(address.MatchPrecisionNot);
            if (!string.IsNullOrWhiteSpace(address.SiteName)) result[nameof(address.SiteName).LowercaseFirstCharacter()].Should().Be(address.SiteName);
            if (!string.IsNullOrWhiteSpace(address.UnitDesignator)) result[nameof(address.UnitDesignator).LowercaseFirstCharacter()].Should().Be(address.UnitDesignator);
            if (!string.IsNullOrWhiteSpace(address.UnitNumber)) result[nameof(address.UnitNumber).LowercaseFirstCharacter()].Should().Be(address.UnitNumber);
            if (!string.IsNullOrWhiteSpace(address.UnitNumberSuffix)) result[nameof(address.UnitNumberSuffix).LowercaseFirstCharacter()].Should().Be(address.UnitNumberSuffix);
            if (!string.IsNullOrWhiteSpace(address.CivicNumber)) result[nameof(address.CivicNumber).LowercaseFirstCharacter()].Should().Be(address.CivicNumber);
            if (!string.IsNullOrWhiteSpace(address.CivicNumberSuffix)) result[nameof(address.CivicNumberSuffix).LowercaseFirstCharacter()].Should().Be(address.CivicNumberSuffix);
            if (!string.IsNullOrWhiteSpace(address.StreetName)) result[nameof(address.StreetName).LowercaseFirstCharacter()].Should().Be(address.StreetName);
            if (!string.IsNullOrWhiteSpace(address.StreetType)) result[nameof(address.StreetType).LowercaseFirstCharacter()].Should().Be(address.StreetType);
            if (!string.IsNullOrWhiteSpace(address.StreetDirection)) result[nameof(address.StreetDirection).LowercaseFirstCharacter()].Should().Be(address.StreetDirection);
            if (!string.IsNullOrWhiteSpace(address.StreetQualifier)) result[nameof(address.StreetQualifier).LowercaseFirstCharacter()].Should().Be(address.StreetQualifier);
            if (!string.IsNullOrWhiteSpace(address.LocalityName)) result[nameof(address.LocalityName).LowercaseFirstCharacter()].Should().Be(address.LocalityName);
            if (!string.IsNullOrWhiteSpace(address.ProvinceCode)) result[nameof(address.ProvinceCode).LowercaseFirstCharacter()].Should().Be(address.ProvinceCode);
            if (!string.IsNullOrWhiteSpace(address.Bbox)) result[nameof(address.Bbox).LowercaseFirstCharacter()].Should().Be(address.Bbox);
            if (!string.IsNullOrWhiteSpace(address.Localities)) result[nameof(address.Localities).LowercaseFirstCharacter()].Should().Be(address.Localities);
            if (!string.IsNullOrWhiteSpace(address.NotLocalities)) result[nameof(address.NotLocalities).LowercaseFirstCharacter()].Should().Be(address.NotLocalities);
            if (!string.IsNullOrWhiteSpace(address.Center)) result[nameof(address.Center).LowercaseFirstCharacter()].Should().Be(address.Center);
            if (!string.IsNullOrWhiteSpace(address.ParcelPoint)) result[nameof(address.ParcelPoint).LowercaseFirstCharacter()].Should().Be(address.ParcelPoint);
        }

        [Fact]
        public void ParseQueryString_QueryString_Success()
        {
            // Arrange
            var query = new QueryString("?addressString=test&fakeprop=will not be included&extrapolate=true&maxresults=3&maxdistance=34.24");

            // Act
            var result = query.ParseQueryString<AddressesParameters>();

            // Assert
            result.Should().NotBeNull();
            result.AddressString.Should().Be("test");
            result.Extrapolate.Should().Be(true);
            result.MaxResults.Should().Be(3);
            result.MaxDistance.Should().Be(34.24);
        }

        [Fact]
        public void ParseQueryString_Dictionary_Success()
        {
            // Arrange
            var values = new Dictionary<string, StringValues>()
            {
                { "addressstring", "test"},
                { "extrapolate", "true"},
                { "maxResults", "3"},
                { "maxDistance", "34.24"},
            };

            // Act
            var result = values.ParseQueryString<AddressesParameters>();

            // Assert
            result.Should().NotBeNull();
            result.AddressString.Should().Be("test");
            result.Extrapolate.Should().Be(true);
            result.MaxResults.Should().Be(3);
            result.MaxDistance.Should().Be(34.24);
        }
        #endregion
    }
}
