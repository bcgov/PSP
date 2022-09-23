using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Pims.Core.Extensions;
using Pims.Core.Test;
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
            var o1 = EntityHelper.CreateProperty(1);

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => o1.CopyValues((Entity.PimsProperty)null));
        }

        [Fact]
        public void CopyValues()
        {
            // Arrange
            var o1 = EntityHelper.CreateProperty(1);
            o1.Description = "test";
            o1.Location = new NetTopologySuite.Geometries.Point(1, 1, 1);
            var o2 = EntityHelper.CreateProperty(2);

            // Act
            o1.CopyValues(o2);

            // Assert
            o2.Pid.Should().Be(o1.Pid);
            o2.Description.Should().Be(o1.Description);
            o2.Location.Should().Be(o1.Location);
        }
        #endregion
        #endregion
    }
}
