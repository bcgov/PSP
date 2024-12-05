using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Pims.Core.Test;
using Pims.Dal.Entities.Models;
using Pims.Dal.Repositories;
using Pims.Core.Security;
using Xunit;

namespace Pims.Dal.Test.Repositories
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("group", "property")]
    [ExcludeFromCodeCoverage]
    public class PropertyActivityRepositoryTest
    {
        private TestHelper _helper;

        public PropertyActivityRepositoryTest()
        {
            _helper = new TestHelper();
        }

        private PropertyActivityRepository CreateRepositoryWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            _helper.CreatePimsContext(user, true);
            return _helper.CreateRepository<PropertyActivityRepository>();
        }

        #region Tests


        [Fact]
        public void GetManagementActivityById_Throw_KeyNotFoundException()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.ManagementView);

            // Act
            Action act = () => repository.GetActivity(9999);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }

        [Fact]
        public void TryDeletePropertyActivity_Returns_False()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.ManagementView);

            // Act
            var updatedProperty = repository.TryDelete(9999);

            // Assert
            Assert.False(updatedProperty);
        }

        #endregion
    }
}
