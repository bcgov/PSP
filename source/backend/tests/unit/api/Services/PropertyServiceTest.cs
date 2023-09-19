using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Moq;
using NetTopologySuite.Geometries;
using Pims.Api.Services;
using Pims.Core.Extensions;
using Pims.Core.Test;
using Pims.Dal;
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
        private TestHelper _helper;

        public PropertyServiceTest()
        {
            this._helper = new TestHelper();
        }

        private PropertyService CreateProjectServiceWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            this._helper.CreatePimsContext(user, true);
            return this._helper.Create<PropertyService>();
        }

        #region Tests
        #region GetById
        [Fact]
        public void GetById_Success()
        {
            // Arrange
            var property = EntityHelper.CreateProperty(1);

            var service = this.CreateProjectServiceWithPermissions(Permissions.PropertyView);
            var repository = this._helper.GetService<Mock<IPropertyRepository>>();
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(property);

            var coordinateService = this._helper.GetService<Mock<ICoordinateTransformService>>();
            coordinateService.Setup(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>()));

            // Act
            var result = service.GetById(1);

            // Assert
            repository.Verify(x => x.GetById(It.IsAny<long>()), Times.Once);
            coordinateService.Verify(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>()), Times.Once);
        }

        [Fact]
        public void GetById_NoPermission()
        {
            // Arrange
            var property = EntityHelper.CreateProperty(1);

            var service = this.CreateProjectServiceWithPermissions();

            // Assert
            Assert.Throws<NotAuthorizedException>(() => service.GetById(1));
        }

        #endregion
        #region GetByPid
        [Fact]
        public void GetByPid_Success()
        {
            // Arrange
            var property = EntityHelper.CreateProperty(1);

            var service = this.CreateProjectServiceWithPermissions(Permissions.PropertyView);
            var repository = this._helper.GetService<Mock<IPropertyRepository>>();
            repository.Setup(x => x.GetByPid(It.IsAny<string>())).Returns(property);

            var coordinateService = this._helper.GetService<Mock<ICoordinateTransformService>>();
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
            var property = EntityHelper.CreateProperty(1);

            var service = this.CreateProjectServiceWithPermissions();

            // Assert
            Assert.Throws<NotAuthorizedException>(() => service.GetByPid(1.ToString()));
        }

        #endregion
        #region Update
        [Fact]
        public void Update_Property_No_Reprojection_Success()
        {
            // Arrange
            var property = EntityHelper.CreateProperty(1);

            var service = this.CreateProjectServiceWithPermissions(Permissions.PropertyView, Permissions.PropertyEdit);
            var repository = this._helper.GetService<Mock<IPropertyRepository>>();
            repository.Setup(x => x.Update(It.IsAny<PimsProperty>(), It.IsAny<bool>())).Returns(property);

            var coordinateService = this._helper.GetService<Mock<ICoordinateTransformService>>();
            coordinateService.Setup(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>()));

            var newValues = new PimsProperty();
            property.CopyValues(newValues);
            newValues.Description = "test";
            newValues.Pid = 200;
            newValues.Location = GeometryHelper.CreatePoint(0, 0, SpatialReference.BCALBERS);

            // Act
            var updatedProperty = service.Update(newValues);

            // Assert
            repository.Verify(x => x.Update(It.IsAny<PimsProperty>(), It.IsAny<bool>()), Times.Once);
            coordinateService.Verify(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>()), Times.Never);
        }

        [Fact]
        public void Update_Property_With_Reprojection_Success()
        {
            // Arrange
            var property = EntityHelper.CreateProperty(1);

            var service = this.CreateProjectServiceWithPermissions(Permissions.PropertyView, Permissions.PropertyEdit);
            var repository = this._helper.GetService<Mock<IPropertyRepository>>();
            repository.Setup(x => x.Update(It.IsAny<PimsProperty>(), It.IsAny<bool>())).Returns(property);

            var projected = new Coordinate(14000, 9200);
            var coordinateService = this._helper.GetService<Mock<ICoordinateTransformService>>();
            coordinateService.Setup(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>())).Returns(projected);

            var newValues = new PimsProperty();
            property.CopyValues(newValues);
            newValues.Description = "test";
            newValues.Pid = 200;
            newValues.Location = GeometryHelper.CreatePoint(-119, 53, SpatialReference.WGS84);

            // Act
            var updatedProperty = service.Update(newValues);

            // Assert
            coordinateService.Verify(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>()), Times.Once);
            repository.Verify(x => x.Update(It.Is<PimsProperty>(p => p.Location.Coordinate.Equals(projected)), It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public void Update_Property_KeyNotFound()
        {
            // Arrange
            var service = this.CreateProjectServiceWithPermissions(Permissions.PropertyView, Permissions.PropertyEdit);

            // Try to update a non-existent property
            var property = EntityHelper.CreateProperty(1);

            var repository = this._helper.GetService<Mock<IPropertyRepository>>();
            repository.Setup(x => x.Update(property, It.IsAny<bool>())).Throws<KeyNotFoundException>();

            // Assert
            Assert.Throws<KeyNotFoundException>(() => service.Update(property));
        }

        [Fact]
        public void Update_Property_NoPermission()
        {
            // Arrange
            var property = EntityHelper.CreateProperty(1);

            var service = this.CreateProjectServiceWithPermissions(Permissions.PropertyView);
            var repository = this._helper.GetService<Mock<IPropertyRepository>>();

            // Assert
            Assert.Throws<NotAuthorizedException>(() => service.Update(property));
            repository.Verify(x => x.Update(It.IsAny<PimsProperty>(), It.IsAny<bool>()), Times.Never);
        }

        #endregion
        #endregion
    }
}
