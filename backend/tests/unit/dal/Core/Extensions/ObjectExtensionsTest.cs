using FluentAssertions;
using Pims.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Test.Core.Extensions
{
    [Trait("category", "unit")]
    [Trait("category", "core")]
    [Trait("category", "function")]
    [ExcludeFromCodeCoverage]
    public class ObjectExtensionsTest
    {
        #region Tests
        #region CopyValues
        [Fact]
        public void CopyValues_ArgumentNullException()
        {
            // Arrange
            var o1 = new Entity.Parcel(1, 1, 1);

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => o1.CopyValues((Entity.Parcel)null));
        }

        [Fact]
        public void CopyValues()
        {
            // Arrange
            var o1 = new Entity.Parcel(1, 1, 1) { Description = "test" };
            var o2 = new Entity.Parcel(2, 2, 2);

            // Act
            o1.CopyValues(o2);

            // Assert
            o2.PID.Should().Be(o1.PID);
            o2.Description.Should().Be(o1.Description);
            o2.Location.Should().NotBe(o1.Location);
        }
        #endregion
        #endregion
    }
}
