using System;
using System.Collections.Generic;
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
    public class AddressTest
    {
        #region Variables
        public static IEnumerable<object[]> Addresses =>
            new List<object[]>
            {
                new object[] { new PimsAddress("address1", "address2", "municipality", EntityHelper.CreateProvince(1, "BC"), EntityHelper.CreateDistrict(1, "district"), "postal"), "address1, address2, municipality, BC, district, Region 1, postal, CAN" },
                new object[] { new PimsAddress("address1", string.Empty, "municipality", EntityHelper.CreateProvince(1, "BC"), EntityHelper.CreateDistrict(1, "district"), "postal"), "address1, municipality, BC, district, Region 1, postal, CAN" },
                new object[] { new PimsAddress("address1", " ", "municipality", EntityHelper.CreateProvince(1, "BC"), EntityHelper.CreateDistrict(1, "district"), "postal"), "address1, municipality, BC, district, Region 1, postal, CAN" },
                new object[] { new PimsAddress("address1", null, "municipality", EntityHelper.CreateProvince(1, "BC"), EntityHelper.CreateDistrict(1, "district"), "postal"), "address1, municipality, BC, district, Region 1, postal, CAN" },
                new object[] { new PimsAddress("address1", null, "municipality", EntityHelper.CreateProvince(1, "BC"), EntityHelper.CreateDistrict(1, "district"), "postal"), "address1, municipality, BC, district, Region 1, postal, CAN" },
            };
        #endregion

        #region Tests
        [Fact]
        public void Address_Constructor_02()
        {
            // Arrange
            var province = EntityHelper.CreateProvince(1, "ON");
            var district = EntityHelper.CreateDistrict(1, "district");

            // Act
            var address = new PimsAddress("address1", "address2", "municipality", province, district, "postal");

            // Assert
            address.StreetAddress1.Should().Be("address1");
            address.StreetAddress2.Should().Be("address2");
            address.MunicipalityName.Should().Be("municipality");
            address.ProvinceState.Should().Be(province);
            address.ProvinceStateId.Should().Be(province.Id);
            address.DistrictCodeNavigation.Should().Be(district);
            address.DistrictCode.Should().Be(district.DistrictCode);
            address.PostalCode.Should().Be("postal");
        }

        [Fact]
        public void Address_Constructor_02_Address_Empty_ArgumentException()
        {
            // Arrange
            var province = EntityHelper.CreateProvince(1, "ON");

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => new PimsAddress(string.Empty, "address2", "municipality", province, null, "postal"));
        }

        [Fact]
        public void Address_Constructor_02_Address_Whitespace_ArgumentException()
        {
            // Arrange
            var province = EntityHelper.CreateProvince(1, "ON");

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => new PimsAddress(" ", "address2", "municipality", province, null, "postal"));
        }

        [Fact]
        public void Address_Constructor_02_Address_Null_ArgumentException()
        {
            // Arrange
            var province = EntityHelper.CreateProvince(1, "ON");

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => new PimsAddress(null, "address2", "municipality", province, null, "postal"));
        }

        [Fact]
        public void Address_Constructor_02_Municipality_ArgumentNullException()
        {
            // Arrange
            var province = EntityHelper.CreateProvince(1, "ON");

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new PimsAddress("address1", "address2", null, province, null, "postal"));
        }

        [Fact]
        public void Address_Constructor_02_Province_ArgumentNullException()
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new PimsAddress("address1", "address2", "municipality", (PimsProvinceState)null, null, "postal"));
        }

        [Fact]
        public void Address_Constructor_02_Country_ArgumentException()
        {
            // Arrange
            var province = EntityHelper.CreateProvince(1, "ON");
            province.Country = null;

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => new PimsAddress("address1", "address2", "municipality", province, null, "postal"));
        }

        [Theory]
        [MemberData(nameof(Addresses))]
        public void Address_ToString(PimsAddress address, string expectedResult)
        {
            // Arrange
            // Act
            // Assert
            address.ToString().Should().Be(expectedResult);
        }
        #endregion
    }
}
