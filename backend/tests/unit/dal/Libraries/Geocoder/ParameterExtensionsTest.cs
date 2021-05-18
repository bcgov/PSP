using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Pims.Geocoder.Extensions;
using Pims.Geocoder.Parameters;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
        public static IEnumerable<object[]> addresses =>
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
        [MemberData(nameof(addresses))]
        public void ToQueryStringDictionary_Success(AddressesParameters address)
        {
            // Arrange
            // Act
            var result = address.ToQueryStringDictionary();

            // Assert
            result.Should().NotBeNull();
            result[nameof(address.MaxResults)].Should().Be($"{address.MaxResults}");
            result[nameof(address.Echo)].Should().Be($"{address.Echo}".ToLower());
            result[nameof(address.Brief)].Should().Be($"{address.Brief}".ToLower());
            result[nameof(address.AutoComplete)].Should().Be($"{address.AutoComplete}".ToLower());
            result[nameof(address.SetBack)].Should().Be($"{address.SetBack}");
            result[nameof(address.OutputSRS)].Should().Be($"{address.OutputSRS}");
            result[nameof(address.MinScore)].Should().Be($"{address.MinScore}");
            result[nameof(address.MaxDistance)].Should().Be($"{address.MaxDistance}");
            result[nameof(address.Extrapolate)].Should().Be($"{address.Extrapolate}".ToLower());
            if (!String.IsNullOrWhiteSpace(address.Interpolation)) result[nameof(address.Interpolation)].Should().Be(address.Interpolation);
            if (!String.IsNullOrWhiteSpace(address.Ver)) result[nameof(address.Ver)].Should().Be(address.Ver);
            if (!String.IsNullOrWhiteSpace(address.AddressString)) result[nameof(address.AddressString)].Should().Be(address.AddressString);
            if (!String.IsNullOrWhiteSpace(address.LocationDescriptor)) result[nameof(address.LocationDescriptor)].Should().Be(address.LocationDescriptor);
            if (!String.IsNullOrWhiteSpace(address.MatchPrecision)) result[nameof(address.MatchPrecision)].Should().Be(address.MatchPrecision);
            if (!String.IsNullOrWhiteSpace(address.MatchPrecisionNot)) result[nameof(address.MatchPrecisionNot)].Should().Be(address.MatchPrecisionNot);
            if (!String.IsNullOrWhiteSpace(address.SiteName)) result[nameof(address.SiteName)].Should().Be(address.SiteName);
            if (!String.IsNullOrWhiteSpace(address.UnitDesignator)) result[nameof(address.UnitDesignator)].Should().Be(address.UnitDesignator);
            if (!String.IsNullOrWhiteSpace(address.UnitNumber)) result[nameof(address.UnitNumber)].Should().Be(address.UnitNumber);
            if (!String.IsNullOrWhiteSpace(address.UnitNumberSuffix)) result[nameof(address.UnitNumberSuffix)].Should().Be(address.UnitNumberSuffix);
            if (!String.IsNullOrWhiteSpace(address.CivicNumber)) result[nameof(address.CivicNumber)].Should().Be(address.CivicNumber);
            if (!String.IsNullOrWhiteSpace(address.CivicNumberSuffix)) result[nameof(address.CivicNumberSuffix)].Should().Be(address.CivicNumberSuffix);
            if (!String.IsNullOrWhiteSpace(address.StreetName)) result[nameof(address.StreetName)].Should().Be(address.StreetName);
            if (!String.IsNullOrWhiteSpace(address.StreetType)) result[nameof(address.StreetType)].Should().Be(address.StreetType);
            if (!String.IsNullOrWhiteSpace(address.StreetDirection)) result[nameof(address.StreetDirection)].Should().Be(address.StreetDirection);
            if (!String.IsNullOrWhiteSpace(address.StreetQualifier)) result[nameof(address.StreetQualifier)].Should().Be(address.StreetQualifier);
            if (!String.IsNullOrWhiteSpace(address.LocalityName)) result[nameof(address.LocalityName)].Should().Be(address.LocalityName);
            if (!String.IsNullOrWhiteSpace(address.ProvinceCode)) result[nameof(address.ProvinceCode)].Should().Be(address.ProvinceCode);
            if (!String.IsNullOrWhiteSpace(address.Bbox)) result[nameof(address.Bbox)].Should().Be(address.Bbox);
            if (!String.IsNullOrWhiteSpace(address.Localities)) result[nameof(address.Localities)].Should().Be(address.Localities);
            if (!String.IsNullOrWhiteSpace(address.NotLocalities)) result[nameof(address.NotLocalities)].Should().Be(address.NotLocalities);
            if (!String.IsNullOrWhiteSpace(address.Center)) result[nameof(address.Center)].Should().Be(address.Center);
            if (!String.IsNullOrWhiteSpace(address.ParcelPoint)) result[nameof(address.ParcelPoint)].Should().Be(address.ParcelPoint);
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
