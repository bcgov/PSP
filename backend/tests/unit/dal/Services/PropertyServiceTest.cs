using FluentAssertions;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Moq;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;
using Pims.Dal.Security;
using Pims.Dal.Services;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Pims.Core.Extensions;

namespace Pims.Dal.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("group", "property")]
    [ExcludeFromCodeCoverage]
    public class PropertyServiceTest
    {
        #region Tests
        #region GetById
        [Fact]
        public void GetById_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.PropertyView);

            var property = EntityHelper.CreateProperty(1);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(property);

            var service = helper.Create<PropertyService>();
            var repository = helper.GetService<Mock<Repositories.IPropertyRepository>>();
            repository.Setup(x => x.Get(It.IsAny<long>()));

            // Act
            var result = service.GetById(1);

            // Assert
            repository.Verify(x => x.Get(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void GetById_NoPermission()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var property = EntityHelper.CreateProperty(1);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(property);

            var service = helper.Create<PropertyService>();
            var repository = helper.GetService<Mock<Repositories.IPropertyRepository>>();
            repository.Setup(x => x.Get(It.IsAny<long>()));

            // Assert
            Assert.Throws<NotAuthorizedException>(() => service.GetById(1));
            repository.Verify(x => x.Get(It.IsAny<long>()), Times.Never);
        }

        #endregion
        #region GetByPid
        [Fact]
        public void GetByPid_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.PropertyView);

            var property = EntityHelper.CreateProperty(1);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(property);

            var service = helper.Create<PropertyService>();
            var repository = helper.GetService<Mock<Repositories.IPropertyRepository>>();
            repository.Setup(x => x.GetByPid(It.IsAny<string>()));

            // Act
            var result = service.GetByPid(1.ToString());

            // Assert
            repository.Verify(x => x.GetByPid(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void GetByPid_NoPermission()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var property = EntityHelper.CreateProperty(1);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(property);

            var service = helper.Create<PropertyService>();
            var repository = helper.GetService<Mock<Repositories.IPropertyRepository>>();
            repository.Setup(x => x.GetByPid(It.IsAny<string>()));

            // Assert
            Assert.Throws<NotAuthorizedException>(() => service.GetByPid(1.ToString()));
            repository.Verify(x => x.GetByPid(It.IsAny<string>()), Times.Never);
        }

        #endregion
        #region Update
        [Fact]
        public void Update_Property_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.PropertyView, Permissions.PropertyEdit);

            var property = EntityHelper.CreateProperty(1);
            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(property);

            helper.Add<Repositories.IPropertyRepository, Repositories.PropertyRepository>(user);
            var service = helper.Create<PropertyService>();

            var newValues = new PimsProperty();
            property.CopyValues(newValues);
            newValues.Description = "test";
            newValues.Pid = 200;

            // Act
            var updatedProperty = service.Update(newValues);

            // Assert
            updatedProperty.Description.Should().Be("test");
            updatedProperty.Pid.Should().Be(200);
        }

        [Fact]
        public void Update_Property_KeyNotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.PropertyView, Permissions.PropertyEdit);
            helper.CreatePimsContext(user, true);
            helper.Add<Repositories.IPropertyRepository, Repositories.PropertyRepository>(user);
            var service = helper.Create<PropertyService>();

            // Try to update a non-existent property
            var property = EntityHelper.CreateProperty(1);

            // Assert
            Assert.Throws<KeyNotFoundException>(() => service.Update(property));
        }

        [Fact]
        public void Update_Property_NoPermission()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.PropertyView);

            var property = EntityHelper.CreateProperty(1);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(property);

            var service = helper.Create<PropertyService>();
            var repository = helper.GetService<Mock<Repositories.IPropertyRepository>>();
            repository.Setup(x => x.Update(It.IsAny<PimsProperty>()));

            // Assert
            Assert.Throws<NotAuthorizedException>(() => service.Update(property));
            repository.Verify(x => x.Update(It.IsAny<PimsProperty>()), Times.Never);
        }

        #endregion
        #endregion
    }
}
