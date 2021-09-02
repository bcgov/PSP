using FluentAssertions;
using Pims.Core.Test;
using Pims.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
                new object[] { new Address("address1", "address2", "municipality", EntityHelper.CreateProvince(1, "BC"), EntityHelper.CreateDistrict(1, "district"), "postal"), "address1, address2, municipality, BC, district, Region 1, postal, CAN" },
                new object[] { new Address("address1", "", "municipality", EntityHelper.CreateProvince(1, "BC"), EntityHelper.CreateDistrict(1, "district"), "postal"), "address1, municipality, BC, district, Region 1, postal, CAN" },
                new object[] { new Address("address1", " ", "municipality", EntityHelper.CreateProvince(1, "BC"), EntityHelper.CreateDistrict(1, "district"), "postal"), "address1, municipality, BC, district, Region 1, postal, CAN" },
                new object[] { new Address("address1", null, "municipality", EntityHelper.CreateProvince(1, "BC"), EntityHelper.CreateDistrict(1, "district"), "postal"), "address1, municipality, BC, district, Region 1, postal, CAN" },
                new object[] { new Address("address1", null, "municipality", EntityHelper.CreateProvince(1, "BC"), EntityHelper.CreateDistrict(1, "district"), "postal"), "address1, municipality, BC, district, Region 1, postal, CAN" }
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
            var address = new Address("address1", "address2", "municipality", province, district, "postal");

            // Assert
            address.StreetAddress1.Should().Be("address1");
            address.StreetAddress2.Should().Be("address2");
            address.Municipality.Should().Be("municipality");
            address.Province.Should().Be(province);
            address.ProvinceId.Should().Be(province.Id);
            address.District.Should().Be(district);
            address.DistrictId.Should().Be(district.Id);
            address.Postal.Should().Be("postal");
        }

        [Fact]
        public void Address_Constructor_02_Address_Empty_ArgumentException()
        {
            // Arrange
            var province = EntityHelper.CreateProvince(1, "ON");

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => new Address("", "address2", "municipality", province, null, "postal"));
        }

        [Fact]
        public void Address_Constructor_02_Address_Whitespace_ArgumentException()
        {
            // Arrange
            var province = EntityHelper.CreateProvince(1, "ON");

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => new Address(" ", "address2", "municipality", province, null, "postal"));
        }

        [Fact]
        public void Address_Constructor_02_Address_Null_ArgumentException()
        {
            // Arrange
            var province = EntityHelper.CreateProvince(1, "ON");

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => new Address(null, "address2", "municipality", province, null, "postal"));
        }

        [Fact]
        public void Address_Constructor_02_Municipality_ArgumentNullException()
        {
            // Arrange
            var province = EntityHelper.CreateProvince(1, "ON");

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new Address("address1", "address2", null, province, null, "postal"));
        }

        [Fact]
        public void Address_Constructor_02_Province_ArgumentNullException()
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new Address("address1", "address2", "municipality", (Province)null, null, "postal"));
        }

        [Fact]
        public void Address_Constructor_02_Country_ArgumentException()
        {
            // Arrange
            var province = EntityHelper.CreateProvince(1, "ON");
            province.Country = null;

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => new Address("address1", "address2", "municipality", province, null, "postal"));
        }

        [Theory]
        [MemberData(nameof(Addresses))]
        public void Address_ToString(Address address, string expectedResult)
        {
            // Arrange
            // Act
            // Assert
            address.ToString().Should().Be(expectedResult);
        }
        #endregion
    }
}
