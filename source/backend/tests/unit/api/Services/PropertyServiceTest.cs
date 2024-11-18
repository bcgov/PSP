using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Moq;
using NetTopologySuite.Geometries;
using Pims.Core.Api.Exceptions;
using Pims.Api.Models.CodeTypes;
using Pims.Api.Services;
using Pims.Core.Exceptions;
using Pims.Core.Extensions;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;
using Pims.Dal.Helpers;
using Pims.Dal.Repositories;
using Pims.Core.Security;
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

        private PropertyService CreatePropertyServiceWithPermissions(params Permissions[] permissions)
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

            var service = this.CreatePropertyServiceWithPermissions(Permissions.PropertyView);
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

            var service = this.CreatePropertyServiceWithPermissions();

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

            var service = this.CreatePropertyServiceWithPermissions(Permissions.PropertyView);
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

            var service = this.CreatePropertyServiceWithPermissions();

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

            var service = this.CreatePropertyServiceWithPermissions(Permissions.PropertyView, Permissions.PropertyEdit);
            var repository = this._helper.GetService<Mock<IPropertyRepository>>();
            repository.Setup(x => x.Update(It.IsAny<PimsProperty>(), It.IsAny<bool>())).Returns(property);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(property);
            var coordinateService = this._helper.GetService<Mock<ICoordinateTransformService>>();
            coordinateService.Setup(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>()));

            var newValues = new PimsProperty();
            property.CopyValues(newValues);
            newValues.Pid = 200;
            newValues.Location = GeometryHelper.CreatePoint(0, 0, SpatialReference.BCALBERS);

            // Act
            var updatedProperty = service.Update(newValues);

            // Assert
            repository.Verify(x => x.Update(It.IsAny<PimsProperty>(), It.IsAny<bool>()), Times.Once);
            coordinateService.Verify(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>()));
        }

        [Fact]
        public void Update_Property_With_Reprojection_Success()
        {
            // Arrange
            var property = EntityHelper.CreateProperty(1);

            var service = this.CreatePropertyServiceWithPermissions(Permissions.PropertyView, Permissions.PropertyEdit);
            var repository = this._helper.GetService<Mock<IPropertyRepository>>();
            repository.Setup(x => x.Update(It.IsAny<PimsProperty>(), It.IsAny<bool>())).Returns(property);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(property);

            var projected = new Coordinate(14000, 9200);
            var coordinateService = this._helper.GetService<Mock<ICoordinateTransformService>>();
            coordinateService.Setup(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>())).Returns(projected);

            var newValues = new PimsProperty();
            property.CopyValues(newValues);
            newValues.Pid = 200;
            newValues.Location = GeometryHelper.CreatePoint(-119, 53, SpatialReference.WGS84);

            // Act
            var updatedProperty = service.Update(newValues);

            // Assert
            coordinateService.Verify(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>()));
            repository.Verify(x => x.Update(It.Is<PimsProperty>(p => p.Location.Coordinate.Equals(projected)), It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public void Update_Property_KeyNotFound()
        {
            // Arrange
            var service = this.CreatePropertyServiceWithPermissions(Permissions.PropertyView, Permissions.PropertyEdit);

            // Try to update a non-existent property
            var property = EntityHelper.CreateProperty(1);

            var repository = this._helper.GetService<Mock<IPropertyRepository>>();
            repository.Setup(x => x.Update(property, It.IsAny<bool>())).Throws<KeyNotFoundException>();
            repository.Setup(x => x.GetById(It.IsAny<long>())).Throws<KeyNotFoundException>();

            // Assert
            Assert.Throws<KeyNotFoundException>(() => service.Update(property));
        }

        [Fact]
        public void Update_Property_NoPermission()
        {
            // Arrange
            var property = EntityHelper.CreateProperty(1);

            var service = this.CreatePropertyServiceWithPermissions(Permissions.PropertyView);
            var repository = this._helper.GetService<Mock<IPropertyRepository>>();

            // Assert
            Assert.Throws<NotAuthorizedException>(() => service.Update(property));
            repository.Verify(x => x.Update(It.IsAny<PimsProperty>(), It.IsAny<bool>()), Times.Never);
        }

        #endregion

        #region UpdateLocation
        [Fact]
        public void UpdateLocation_Requires_UserOverride()
        {
            // Arrange
            var property = EntityHelper.CreateProperty(200);
            property.Location = null;

            var service = this.CreatePropertyServiceWithPermissions(Permissions.PropertyView, Permissions.PropertyEdit);
            var repository = this._helper.GetService<Mock<IPropertyRepository>>();
            repository.Setup(x => x.Update(It.IsAny<PimsProperty>(), It.IsAny<bool>())).Returns(property);

            var coordinateService = this._helper.GetService<Mock<ICoordinateTransformService>>();
            coordinateService.Setup(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>())).Returns(new Coordinate(14000, 9200));

            var incomingProperty = new PimsProperty();
            incomingProperty.Pid = 200;
            incomingProperty.Location = EntityHelper.CreatePoint(-119, 53, SpatialReference.WGS84);

            // Act
            Action act = () => service.UpdateLocation(incomingProperty, ref property, new List<UserOverrideCode>());

            // Assert
            var ex = act.Should().Throw<UserOverrideException>();
            ex.Which.UserOverride.Should().Be(UserOverrideCode.AddLocationToProperty);

            coordinateService.Verify(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>()), Times.Never);
            repository.Verify(x => x.Update(It.IsAny<PimsProperty>(), It.IsAny<bool>()), Times.Never);
        }

        [Fact]
        public void UpdateLocation_Success()
        {
            // Arrange
            var property = EntityHelper.CreateProperty(200);
            property.Location = null;

            var service = this.CreatePropertyServiceWithPermissions(Permissions.PropertyView, Permissions.PropertyEdit);
            var repository = this._helper.GetService<Mock<IPropertyRepository>>();
            repository.Setup(x => x.Update(It.IsAny<PimsProperty>(), It.IsAny<bool>())).Returns(property);

            var coordinateService = this._helper.GetService<Mock<ICoordinateTransformService>>();
            coordinateService.Setup(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>()))
                .Returns(new Coordinate(14000, 9200));

            var incomingProperty = new PimsProperty();
            incomingProperty.Pid = 200;
            incomingProperty.Location = EntityHelper.CreatePoint(-119, 53, SpatialReference.WGS84);

            // Act
            service.UpdateLocation(incomingProperty, ref property, new List<UserOverrideCode>() { UserOverrideCode.AddLocationToProperty });

            // Assert
            coordinateService.Verify(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>()), Times.Once);
            repository.Verify(x => x.Update(It.Is<PimsProperty>(p => p.Location.Coordinate.Equals(new Coordinate(14000, 9200))), It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public void UpdateLocation_Boundary_Success()
        {
            // Arrange
            var property = EntityHelper.CreateProperty(200);
            property.Location = null;
            property.Boundary = null;

            var service = this.CreatePropertyServiceWithPermissions(Permissions.PropertyView, Permissions.PropertyEdit);
            var repository = this._helper.GetService<Mock<IPropertyRepository>>();
            repository.Setup(x => x.Update(It.IsAny<PimsProperty>(), It.IsAny<bool>())).Returns(property);

            var coordinateService = this._helper.GetService<Mock<ICoordinateTransformService>>();
            coordinateService.Setup(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>()))
                .Returns(new Coordinate(14000, 9200));
            coordinateService.Setup(x => x.TransformGeometry(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Geometry>()))
                .Callback<int, int, Geometry>((sourceSrid, targetSrid, boundary) =>
                {
                    // "apply" mock transformation
                    var polygon = boundary as Polygon;
                    polygon.SRID = targetSrid;
                    polygon.ExteriorRing.CoordinateSequence.SetX(0, 1000);
                    polygon.ExteriorRing.CoordinateSequence.SetY(0, 1000);
                });

            var incomingProperty = new PimsProperty();
            incomingProperty.Pid = 200;
            incomingProperty.Location = EntityHelper.CreatePoint(-119, 53, SpatialReference.WGS84);
            incomingProperty.Boundary = EntityHelper.CreatePolygon(SpatialReference.WGS84);

            // Act
            service.UpdateLocation(incomingProperty, ref property, new List<UserOverrideCode>() { UserOverrideCode.AddLocationToProperty });

            // Assert
            coordinateService.Verify(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>()), Times.Once);
            coordinateService.Verify(x => x.TransformGeometry(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Geometry>()), Times.Once);
            repository.Verify(x => x.Update(property, true), Times.Once);
            property.Location.Coordinate.Should().Be(new Coordinate(14000, 9200));
            property.Boundary.Should().BeOfType<Polygon>();
            var updatedBoundary = property.Boundary as Polygon;
            updatedBoundary.ExteriorRing.GetCoordinateN(0).Should().Be(new Coordinate(1000, 1000));
        }

        #endregion

        #region PopulateNewProperty
        [Fact]
        public void PopulateNewProperty_Success()
        {
            // Arrange
            var property = EntityHelper.CreateProperty(200);
            property.Location = EntityHelper.CreatePoint(-119, 53, SpatialReference.WGS84);
            property.Boundary = null;

            var service = this.CreatePropertyServiceWithPermissions(Permissions.PropertyView, Permissions.PropertyEdit);

            var lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();
            lookupRepository.Setup(x => x.GetAllCountries()).Returns(new[] { EntityHelper.CreateCountry(1, "CA") });
            lookupRepository.Setup(x => x.GetAllProvinces()).Returns(new[] { EntityHelper.CreateProvince(1, "BC") });

            var coordinateService = this._helper.GetService<Mock<ICoordinateTransformService>>();
            coordinateService.Setup(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>()))
                .Returns(new Coordinate(14000, 9200));

            // Act
            service.PopulateNewProperty(property);

            // Assert
            coordinateService.Verify(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>()), Times.Once);
            coordinateService.Verify(x => x.TransformGeometry(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Geometry>()), Times.Never);

            property.PropertyTypeCode.Should().Be("UNKNOWN");
            property.PropertyStatusTypeCode.Should().Be("UNKNOWN");
            property.SurplusDeclarationTypeCode.Should().Be("UNKNOWN");
            property.Location.Coordinate.Should().Be(new Coordinate(14000, 9200));
            property.Location.SRID.Should().Be(SpatialReference.BCALBERS); // Spatial reference should be in BC ALBERS for DB storage.
        }

        [Fact]
        public void PopulateNewProperty_Boundary_Success()
        {
            // Arrange
            var property = EntityHelper.CreateProperty(200);
            property.Location = EntityHelper.CreatePoint(-119, 53, SpatialReference.WGS84);
            property.Boundary = EntityHelper.CreatePolygon(SpatialReference.WGS84);

            var service = this.CreatePropertyServiceWithPermissions(Permissions.PropertyView, Permissions.PropertyEdit);

            var lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();
            lookupRepository.Setup(x => x.GetAllCountries()).Returns(new[] { EntityHelper.CreateCountry(1, "CA") });
            lookupRepository.Setup(x => x.GetAllProvinces()).Returns(new[] { EntityHelper.CreateProvince(1, "BC") });

            var coordinateService = this._helper.GetService<Mock<ICoordinateTransformService>>();
            coordinateService.Setup(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>()))
                .Returns(new Coordinate(14000, 9200));
            coordinateService.Setup(x => x.TransformGeometry(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Geometry>()))
                .Callback<int, int, Geometry>((sourceSrid, targetSrid, boundary) =>
                {
                    // "apply" mock transformation
                    var polygon = boundary as Polygon;
                    polygon.SRID = targetSrid;
                    polygon.ExteriorRing.CoordinateSequence.SetX(0, 1000);
                    polygon.ExteriorRing.CoordinateSequence.SetY(0, 1000);
                });

            var incomingProperty = new PimsProperty();
            incomingProperty.Pid = 200;
            incomingProperty.Location = EntityHelper.CreatePoint(-119, 53, SpatialReference.WGS84);
            incomingProperty.Boundary = EntityHelper.CreatePolygon(SpatialReference.WGS84);

            // Act
            service.PopulateNewProperty(property);

            // Assert
            coordinateService.Verify(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>()), Times.Once);
            coordinateService.Verify(x => x.TransformGeometry(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Geometry>()), Times.Once);

            property.PropertyTypeCode.Should().Be("UNKNOWN");
            property.PropertyStatusTypeCode.Should().Be("UNKNOWN");
            property.SurplusDeclarationTypeCode.Should().Be("UNKNOWN");
            property.Location.Coordinate.Should().Be(new Coordinate(14000, 9200));
            property.Location.SRID.Should().Be(SpatialReference.BCALBERS); // Spatial reference should be in BC ALBERS for DB storage.
            property.Boundary.Should().BeOfType<Polygon>();

            var updatedBoundary = property.Boundary as Polygon;
            updatedBoundary.ExteriorRing.GetCoordinateN(0).Should().Be(new Coordinate(1000, 1000));
            updatedBoundary.SRID.Should().Be(SpatialReference.BCALBERS); // Spatial reference should be in BC ALBERS for DB storage.
        }

        #endregion

        #region Property Management
        [Fact]
        public void GetPropertyManagement_Success()
        {
            // Arrange
            var property = EntityHelper.CreateProperty(1);

            var service = this.CreatePropertyServiceWithPermissions(Permissions.PropertyView, Permissions.ManagementView);
            var repository = this._helper.GetService<Mock<IPropertyRepository>>();
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(property);

            var coordinateService = this._helper.GetService<Mock<ICoordinateTransformService>>();
            coordinateService.Setup(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>()));

            var propertyLeasesRepository = this._helper.GetService<Mock<IPropertyLeaseRepository>>();
            propertyLeasesRepository.Setup(x => x.GetAllByPropertyId(It.IsAny<long>())).Returns(new List<PimsPropertyLease>());

            // Act
            var result = service.GetPropertyManagement(1);

            // Assert
            repository.Verify(x => x.GetById(It.IsAny<long>()), Times.Once);
            coordinateService.Verify(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>()), Times.Once);
            propertyLeasesRepository.Verify(x => x.GetAllByPropertyId(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void GetPropertyManagement_NoPermission()
        {
            // Arrange
            var property = EntityHelper.CreateProperty(1);
            var service = this.CreatePropertyServiceWithPermissions();

            // Act
            Action act = () => service.GetPropertyManagement(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void GetPropertyManagement_HasNoActiveLease_Success()
        {
            // Arrange
            var property = EntityHelper.CreateProperty(1);

            var service = this.CreatePropertyServiceWithPermissions(Permissions.PropertyView, Permissions.ManagementView);
            var repository = this._helper.GetService<Mock<IPropertyRepository>>();
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(property);

            var coordinateService = this._helper.GetService<Mock<ICoordinateTransformService>>();
            coordinateService.Setup(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>()));

            var propertyLeasesRepository = this._helper.GetService<Mock<IPropertyLeaseRepository>>();
            propertyLeasesRepository.Setup(x => x.GetAllByPropertyId(It.IsAny<long>())).Returns(new List<PimsPropertyLease>()
            {
                new PimsPropertyLease()
                {
                    PropertyLeaseId = 100,
                    Property = new PimsProperty()
                    {
                        PropertyId = 1,
                    },
                    Lease = new PimsLease()
                    {
                        LeaseStatusTypeCode = LeaseStatusTypes.TERMINATED.ToString()
                    }
                },
                new PimsPropertyLease()
                {
                    PropertyLeaseId = 200,
                    Property = new PimsProperty()
                    {
                        PropertyId = 1,
                    },
                    Lease = new PimsLease()
                    {
                        LeaseStatusTypeCode = LeaseStatusTypes.DRAFT.ToString()
                    }
                },
            });

            // Act
            var result = service.GetPropertyManagement(1);

            // Assert
            Assert.False(result.HasActiveLease);
            Assert.False(result.ActiveLeaseHasExpiryDate);

            repository.Verify(x => x.GetById(It.IsAny<long>()), Times.Once);
            coordinateService.Verify(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>()), Times.Once);
            propertyLeasesRepository.Verify(x => x.GetAllByPropertyId(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void GetPropertyManagement_HasActiveLease_NoRenewal_HasTerminationDate_Success()
        {
            // Arrange
            var property = EntityHelper.CreateProperty(1);

            var service = this.CreatePropertyServiceWithPermissions(Permissions.PropertyView, Permissions.ManagementView);
            var repository = this._helper.GetService<Mock<IPropertyRepository>>();
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(property);

            var coordinateService = this._helper.GetService<Mock<ICoordinateTransformService>>();
            coordinateService.Setup(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>()));

            var propertyLeasesRepository = this._helper.GetService<Mock<IPropertyLeaseRepository>>();
            propertyLeasesRepository.Setup(x => x.GetAllByPropertyId(It.IsAny<long>())).Returns(new List<PimsPropertyLease>()
            {
                new PimsPropertyLease()
                {
                    PropertyLeaseId = 100,
                    Property = new PimsProperty()
                    {
                        PropertyId = 1,
                    },
                    Lease = new PimsLease()
                    {
                        LeaseStatusTypeCode = LeaseStatusTypes.EXPIRED.ToString()
                    }
                },
                new PimsPropertyLease()
                {
                    PropertyLeaseId = 200,
                    Property = new PimsProperty()
                    {
                        PropertyId = 1,
                    },
                    Lease = new PimsLease()
                    {
                        LeaseStatusTypeCode = LeaseStatusTypes.ACTIVE.ToString(),
                        PimsLeaseRenewals = new List<PimsLeaseRenewal>(),
                        TerminationDate = DateTime.UtcNow.AddDays(30).Date,
                    }
                },
            });

            // Act
            var result = service.GetPropertyManagement(1);

            // Assert
            Assert.False(result.HasActiveLease);
            Assert.False(result.ActiveLeaseHasExpiryDate);

            repository.Verify(x => x.GetById(It.IsAny<long>()), Times.Once);
            coordinateService.Verify(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>()), Times.Once);
            propertyLeasesRepository.Verify(x => x.GetAllByPropertyId(It.IsAny<long>()), Times.Once);
        }

        //TODO: fix this
        //[Fact]
        public void GetPropertyManagement_HasActiveLease_NoRenewal_HasNoTerminationDate_Expired_Success()
        {
            // Arrange
            var property = EntityHelper.CreateProperty(1);

            var service = this.CreatePropertyServiceWithPermissions(Permissions.PropertyView, Permissions.ManagementView);
            var repository = this._helper.GetService<Mock<IPropertyRepository>>();
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(property);

            var coordinateService = this._helper.GetService<Mock<ICoordinateTransformService>>();
            coordinateService.Setup(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>()));

            var propertyLeasesRepository = this._helper.GetService<Mock<IPropertyLeaseRepository>>();
            propertyLeasesRepository.Setup(x => x.GetAllByPropertyId(It.IsAny<long>())).Returns(new List<PimsPropertyLease>()
            {
                new PimsPropertyLease()
                {
                    PropertyLeaseId = 100,
                    Property = new PimsProperty()
                    {
                        PropertyId = 1,
                    },
                    Lease = new PimsLease()
                    {
                        LeaseStatusTypeCode = LeaseStatusTypes.EXPIRED.ToString()
                    }
                },
                new PimsPropertyLease()
                {
                    PropertyLeaseId = 200,
                    Property = new PimsProperty()
                    {
                        PropertyId = 1,
                    },
                    Lease = new PimsLease()
                    {
                        LeaseStatusTypeCode = LeaseStatusTypes.ACTIVE.ToString(),
                        PimsLeaseRenewals = new List<PimsLeaseRenewal>(),
                        TerminationDate = null,
                        OrigExpiryDate = DateTime.UtcNow.AddDays(-1).Date,
                    }
                },
            }); ;

            // Act
            var result = service.GetPropertyManagement(1);

            // Assert
            Assert.False(result.HasActiveLease);
            Assert.False(result.ActiveLeaseHasExpiryDate);

            repository.Verify(x => x.GetById(It.IsAny<long>()), Times.Once);
            coordinateService.Verify(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>()), Times.Once);
            propertyLeasesRepository.Verify(x => x.GetAllByPropertyId(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void GetPropertyManagement_HasActiveLease_NoRenewal_HasNoTerminationDate_Valid_Success()
        {
            // Arrange
            var property = EntityHelper.CreateProperty(1);

            var service = this.CreatePropertyServiceWithPermissions(Permissions.PropertyView, Permissions.ManagementView);
            var repository = this._helper.GetService<Mock<IPropertyRepository>>();
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(property);

            var coordinateService = this._helper.GetService<Mock<ICoordinateTransformService>>();
            coordinateService.Setup(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>()));

            var propertyLeasesRepository = this._helper.GetService<Mock<IPropertyLeaseRepository>>();
            propertyLeasesRepository.Setup(x => x.GetAllByPropertyId(It.IsAny<long>())).Returns(new List<PimsPropertyLease>()
            {
                new PimsPropertyLease()
                {
                    PropertyLeaseId = 100,
                    Property = new PimsProperty()
                    {
                        PropertyId = 1,
                    },
                    Lease = new PimsLease()
                    {
                        LeaseStatusTypeCode = LeaseStatusTypes.EXPIRED.ToString()
                    }
                },
                new PimsPropertyLease()
                {
                    PropertyLeaseId = 200,
                    Property = new PimsProperty()
                    {
                        PropertyId = 1,
                    },
                    Lease = new PimsLease()
                    {
                        LeaseStatusTypeCode = LeaseStatusTypes.ACTIVE.ToString(),
                        PimsLeaseRenewals = new List<PimsLeaseRenewal>(),
                        TerminationDate = null,
                        OrigExpiryDate = DateTime.UtcNow.Date,
                    }
                },
            }); ;

            // Act
            var result = service.GetPropertyManagement(1);

            // Assert
            Assert.True(result.HasActiveLease);
            Assert.True(result.ActiveLeaseHasExpiryDate);

            repository.Verify(x => x.GetById(It.IsAny<long>()), Times.Once);
            coordinateService.Verify(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>()), Times.Once);
            propertyLeasesRepository.Verify(x => x.GetAllByPropertyId(It.IsAny<long>()), Times.Once);
        }


        [Fact]
        public void Update_PropertyManagement_Success()
        {
            // Arrange
            var property = EntityHelper.CreateProperty(1);

            var service = this.CreatePropertyServiceWithPermissions(Permissions.PropertyView, Permissions.PropertyEdit, Permissions.ManagementView, Permissions.ManagementEdit);
            var repository = this._helper.GetService<Mock<IPropertyRepository>>();
            repository.Setup(x => x.UpdatePropertyManagement(It.IsAny<PimsProperty>())).Returns(property);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(property);

            var newValues = new PimsProperty();
            property.CopyValues(newValues);
            newValues.AdditionalDetails = "test";
            newValues.IsTaxesPayable = true;

            // Act
            var updatedProperty = service.UpdatePropertyManagement(newValues);

            // Assert
            repository.Verify(x => x.UpdatePropertyManagement(It.IsAny<PimsProperty>()), Times.Once);
            repository.Verify(x => x.GetById(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void Update_PropertyManagement_KeyNotFound()
        {
            // Arrange
            var service = this.CreatePropertyServiceWithPermissions(Permissions.PropertyView, Permissions.PropertyEdit, Permissions.ManagementView, Permissions.ManagementEdit);

            // Try to update a non-existent property
            var property = EntityHelper.CreateProperty(1);

            var repository = this._helper.GetService<Mock<IPropertyRepository>>();
            repository.Setup(x => x.UpdatePropertyManagement(property)).Throws<KeyNotFoundException>();

            // Act
            Action act = () => service.UpdatePropertyManagement(property);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }

        [Fact]
        public void Update_PropertyManagement_NoPermission()
        {
            // Arrange
            var property = EntityHelper.CreateProperty(1);

            var service = this.CreatePropertyServiceWithPermissions(Permissions.PropertyView, Permissions.ManagementView);
            var repository = this._helper.GetService<Mock<IPropertyRepository>>();

            // Act
            Action act = () => service.UpdatePropertyManagement(property);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.UpdatePropertyManagement(It.IsAny<PimsProperty>()), Times.Never);
        }

        [Fact]
        public void Create_PropertyManagementActivity_NoPermission()
        {
            // Arrange
            var service = this.CreatePropertyServiceWithPermissions();
            var repository = this._helper.GetService<Mock<IPropertyActivityRepository>>();

            // Act
            Action act = () => service.CreateActivity(new PimsPropertyActivity());

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.Create(It.IsAny<PimsPropertyActivity>()), Times.Never);
        }

        [Fact]
        public void Create_PropertyManagementActivity_Success()
        {
            // Arrange
            var service = this.CreatePropertyServiceWithPermissions(Permissions.ManagementAdd, Permissions.PropertyEdit);
            var repository = this._helper.GetService<Mock<IPropertyActivityRepository>>();
            var activity = new PimsPropertyActivity();

            // Act
            var result = service.CreateActivity(activity);

            // Assert
            activity.PropMgmtActivityStatusTypeCode.Should().Be("NOTSTARTED");
            repository.Verify(x => x.Create(It.IsAny<PimsPropertyActivity>()), Times.Once);
        }

        [Fact]
        public void Get_PropertyManagement_Activities_NoPermission()
        {
            // Arrange
            var service = this.CreatePropertyServiceWithPermissions();
            var repository = this._helper.GetService<Mock<IPropertyActivityRepository>>();

            // Act
            Action act = () => service.GetActivities(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.GetActivity(It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public void Update_PropertyManagement_Activity_NoPermission()
        {
            // Arrange
            var service = this.CreatePropertyServiceWithPermissions();
            var repository = this._helper.GetService<Mock<IPropertyActivityRepository>>();

            // Act
            Action act = () => service.UpdateActivity(1, 100, new PimsPropertyActivity());

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.Update(It.IsAny<PimsPropertyActivity>()), Times.Never);
        }

        [Fact]
        public void Update_PropertyManagement_Activity_InvalidIdentifiers_Wrong_PropertyId()
        {
            // Arrange
            var service = this.CreatePropertyServiceWithPermissions(Permissions.ManagementEdit, Permissions.PropertyEdit);
            var repository = this._helper.GetService<Mock<IPropertyActivityRepository>>();

            // Act
            Action act = () => service.UpdateActivity(1, 10, new PimsPropertyActivity()
            {
                PimsPropertyActivityId = 10,
                PimsPropPropActivities = new List<PimsPropPropActivity>()
                {
                    new PimsPropPropActivity()
                    {
                        PropPropActivityId = 100,
                        PropertyId = 2,
                        PimsPropertyActivityId = 10,
                    },
                    new PimsPropPropActivity()
                    {
                        PropPropActivityId = 101,
                        PropertyId = 2,
                        PimsPropertyActivityId = 11,
                    }
                }
            }); ;

            // Assert
            act.Should().Throw<BadRequestException>();
            repository.Verify(x => x.Update(It.IsAny<PimsPropertyActivity>()), Times.Never);
        }

        [Fact]
        public void Update_PropertyManagement_Activity_InvalidIdentifiers_Wrong_ActivityId()
        {
            // Arrange
            var service = this.CreatePropertyServiceWithPermissions(Permissions.ManagementEdit, Permissions.PropertyEdit);
            var repository = this._helper.GetService<Mock<IPropertyActivityRepository>>();

            // Act
            Action act = () => service.UpdateActivity(1, 20, new PimsPropertyActivity()
            {
                PimsPropertyActivityId = 10,
                PimsPropPropActivities = new List<PimsPropPropActivity>()
                {
                    new PimsPropPropActivity()
                    {
                        PropPropActivityId = 100,
                        PropertyId = 1,
                        PimsPropertyActivityId = 10,
                    },
                    new PimsPropPropActivity()
                    {
                        PropPropActivityId = 101,
                        PropertyId = 1,
                        PimsPropertyActivityId = 30,
                    }
                }
            }); ;

            // Assert
            act.Should().Throw<BadRequestException>();
            repository.Verify(x => x.Update(It.IsAny<PimsPropertyActivity>()), Times.Never);
        }

        [Fact]
        public void Update_PropertyManagement_Activity_InvalidIdentifiers_Wrong_Model_ActivityId()
        {
            // Arrange
            var service = this.CreatePropertyServiceWithPermissions(Permissions.ManagementEdit, Permissions.PropertyEdit);
            var repository = this._helper.GetService<Mock<IPropertyActivityRepository>>();

            // Act
            Action act = () => service.UpdateActivity(1, 20, new PimsPropertyActivity()
            {
                PimsPropertyActivityId = 500,
                PimsPropPropActivities = new List<PimsPropPropActivity>()
                {
                    new PimsPropPropActivity()
                    {
                        PropPropActivityId = 100,
                        PropertyId = 1,
                        PimsPropertyActivityId = 20,
                    }
                }
            }); ;

            // Assert
            act.Should().Throw<BadRequestException>();
            repository.Verify(x => x.Update(It.IsAny<PimsPropertyActivity>()), Times.Never);
        }

        [Fact]
        public void Update_PropertyManagement_Activity_Success()
        {
            // Arrange
            var service = this.CreatePropertyServiceWithPermissions(Permissions.ManagementEdit, Permissions.PropertyEdit);
            var repository = this._helper.GetService<Mock<IPropertyActivityRepository>>();
            repository.Setup(x => x.Update(It.IsAny<PimsPropertyActivity>())).Returns(new PimsPropertyActivity());

            // Act
            var result = service.UpdateActivity(1, 10, new PimsPropertyActivity()
            {
                PimsPropertyActivityId = 10,
                PimsPropPropActivities = new List<PimsPropPropActivity>()
                {
                    new PimsPropPropActivity()
                    {
                        PropPropActivityId = 100,
                        PropertyId = 1,
                        PimsPropertyActivityId = 10,
                    },
                    new PimsPropPropActivity()
                    {
                        PropPropActivityId = 101,
                        PropertyId = 1,
                        PimsPropertyActivityId = 11,
                    }
                }
            });

            // Assert
            Assert.NotNull(result);
            repository.Verify(x => x.Update(It.IsAny<PimsPropertyActivity>()), Times.Once);
        }

        [Fact]
        public void Delete_PropertyManagementActivity_NoPermission()
        {
            // Arrange
            var service = this.CreatePropertyServiceWithPermissions();
            var repository = this._helper.GetService<Mock<IPropertyActivityRepository>>();

            // Act
            Action act = () => service.DeleteActivity(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.TryDelete(It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public void Delete_PropertyManagementActivity_BadRequest_Activity_Status_IsNot_NotStarted()
        {
            // Arrange
            var service = this.CreatePropertyServiceWithPermissions(Permissions.ManagementDelete);
            var repository = this._helper.GetService<Mock<IPropertyActivityRepository>>();

            var propertyManagementActivity = EntityHelper.CreatePropertyActivity(10, activityStatusTypeCode: "STARTED");

            repository.Setup(x => x.GetActivity(It.IsAny<long>())).Returns(propertyManagementActivity);

            // Act
            Action act = () => service.DeleteActivity(1);

            // Assert
            act.Should().Throw<BadRequestException>();
            repository.Verify(x => x.TryDelete(It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public void Delete_PropertyManagementActivity_Success()
        {
            // Arrange
            var service = this.CreatePropertyServiceWithPermissions(Permissions.ManagementDelete);
            var repository = this._helper.GetService<Mock<IPropertyActivityRepository>>();

            var propertyManagementActivity = EntityHelper.CreatePropertyActivity(1);

            repository.Setup(x => x.GetActivity(It.IsAny<long>())).Returns(propertyManagementActivity);
            repository.Setup(x => x.TryDelete(It.IsAny<long>())).Returns(true);

            // Act
            var result = service.DeleteActivity(1);

            // Assert
            Assert.True(result);
            repository.Verify(x => x.TryDelete(It.IsAny<long>()), Times.Once);
        }
        #endregion

        [Fact]
        public void Update_HistoricalFileNumbers_NoPermission()
        {
            // Arrange
            var property = EntityHelper.CreateProperty(1);

            var service = this.CreatePropertyServiceWithPermissions(Permissions.PropertyView);
            var repository = this._helper.GetService<Mock<IHistoricalNumberRepository>>();

            List<PimsHistoricalFileNumber> historicalNumbers = new() { };

            // Assert
            Assert.Throws<NotAuthorizedException>(() => service.UpdateHistoricalFileNumbers(property.PropertyId, historicalNumbers));
            repository.Verify(x => x.UpdateHistoricalFileNumbers(It.IsAny<long>(), It.IsAny<IEnumerable<PimsHistoricalFileNumber>>()), Times.Never);
        }

        [Fact]
        public void Update_HistoricalFileNumbers_Duplicate_FileNumberType()
        {
            // Arrange
            var property = EntityHelper.CreateProperty(1);

            var service = this.CreatePropertyServiceWithPermissions(Permissions.PropertyEdit);
            var repository = this._helper.GetService<Mock<IHistoricalNumberRepository>>();

            List<PimsHistoricalFileNumber> historicalNumbers = new() {
                new()
                {
                    HistoricalFileNumber = "123",
                    HistoricalFileNumberTypeCode = HistoricalFileNumberTypes.LISNO.ToString(),
                },
                new()
                {
                    HistoricalFileNumber = "123",
                    HistoricalFileNumberTypeCode = HistoricalFileNumberTypes.LISNO.ToString(),
                }
            };

            // Assert
            Assert.Throws<DuplicateEntityException>(() => service.UpdateHistoricalFileNumbers(property.PropertyId, historicalNumbers));
            repository.Verify(x => x.UpdateHistoricalFileNumbers(It.IsAny<long>(), It.IsAny<IEnumerable<PimsHistoricalFileNumber>>()), Times.Never);
        }

        [Fact]
        public void Update_HistoricalFileNumbers_Duplicate_OTHER_FileNumberType()
        {
            // Arrange
            var property = EntityHelper.CreateProperty(1);

            var service = this.CreatePropertyServiceWithPermissions(Permissions.PropertyEdit);
            var repository = this._helper.GetService<Mock<IHistoricalNumberRepository>>();

            List<PimsHistoricalFileNumber> historicalNumbers = new() {
                new()
                {
                    HistoricalFileNumber = "123",
                    HistoricalFileNumberTypeCode = HistoricalFileNumberTypes.OTHER.ToString(),
                    OtherHistFileNumberTypeCode = "TEST",
                },
                new()
                {
                    HistoricalFileNumber = "123",
                    HistoricalFileNumberTypeCode = HistoricalFileNumberTypes.OTHER.ToString(),
                    OtherHistFileNumberTypeCode = "TEST",
                }
            };

            // Assert
            Assert.Throws<DuplicateEntityException>(() => service.UpdateHistoricalFileNumbers(property.PropertyId, historicalNumbers));
            repository.Verify(x => x.UpdateHistoricalFileNumbers(It.IsAny<long>(), It.IsAny<IEnumerable<PimsHistoricalFileNumber>>()), Times.Never);
        }

        #endregion
    }
}
