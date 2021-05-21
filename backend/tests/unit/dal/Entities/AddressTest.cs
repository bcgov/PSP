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
                new object[] { new Address("address1", "address2", "administrativeArea", "provinceId", "postal"), "address1, address2, administrativeArea, provinceId, postal" },
                new object[] { new Address("address1", "", "administrativeArea", "provinceId", "postal"), "address1, administrativeArea, provinceId, postal" },
                new object[] { new Address("address1", " ", "administrativeArea", "provinceId", "postal"), "address1, administrativeArea, provinceId, postal" },
                new object[] { new Address("address1", null, "administrativeArea", "provinceId", "postal"), "address1, administrativeArea, provinceId, postal" }
            };
        #endregion

        #region Tests
        [Fact]
        public void Address_Constructor_01()
        {
            // Arrange
            // Act
            var address = new Address("address1", "address2", "administrativeArea", "provinceId", "postal");

            // Assert
            address.Address1.Should().Be("address1");
            address.Address2.Should().Be("address2");
            address.AdministrativeArea.Should().Be("administrativeArea");
            address.ProvinceId.Should().Be("provinceId");
            address.Postal.Should().Be("postal");
        }

        [Fact]
        public void Address_Constructor_01_ArgumentNullException()
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new Address("address1", "address2", null, "provinceId", "postal"));
        }

        [Fact]
        public void Address_Constructor_02()
        {
            // Arrange
            var province = EntityHelper.CreateProvince("ON", "Ontario");

            // Act
            var address = new Address("address1", "address2", "administrativeArea", province, "postal");

            // Assert
            address.Address1.Should().Be("address1");
            address.Address2.Should().Be("address2");
            address.AdministrativeArea.Should().Be("administrativeArea");
            address.Province.Should().Be(province);
            address.ProvinceId.Should().Be(province.Id);
            address.Postal.Should().Be("postal");
        }

        [Fact]
        public void Address_Constructor_02_AdministrativeArea_ArgumentNullException()
        {
            // Arrange
            var province = EntityHelper.CreateProvince("ON", "Ontario");

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new Address("address1", "address2", null, province, "postal"));
        }

        [Fact]
        public void Address_Constructor_02_Province_ArgumentNullException()
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new Address("address1", "address2", "administrativeArea", (Province)null, "postal"));
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
