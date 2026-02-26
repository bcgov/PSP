using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Moq;
using NetTopologySuite.Geometries;
using Pims.Api.Constants;
using Pims.Api.Models.CodeTypes;
using Pims.Api.Services;
using Pims.Core.Api.Exceptions;
using Pims.Core.Exceptions;
using Pims.Core.Security;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Xunit;

namespace Pims.Api.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "management")]
    [ExcludeFromCodeCoverage]
    public class ManagementFileServiceTest
    {
        private readonly TestHelper _helper;

        public ManagementFileServiceTest()
        {
            this._helper = new TestHelper();
        }

        private ManagementFileService CreateManagementServiceWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            return this._helper.Create<ManagementFileService>(user);
        }

        #region GetById
        [Fact]
        public void GetById_Success()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions(Permissions.ManagementView);

            var managementFile = EntityHelper.CreateManagementFile();

            var repository = this._helper.GetService<Mock<IManagementFileRepository>>();
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(managementFile);

            // Act
            var result = service.GetById(1);

            // Assert
            repository.Verify(x => x.GetById(It.IsAny<long>()), Times.Exactly(1));
        }

        [Fact]
        public void GetById_NoPermission()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions();

            var managementFile = EntityHelper.CreateManagementFile();

            // Act
            Action act = () => service.GetById(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }
        #endregion

        #region Properties

        [Fact]
        public void GetProperties_ByManagementFileId_NoPermission()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions(Permissions.ManagementView);

            var managementFile = EntityHelper.CreateManagementFile();

            var repository = this._helper.GetService<Mock<IManagementFilePropertyRepository>>();
            repository.Setup(x => x.GetPropertiesByManagementFileId(It.IsAny<long>())).Returns(new List<PimsManagementFileProperty>());

            // Act
            Action act = () => service.GetProperties(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void GetProperties_ByManagementFileId_Success()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions(Permissions.ManagementView, Permissions.PropertyView);

            var managementFile = EntityHelper.CreateManagementFile();

            var repository = this._helper.GetService<Mock<IManagementFilePropertyRepository>>();
            repository.Setup(x => x.GetPropertiesByManagementFileId(It.IsAny<long>())).Returns(new List<PimsManagementFileProperty>());

            // Act
            var properties = service.GetProperties(1);

            // Assert
            repository.Verify(x => x.GetPropertiesByManagementFileId(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void GetProperties_ByManagementFileId_Success_Reproject()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions(Permissions.ManagementView, Permissions.PropertyView);

            var managementFile = EntityHelper.CreateManagementFile();

            var repository = this._helper.GetService<Mock<IManagementFilePropertyRepository>>();
            repository.Setup(x => x.GetPropertiesByManagementFileId(It.IsAny<long>()))
                .Returns(new List<PimsManagementFileProperty>() { new PimsManagementFileProperty() { Property = new PimsProperty() { Location = new Point(1, 1) } } });

            var propertyService = this._helper.GetService<Mock<IPropertyService>>();
            propertyService.Setup(x => x.TransformAllPropertiesToLatLong(It.IsAny<List<PimsManagementFileProperty>>()))
                .Returns<List<PimsManagementFileProperty>>(x => x);

            // Act
            var properties = service.GetProperties(1);

            // Assert
            repository.Verify(x => x.GetPropertiesByManagementFileId(It.IsAny<long>()), Times.Once);
            propertyService.Verify(x => x.TransformAllPropertiesToLatLong(It.IsAny<List<PimsManagementFileProperty>>()), Times.Once);
            properties.FirstOrDefault().Property.Location.Coordinates.Should().BeEquivalentTo(new Coordinate[] { new Coordinate(1, 1) });
        }
        #endregion

        #region GetLastUpdate

        [Fact]
        public void GetLastUpdate_ByManagementFileId_NoPermission()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions();

            var managementFile = EntityHelper.CreateManagementFile();

            var repository = this._helper.GetService<Mock<IManagementFileRepository>>();
            repository.Setup(x => x.GetLastUpdateBy(It.IsAny<long>())).Returns(new LastUpdatedByModel());

            // Act
            Action act = () => service.GetLastUpdateInformation(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void GetLastUpdate_ByManagementFileId_Success()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions(Permissions.ManagementView);

            var managementFile = EntityHelper.CreateManagementFile();

            var repository = this._helper.GetService<Mock<IManagementFileRepository>>();
            repository.Setup(x => x.GetLastUpdateBy(It.IsAny<long>())).Returns(new LastUpdatedByModel());

            // Act
            var properties = service.GetLastUpdateInformation(1);

            // Assert
            repository.Verify(x => x.GetLastUpdateBy(It.IsAny<long>()), Times.Once);
        }
        #endregion

        #region Add

        [Fact]
        public void Add_Should_Fail_NoPermission()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions();
            var managementFile = EntityHelper.CreateManagementFile(1);

            // Act
            Action act = () => service.Add(managementFile, new List<UserOverrideCode>());

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void Add_ThrowIfNull()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions(Permissions.ManagementAdd);

            var managementFile = EntityHelper.CreateManagementFile();

            var repository = this._helper.GetService<Mock<IManagementFileRepository>>();
            var propertyService = this._helper.GetService<Mock<IPropertyService>>();

            // Act
            Action act = () => service.Add(null, new List<UserOverrideCode>());

            // Assert
            act.Should().Throw<ArgumentNullException>();
            repository.Verify(x => x.Add(It.IsAny<PimsManagementFile>()), Times.Never);
            propertyService.Verify(x => x.PopulateNewFileProperty(It.IsAny<PimsManagementFileProperty>()), Times.Never);
        }

        [Fact]
        public void Add_Success_SameRole_Team()
        {
            var service = this.CreateManagementServiceWithPermissions(Permissions.ManagementAdd);
            var managementFile = EntityHelper.CreateManagementFile();

            managementFile.PimsManagementFileTeams.Add(new PimsManagementFileTeam() { PersonId = 1, ManagementFileProfileTypeCode = "LISTAGENT" });
            managementFile.PimsManagementFileTeams.Add(new PimsManagementFileTeam() { PersonId = 2, ManagementFileProfileTypeCode = "LISTAGENT" });

            var repository = this._helper.GetService<Mock<IManagementFileRepository>>();
            repository.Setup(x => x.GetByName(It.IsAny<string>())).Returns(managementFile);
            repository.Setup(x => x.Add(It.IsAny<PimsManagementFile>())).Returns(managementFile);

            var lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();
            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });

            var propertyService = this._helper.GetService<Mock<IPropertyService>>();

            // Act
            service.Add(managementFile, new List<UserOverrideCode>() { UserOverrideCode.AddPropertyToInventory });

            // Assert
            repository.Verify(x => x.Add(It.IsAny<PimsManagementFile>()), Times.Once);
        }

        [Fact]
        public void Add_Fails_Duplicate_Team()
        {
            var service = this.CreateManagementServiceWithPermissions(Permissions.ManagementAdd);
            var managementFile = EntityHelper.CreateManagementFile();

            managementFile.PimsManagementFileTeams.Add(new PimsManagementFileTeam() { PersonId = 1, ManagementFileProfileTypeCode = "LISTAGENT" });
            managementFile.PimsManagementFileTeams.Add(new PimsManagementFileTeam() { PersonId = 1, ManagementFileProfileTypeCode = "LISTAGENT" });

            var repository = this._helper.GetService<Mock<IManagementFileRepository>>();
            repository.Setup(x => x.GetByName(It.IsAny<string>())).Returns(managementFile);

            var lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();
            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });

            var propertyService = this._helper.GetService<Mock<IPropertyService>>();

            // Act
            Action act = () => service.Add(managementFile, new List<UserOverrideCode>() { UserOverrideCode.AddPropertyToInventory });

            // Assert
            act.Should().Throw<BadRequestException>();
            repository.Verify(x => x.Add(It.IsAny<PimsManagementFile>()), Times.Never);
            propertyService.Verify(x => x.PopulateNewFileProperty(It.IsAny<PimsManagementFileProperty>()), Times.Never);
        }

        [Fact]
        public void Add_Success_FileMarkerLocation()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions(Permissions.ManagementAdd);

            var managementFile = EntityHelper.CreateManagementFile();
            var property = EntityHelper.CreateProperty(1000, regionCode: 1);

            managementFile.PimsManagementFileProperties = new List<PimsManagementFileProperty>() { new PimsManagementFileProperty() { Property = property } };

            var repository = this._helper.GetService<Mock<IManagementFileRepository>>();
            repository.Setup(x => x.Add(It.IsAny<PimsManagementFile>())).Returns(managementFile);
            repository.Setup(x => x.GetByName(It.IsAny<string>())).Returns(managementFile);

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), true)).Returns(property);

            var lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();
            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });

            var propertyService = this._helper.GetService<Mock<IPropertyService>>();

            // Act
            var result = service.Add(managementFile, new List<UserOverrideCode>());

            // Assert
            repository.Verify(x => x.Add(It.IsAny<PimsManagementFile>()), Times.Once);
            propertyService.Verify(x => x.PopulateNewFileProperty(It.IsAny<PimsManagementFileProperty>()), Times.Once);
        }

        [Fact]
        public void Add_WithRetiredProperty_Should_Fail()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions(Permissions.ManagementAdd);

            var managementFile = EntityHelper.CreateManagementFile();
            var pimsProperty = EntityHelper.CreateProperty(1000, isRetired: true);

            managementFile.PimsManagementFileProperties = new List<PimsManagementFileProperty>()
            {
                new PimsManagementFileProperty()
                {
                    ManagementFilePropertyId = 0,
                    Property = pimsProperty,
                },
            };

            var repository = this._helper.GetService<Mock<IManagementFileRepository>>();
            repository.Setup(x => x.GetByName(It.IsAny<string>())).Returns(managementFile);
            repository.Setup(x => x.Add(It.IsAny<PimsManagementFile>())).Returns(managementFile);

            var lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();
            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), true)).Returns(pimsProperty);

            var propertyService = this._helper.GetService<Mock<IPropertyService>>();

            // Act
            Action act = () => service.Add(managementFile, new List<UserOverrideCode>());

            // Assert
            var ex = act.Should().Throw<BusinessRuleViolationException>();
            ex.WithMessage("New retired property can not be added.");
            repository.Verify(x => x.Add(It.IsAny<PimsManagementFile>()), Times.Never);
            propertyService.Verify(x => x.PopulateNewFileProperty(It.IsAny<PimsManagementFileProperty>()), Times.Never);
        }

        #endregion

        #region Update
        [Fact]
        public void Update_Should_Fail_NoPermission()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions();
            var managementFile = EntityHelper.CreateManagementFile(1);

            // Act
            Action act = () => service.Update(1, managementFile, new List<UserOverrideCode>());

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void Update_Should_Fail_Invalid_ManagementFileId()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions(Permissions.ManagementEdit);
            var repository = this._helper.GetService<Mock<IManagementFileRepository>>();
            var managementFile = EntityHelper.CreateManagementFile(1);

            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);

            // Act
            Action act = () => service.Update(2, managementFile, new List<UserOverrideCode>());

            // Assert
            act.Should().Throw<BadRequestException>();
        }

        [Fact]
        public void Update_Success_Team_SameRole()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions(Permissions.ManagementEdit);
            var repository = this._helper.GetService<Mock<IManagementFileRepository>>();

            var statusMock = this._helper.GetService<Mock<IManagementFileStatusSolver>>();
            statusMock.Setup(x => x.CanEditDetails(It.IsAny<ManagementFileStatusTypes>())).Returns(true);

            var managementFile = EntityHelper.CreateManagementFile(1);

            managementFile.PimsManagementFileTeams.Add(new PimsManagementFileTeam() { PersonId = 1, ManagementFileProfileTypeCode = "LISTAGENT" });
            managementFile.PimsManagementFileTeams.Add(new PimsManagementFileTeam() { OrganizationId = 1, ManagementFileProfileTypeCode = "LISTAGENT" });

            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(managementFile);
            repository.Setup(x => x.GetByName(It.IsAny<string>())).Returns(managementFile);

            var lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();
            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });

            var managementFilePropertyRepository = this._helper.GetService<Mock<IManagementFilePropertyRepository>>();
            managementFilePropertyRepository.Setup(x => x.GetPropertiesByManagementFileId(It.IsAny<long>())).Returns(new List<PimsManagementFileProperty>());

            // Act
            var result = service.Update(1, managementFile, new List<UserOverrideCode>() { });

            // Assert
            Assert.NotNull(result);
            repository.Verify(x => x.Update(It.IsAny<long>(), It.IsAny<PimsManagementFile>()), Times.Once);
        }

        [Fact]
        public void Update_Should_Fail_Duplicate_Team()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions(Permissions.ManagementEdit);
            var repository = this._helper.GetService<Mock<IManagementFileRepository>>();

            var statusMock = this._helper.GetService<Mock<IManagementFileStatusSolver>>();
            statusMock.Setup(x => x.CanEditDetails(It.IsAny<ManagementFileStatusTypes>())).Returns(true);

            var managementFile = EntityHelper.CreateManagementFile(1);

            managementFile.PimsManagementFileTeams.Add(new PimsManagementFileTeam() { PersonId = 1, ManagementFileProfileTypeCode = "LISTAGENT" });
            managementFile.PimsManagementFileTeams.Add(new PimsManagementFileTeam() { PersonId = 1, ManagementFileProfileTypeCode = "LISTAGENT" });

            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(managementFile);

            var lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();
            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });

            // Act
            Action act = () => service.Update(1, managementFile, new List<UserOverrideCode>());

            // Assert
            act.Should().Throw<BadRequestException>();
        }

        [Fact]
        public void Update_Success_FileMarkerLocation()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions(Permissions.ManagementEdit);

            var managementFile = EntityHelper.CreateManagementFile();
            var property = EntityHelper.CreateProperty(1000, regionCode: 1);

            managementFile.PimsManagementFileProperties = new List<PimsManagementFileProperty>() { new PimsManagementFileProperty() { Property = property } };
            managementFile.ManagementFileId = 1;

            var repository = this._helper.GetService<Mock<IManagementFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(managementFile);
            repository.Setup(x => x.GetByName(It.IsAny<string>())).Returns(managementFile);

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), true)).Returns(property);

            var lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();
            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });

            var managementFilePropertyRepository = this._helper.GetService<Mock<IManagementFilePropertyRepository>>();
            managementFilePropertyRepository.Setup(x => x.GetPropertiesByManagementFileId(It.IsAny<long>())).Returns(new List<PimsManagementFileProperty>());

            var statusMock = this._helper.GetService<Mock<IManagementFileStatusSolver>>();
            statusMock.Setup(x => x.CanEditDetails(It.IsAny<ManagementFileStatusTypes>())).Returns(true);

            var propertyService = this._helper.GetService<Mock<IPropertyService>>();

            // Act
            service.Update(1, managementFile, new List<UserOverrideCode>());

            // Assert
            repository.Verify(x => x.Update(It.IsAny<long>(), It.IsAny<PimsManagementFile>()), Times.Once);
            propertyService.Verify(x => x.UpdateFilePropertyLocation<PimsManagementFileProperty>(It.IsAny<PimsManagementFileProperty>(), It.IsAny<PimsManagementFileProperty>()), Times.Never);
        }

        [Fact]
        public void Update_Should_Fail_Inactive_Team_Role()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions(Permissions.ManagementEdit);
            var repository = this._helper.GetService<Mock<IManagementFileRepository>>();

            var statusMock = this._helper.GetService<Mock<IManagementFileStatusSolver>>();
            statusMock.Setup(x => x.CanEditDetails(It.IsAny<ManagementFileStatusTypes>())).Returns(true);

            var managementFile = EntityHelper.CreateManagementFile(1);

            managementFile.PimsManagementFileTeams.Add(new PimsManagementFileTeam() { PersonId = 1, ManagementFileProfileTypeCode = "LISTAGENT" });
            managementFile.PimsManagementFileTeams.Add(new PimsManagementFileTeam() { PersonId = 2, ManagementFileProfileTypeCode = "APPROVER" });

            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(managementFile);
            repository.Setup(x => x.GetByName(It.IsAny<string>())).Returns(managementFile);

            var lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();
            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });

            var managementFilePropertyRepository = this._helper.GetService<Mock<IManagementFilePropertyRepository>>();
            managementFilePropertyRepository.Setup(x => x.GetPropertiesByManagementFileId(It.IsAny<long>())).Returns(new List<PimsManagementFileProperty>());

            // Act
            var result = service.Update(1, managementFile, new List<UserOverrideCode>() { });

            // Assert
            Assert.NotNull(result);
            repository.Verify(x => x.Update(It.IsAny<long>(), It.IsAny<PimsManagementFile>()), Times.Once);
        }

        [Fact]
        public void Update_Success_ReplacesFileMarkerLocation()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions(Permissions.ManagementEdit);

            var managementFile = EntityHelper.CreateManagementFile();
            var property = EntityHelper.CreateProperty(1000, regionCode: 1);
            var property2 = EntityHelper.CreateProperty(1001, regionCode: 1);

            managementFile.PimsManagementFileProperties = new List<PimsManagementFileProperty>()
            {
                new PimsManagementFileProperty() { Property = property, Internal_Id = 1 },
                new PimsManagementFileProperty() { Property = property2, Internal_Id = 2 },
            };
            managementFile.ManagementFileId = 1;

            var repository = this._helper.GetService<Mock<IManagementFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(managementFile);
            repository.Setup(x => x.GetByName(It.IsAny<string>())).Returns(managementFile);

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), true)).Returns(property);

            var lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();
            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });

            var managementFilePropertyRepository = this._helper.GetService<Mock<IManagementFilePropertyRepository>>();
            managementFilePropertyRepository.Setup(x => x.GetPropertiesByManagementFileId(It.IsAny<long>())).Returns(new List<PimsManagementFileProperty>());

            var statusMock = this._helper.GetService<Mock<IManagementFileStatusSolver>>();
            statusMock.Setup(x => x.CanEditDetails(It.IsAny<ManagementFileStatusTypes>())).Returns(true);

            var propertyService = this._helper.GetService<Mock<IPropertyService>>();

            // Act
            service.Update(1, managementFile, new List<UserOverrideCode>());

            // Assert
            repository.Verify(x => x.Update(It.IsAny<long>(), It.IsAny<PimsManagementFile>()), Times.Once);
            propertyService.Verify(x => x.UpdateFilePropertyLocation<PimsManagementFileProperty>(It.IsAny<PimsManagementFileProperty>(), It.IsAny<PimsManagementFileProperty>()), Times.Never);
        }

        [Fact]
        public void Update_Success_OverwritesFileMarkerLocation()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions(Permissions.ManagementEdit);

            var managementFile = EntityHelper.CreateManagementFile();
            var property = EntityHelper.CreateProperty(1000, regionCode: 1);
            var property2 = EntityHelper.CreateProperty(1001, regionCode: 1);

            managementFile.PimsManagementFileProperties = new List<PimsManagementFileProperty>()
            {
                new PimsManagementFileProperty() { Property = property, Internal_Id = 1 },
                new PimsManagementFileProperty() { Property = property2, Internal_Id = 2 },
            };
            managementFile.ManagementFileId = 1;

            var repository = this._helper.GetService<Mock<IManagementFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(managementFile);
            repository.Setup(x => x.GetByName(It.IsAny<string>())).Returns(managementFile);

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), true)).Returns(property2);

            var lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();
            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });

            var managementFilePropertyRepository = this._helper.GetService<Mock<IManagementFilePropertyRepository>>();
            managementFilePropertyRepository.Setup(x => x.GetPropertiesByManagementFileId(It.IsAny<long>())).Returns(new List<PimsManagementFileProperty>());

            var statusMock = this._helper.GetService<Mock<IManagementFileStatusSolver>>();
            statusMock.Setup(x => x.CanEditDetails(It.IsAny<ManagementFileStatusTypes>())).Returns(true);

            var propertyService = this._helper.GetService<Mock<IPropertyService>>();

            // Act
            var result = service.Update(1, managementFile, new List<UserOverrideCode>());

            // Assert
            repository.Verify(x => x.Update(It.IsAny<long>(), It.IsAny<PimsManagementFile>()), Times.Once);
            propertyService.Verify(x => x.UpdateFilePropertyLocation<PimsManagementFileProperty>(It.IsAny<PimsManagementFileProperty>(), It.IsAny<PimsManagementFileProperty>()), Times.Never);
        }

        [Fact]
        public void Update_Success_NeitherAgent()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions(Permissions.ManagementEdit);
            var repository = this._helper.GetService<Mock<IManagementFileRepository>>();

            var statusMock = this._helper.GetService<Mock<IManagementFileStatusSolver>>();
            statusMock.Setup(x => x.CanEditDetails(It.IsAny<ManagementFileStatusTypes>())).Returns(true);

            var managementFile = EntityHelper.CreateManagementFile(1);

            managementFile.PimsManagementFileTeams.Add(new PimsManagementFileTeam() { PersonId = 3, ManagementFileProfileTypeCode = "LISTAGENT" });
            managementFile.PimsManagementFileTeams.Add(new PimsManagementFileTeam() { PersonId = 4, ManagementFileProfileTypeCode = "LISTAGENT" });

            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(managementFile);
            repository.Setup(x => x.GetByName(It.IsAny<string>())).Returns(managementFile);

            var lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();
            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });

            var managementFilePropertyRepository = this._helper.GetService<Mock<IManagementFilePropertyRepository>>();
            managementFilePropertyRepository.Setup(x => x.GetPropertiesByManagementFileId(It.IsAny<long>())).Returns(new List<PimsManagementFileProperty>());

            // Act
            var result = service.Update(1, managementFile, new List<UserOverrideCode>() { });

            // Assert
            Assert.NotNull(result);
            repository.Verify(x => x.Update(It.IsAny<long>(), It.IsAny<PimsManagementFile>()), Times.Once);
        }

        [Fact]
        public void Update_Success_FinalButAdmin()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions(Permissions.SystemAdmin, Permissions.ManagementEdit);
            var repository = this._helper.GetService<Mock<IManagementFileRepository>>();
            var managementFile = EntityHelper.CreateManagementFile(1);
            var lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();

            repository.Setup(x => x.GetByName(It.IsAny<string>())).Returns(managementFile);
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.Update(It.IsAny<long>(), It.IsAny<PimsManagementFile>())).Returns(managementFile);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(managementFile);

            var statusMock = this._helper.GetService<Mock<IManagementFileStatusSolver>>();
            statusMock.Setup(x => x.CanEditDetails(It.IsAny<ManagementFileStatusTypes>())).Returns(true);

            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });
            lookupRepository.Setup(x => x.GetAllManagementFileStatusTypes()).Returns(new PimsManagementFileStatusType[]{ new PimsManagementFileStatusType() {
                Id = ManagementFileStatusTypes.COMPLETE.ToString(),
                Description = "COMPLETE",
            },});


            PimsManagementFile updatedFile = new()
            {
                ManagementFileId = 1,
                ManagementFileStatusTypeCode = ManagementFileStatusTypes.COMPLETE.ToString(),
                ManagementFilePurposeTypeCode = managementFile.ManagementFilePurposeTypeCode,
                ConcurrencyControlNumber = 1,
            };

            // Act
            var result = service.Update(1, updatedFile, new List<UserOverrideCode>() { });

            // Assert
            Assert.NotNull(result);
            repository.Verify(x => x.Update(It.IsAny<long>(), It.IsAny<PimsManagementFile>()), Times.Once);
        }

        [Fact]
        public void Update_Success_AddsNote()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions(Permissions.ManagementEdit);

            var managementFile = EntityHelper.CreateManagementFile();
            managementFile.ConcurrencyControlNumber = 1;
            managementFile.AppCreateUserid = "TESTER";

            var repository = this._helper.GetService<Mock<IManagementFileRepository>>();
            var noteRepository = this._helper.GetService<Mock<INoteRelationshipRepository<PimsManagementFileNote>>>();
            var lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();

            var statusMock = this._helper.GetService<Mock<IManagementFileStatusSolver>>();
            statusMock.Setup(x => x.CanEditDetails(It.IsAny<ManagementFileStatusTypes>())).Returns(true);

            repository.Setup(x => x.Update(It.IsAny<long>(), It.IsAny<PimsManagementFile>())).Returns(managementFile);
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(new PimsManagementFile()
            {
                ManagementFileStatusTypeCode = "CLOSED",
                ManagementFileStatusTypeCodeNavigation = new PimsManagementFileStatusType() { Description = "Closed" },
            });
            repository.Setup(x => x.GetByName(It.IsAny<string>())).Returns(managementFile);
            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });
            lookupRepository.Setup(x => x.GetAllManagementFileStatusTypes()).Returns(new PimsManagementFileStatusType[]{ new PimsManagementFileStatusType() {
                Id = managementFile.ManagementFileStatusTypeCodeNavigation.Id,
                Description = managementFile.ManagementFileStatusTypeCodeNavigation.Description,
            },});

            // Act
            var result = service.Update(managementFile.Internal_Id, managementFile, new List<UserOverrideCode>() { });

            // Assert
            repository.Verify(x => x.Update(It.IsAny<long>(), It.IsAny<PimsManagementFile>()), Times.Once);
            noteRepository.Verify(x => x.AddNoteRelationship(It.Is<PimsManagementFileNote>(x => x.ManagementFileId == 1
                    && x.Note.NoteTxt == "Management File status changed from Closed to Active")), Times.Once);
        }
        #endregion

        #region UpdateProperties
        [Fact]
        public void UpdateProperties_Success()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions(Permissions.ManagementEdit, Permissions.PropertyAdd, Permissions.PropertyView);

            var managementFile = EntityHelper.CreateManagementFile();
            managementFile.ConcurrencyControlNumber = 1;

            var repository = this._helper.GetService<Mock<IManagementFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(managementFile);

            var filePropertyRepository = this._helper.GetService<Mock<IManagementFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByManagementFileId(It.IsAny<long>())).Returns(managementFile.PimsManagementFileProperties.ToList());

            var propertyActivityRepository = this._helper.GetService<Mock<IManagementActivityRepository>>();
            propertyActivityRepository.Setup(x => x.GetActivitiesByManagementFile(It.IsAny<long>())).Returns(new List<PimsManagementActivity>());

            var statusMock = this._helper.GetService<Mock<IManagementFileStatusSolver>>();
            statusMock.Setup(x => x.GetCurrentManagementStatus(It.IsAny<string>())).Returns(ManagementFileStatusTypes.ACTIVE);
            statusMock.Setup(x => x.CanEditProperties(It.IsAny<ManagementFileStatusTypes>())).Returns(true);

            var locationSolver = this._helper.GetService<Mock<IFilePropertyLocationUpdateSolver>>();
            locationSolver.Setup(x => x.CanEditFilePropertyLocation(It.IsAny<PimsManagementFileProperty>(), It.IsAny<PimsManagementFileProperty>())).Returns(true);
            locationSolver.Setup(x => x.CanEditFilePropertyBoundary(It.IsAny<PimsManagementFileProperty>(), It.IsAny<PimsManagementFileProperty>())).Returns(true);

            // Act
            service.UpdateProperties(managementFile, new List<UserOverrideCode>());

            // Assert
            filePropertyRepository.Verify(x => x.GetPropertiesByManagementFileId(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void UpdateProperties_MatchProperties_Success()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions(Permissions.ManagementEdit, Permissions.PropertyAdd, Permissions.PropertyView);

            var managementFile = EntityHelper.CreateManagementFile();
            managementFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345, regionCode: 1);
            managementFile.PimsManagementFileProperties = new List<PimsManagementFileProperty>() { new PimsManagementFileProperty() { Internal_Id = 1, Property = property } };

            var repository = this._helper.GetService<Mock<IManagementFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(managementFile);

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), true)).Returns(property);
            propertyRepository.Setup(x => x.GetPropertyRegion(It.IsAny<long>())).Returns(1);

            var filePropertyRepository = this._helper.GetService<Mock<IManagementFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByManagementFileId(It.IsAny<long>())).Returns(managementFile.PimsManagementFileProperties.ToList());

            var propertyService = this._helper.GetService<Mock<IPropertyService>>();
            propertyService.Setup(x => x.UpdateLocation(It.IsAny<PimsProperty>(), ref It.Ref<PimsProperty>.IsAny, It.IsAny<IEnumerable<UserOverrideCode>>(), false));
            propertyService.Setup(x => x.UpdateFilePropertyLocation<PimsManagementFileProperty>(It.IsAny<PimsManagementFileProperty>(), It.IsAny<PimsManagementFileProperty>()));
            propertyService.Setup(x => x.UpdateFilePropertyBoundary<PimsManagementFileProperty>(It.IsAny<PimsManagementFileProperty>(), It.IsAny<PimsManagementFileProperty>()));

            var statusMock = this._helper.GetService<Mock<IManagementFileStatusSolver>>();
            statusMock.Setup(x => x.GetCurrentManagementStatus(It.IsAny<string>())).Returns(ManagementFileStatusTypes.ACTIVE);
            statusMock.Setup(x => x.CanEditProperties(It.IsAny<ManagementFileStatusTypes>())).Returns(true);

            var locationSolver = this._helper.GetService<Mock<IFilePropertyLocationUpdateSolver>>();
            locationSolver.Setup(x => x.CanEditFilePropertyLocation(It.IsAny<PimsManagementFileProperty>(), It.IsAny<PimsManagementFileProperty>())).Returns(true);
            locationSolver.Setup(x => x.CanEditFilePropertyBoundary(It.IsAny<PimsManagementFileProperty>(), It.IsAny<PimsManagementFileProperty>())).Returns(true);

            var propertyActivityRepository = this._helper.GetService<Mock<IManagementActivityRepository>>();
            propertyActivityRepository.Setup(x => x.GetActivitiesByManagementFile(It.IsAny<long>())).Returns(new List<PimsManagementActivity>());

            // Act
            service.UpdateProperties(managementFile, new List<UserOverrideCode>() { UserOverrideCode.AddLocationToProperty });

            // Assert
            filePropertyRepository.Verify(x => x.GetPropertiesByManagementFileId(It.IsAny<long>()), Times.Once);
            filePropertyRepository.Verify(x => x.Update(It.IsAny<PimsManagementFileProperty>()), Times.Once);
            propertyService.Verify(x => x.UpdateLocation(It.IsAny<PimsProperty>(), ref It.Ref<PimsProperty>.IsAny, It.IsAny<IEnumerable<UserOverrideCode>>(), false), Times.Once);
            propertyService.Verify(x => x.UpdateFilePropertyLocation<PimsManagementFileProperty>(It.IsAny<PimsManagementFileProperty>(), It.IsAny<PimsManagementFileProperty>()), Times.Once);
            propertyService.Verify(x => x.UpdateFilePropertyBoundary<PimsManagementFileProperty>(It.IsAny<PimsManagementFileProperty>(), It.IsAny<PimsManagementFileProperty>()), Times.Once);
        }

        [Fact]
        public void UpdateProperties_MatchProperties_Success_NoInternalId()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions(Permissions.ManagementEdit, Permissions.PropertyAdd, Permissions.PropertyView);

            var managementFile = EntityHelper.CreateManagementFile();
            managementFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(1, regionCode: 1);
            managementFile.PimsManagementFileProperties = new List<PimsManagementFileProperty>() { new PimsManagementFileProperty() { Internal_Id = 0, Property = property, PropertyId = 1 } };
            var managementFileProperties = new List<PimsManagementFileProperty>() { new PimsManagementFileProperty() { Internal_Id = 1, Property = property, PropertyId = 1 } };

            var repository = this._helper.GetService<Mock<IManagementFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(managementFile);

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), true)).Returns(property);
            propertyRepository.Setup(x => x.GetPropertyRegion(It.IsAny<long>())).Returns(1);

            var filePropertyRepository = this._helper.GetService<Mock<IManagementFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByManagementFileId(It.IsAny<long>())).Returns(managementFileProperties);

            var propertyService = this._helper.GetService<Mock<IPropertyService>>();
            propertyService.Setup(x => x.UpdateLocation(It.IsAny<PimsProperty>(), ref It.Ref<PimsProperty>.IsAny, It.IsAny<IEnumerable<UserOverrideCode>>(), false));
            propertyService.Setup(x => x.UpdateFilePropertyLocation<PimsManagementFileProperty>(It.IsAny<PimsManagementFileProperty>(), It.IsAny<PimsManagementFileProperty>()));
            propertyService.Setup(x => x.UpdateFilePropertyBoundary<PimsManagementFileProperty>(It.IsAny<PimsManagementFileProperty>(), It.IsAny<PimsManagementFileProperty>()));

            var statusMock = this._helper.GetService<Mock<IManagementFileStatusSolver>>();
            statusMock.Setup(x => x.GetCurrentManagementStatus(It.IsAny<string>())).Returns(ManagementFileStatusTypes.ACTIVE);
            statusMock.Setup(x => x.CanEditProperties(It.IsAny<ManagementFileStatusTypes>())).Returns(true);

            var locationSolver = this._helper.GetService<Mock<IFilePropertyLocationUpdateSolver>>();
            locationSolver.Setup(x => x.CanEditFilePropertyLocation(It.IsAny<PimsManagementFileProperty>(), It.IsAny<PimsManagementFileProperty>())).Returns(true);
            locationSolver.Setup(x => x.CanEditFilePropertyBoundary(It.IsAny<PimsManagementFileProperty>(), It.IsAny<PimsManagementFileProperty>())).Returns(true);

            var propertyActivityRepository = this._helper.GetService<Mock<IManagementActivityRepository>>();
            propertyActivityRepository.Setup(x => x.GetActivitiesByManagementFile(It.IsAny<long>())).Returns(new List<PimsManagementActivity>());

            // Act
            var updatedManagementFile = service.UpdateProperties(managementFile, new List<UserOverrideCode>() { UserOverrideCode.AddLocationToProperty });

            // Assert
            var updatedProperty = updatedManagementFile.PimsManagementFileProperties.FirstOrDefault().Internal_Id.Should().Be(1);
        }

        [Fact]
        public void UpdateProperties_MatchProperties_NewProperty_UserOverride()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions(Permissions.ManagementEdit, Permissions.PropertyAdd, Permissions.PropertyView);

            var managementFile = EntityHelper.CreateManagementFile();
            managementFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345, regionCode: 1);
            managementFile.PimsManagementFileProperties = new List<PimsManagementFileProperty>() { new PimsManagementFileProperty() { Property = property } };

            PimsManagementFileProperty updatedManagementFileProperty = null;

            var repository = this._helper.GetService<Mock<IManagementFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(managementFile);
            repository.Setup(x => x.GetByName(It.IsAny<string>())).Returns(managementFile);

            var filePropertyRepository = this._helper.GetService<Mock<IManagementFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByManagementFileId(It.IsAny<long>())).Returns(managementFile.PimsManagementFileProperties.ToList());
            filePropertyRepository.Setup(x => x.Add(It.IsAny<PimsManagementFileProperty>())).Callback<PimsManagementFileProperty>(x => updatedManagementFileProperty = x).Returns(managementFile.PimsManagementFileProperties.FirstOrDefault());

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), true)).Throws<KeyNotFoundException>();

            var propertyService = this._helper.GetService<Mock<IPropertyService>>();
            propertyService.Setup(x => x.PopulateNewProperty(It.IsAny<PimsProperty>(), It.IsAny<Boolean>(), It.IsAny<Boolean>())).Returns(new PimsProperty()
            {
                PropertyDataSourceEffectiveDate = DateOnly.FromDateTime(System.DateTime.Now),
                PropertyDataSourceTypeCode = "PMBC",
                PropertyTypeCode = "UNKNOWN",
                PropertyStatusTypeCode = "UNKNOWN",
                SurplusDeclarationTypeCode = "UNKNOWN",
                RegionCode = 1
            });
            propertyService.Setup(x => x.PopulateNewFileProperty(It.IsAny<PimsManagementFileProperty>())).Returns<PimsManagementFileProperty>(x => x);

            var statusMock = this._helper.GetService<Mock<IManagementFileStatusSolver>>();
            statusMock.Setup(x => x.GetCurrentManagementStatus(It.IsAny<string>())).Returns(ManagementFileStatusTypes.ACTIVE);
            statusMock.Setup(x => x.CanEditProperties(It.IsAny<ManagementFileStatusTypes>())).Returns(true);

            var locationSolver = this._helper.GetService<Mock<IFilePropertyLocationUpdateSolver>>();
            locationSolver.Setup(x => x.CanEditFilePropertyLocation(It.IsAny<PimsManagementFileProperty>(), It.IsAny<PimsManagementFileProperty>())).Returns(true);
            locationSolver.Setup(x => x.CanEditFilePropertyBoundary(It.IsAny<PimsManagementFileProperty>(), It.IsAny<PimsManagementFileProperty>())).Returns(true);

            var propertyActivityRepository = this._helper.GetService<Mock<IManagementActivityRepository>>();
            propertyActivityRepository.Setup(x => x.GetActivitiesByManagementFile(It.IsAny<long>())).Returns(new List<PimsManagementActivity>());

            // Act
            Action act = () => service.UpdateProperties(managementFile, new List<UserOverrideCode>());

            // Assert
            var exception = act.Should().Throw<UserOverrideException>();
            exception.WithMessage("You have added one or more properties to the management file that are not in the MOTT Inventory. To acquire these properties, add them to a management file. Do you want to proceed?");
        }

        [Fact]
        public void UpdateProperties_MatchProperties_NewProperty_Success()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions(Permissions.ManagementEdit, Permissions.PropertyAdd, Permissions.PropertyView);

            var managementFile = EntityHelper.CreateManagementFile();
            managementFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345, regionCode: 1);
            managementFile.PimsManagementFileProperties = new List<PimsManagementFileProperty>() { new PimsManagementFileProperty() { Property = property } };

            PimsManagementFileProperty updatedManagementFileProperty = null;

            var repository = this._helper.GetService<Mock<IManagementFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(managementFile);

            var filePropertyRepository = this._helper.GetService<Mock<IManagementFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByManagementFileId(It.IsAny<long>())).Returns(managementFile.PimsManagementFileProperties.ToList());
            filePropertyRepository.Setup(x => x.Add(It.IsAny<PimsManagementFileProperty>())).Callback<PimsManagementFileProperty>(x => updatedManagementFileProperty = x).Returns(managementFile.PimsManagementFileProperties.FirstOrDefault());

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), true)).Throws<KeyNotFoundException>();

            var propertyService = this._helper.GetService<Mock<IPropertyService>>();
            propertyService.Setup(x => x.PopulateNewProperty(It.IsAny<PimsProperty>(), It.IsAny<Boolean>(), It.IsAny<Boolean>())).Returns(new PimsProperty()
            {
                PropertyDataSourceEffectiveDate = DateOnly.FromDateTime(System.DateTime.Now),
                PropertyDataSourceTypeCode = "PMBC",
                PropertyTypeCode = "UNKNOWN",
                PropertyStatusTypeCode = "UNKNOWN",
                SurplusDeclarationTypeCode = "UNKNOWN",
                RegionCode = 1
            });
            propertyService.Setup(x => x.PopulateNewFileProperty(It.IsAny<PimsManagementFileProperty>())).Returns<PimsManagementFileProperty>(x => x);

            var statusMock = this._helper.GetService<Mock<IManagementFileStatusSolver>>();
            statusMock.Setup(x => x.GetCurrentManagementStatus(It.IsAny<string>())).Returns(ManagementFileStatusTypes.ACTIVE);
            statusMock.Setup(x => x.CanEditProperties(It.IsAny<ManagementFileStatusTypes>())).Returns(true);

            var locationSolver = this._helper.GetService<Mock<IFilePropertyLocationUpdateSolver>>();
            locationSolver.Setup(x => x.CanEditFilePropertyLocation(It.IsAny<PimsManagementFileProperty>(), It.IsAny<PimsManagementFileProperty>())).Returns(true);
            locationSolver.Setup(x => x.CanEditFilePropertyBoundary(It.IsAny<PimsManagementFileProperty>(), It.IsAny<PimsManagementFileProperty>())).Returns(true);

            var propertyActivityRepository = this._helper.GetService<Mock<IManagementActivityRepository>>();
            propertyActivityRepository.Setup(x => x.GetActivitiesByManagementFile(It.IsAny<long>())).Returns(new List<PimsManagementActivity>());

            // Act
            service.UpdateProperties(managementFile, new List<UserOverrideCode>() { UserOverrideCode.ManagingPropertyNotInventoried });

            // Assert
            // since this is a new property, the following default fields should be set.
            var updatedProperty = updatedManagementFileProperty.Property;
            updatedProperty.PropertyTypeCode.Should().Be("UNKNOWN");
            updatedProperty.PropertyStatusTypeCode.Should().Be("UNKNOWN");
            updatedProperty.SurplusDeclarationTypeCode.Should().Be("UNKNOWN");
            updatedProperty.PropertyDataSourceEffectiveDate.Should().Be(DateOnly.FromDateTime(DateTime.Now));
            updatedProperty.PropertyDataSourceTypeCode.Should().Be("PMBC");
            updatedProperty.IsOwned.Should().Be(false);

            filePropertyRepository.Verify(x => x.GetPropertiesByManagementFileId(It.IsAny<long>()), Times.Once);
            filePropertyRepository.Verify(x => x.Add(It.IsAny<PimsManagementFileProperty>()), Times.Once);
            propertyService.Verify(x => x.PopulateNewProperty(It.IsAny<PimsProperty>(), It.IsAny<Boolean>(), It.IsAny<Boolean>()), Times.Once);
            propertyService.Verify(x => x.PopulateNewFileProperty(It.IsAny<PimsManagementFileProperty>()), Times.Once);
        }

        [Fact]
        public void UpdateProperties_UpdatePropertyFile_Success()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions(Permissions.ManagementEdit, Permissions.PropertyAdd, Permissions.PropertyView);

            var property = EntityHelper.CreateProperty(12345, regionCode: 1);

            var managementFile = EntityHelper.CreateManagementFile();
            managementFile.PimsManagementFileProperties = new List<PimsManagementFileProperty>() { new PimsManagementFileProperty() { Internal_Id = 1, Property = property } };
            managementFile.ConcurrencyControlNumber = 1;

            var repository = this._helper.GetService<Mock<IManagementFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(managementFile);

            var filePropertyRepository = this._helper.GetService<Mock<IManagementFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByManagementFileId(It.IsAny<long>())).Returns(new List<PimsManagementFileProperty>() { new PimsManagementFileProperty() { Internal_Id = 1, Property = property, PropertyName = "updated" } });

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), true)).Throws<KeyNotFoundException>();
            propertyRepository.Setup(x => x.GetPropertyRegion(It.IsAny<long>())).Returns(1);

            var propertyActivityRepository = this._helper.GetService<Mock<IManagementActivityRepository>>();
            propertyActivityRepository.Setup(x => x.GetActivitiesByManagementFile(It.IsAny<long>())).Returns(new List<PimsManagementActivity>());

            var propertyService = this._helper.GetService<Mock<IPropertyService>>();
            propertyService.Setup(x => x.UpdateFilePropertyLocation<PimsManagementFileProperty>(It.IsAny<PimsManagementFileProperty>(), It.IsAny<PimsManagementFileProperty>()));
            propertyService.Setup(x => x.UpdateFilePropertyBoundary<PimsManagementFileProperty>(It.IsAny<PimsManagementFileProperty>(), It.IsAny<PimsManagementFileProperty>()));

            var statusMock = this._helper.GetService<Mock<IManagementFileStatusSolver>>();
            statusMock.Setup(x => x.GetCurrentManagementStatus(It.IsAny<string>())).Returns(ManagementFileStatusTypes.ACTIVE);
            statusMock.Setup(x => x.CanEditProperties(It.IsAny<ManagementFileStatusTypes>())).Returns(true);

            var locationSolver = this._helper.GetService<Mock<IFilePropertyLocationUpdateSolver>>();
            locationSolver.Setup(x => x.CanEditFilePropertyLocation(It.IsAny<PimsManagementFileProperty>(), It.IsAny<PimsManagementFileProperty>())).Returns(true);
            locationSolver.Setup(x => x.CanEditFilePropertyBoundary(It.IsAny<PimsManagementFileProperty>(), It.IsAny<PimsManagementFileProperty>())).Returns(true);

            // Act
            service.UpdateProperties(managementFile, new List<UserOverrideCode>() { UserOverrideCode.ManagingPropertyNotInventoried });

            // Assert
            filePropertyRepository.Verify(x => x.Update(It.IsAny<PimsManagementFileProperty>()), Times.Once);
            propertyService.Verify(x => x.UpdateFilePropertyLocation<PimsManagementFileProperty>(It.IsAny<PimsManagementFileProperty>(), It.IsAny<PimsManagementFileProperty>()), Times.Once);
            propertyService.Verify(x => x.UpdateFilePropertyBoundary<PimsManagementFileProperty>(It.IsAny<PimsManagementFileProperty>(), It.IsAny<PimsManagementFileProperty>()), Times.Once);
        }

        [Fact]
        public void UpdateProperties_RemovePropertyFile_Success()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions(Permissions.ManagementEdit, Permissions.PropertyAdd, Permissions.PropertyView);

            var managementFile = EntityHelper.CreateManagementFile();
            var property = EntityHelper.CreateProperty(12345);

            var repository = this._helper.GetService<Mock<IManagementFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(managementFile);

            var filePropertyRepository = this._helper.GetService<Mock<IManagementFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByManagementFileId(It.IsAny<long>())).Returns(new List<PimsManagementFileProperty>() { new PimsManagementFileProperty() { Internal_Id = 1, Property = property } });

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), false)).Returns(property);
            propertyRepository.Setup(x => x.GetAllAssociationsCountById(It.IsAny<long>())).Returns(3);

            var propertyOperationService = this._helper.GetService<Mock<IPropertyOperationService>>();
            propertyOperationService.Setup(x => x.GetOperationsForProperty(It.IsAny<long>())).Returns(new List<PimsPropertyOperation>());

            var propertyActivityRepository = this._helper.GetService<Mock<IManagementActivityRepository>>();
            propertyActivityRepository.Setup(x => x.GetActivitiesByManagementFile(It.IsAny<long>())).Returns(new List<PimsManagementActivity>());

            var statusMock = this._helper.GetService<Mock<IManagementFileStatusSolver>>();
            statusMock.Setup(x => x.GetCurrentManagementStatus(It.IsAny<string>())).Returns(ManagementFileStatusTypes.ACTIVE);
            statusMock.Setup(x => x.CanEditProperties(It.IsAny<ManagementFileStatusTypes?>())).Returns(true);

            var locationSolver = this._helper.GetService<Mock<IFilePropertyLocationUpdateSolver>>();
            locationSolver.Setup(x => x.CanEditFilePropertyLocation(It.IsAny<PimsManagementFileProperty>(), It.IsAny<PimsManagementFileProperty>())).Returns(true);
            locationSolver.Setup(x => x.CanEditFilePropertyBoundary(It.IsAny<PimsManagementFileProperty>(), It.IsAny<PimsManagementFileProperty>())).Returns(true);

            // Act
            service.UpdateProperties(managementFile, new List<UserOverrideCode>());

            // Assert
            filePropertyRepository.Verify(x => x.Delete(It.IsAny<PimsManagementFileProperty>()), Times.Once);
            propertyRepository.Verify(x => x.Delete(property), Times.Never);
        }

        [Fact]
        public void UpdateProperties_RemoveProperty_Success()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions(Permissions.ManagementEdit, Permissions.PropertyAdd, Permissions.PropertyView);

            var managementFile = EntityHelper.CreateManagementFile(1);

            var deletedProperty = EntityHelper.CreateProperty(12345);
            deletedProperty.PimsPropertyResearchFiles = new List<PimsPropertyResearchFile>();
            deletedProperty.PimsPropertyLeases = new List<PimsPropertyLease>();
            deletedProperty.PimsManagementFileProperties = new List<PimsManagementFileProperty>() { new PimsManagementFileProperty() };

            var updatedManagementFile = EntityHelper.CreateManagementFile(1);
            updatedManagementFile.PimsManagementFileProperties.Clear();

            var repository = this._helper.GetService<Mock<IManagementFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(managementFile);

            var filePropertyRepository = this._helper.GetService<Mock<IManagementFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByManagementFileId(It.IsAny<long>())).Returns(new List<PimsManagementFileProperty>() { new PimsManagementFileProperty() { Property = deletedProperty } });
            filePropertyRepository.Setup(x => x.GetManagementFilePropertyRelatedCount(It.IsAny<long>())).Returns(1);

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), false)).Returns(deletedProperty);
            propertyRepository.Setup(x => x.GetAllAssociationsById(It.IsAny<long>())).Returns(deletedProperty);
            propertyRepository.Setup(x => x.GetAllAssociationsCountById(It.IsAny<long>())).Returns(1);

            var propertyOperationService = this._helper.GetService<Mock<IPropertyOperationService>>();
            propertyOperationService.Setup(x => x.GetOperationsForProperty(It.IsAny<long>())).Returns(new List<PimsPropertyOperation>());

            var propertyActivityRepository = this._helper.GetService<Mock<IManagementActivityRepository>>();
            propertyActivityRepository.Setup(x => x.GetActivitiesByManagementFile(It.IsAny<long>())).Returns(new List<PimsManagementActivity>());

            var statusMock = this._helper.GetService<Mock<IManagementFileStatusSolver>>();
            statusMock.Setup(x => x.GetCurrentManagementStatus(It.IsAny<string>())).Returns(ManagementFileStatusTypes.ACTIVE);
            statusMock.Setup(x => x.CanEditProperties(It.IsAny<ManagementFileStatusTypes>())).Returns(true);

            var locationSolver = this._helper.GetService<Mock<IFilePropertyLocationUpdateSolver>>();
            locationSolver.Setup(x => x.CanEditFilePropertyLocation(It.IsAny<PimsManagementFileProperty>(), It.IsAny<PimsManagementFileProperty>())).Returns(true);
            locationSolver.Setup(x => x.CanEditFilePropertyBoundary(It.IsAny<PimsManagementFileProperty>(), It.IsAny<PimsManagementFileProperty>())).Returns(true);

            // Act
            service.UpdateProperties(updatedManagementFile, new List<UserOverrideCode>());

            // Assert
            filePropertyRepository.Verify(x => x.Delete(It.IsAny<PimsManagementFileProperty>()), Times.Once);
            propertyRepository.Verify(x => x.Delete(It.IsAny<PimsProperty>()), Times.Once);
        }

        [Fact]
        public void UpdateProperties_RemoveProperty_Fails_PropertyIsSubdividedOrConsolidated()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions(Permissions.ManagementEdit, Permissions.PropertyAdd, Permissions.PropertyView);

            var managementFile = EntityHelper.CreateManagementFile(1);

            var deletedProperty = EntityHelper.CreateProperty(12345);
            deletedProperty.PimsPropertyResearchFiles = new List<PimsPropertyResearchFile>();
            deletedProperty.PimsPropertyLeases = new List<PimsPropertyLease>();
            deletedProperty.PimsManagementFileProperties = new List<PimsManagementFileProperty>() { new PimsManagementFileProperty() };

            var updatedManagementFile = EntityHelper.CreateManagementFile(1);
            updatedManagementFile.PimsManagementFileProperties.Clear();

            var repository = this._helper.GetService<Mock<IManagementFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(managementFile);

            var filePropertyRepository = this._helper.GetService<Mock<IManagementFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByManagementFileId(It.IsAny<long>())).Returns(new List<PimsManagementFileProperty>() { new PimsManagementFileProperty() { Property = deletedProperty } });
            filePropertyRepository.Setup(x => x.GetManagementFilePropertyRelatedCount(It.IsAny<long>())).Returns(1);

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), false)).Returns(deletedProperty);
            propertyRepository.Setup(x => x.GetAllAssociationsById(It.IsAny<long>())).Returns(deletedProperty);
            propertyRepository.Setup(x => x.GetAllAssociationsCountById(It.IsAny<long>())).Returns(1);

            var propertyOperationService = this._helper.GetService<Mock<IPropertyOperationService>>();
            propertyOperationService.Setup(x => x.GetOperationsForProperty(It.IsAny<long>())).Returns(new List<PimsPropertyOperation>() { new() { Internal_Id = 1, SourcePropertyId = deletedProperty.Internal_Id, SourceProperty = deletedProperty } });

            var propertyActivityRepository = this._helper.GetService<Mock<IManagementActivityRepository>>();
            propertyActivityRepository.Setup(x => x.GetActivitiesByManagementFile(It.IsAny<long>())).Returns(new List<PimsManagementActivity>());

            var statusMock = this._helper.GetService<Mock<IManagementFileStatusSolver>>();
            statusMock.Setup(x => x.GetCurrentManagementStatus(It.IsAny<string>())).Returns(ManagementFileStatusTypes.ACTIVE);
            statusMock.Setup(x => x.CanEditProperties(It.IsAny<ManagementFileStatusTypes?>())).Returns(true);

            var locationSolver = this._helper.GetService<Mock<IFilePropertyLocationUpdateSolver>>();
            locationSolver.Setup(x => x.CanEditFilePropertyLocation(It.IsAny<PimsManagementFileProperty>(), It.IsAny<PimsManagementFileProperty>())).Returns(true);
            locationSolver.Setup(x => x.CanEditFilePropertyBoundary(It.IsAny<PimsManagementFileProperty>(), It.IsAny<PimsManagementFileProperty>())).Returns(true);

            // Act
            Action act = () => service.UpdateProperties(updatedManagementFile, new List<UserOverrideCode>());

            // Assert
            act.Should().Throw<BusinessRuleViolationException>().WithMessage("This property cannot be deleted because it is part of a subdivision or consolidation");
        }

        [Fact]
        public void UpdateProperties_NoPermission()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions();

            var managementFile = EntityHelper.CreateManagementFile();

            var repository = this._helper.GetService<Mock<IManagementFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(managementFile);

            // Act
            Action act = () => service.UpdateProperties(managementFile, new List<UserOverrideCode>());

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.GetById(It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public void UpdateProperties_ValidatePropertyRegions_Success()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions(Permissions.ManagementEdit, Permissions.PropertyAdd, Permissions.PropertyView);

            var managementFile = EntityHelper.CreateManagementFile();
            managementFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345, regionCode: 1);
            managementFile.PimsManagementFileProperties = new List<PimsManagementFileProperty>() { new PimsManagementFileProperty() { Property = property } };

            var repository = this._helper.GetService<Mock<IManagementFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(managementFile);

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), true)).Returns(property);
            propertyRepository.Setup(x => x.GetPropertyRegion(It.IsAny<long>())).Returns(1);

            var filePropertyRepository = this._helper.GetService<Mock<IManagementFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByManagementFileId(It.IsAny<long>())).Returns(managementFile.PimsManagementFileProperties.ToList());

            var propertyActivityRepository = this._helper.GetService<Mock<IManagementActivityRepository>>();
            propertyActivityRepository.Setup(x => x.GetActivitiesByManagementFile(It.IsAny<long>())).Returns(new List<PimsManagementActivity>());

            var statusMock = this._helper.GetService<Mock<IManagementFileStatusSolver>>();
            statusMock.Setup(x => x.GetCurrentManagementStatus(It.IsAny<string>())).Returns(ManagementFileStatusTypes.ACTIVE);
            statusMock.Setup(x => x.CanEditProperties(It.IsAny<ManagementFileStatusTypes>())).Returns(true);

            var locationSolver = this._helper.GetService<Mock<IFilePropertyLocationUpdateSolver>>();
            locationSolver.Setup(x => x.CanEditFilePropertyLocation(It.IsAny<PimsManagementFileProperty>(), It.IsAny<PimsManagementFileProperty>())).Returns(true);
            locationSolver.Setup(x => x.CanEditFilePropertyBoundary(It.IsAny<PimsManagementFileProperty>(), It.IsAny<PimsManagementFileProperty>())).Returns(true);

            // Act
            service.UpdateProperties(managementFile, new List<UserOverrideCode>());

            // Assert
            filePropertyRepository.Verify(x => x.GetPropertiesByManagementFileId(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void UpdateProperties_DisableProperties_AddsNote()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions(Permissions.ManagementEdit, Permissions.PropertyAdd, Permissions.PropertyView);

            var managementFile = EntityHelper.CreateManagementFile();
            managementFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345, regionCode: 1);
            managementFile.PimsManagementFileProperties = new List<PimsManagementFileProperty>() { new PimsManagementFileProperty() { Property = property, Internal_Id = 1, IsActive = true } };

            var repository = this._helper.GetService<Mock<IManagementFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(managementFile);

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), true)).Returns(property);
            propertyRepository.Setup(x => x.GetPropertyRegion(It.IsAny<long>())).Returns(1);

            var filePropertyRepository = this._helper.GetService<Mock<IManagementFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByManagementFileId(It.IsAny<long>())).Returns(new List<PimsManagementFileProperty>() { new PimsManagementFileProperty() { Property = property, Internal_Id = 1, IsActive = false } });

            var propertyActivityRepository = this._helper.GetService<Mock<IManagementActivityRepository>>();
            propertyActivityRepository.Setup(x => x.GetActivitiesByManagementFile(It.IsAny<long>())).Returns(new List<PimsManagementActivity>());

            var statusMock = this._helper.GetService<Mock<IManagementFileStatusSolver>>();
            statusMock.Setup(x => x.GetCurrentManagementStatus(It.IsAny<string>())).Returns(ManagementFileStatusTypes.ACTIVE);
            statusMock.Setup(x => x.CanEditProperties(It.IsAny<ManagementFileStatusTypes?>())).Returns(true);

            List<PimsManagementFileNote> note = new List<PimsManagementFileNote>();
            var entityNoteRepository = this._helper.GetService<Mock<INoteRelationshipRepository<PimsManagementFileNote>>>();
            entityNoteRepository.Setup(x => x.AddNoteRelationship(Capture.In<PimsManagementFileNote>(note))).Returns(new PimsManagementFileNote());

            var locationSolver = this._helper.GetService<Mock<IFilePropertyLocationUpdateSolver>>();
            locationSolver.Setup(x => x.CanEditFilePropertyLocation(It.IsAny<PimsManagementFileProperty>(), It.IsAny<PimsManagementFileProperty>())).Returns(true);
            locationSolver.Setup(x => x.CanEditFilePropertyBoundary(It.IsAny<PimsManagementFileProperty>(), It.IsAny<PimsManagementFileProperty>())).Returns(true);

            // Act
            service.UpdateProperties(managementFile, new List<UserOverrideCode>());

            // Assert
            entityNoteRepository.Verify(x => x.AddNoteRelationship(It.IsAny<PimsManagementFileNote>()), Times.Once);
            note.FirstOrDefault().Note.NoteTxt.Should().Be("Management File property 000-012-345 Enabled");
        }

        [Fact]
        public void UpdateProperties_FinalFile_Error()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions(Permissions.ManagementEdit, Permissions.PropertyAdd, Permissions.PropertyView);

            var managementFile = EntityHelper.CreateManagementFile();
            managementFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345, regionCode: 1);
            managementFile.PimsManagementFileProperties = new List<PimsManagementFileProperty>() { new PimsManagementFileProperty() { Property = property } };

            var repository = this._helper.GetService<Mock<IManagementFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(managementFile);

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), true)).Returns(property);
            propertyRepository.Setup(x => x.GetPropertyRegion(It.IsAny<long>())).Returns(1);

            var filePropertyRepository = this._helper.GetService<Mock<IManagementFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByManagementFileId(It.IsAny<long>())).Returns(managementFile.PimsManagementFileProperties.ToList());

            var statusMock = this._helper.GetService<Mock<IManagementFileStatusSolver>>();
            statusMock.Setup(x => x.GetCurrentManagementStatus(It.IsAny<string>())).Returns(ManagementFileStatusTypes.ACTIVE);
            statusMock.Setup(x => x.CanEditProperties(It.IsAny<ManagementFileStatusTypes?>())).Returns(false);

            var locationSolver = this._helper.GetService<Mock<IFilePropertyLocationUpdateSolver>>();
            locationSolver.Setup(x => x.CanEditFilePropertyLocation(It.IsAny<PimsManagementFileProperty>(), It.IsAny<PimsManagementFileProperty>())).Returns(true);
            locationSolver.Setup(x => x.CanEditFilePropertyBoundary(It.IsAny<PimsManagementFileProperty>(), It.IsAny<PimsManagementFileProperty>())).Returns(true);

            // Act
            Action act = () => service.UpdateProperties(managementFile, new List<UserOverrideCode>());

            // Assert
            act.Should().Throw<BusinessRuleViolationException>("The file you are editing is not active, so you cannot save changes. Refresh your browser to see file state.");
        }

        [Fact]
        public void UpdateProperties_With_New_RetiredProperty_Should_Fail()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions(Permissions.ManagementEdit, Permissions.PropertyAdd, Permissions.PropertyView);

            var managementFile = EntityHelper.CreateManagementFile();
            managementFile.ConcurrencyControlNumber = 1;

            PimsProperty retiredProperty = new PimsProperty()
            {
                PropertyId = 100,
                Pid = 1000,
                IsRetired = true,
            };

            // Attempt to add the retired property as a new property
            var newProperties = new List<PimsManagementFileProperty>
            {
                new PimsManagementFileProperty() { Property = retiredProperty, Internal_Id = 0 }
            };
            managementFile.PimsManagementFileProperties = newProperties;

            var repository = this._helper.GetService<Mock<IManagementFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(managementFile);

            var filePropertyRepository = this._helper.GetService<Mock<IManagementFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByManagementFileId(It.IsAny<long>())).Returns(managementFile.PimsManagementFileProperties.ToList());

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), true)).Returns(retiredProperty);

            var statusMock = this._helper.GetService<Mock<IManagementFileStatusSolver>>();
            statusMock.Setup(x => x.GetCurrentManagementStatus(It.IsAny<string>())).Returns(ManagementFileStatusTypes.ACTIVE);
            statusMock.Setup(x => x.CanEditProperties(It.IsAny<ManagementFileStatusTypes?>())).Returns(true);

            var locationSolver = this._helper.GetService<Mock<IFilePropertyLocationUpdateSolver>>();
            locationSolver.Setup(x => x.CanEditFilePropertyLocation(It.IsAny<PimsManagementFileProperty>(), It.IsAny<PimsManagementFileProperty>())).Returns(true);
            locationSolver.Setup(x => x.CanEditFilePropertyBoundary(It.IsAny<PimsManagementFileProperty>(), It.IsAny<PimsManagementFileProperty>())).Returns(true);

            // Act
            Action act = () => service.UpdateProperties(managementFile, new List<UserOverrideCode>());

            // Assert
            var ex = act.Should().Throw<BusinessRuleViolationException>();
            ex.WithMessage("New retired property can not be added.");
        }

        public void UpdateProperties_With_Existing_RetiredProperty_Success()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions(Permissions.ManagementEdit, Permissions.PropertyAdd, Permissions.PropertyView);

            var managementFile = EntityHelper.CreateManagementFile();
            managementFile.ConcurrencyControlNumber = 1;

            PimsProperty retiredProperty = new PimsProperty()
            {
                PropertyId = 100,
                Pid = 1000,
                IsRetired = true,
            };

            // Simulate the retired property is already attached to the file
            var existingProperties = new List<PimsManagementFileProperty>
            {
                new PimsManagementFileProperty() { Property = retiredProperty, Internal_Id = 1 }
            };
            managementFile.PimsManagementFileProperties = existingProperties;

            var repository = this._helper.GetService<Mock<IManagementFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(managementFile);

            var filePropertyRepository = this._helper.GetService<Mock<IManagementFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByManagementFileId(It.IsAny<long>())).Returns(managementFile.PimsManagementFileProperties.ToList());

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), true)).Returns(retiredProperty);

            var statusMock = this._helper.GetService<Mock<IManagementFileStatusSolver>>();
            statusMock.Setup(x => x.GetCurrentManagementStatus(It.IsAny<string>())).Returns(ManagementFileStatusTypes.ACTIVE);
            statusMock.Setup(x => x.CanEditProperties(It.IsAny<ManagementFileStatusTypes?>())).Returns(true);

            var locationSolver = this._helper.GetService<Mock<IFilePropertyLocationUpdateSolver>>();
            locationSolver.Setup(x => x.CanEditFilePropertyLocation(It.IsAny<PimsManagementFileProperty>(), It.IsAny<PimsManagementFileProperty>())).Returns(true);
            locationSolver.Setup(x => x.CanEditFilePropertyBoundary(It.IsAny<PimsManagementFileProperty>(), It.IsAny<PimsManagementFileProperty>())).Returns(true);

            // Act
            Action act = () => service.UpdateProperties(managementFile, new List<UserOverrideCode>());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void UpdateProperties_WithManagementPropertyActivity_Should_Fail()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions(Permissions.ManagementEdit, Permissions.PropertyAdd, Permissions.PropertyView);

            var managementFile = EntityHelper.CreateManagementFile();
            managementFile.ConcurrencyControlNumber = 1;

            PimsProperty property = new PimsProperty()
            {
                PropertyId = 100,
                Pid = 1000,
                IsRetired = false,
            };

            PimsManagementActivity managementActivity = new PimsManagementActivity()
            {
                ManagementFileId = managementFile.Internal_Id,
                ManagementFile = managementFile,
                PimsManagementActivityProperties = new List<PimsManagementActivityProperty>()
                {
                    new PimsManagementActivityProperty()
                    {

                        Property = property,
                    }
                }
            };

            var repository = this._helper.GetService<Mock<IManagementFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(managementFile);

            var filePropertyRepository = this._helper.GetService<Mock<IManagementFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByManagementFileId(It.IsAny<long>())).Returns(new List<PimsManagementFileProperty>() { new PimsManagementFileProperty() { Property = property } });

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), true)).Returns(property);

            var propertyActivityRepository = this._helper.GetService<Mock<IManagementActivityRepository>>();
            propertyActivityRepository.Setup(x => x.GetActivitiesByManagementFile(It.IsAny<long>())).Returns(new List<PimsManagementActivity>() { managementActivity });

            var propertyOperationsService = this._helper.GetService<Mock<IPropertyOperationService>>();
            propertyOperationsService.Setup(x => x.GetOperationsForProperty(It.IsAny<long>())).Returns(new List<PimsPropertyOperation>());

            var statusMock = this._helper.GetService<Mock<IManagementFileStatusSolver>>();
            statusMock.Setup(x => x.GetCurrentManagementStatus(It.IsAny<string>())).Returns(ManagementFileStatusTypes.ACTIVE);
            statusMock.Setup(x => x.CanEditProperties(It.IsAny<ManagementFileStatusTypes?>())).Returns(true);

            var locationSolver = this._helper.GetService<Mock<IFilePropertyLocationUpdateSolver>>();
            locationSolver.Setup(x => x.CanEditFilePropertyLocation(It.IsAny<PimsManagementFileProperty>(), It.IsAny<PimsManagementFileProperty>())).Returns(true);
            locationSolver.Setup(x => x.CanEditFilePropertyBoundary(It.IsAny<PimsManagementFileProperty>(), It.IsAny<PimsManagementFileProperty>())).Returns(true);

            // Act
            Action act = () => service.UpdateProperties(managementFile, new List<UserOverrideCode>());

            // Assert
            var ex = act.Should().Throw<BusinessRuleViolationException>();
            ex.WithMessage("This property cannot be deleted as it is part of an activity in this file");
        }
        #endregion

        #region GetTeamMembers
        [Fact]
        public void GetTeamMembers_Success()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions(Permissions.ManagementView, Permissions.ContactView);

            var managementFile = EntityHelper.CreateManagementFile();
            var person = EntityHelper.CreatePerson(1, "tester", "chester");
            var org = EntityHelper.CreateOrganization(1, "tester org");
            List<PimsManagementFileTeam> allTeamMembers = new()
            {
                new() { ManagementFileId = managementFile.Internal_Id, PersonId = person.Internal_Id, Person = person },
                new() { ManagementFileId = managementFile.Internal_Id, OrganizationId = org.Internal_Id, Organization = org }
            };

            var repository = this._helper.GetService<Mock<IManagementFileRepository>>();
            repository.Setup(x => x.GetTeamMembers()).Returns(allTeamMembers);

            // Act
            var result = service.GetTeamMembers();

            // Assert
            repository.Verify(x => x.GetTeamMembers(), Times.Once);
            result.Should().HaveCount(2);
        }

        [Fact]
        public void GetTeamMembers_NoPermission()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions();

            var managementFile = EntityHelper.CreateManagementFile();

            // Act
            Action act = () => service.GetTeamMembers();

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        #endregion

        #region GetPage
        [Fact]
        public void GetPage_Success()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions(Permissions.ManagementView);

            var managementFile = EntityHelper.CreateManagementFile();

            var repository = this._helper.GetService<Mock<IManagementFileRepository>>();
            repository.Setup(x => x.GetPageDeep(It.IsAny<ManagementFilter>())).Returns(new Paged<PimsManagementFile>(new[] { managementFile }));

            // Act
            var result = service.GetPage(new ManagementFilter());

            // Assert
            repository.Verify(x => x.GetPageDeep(It.IsAny<ManagementFilter>()), Times.Once);
        }

        [Fact]
        public void GetPage_NoPermission()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions();

            var managementFile = EntityHelper.CreateManagementFile();

            // Act
            Action act = () => service.GetPage(new ManagementFilter());

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }
        #endregion

        #region GetContacts
        [Fact]
        public void GetContacts_Success()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions(Permissions.ManagementView);

            var managementFile = EntityHelper.CreateManagementFile();
            var person = EntityHelper.CreatePerson(1, "tester", "chester");
            var contact = new PimsManagementFileContact() 
            { 
                ManagementFileContactId = 1,
                ManagementFileId = managementFile.Internal_Id, 
                PersonId = person.Internal_Id, 
                Person = person 
            };

            var repository = this._helper.GetService<Mock<IManagementFileRepository>>();
            repository.Setup(x => x.GetContacts(It.IsAny<long>())).Returns(new List<PimsManagementFileContact> { contact });

            // Act
            var result = service.GetContacts(1);

            // Assert
            repository.Verify(x => x.GetContacts(It.IsAny<long>()), Times.Once);
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetContacts_NoPermission()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions();

            // Act
            Action act = () => service.GetContacts(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }
        #endregion

        #region GetContact
        [Fact]
        public void GetContact_Success()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions(Permissions.ManagementView);

            var managementFile = EntityHelper.CreateManagementFile();
            var person = EntityHelper.CreatePerson(1, "tester", "chester");
            var contact = new PimsManagementFileContact() 
            { 
                ManagementFileContactId = 1,
                ManagementFileId = managementFile.Internal_Id, 
                PersonId = person.Internal_Id, 
                Person = person 
            };

            var repository = this._helper.GetService<Mock<IManagementFileRepository>>();
            repository.Setup(x => x.GetContact(It.IsAny<long>(), It.IsAny<long>())).Returns(contact);

            // Act
            var result = service.GetContact(1, 1);

            // Assert
            repository.Verify(x => x.GetContact(It.IsAny<long>(), It.IsAny<long>()), Times.Once);
            result.Should().NotBeNull();
            result.ManagementFileContactId.Should().Be(1);
        }

        [Fact]
        public void GetContact_NoPermission()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions();

            // Act
            Action act = () => service.GetContact(1, 1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }
        #endregion

        #region AddContact
        [Fact]
        public void AddContact_Success()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions(Permissions.ManagementEdit);

            var managementFile = EntityHelper.CreateManagementFile();
            var person = EntityHelper.CreatePerson(1, "tester", "chester");
            var contact = new PimsManagementFileContact() 
            { 
                ManagementFileId = managementFile.Internal_Id, 
                PersonId = person.Internal_Id, 
                Person = person 
            };

            var repository = this._helper.GetService<Mock<IManagementFileRepository>>();
            repository.Setup(x => x.AddContact(It.IsAny<PimsManagementFileContact>())).Returns(contact);

            var activityRepository = this._helper.GetService<Mock<IManagementActivityRepository>>();

            // Act
            var result = service.AddContact(contact);

            // Assert
            repository.Verify(x => x.AddContact(It.IsAny<PimsManagementFileContact>()), Times.Once);
            activityRepository.Verify(x => x.CommitTransaction(), Times.Once);
            result.Should().NotBeNull();
        }

        [Fact]
        public void AddContact_NoPermission()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions();
            var contact = new PimsManagementFileContact() { ManagementFileId = 1 };

            // Act
            Action act = () => service.AddContact(contact);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }
        #endregion

        #region UpdateContact
        [Fact]
        public void UpdateContact_Success()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions(Permissions.ManagementEdit);

            var managementFile = EntityHelper.CreateManagementFile();
            var person = EntityHelper.CreatePerson(1, "tester", "chester");
            var contact = new PimsManagementFileContact() 
            { 
                ManagementFileContactId = 1,
                ManagementFileId = managementFile.Internal_Id, 
                PersonId = person.Internal_Id, 
                Person = person 
            };

            var repository = this._helper.GetService<Mock<IManagementFileRepository>>();
            repository.Setup(x => x.UpdateContact(It.IsAny<PimsManagementFileContact>())).Returns(contact);

            var activityRepository = this._helper.GetService<Mock<IManagementActivityRepository>>();

            // Act
            var result = service.UpdateContact(contact);

            // Assert
            repository.Verify(x => x.UpdateContact(It.IsAny<PimsManagementFileContact>()), Times.Once);
            activityRepository.Verify(x => x.CommitTransaction(), Times.Once);
            result.Should().NotBeNull();
            result.ManagementFileContactId.Should().Be(1);
        }

        [Fact]
        public void UpdateContact_NoPermission()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions();
            var contact = new PimsManagementFileContact() { ManagementFileContactId = 1 };

            // Act
            Action act = () => service.UpdateContact(contact);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }
        #endregion

        #region DeleteContact
        [Fact]
        public void DeleteContact_Success()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions(Permissions.ManagementEdit);

            var repository = this._helper.GetService<Mock<IManagementFileRepository>>();
            repository.Setup(x => x.DeleteContact(It.IsAny<long>(), It.IsAny<long>()));

            var activityRepository = this._helper.GetService<Mock<IManagementActivityRepository>>();

            // Act
            var result = service.DeleteContact(1, 1);

            // Assert
            repository.Verify(x => x.DeleteContact(It.IsAny<long>(), It.IsAny<long>()), Times.Once);
            activityRepository.Verify(x => x.CommitTransaction(), Times.Once);
            result.Should().BeTrue();
        }

        [Fact]
        public void DeleteContact_NoPermission()
        {
            // Arrange
            var service = this.CreateManagementServiceWithPermissions();

            // Act
            Action act = () => service.DeleteContact(1, 1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }
        #endregion
    }
}
