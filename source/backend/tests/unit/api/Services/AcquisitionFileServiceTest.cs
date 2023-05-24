using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DocumentFormat.OpenXml.Office2010.Excel;
using FluentAssertions;
using MapsterMapper;
using Moq;
using NetTopologySuite.Geometries;
using Pims.Api.Constants;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Models.Concepts;
using Pims.Api.Services;
using Pims.Core.Exceptions;
using Pims.Core.Test;
using Pims.Dal;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Xunit;

namespace Pims.Api.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "acquisition")]
    [ExcludeFromCodeCoverage]
    public class AcquisitionFileServiceTest
    {
        #region Tests
        private readonly TestHelper _helper;

        public AcquisitionFileServiceTest()
        {
            _helper = new TestHelper();
        }

        private AcquisitionFileService CreateAcquisitionServiceWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            return _helper.Create<AcquisitionFileService>(user);
        }

        #region Add
        [Fact]
        public void Add_Success()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileAdd);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.Add(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);
            var lookupRepository = _helper.GetService<Mock<ILookupRepository>>();
            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });

            // Act
            var result = service.Add(acqFile);

            // Assert
            repository.Verify(x => x.Add(It.IsAny<PimsAcquisitionFile>()), Times.Once);
        }

        [Fact]
        public void Add_CannotDetermineRegion()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileAdd);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.RegionCode = 4;

            var repository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.Add(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);

            var lookupRepository = _helper.GetService<Mock<ILookupRepository>>();
            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });

            // Act
            Action act = () => service.Add(acqFile);

            // Assert
            act.Should().Throw<BadRequestException>();
        }

        [Fact]
        public void Add_NoPermission()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions();

            var acqFile = EntityHelper.CreateAcquisitionFile();

            // Act
            Action act = () => service.Add(acqFile);

            var repository = _helper.GetService<Mock<IAcquisitionFileRepository>>();

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.Add(It.IsAny<PimsAcquisitionFile>()), Times.Never);
        }

        [Fact]
        public void Add_ThrowIfNull()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileAdd);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = _helper.GetService<Mock<IAcquisitionFileRepository>>();

            // Act
            Action act = () => service.Add(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
            repository.Verify(x => x.Add(It.IsAny<PimsAcquisitionFile>()), Times.Never);
        }

        [Fact]
        public void Add_DuplicateTeam()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileAdd);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.PimsAcquisitionFilePeople.Add(new PimsAcquisitionFilePerson() { PersonId = 1, AcqFlPersonProfileTypeCode = "test" });
            acqFile.PimsAcquisitionFilePeople.Add(new PimsAcquisitionFilePerson() { PersonId = 1, AcqFlPersonProfileTypeCode = "test" });

            var repository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
            var lookupRepository = _helper.GetService<Mock<ILookupRepository>>();
            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });

            // Act
            Action act = () => service.Add(acqFile);

            // Assert
            act.Should().Throw<BadRequestException>();
            repository.Verify(x => x.Add(It.IsAny<PimsAcquisitionFile>()), Times.Never);
        }
        #endregion

        #region GetById
        [Fact]
        public void GetById_Success()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileView);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
            var userRepository = _helper.GetService<Mock<IUserRepository>>();
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            var result = service.GetById(1);

            // Assert
            repository.Verify(x => x.GetById(It.IsAny<long>()), Times.Exactly(2));
        }

        [Fact]
        public void GetById_NoPermission_IsContractor()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileView);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
            var userRepository = _helper.GetService<Mock<IUserRepository>>();
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var contractorUser = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: true);
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(contractorUser);

            // Act
            Action act = () => service.GetById(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void GetById_NoPermission()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions();

            var acqFile = EntityHelper.CreateAcquisitionFile();

            // Act
            Action act = () => service.GetById(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }
        #endregion

        #region Update
        [Fact]
        public void Update_Success()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.ConcurrencyControlNumber = 1;

            var repository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.Update(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var lookupRepository = _helper.GetService<Mock<ILookupRepository>>();
            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });
            var userRepository = _helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            var result = service.Update(acqFile, true, true);

            // Assert
            repository.Verify(x => x.Update(It.IsAny<PimsAcquisitionFile>()), Times.Once);
        }

        [Fact]
        public void Update_NotAuthorized_IsContractor()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.ConcurrencyControlNumber = 1;

            var repository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.Update(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);
            var lookupRepository = _helper.GetService<Mock<ILookupRepository>>();
            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(new PimsAcquisitionFile()
            {
                AcquisitionFileStatusTypeCode = acqFile.AcquisitionFileStatusTypeCode,
            });
            var userRepository = _helper.GetService<Mock<IUserRepository>>();
            var contractorUser = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: true);
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(contractorUser);

            // Act
            Action act = () => service.Update(acqFile, true);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void Update_CannotDetermineRegion()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.RegionCode = 4;
            acqFile.ConcurrencyControlNumber = 1;

            var repository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.Update(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetRegion(It.IsAny<long>())).Returns(acqFile.RegionCode);

            var lookupRepository = _helper.GetService<Mock<ILookupRepository>>();
            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });

            var userRepository = _helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            Action act = () => service.Update(acqFile, true, true);

            // Assert
            act.Should().Throw<BadRequestException>();
        }

        [Fact]
        public void Update_NoPermission()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions();

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
            // Act
            Action act = () => service.Update(acqFile, true);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.Update(It.IsAny<PimsAcquisitionFile>()), Times.Never);
        }

        [Fact]
        public void Update_ThrowIf_Null()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = _helper.GetService<Mock<IAcquisitionFileRepository>>();

            // Act
            Action act = () => service.Update(null, false);

            // Assert
            act.Should().Throw<ArgumentNullException>();
            repository.Verify(x => x.Update(It.IsAny<PimsAcquisitionFile>()), Times.Never);
        }

        [Fact]
        public void Update_Region_Violation()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetRegion(It.IsAny<long>())).Returns((short)(acqFile.RegionCode + 100));

            var userRepository = _helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            Action act = () => service.Update(acqFile, ministryOverride: false, propertiesOverride: true);

            // Assert
            var ex = act.Should().Throw<UserOverrideException>();
            ex.Which.ErrorCode.Should().Be("region_violation");
            repository.Verify(x => x.Update(It.IsAny<PimsAcquisitionFile>()), Times.Never);
        }

        [Fact]
        public void Update_Success_Region_UserOverride()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.Update(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var lookupRepository = _helper.GetService<Mock<ILookupRepository>>();
            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });

            var userRepository = _helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            var result = service.Update(acqFile, ministryOverride: true, propertiesOverride: true);

            // Assert
            repository.Verify(x => x.Update(It.IsAny<PimsAcquisitionFile>()), Times.Once);
        }

        [Fact]
        public void Update_PropertyOfInterest_Violation()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.AcquisitionFileStatusTypeCode = "COMPLT";
            acqFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345);
            property.IsPropertyOfInterest = true;
            var propertyAcqFile = new PimsPropertyAcquisitionFile() { Property = property };
            acqFile.PimsPropertyAcquisitionFiles = new List<PimsPropertyAcquisitionFile>() { propertyAcqFile };

            var repository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var filePropertyRepository = _helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>())).Returns(acqFile.PimsPropertyAcquisitionFiles.ToList());

            var userRepository = _helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            Action act = () => service.Update(acqFile, ministryOverride: true, propertiesOverride: false);

            // Assert
            var ex = act.Should().Throw<UserOverrideException>();
            ex.Which.ErrorCode.Should().Be("properties_of_interest_violation");
            repository.Verify(x => x.Update(It.IsAny<PimsAcquisitionFile>()), Times.Never);
        }

        [Fact]
        public void Update_Success_PropertyOfInterest_UserOverride()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.AcquisitionFileStatusTypeCode = "COMPLT";
            acqFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345);
            property.IsPropertyOfInterest = true;
            var propertyAcqFile = new PimsPropertyAcquisitionFile() { Property = property };
            acqFile.PimsPropertyAcquisitionFiles = new List<PimsPropertyAcquisitionFile>() { propertyAcqFile };

            var repository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);
            repository.Setup(x => x.Update(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);

            var filePropertyRepository = _helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>())).Returns(acqFile.PimsPropertyAcquisitionFiles.ToList());

            var propertyRepository = _helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.TransferToCoreInventory(It.IsAny<PimsProperty>()));

            var lookupRepository = _helper.GetService<Mock<ILookupRepository>>();
            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });

            var userRepository = _helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            var result = service.Update(acqFile, ministryOverride: true, propertiesOverride: true);

            // Assert
            repository.Verify(x => x.Update(It.IsAny<PimsAcquisitionFile>()), Times.Once);
            propertyRepository.Verify(x => x.TransferToCoreInventory(It.IsAny<PimsProperty>()), Times.Once);
        }

        [Fact]
        public void Update_Success_AddsNote()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.ConcurrencyControlNumber = 1;
            acqFile.AppCreateUserid = "TESTER";

            var repository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
            var noteRepository = _helper.GetService<Mock<IEntityNoteRepository>>();
            var lookupRepository = _helper.GetService<Mock<ILookupRepository>>();

            repository.Setup(x => x.Update(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(new PimsAcquisitionFile()
            {
                AcquisitionFileStatusTypeCode = "CLOSED",
                AcquisitionFileStatusTypeCodeNavigation = new PimsAcquisitionFileStatusType() { Description = "Closed" }
            });
            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });
            lookupRepository.Setup(x => x.GetAllAcquisitionFileStatusTypes()).Returns(new PimsAcquisitionFileStatusType[]{ new PimsAcquisitionFileStatusType() {
                Id = acqFile.AcquisitionFileStatusTypeCodeNavigation.Id,
                Description = acqFile.AcquisitionFileStatusTypeCodeNavigation.Description,
            }});

            var userRepository = _helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            var result = service.Update(acqFile, true);

            // Assert
            repository.Verify(x => x.Update(It.IsAny<PimsAcquisitionFile>()), Times.Once);
            noteRepository.Verify(x => x.Add(It.Is<PimsAcquisitionFileNote>(x => x.AcquisitionFileId == 1
                    && x.Note.NoteTxt == "Acquisition File status changed from Closed to Active")), Times.Once);
        }

        [Fact]
        public void UpdateProperties_Success()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit, Permissions.AcquisitionFileAdd, Permissions.AcquisitionFileView);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.ConcurrencyControlNumber = 1;

            var repository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var filePropertyRepository = _helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>())).Returns(acqFile.PimsPropertyAcquisitionFiles.ToList());

            var userRepository = _helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            service.UpdateProperties(acqFile);

            // Assert
            filePropertyRepository.Verify(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void UpdateProperties_Activities_Violation()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit, Permissions.AcquisitionFileAdd, Permissions.AcquisitionFileView);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345);
            var propertyAcqFile = new PimsPropertyAcquisitionFile() { Property = property };
            propertyAcqFile.PimsActInstPropAcqFiles = new List<PimsActInstPropAcqFile>() { new PimsActInstPropAcqFile() };
            acqFile.PimsPropertyAcquisitionFiles = new List<PimsPropertyAcquisitionFile>() { propertyAcqFile };

            var repository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var propertyRepository = _helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>())).Returns(property);

            var filePropertyRepository = _helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>())).Returns(acqFile.PimsPropertyAcquisitionFiles.ToList());

            var userRepository = _helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            Action act = () => service.UpdateProperties(EntityHelper.CreateAcquisitionFile());

            // Assert
            act.Should().Throw<BusinessRuleViolationException>();
        }

        [Fact]
        public void UpdateProperties_Takes_Violation()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit, Permissions.AcquisitionFileAdd, Permissions.AcquisitionFileView);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345);
            var propertyAcqFile = new PimsPropertyAcquisitionFile() { Property = property };
            propertyAcqFile.PimsTakes = new List<PimsTake>() { new PimsTake() };
            acqFile.PimsPropertyAcquisitionFiles = new List<PimsPropertyAcquisitionFile>() { propertyAcqFile };

            var repository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var propertyRepository = _helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>())).Returns(property);

            var filePropertyRepository = _helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>())).Returns(acqFile.PimsPropertyAcquisitionFiles.ToList());

            var userRepository = _helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            Action act = () => service.UpdateProperties(EntityHelper.CreateAcquisitionFile());

            // Assert
            act.Should().Throw<BusinessRuleViolationException>();
        }

        [Fact]
        public void UpdateProperties_MatchProperties_Success()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345);
            acqFile.PimsPropertyAcquisitionFiles = new List<PimsPropertyAcquisitionFile>() { new PimsPropertyAcquisitionFile() { Property = property } };

            var repository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var propertyRepository = _helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>())).Returns(property);

            var filePropertyRepository = _helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>())).Returns(acqFile.PimsPropertyAcquisitionFiles.ToList());

            var userRepository = _helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            service.UpdateProperties(acqFile);

            // Assert
            filePropertyRepository.Verify(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void UpdateProperties_MatchProperties_NewProperty_Success()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345);
            acqFile.PimsPropertyAcquisitionFiles = new List<PimsPropertyAcquisitionFile>() { new PimsPropertyAcquisitionFile() { Property = property } };

            var repository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
            PimsPropertyAcquisitionFile updatedAcquisitionFileProperty = null;
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var filePropertyRepository = _helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>())).Returns(acqFile.PimsPropertyAcquisitionFiles.ToList());
            filePropertyRepository.Setup(x => x.Add(It.IsAny<PimsPropertyAcquisitionFile>())).Callback<PimsPropertyAcquisitionFile>(x => updatedAcquisitionFileProperty = x).Returns(acqFile.PimsPropertyAcquisitionFiles.FirstOrDefault());

            var propertyRepository = _helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>())).Throws<KeyNotFoundException>();

            var coordinateService = _helper.GetService<Mock<ICoordinateTransformService>>();
            coordinateService.Setup(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>())).Returns(new Coordinate(924046.3314288399, 1088892.9140135897));

            var userRepository = _helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            service.UpdateProperties(acqFile);

            // Assert
            // since this is a new property, the following default fields should be set.
            var updatedProperty = updatedAcquisitionFileProperty.Property;
            updatedProperty.PropertyClassificationTypeCode.Should().Be("UNKNOWN");
            updatedProperty.PropertyTypeCode.Should().Be("UNKNOWN");
            updatedProperty.PropertyStatusTypeCode.Should().Be("UNKNOWN");
            updatedProperty.SurplusDeclarationTypeCode.Should().Be("UNKNOWN");
            updatedProperty.PropertyDataSourceEffectiveDate.Should().BeCloseTo(System.DateTime.Now, 500);
            updatedProperty.PropertyDataSourceTypeCode.Should().Be("PMBC");
            updatedProperty.IsPropertyOfInterest.Should().Be(true);

            filePropertyRepository.Verify(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>()), Times.Once);
            coordinateService.Verify(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>()));
        }

        [Fact]
        public void UpdateProperties_UpdatePropertyFile_Success()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var property = EntityHelper.CreateProperty(12345);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.PimsPropertyAcquisitionFiles = new List<PimsPropertyAcquisitionFile>() { new PimsPropertyAcquisitionFile() { Internal_Id = 1, Property = property } };
            acqFile.ConcurrencyControlNumber = 1;

            var repository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var filePropertyRepository = _helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>())).Returns(new List<PimsPropertyAcquisitionFile>() { new PimsPropertyAcquisitionFile() { Internal_Id = 1, Property = property, PropertyName = "updated" } });

            var propertyRepository = _helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>())).Throws<KeyNotFoundException>();

            var userRepository = _helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            service.UpdateProperties(acqFile);

            // Assert
            filePropertyRepository.Verify(x => x.Update(It.IsAny<PimsPropertyAcquisitionFile>()), Times.Once);
        }

        [Fact]
        public void UpdateProperties_RemovePropertyFile_Success()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345);

            var repository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var filePropertyRepository = _helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>())).Returns(new List<PimsPropertyAcquisitionFile>() { new PimsPropertyAcquisitionFile() { Property = property } });

            var propertyRepository = _helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>())).Throws<KeyNotFoundException>();

            var userRepository = _helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            service.UpdateProperties(acqFile);

            // Assert
            filePropertyRepository.Verify(x => x.Delete(It.IsAny<PimsPropertyAcquisitionFile>()), Times.Once);
        }

        [Fact]
        public void UpdateProperties_RemoveProperty_Success()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345);
            property.IsPropertyOfInterest = true;
            property.PimsPropertyResearchFiles = new List<PimsPropertyResearchFile>();
            property.PimsPropertyLeases = new List<PimsPropertyLease>();
            property.PimsPropertyAcquisitionFiles = new List<PimsPropertyAcquisitionFile>() { new PimsPropertyAcquisitionFile() };

            var repository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var filePropertyRepository = _helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>())).Returns(new List<PimsPropertyAcquisitionFile>() { new PimsPropertyAcquisitionFile() { Property = property } });
            filePropertyRepository.Setup(x => x.GetAcquisitionFilePropertyRelatedCount(It.IsAny<long>())).Returns(1);

            var propertyRepository = _helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>())).Throws<KeyNotFoundException>();
            propertyRepository.Setup(x => x.GetAllAssociationsById(It.IsAny<long>())).Returns(property);

            var userRepository = _helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            service.UpdateProperties(acqFile);

            // Assert
            filePropertyRepository.Verify(x => x.Delete(It.IsAny<PimsPropertyAcquisitionFile>()), Times.Once);
            propertyRepository.Verify(x => x.Delete(It.IsAny<PimsProperty>()), Times.Once);
        }

        [Fact]
        public void UpdateProperties_NoPermission()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions();

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.Update(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);

            // Act
            Action act = () => service.UpdateProperties(acqFile);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.Update(It.IsAny<PimsAcquisitionFile>()), Times.Never);
        }

        [Fact]
        public void UpdateProperties_NotAuthorized_Contractor()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.Update(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);

            var userRepository = _helper.GetService<Mock<IUserRepository>>();
            var contractorUser = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: true);
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(contractorUser);

            var acqFileRepository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
            acqFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            // Act
            Action act = () => service.UpdateProperties(acqFile);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void Update_DuplicateTeam()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.PimsAcquisitionFilePeople.Add(new PimsAcquisitionFilePerson() { PersonId = 1, AcqFlPersonProfileTypeCode = "test" });
            acqFile.PimsAcquisitionFilePeople.Add(new PimsAcquisitionFilePerson() { PersonId = 1, AcqFlPersonProfileTypeCode = "test" });
            acqFile.ConcurrencyControlNumber = 1;

            var repository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.Update(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetRegion(It.IsAny<long>())).Returns(acqFile.RegionCode);
            var lookupRepository = _helper.GetService<Mock<ILookupRepository>>();
            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });

            var userRepository = _helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            Action act = () => service.Update(acqFile, true);

            // Assert
            act.Should().Throw<BadRequestException>();
        }

        #endregion

        #region Checklist
        [Fact]
        public void GetChecklist_Success()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileView);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = _helper.GetService<Mock<IAcquisitionFileChecklistRepository>>();
            var acquisitionRepository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetAllChecklistItemsByAcquisitionFileId(It.IsAny<long>())).Returns(acqFile.PimsAcquisitionChecklistItems.ToList());
            acquisitionRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var userRepository = _helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            var result = service.GetChecklistItems(1);

            // Assert
            repository.Verify(x => x.GetAllChecklistItemsByAcquisitionFileId(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void GetChecklist_Append_Success()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileView);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = _helper.GetService<Mock<IAcquisitionFileChecklistRepository>>();
            var acquisitionRepository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetAllChecklistItemsByAcquisitionFileId(It.IsAny<long>())).Returns(new List<PimsAcquisitionChecklistItem>());
            repository.Setup(x => x.GetAllChecklistItemTypes()).Returns(new List<PimsAcqChklstItemType>() { new PimsAcqChklstItemType() { AcqChklstItemTypeCode = "TEST" } });
            acquisitionRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            // Act
            var result = service.GetChecklistItems(1);

            // Assert
            repository.Verify(x => x.GetAllChecklistItemsByAcquisitionFileId(It.IsAny<long>()), Times.Once);
            result.Count().Should().Be(1);
            result.FirstOrDefault().AcqChklstItemTypeCode.Should().Be("TEST");
            result.FirstOrDefault().AcqChklstItemStatusTypeCode.Should().Be("INCOMP");
        }

        [Fact]
        public void GetChecklist_Append_IgnoreAcqFileByStatus()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileView);

            var acqFile = EntityHelper.CreateAcquisitionFile(1);
            acqFile.AcqPhysFileStatusTypeCode = "COMPLT";

            var repository = _helper.GetService<Mock<IAcquisitionFileChecklistRepository>>();
            var acquisitionRepository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetAllChecklistItemsByAcquisitionFileId(It.IsAny<long>())).Returns(new List<PimsAcquisitionChecklistItem>());
            repository.Setup(x => x.GetAllChecklistItemTypes()).Returns(new List<PimsAcqChklstItemType>() { new PimsAcqChklstItemType() { AcqChklstItemTypeCode = "TEST" } });
            acquisitionRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            // Act
            var result = service.GetChecklistItems(1);

            // Assert
            repository.Verify(x => x.GetAllChecklistItemsByAcquisitionFileId(It.IsAny<long>()), Times.Once);
            result.Count().Should().Be(0);
        }

        [Fact]
        public void GetChecklist_Append_IgnoreItemByDate()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileView);

            var acqFile = EntityHelper.CreateAcquisitionFile(1);
            acqFile.AppCreateTimestamp = new DateTime(2023, 1, 1);

            var repository = _helper.GetService<Mock<IAcquisitionFileChecklistRepository>>();
            var acquisitionRepository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetAllChecklistItemsByAcquisitionFileId(It.IsAny<long>())).Returns(new List<PimsAcquisitionChecklistItem>());
            repository.Setup(x => x.GetAllChecklistItemTypes()).Returns(new List<PimsAcqChklstItemType>() { new PimsAcqChklstItemType() { AcqChklstItemTypeCode = "TEST", EffectiveDate = new DateTime(2024, 1, 1) } });
            acquisitionRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            // Act
            var result = service.GetChecklistItems(1);

            // Assert
            repository.Verify(x => x.GetAllChecklistItemsByAcquisitionFileId(It.IsAny<long>()), Times.Once);
            result.Count().Should().Be(0);
        }

        [Fact]
        public void GetChecklist_NoPermission()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions();

            var acqFile = EntityHelper.CreateAcquisitionFile();

            // Act
            Action act = () => service.GetChecklistItems(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void GetChecklist_NotAuthorized_Contractor()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions();

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var acqFileRepository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
            acqFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var userRepository = _helper.GetService<Mock<IUserRepository>>();
            var contractorUser = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: true);
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(contractorUser);

            // Act
            Action act = () => service.GetChecklistItems(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void UpdateChecklist_Success()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.PimsAcquisitionChecklistItems = new List<PimsAcquisitionChecklistItem>() { new PimsAcquisitionChecklistItem() { Internal_Id = 1, AcqChklstItemStatusTypeCode = "COMPLT" } };

            var repository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var fileChecklistRepository = _helper.GetService<Mock<IAcquisitionFileChecklistRepository>>();
            fileChecklistRepository.Setup(x => x.GetAllChecklistItemsByAcquisitionFileId(It.IsAny<long>()))
                .Returns(new List<PimsAcquisitionChecklistItem>() { new PimsAcquisitionChecklistItem() { Internal_Id = 1, AcqChklstItemStatusTypeCode = "INCOMP" } });

            var userRepository = _helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            service.UpdateChecklistItems(acqFile);

            // Assert
            fileChecklistRepository.Verify(x => x.GetAllChecklistItemsByAcquisitionFileId(It.IsAny<long>()), Times.Once);
            fileChecklistRepository.Verify(x => x.Update(It.IsAny<PimsAcquisitionChecklistItem>()), Times.Once);
            repository.Verify(x => x.GetById(It.IsAny<long>()), Times.Exactly(2));
        }

        [Fact]
        public void UpdateChecklist_ItemNotFound()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.PimsAcquisitionChecklistItems = new List<PimsAcquisitionChecklistItem>() { new PimsAcquisitionChecklistItem() { Internal_Id = 999, AcqChklstItemStatusTypeCode = "COMPLT" } };

            var repository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var fileChecklistRepository = _helper.GetService<Mock<IAcquisitionFileChecklistRepository>>();
            fileChecklistRepository.Setup(x => x.GetAllChecklistItemsByAcquisitionFileId(It.IsAny<long>()))
                .Returns(new List<PimsAcquisitionChecklistItem>() { new PimsAcquisitionChecklistItem() { Internal_Id = 1, AcqChklstItemStatusTypeCode = "INCOMP" } });

            var userRepository = _helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            Action act = () => service.UpdateChecklistItems(acqFile);

            // Assert
            act.Should().Throw<BadRequestException>();

            fileChecklistRepository.Verify(x => x.GetAllChecklistItemsByAcquisitionFileId(It.IsAny<long>()), Times.Once);
            fileChecklistRepository.Verify(x => x.Update(It.IsAny<PimsAcquisitionChecklistItem>()), Times.Never);
            repository.Verify(x => x.GetById(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void UpdateChecklist_NoPermission()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions();

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = _helper.GetService<Mock<IAcquisitionFileChecklistRepository>>();
            repository.Setup(x => x.GetAllChecklistItemsByAcquisitionFileId(It.IsAny<long>())).Returns(acqFile.PimsAcquisitionChecklistItems.ToList());

            // Act
            Action act = () => service.UpdateChecklistItems(acqFile);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.GetAllChecklistItemsByAcquisitionFileId(It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public void UpdateChecklist_NotAuthorized_Contractor()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = _helper.GetService<Mock<IAcquisitionFileChecklistRepository>>();
            repository.Setup(x => x.GetAllChecklistItemsByAcquisitionFileId(It.IsAny<long>())).Returns(acqFile.PimsAcquisitionChecklistItems.ToList());

            var acqFileRepository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
            acqFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var userRepository = _helper.GetService<Mock<IUserRepository>>();
            var contractorUser = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: true);
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(contractorUser);

            // Act
            Action act = () => service.UpdateChecklistItems(acqFile);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }
        #endregion

        #region CompensationRequisition

        [Fact]
        public void GetCompensationsRequisitions_NoPermissions()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions();

            // Act
            Action act = () => service.GetAcquisitionCompensations(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void GetCompensationsRequisitions_NotAuthorized_Contractor()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileView, Permissions.CompensationRequisitionView);

            var userRepository = _helper.GetService<Mock<IUserRepository>>();
            var contractorUser = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: true);
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(contractorUser);

            var acqFileRepository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
            acqFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(EntityHelper.CreateAcquisitionFile());

            // Act
            Action act = () => service.GetAcquisitionCompensations(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void GetCompensationsRequisitions_Success()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileView, Permissions.CompensationRequisitionView);

            var repository = _helper.GetService<Mock<ICompensationRequisitionRepository>>();
            repository.Setup(x => x.GetAllByAcquisitionFileId(It.IsAny<long>()))
                .Returns(new List<PimsCompensationRequisition>()
                {
                    new PimsCompensationRequisition(),
                });

            var userRepository = _helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            var result = service.GetAcquisitionCompensations(1);

            // Assert
            repository.Verify(x => x.GetAllByAcquisitionFileId(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void AddCompensationsRequisitions_NoPermissions()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions();

            // Act
            Action act = () => service.AddCompensationRequisition(1, new PimsCompensationRequisition());

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void AddCompensationsRequisitions_NullException()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions(Permissions.CompensationRequisitionAdd);
            var userRepository = _helper.GetService<Mock<IUserRepository>>();

            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            Action act = () => service.AddCompensationRequisition(1, null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void AddCompensationsRequisitions_BadRequest_IdMissmatch()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions(Permissions.CompensationRequisitionAdd);
            var userRepository = _helper.GetService<Mock<IUserRepository>>();

            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            Action act = () => service.AddCompensationRequisition(1, new PimsCompensationRequisition() { Internal_Id = 2 });

            // Assert
            act.Should().Throw<BadRequestException>();
        }

        [Fact]
        public void AddCompensationsRequisitions_NotAuthorized_Contractor()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions(Permissions.CompensationRequisitionAdd);
            var repository = _helper.GetService<Mock<ICompensationRequisitionRepository>>();
            var acqFilerepository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
            var newCompensationReq = EntityHelper.CreateCompensationRequisition(1, 1);
            var acquisitionFile = EntityHelper.CreateAcquisitionFile(1);

            acqFilerepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acquisitionFile);
            repository.Setup(x => x.Add(It.IsAny<PimsCompensationRequisition>())).Returns(newCompensationReq);

            var userRepository = _helper.GetService<Mock<IUserRepository>>();
            var contractorUser = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: true);
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(contractorUser);

            // Act
            Action act = () => service.AddCompensationRequisition(1, newCompensationReq);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void AddCompensationsRequisitions_Success()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions(Permissions.CompensationRequisitionAdd);
            var repository = _helper.GetService<Mock<ICompensationRequisitionRepository>>();
            var acqFilerepository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
            var newCompensationReq = EntityHelper.CreateCompensationRequisition(1, 1);
            var acquisitionFile = EntityHelper.CreateAcquisitionFile(1);

            acqFilerepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acquisitionFile);
            repository.Setup(x => x.Add(It.IsAny<PimsCompensationRequisition>())).Returns(newCompensationReq);

            var userRepository = _helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            var result = service.AddCompensationRequisition(1, newCompensationReq);

            // Assert
            repository.Verify(x => x.Add(It.IsAny<PimsCompensationRequisition>()), Times.Once);
        }


        #endregion

        #region Agreements

        [Fact]
        public void GetAgreementsByAcquisitionFileId_NoPermission_IsContractor()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileView, Permissions.AgreementView);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
            var userRepository = _helper.GetService<Mock<IUserRepository>>();
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var contractorUser = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: true);
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(contractorUser);

            // Act
            Action act = () => service.GetAgreements(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void UpdateAgreementsByAcquisitionFileId_NoPermission_IsContractor()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileView, Permissions.AgreementView);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
            var userRepository = _helper.GetService<Mock<IUserRepository>>();
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var contractorUser = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: true);
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(contractorUser);

            // Act
            Action act = () => service.UpdateAgreements(1, It.IsAny<List<PimsAgreement>>());

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        #endregion

        #region Owners

        [Fact]
        public void GetOwners_ByAcquisitionFileId_NoPermission_IsContractor()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileView, Permissions.AgreementView);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
            var userRepository = _helper.GetService<Mock<IUserRepository>>();
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var contractorUser = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: true);
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(contractorUser);

            // Act
            Action act = () => service.GetOwners(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        #endregion

        #region Properties

        [Fact]
        public void GetProperties_ByAcquisitionFileId_NoPermission_IsContractor()
        {
            // Arrange
            var service = CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileView, Permissions.AgreementView);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
            var userRepository = _helper.GetService<Mock<IUserRepository>>();
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var contractorUser = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: true);
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(contractorUser);

            // Act
            Action act = () => service.GetProperties(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }
        #endregion


        #endregion
    }
}
