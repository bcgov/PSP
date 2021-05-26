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
    public class ParcelParcelTest
    {
        #region Variables
        public static IEnumerable<object[]> Constructor_01 =>
            new List<object[]>
            {
                new object[] { new Parcel(123, 0, 0), null },
                new object[] { null, new Parcel(123, 0, 0) },
            };
        #endregion

        #region Tests
        [Fact]
        public void ParcelParcel_Constructor_01()
        {
            // Arrange
            var parcel = EntityHelper.CreateParcel(123);
            var subParcel = EntityHelper.CreateParcel(321);

            // Act
            var parcelParcel = new ParcelParcel(parcel, subParcel);

            // Assert
            parcelParcel.ParcelId.Should().Be(parcel.Id);
            parcelParcel.Parcel.Should().Be(parcel);
            parcelParcel.SubdivisionId.Should().Be(subParcel.Id);
            parcelParcel.Subdivision.Should().Be(subParcel);
        }

        [Theory]
        [MemberData(nameof(Constructor_01))]
        public void ParcelParcel_Constructor_01_ArgumentNullException(Parcel parcel, Parcel subdivision)
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new ParcelParcel(parcel, subdivision));
        }
        #endregion
    }
}
