using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Moq;
using NetTopologySuite.Geometries;
using Pims.Api.Services;
using Pims.Core.Extensions;
using Pims.Core.Test;
using Pims.Dal.Constants;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;
using Pims.Dal.Helpers;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Pims.Dal.Services;
using Xunit;

namespace Pims.Api.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
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

            var service = helper.Create<PropertyService>(user);
            var repository = helper.GetService<Mock<IPropertyRepository>>();
            repository.Setup(x => x.Get(It.IsAny<long>())).Returns(property);

            var coordinateService = helper.GetService<Mock<ICoordinateTransformService>>();
            coordinateService.Setup(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>()));

            // Act
            var result = service.GetById(1);

            // Assert
            repository.Verify(x => x.Get(It.IsAny<long>()), Times.Once);
            coordinateService.Verify(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>()), Times.Once);
        }

        [Fact]
        public void GetById_NoPermission()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var property = EntityHelper.CreateProperty(1);

            var service = helper.Create<PropertyService>(user);
            var repository = helper.GetService<Mock<IPropertyRepository>>();
            repository.Setup(x => x.Get(It.IsAny<long>())).Returns(property);

            var coordinateService = helper.GetService<Mock<ICoordinateTransformService>>();
            coordinateService.Setup(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>()));

            // Assert
            Assert.Throws<NotAuthorizedException>(() => service.GetById(1));
            repository.Verify(x => x.Get(It.IsAny<long>()), Times.Never);
            coordinateService.Verify(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>()), Times.Never);
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

            var service = helper.Create<PropertyService>(user);
            var repository = helper.GetService<Mock<IPropertyRepository>>();
            repository.Setup(x => x.GetByPid(It.IsAny<string>())).Returns(property);

            var coordinateService = helper.GetService<Mock<ICoordinateTransformService>>();
            coordinateService.Setup(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>()));

            // Act
            var result = service.GetByPid(1.ToString());

            // Assert
            repository.Verify(x => x.GetByPid(It.IsAny<string>()), Times.Once);
            coordinateService.Verify(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>()), Times.Once);
        }

        [Fact]
        public void GetByPid_NoPermission()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var property = EntityHelper.CreateProperty(1);

            var service = helper.Create<PropertyService>(user);
            var repository = helper.GetService<Mock<IPropertyRepository>>();
            repository.Setup(x => x.GetByPid(It.IsAny<string>())).Returns(property);

            var coordinateService = helper.GetService<Mock<ICoordinateTransformService>>();
            coordinateService.Setup(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>()));

            // Assert
            Assert.Throws<NotAuthorizedException>(() => service.GetByPid(1.ToString()));
            repository.Verify(x => x.GetByPid(It.IsAny<string>()), Times.Never);
            coordinateService.Verify(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>()), Times.Never);
        }

        #endregion
        #region Update
        [Fact]
        public void Update_Property_No_Reprojection_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.PropertyView, Permissions.PropertyEdit);

            var property = EntityHelper.CreateProperty(1);

            var service = helper.Create<PropertyService>(user);
            var repository = helper.GetService<Mock<IPropertyRepository>>();
            repository.Setup(x => x.Update(It.IsAny<PimsProperty>())).Returns(property);

            var coordinateService = helper.GetService<Mock<ICoordinateTransformService>>();
            coordinateService.Setup(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>()));

            var newValues = new PimsProperty();
            property.CopyValues(newValues);
            newValues.Description = "test";
            newValues.Pid = 200;
            newValues.Location = GeometryHelper.CreatePoint(0, 0, SpatialReference.BC_ALBERS);

            // Act
            var updatedProperty = service.Update(newValues);

            // Assert
            repository.Verify(x => x.Update(It.IsAny<PimsProperty>()), Times.Once);
            coordinateService.Verify(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>()), Times.Never);
        }

        [Fact]
        public void Update_Property_With_Reprojection_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.PropertyView, Permissions.PropertyEdit);

            var property = EntityHelper.CreateProperty(1);

            var service = helper.Create<PropertyService>(user);
            var repository = helper.GetService<Mock<IPropertyRepository>>();
            repository.Setup(x => x.Update(It.IsAny<PimsProperty>())).Returns(property);

            var projected = new Coordinate(14000, 9200);
            var coordinateService = helper.GetService<Mock<ICoordinateTransformService>>();
            coordinateService.Setup(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>())).Returns(projected);

            var newValues = new PimsProperty();
            property.CopyValues(newValues);
            newValues.Description = "test";
            newValues.Pid = 200;
            newValues.Location = GeometryHelper.CreatePoint(-119, 53, SpatialReference.WGS_84);

            // Act
            var updatedProperty = service.Update(newValues);

            // Assert
            coordinateService.Verify(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>()), Times.Once);
            repository.Verify(x => x.Update(It.Is<PimsProperty>(p => p.Location.Coordinate.Equals(projected))), Times.Once);
        }

        [Fact]
        public void Update_Property_KeyNotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.PropertyView, Permissions.PropertyEdit);
            var service = helper.Create<PropertyService>(user);

            // Try to update a non-existent property
            var property = EntityHelper.CreateProperty(1);

            var repository = helper.GetService<Mock<IPropertyRepository>>();
            repository.Setup(x => x.Update(property)).Throws<KeyNotFoundException>();

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

            var service = helper.Create<PropertyService>();
            var repository = helper.GetService<Mock<IPropertyRepository>>();
            repository.Setup(x => x.Update(It.IsAny<PimsProperty>())).Returns(property);

            // Assert
            Assert.Throws<NotAuthorizedException>(() => service.Update(property));
            repository.Verify(x => x.Update(It.IsAny<PimsProperty>()), Times.Never);
        }

        #endregion
        #endregion
    }
}
