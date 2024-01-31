using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Channels;
using DocumentFormat.OpenXml.Office2010.Excel;
using FluentAssertions;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using NetTopologySuite.Geometries;
using NExpect.Interfaces;
using Pims.Api.Constants;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Models.Concepts;
using Pims.Api.Services;
using Pims.Core.Exceptions;
using Pims.Core.Test;
using Pims.Dal;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Xunit;

namespace Pims.Api.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "disposition")]
    [ExcludeFromCodeCoverage]
    public class DispositionFileServiceTest
    {
        private readonly TestHelper _helper;

        public DispositionFileServiceTest()
        {
            this._helper = new TestHelper();
        }

        private DispositionFileService CreateDispositionServiceWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            return this._helper.Create<DispositionFileService>(user);
        }

        #region GetById
        [Fact]
        public void GetById_Success()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionView);

            var dispFile = EntityHelper.CreateDispositionFile();

            var repository = this._helper.GetService<Mock<IDispositionFileRepository>>();
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(dispFile);

            // Act
            var result = service.GetById(1);

            // Assert
            repository.Verify(x => x.GetById(It.IsAny<long>()), Times.Exactly(2));
        }

        [Fact]
        public void GetById_NoPermission()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions();

            var dispFile = EntityHelper.CreateDispositionFile();

            // Act
            Action act = () => service.GetById(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }
        #endregion

        #region Properties

        [Fact]
        public void GetProperties_ByDispositionFileId_NoPermission()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionView);

            var dispFile = EntityHelper.CreateDispositionFile();

            var repository = this._helper.GetService<Mock<IDispositionFilePropertyRepository>>();
            repository.Setup(x => x.GetPropertiesByDispositionFileId(It.IsAny<long>())).Returns(new List<PimsDispositionFileProperty>());

            // Act
            Action act = () => service.GetProperties(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void GetProperties_ByDispositionFileId_Success()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionView, Permissions.PropertyView);

            var dispFile = EntityHelper.CreateDispositionFile();

            var repository = this._helper.GetService<Mock<IDispositionFilePropertyRepository>>();
            repository.Setup(x => x.GetPropertiesByDispositionFileId(It.IsAny<long>())).Returns(new List<PimsDispositionFileProperty>());

            // Act
            var properties = service.GetProperties(1);

            // Assert
            repository.Verify(x => x.GetPropertiesByDispositionFileId(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void GetProperties_ByDispositionFileId_Success_Reproject()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionView, Permissions.PropertyView);

            var dispFile = EntityHelper.CreateDispositionFile();

            var repository = this._helper.GetService<Mock<IDispositionFilePropertyRepository>>();
            repository.Setup(x => x.GetPropertiesByDispositionFileId(It.IsAny<long>())).Returns(new List<PimsDispositionFileProperty>() { new PimsDispositionFileProperty() { Property = new PimsProperty() { Location = new Point(1, 1) } } });
            var coordinateService = this._helper.GetService<Mock<ICoordinateTransformService>>();
            coordinateService.Setup(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>())).Returns(new Coordinate(1, 1));
            // Act
            var properties = service.GetProperties(1);

            // Assert
            repository.Verify(x => x.GetPropertiesByDispositionFileId(It.IsAny<long>()), Times.Once);
            coordinateService.Verify(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>()), Times.Once);
            properties.FirstOrDefault().Property.Location.Coordinates.Should().BeEquivalentTo(new Coordinate[] { new Coordinate(1, 1) });
        }
        #endregion

        #region GetLastUpdate

        [Fact]
        public void GetLastUpdate_ByDispositionFileId_NoPermission()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions();

            var dispFile = EntityHelper.CreateDispositionFile();

            var repository = this._helper.GetService<Mock<IDispositionFileRepository>>();
            repository.Setup(x => x.GetLastUpdateBy(It.IsAny<long>())).Returns(new LastUpdatedByModel());

            // Act
            Action act = () => service.GetLastUpdateInformation(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void GetLastUpdate_ByDispositionFileId_Success()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionView);

            var dispFile = EntityHelper.CreateDispositionFile();

            var repository = this._helper.GetService<Mock<IDispositionFileRepository>>();
            repository.Setup(x => x.GetLastUpdateBy(It.IsAny<long>())).Returns(new LastUpdatedByModel());

            // Act
            var properties = service.GetLastUpdateInformation(1);

            // Assert
            repository.Verify(x => x.GetLastUpdateBy(It.IsAny<long>()), Times.Once);
        }
        #endregion

        #region GetPage
        [Fact]
        public void GetPage_Success()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionView);

            var dispFile = EntityHelper.CreateDispositionFile();

            var repository = this._helper.GetService<Mock<IDispositionFileRepository>>();
            repository.Setup(x => x.GetPageDeep(It.IsAny<DispositionFilter>(), null)).Returns(new Paged<PimsDispositionFile>(new[] { dispFile }));

            // Act
            var result = service.GetPage(new DispositionFilter());

            // Assert
            repository.Verify(x => x.GetPageDeep(It.IsAny<DispositionFilter>(), null), Times.Once);
        }

        [Fact]
        public void GetPage_NoPermission()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions();

            var dispFile = EntityHelper.CreateDispositionFile();

            // Act
            Action act = () => service.GetPage(new DispositionFilter());

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }
        #endregion

        #region Add

        [Fact]
        public void Add_Should_Fail_NoPermission()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions();
            var dispFile = EntityHelper.CreateDispositionFile(1);

            // Act
            Action act = () => service.Add(dispFile, new List<UserOverrideCode>());

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void Add_ThrowIfNull()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionAdd);

            var acqFile = EntityHelper.CreateDispositionFile();

            var repository = this._helper.GetService<Mock<IDispositionFileRepository>>();

            // Act
            Action act = () => service.Add(null, new List<UserOverrideCode>());

            // Assert
            act.Should().Throw<ArgumentNullException>();
            repository.Verify(x => x.Add(It.IsAny<PimsDispositionFile>()), Times.Never);
        }

        [Fact]
        public void Add_Fails_Duplicate_Team()
        {
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionAdd);
            var dispFile = EntityHelper.CreateDispositionFile();

            dispFile.PimsDispositionFileTeams.Add(new PimsDispositionFileTeam() { PersonId = 1, DspFlTeamProfileTypeCode = "LISTAGENT" });
            dispFile.PimsDispositionFileTeams.Add(new PimsDispositionFileTeam() { PersonId = 2, DspFlTeamProfileTypeCode = "LISTAGENT" });

            // Act
            Action act = () => service.Add(dispFile, new List<UserOverrideCode>() { UserOverrideCode.AddPropertyToInventory });

            // Assert
            act.Should().Throw<BadRequestException>();
        }

        [Fact]
        public void Add_ContractorNotInTeamException_Fail_IsContractor()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionAdd);

            var dispositionFile = EntityHelper.CreateDispositionFile();

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            var contractorUser = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: true);
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(contractorUser);

            // Act
            Action act = () => service.Add(dispositionFile, new List<UserOverrideCode>() { UserOverrideCode.UpdateRegion });

            // Assert
            var ex = act.Should().Throw<ContractorNotInTeamException>();
        }

        [Fact]
        public void Add_Success_IsContractor_AssignedToTeam()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionAdd);

            var dispositionFile = EntityHelper.CreateDispositionFile();
            dispositionFile.PimsDispositionFileTeams.Add(new PimsDispositionFileTeam() { PersonId = 1, DspFlTeamProfileTypeCode = "test" });

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();

            var newGuid = Guid.NewGuid();
            var contractorUser = EntityHelper.CreateUser(1, newGuid, username: "Test", isContractor: true);
            contractorUser.PersonId = 1;
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(contractorUser);

            var repository = this._helper.GetService<Mock<IDispositionFileRepository>>();
            repository.Setup(x => x.Add(It.IsAny<PimsDispositionFile>())).Returns(dispositionFile);

            var lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();
            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });

            // Act
            var result = service.Add(dispositionFile, new List<UserOverrideCode>());

            // Assert
            repository.Verify(x => x.Add(It.IsAny<PimsDispositionFile>()), Times.Once);
        }

        #endregion

        #region Update

        [Fact]
        public void Update_Should_Fail_NoPermission()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions();
            var dispFile = EntityHelper.CreateDispositionFile(1);

            // Act
            Action act = () => service.Update(1, dispFile, new List<UserOverrideCode>());

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void Update_Should_Fail_Invalid_DispositionFileId()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionEdit);
            var repository = this._helper.GetService<Mock<IDispositionFileRepository>>();
            var dispFile = EntityHelper.CreateDispositionFile(1);

            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);

            // Act
            Action act = () => service.Update(2, dispFile, new List<UserOverrideCode>());

            // Assert
            act.Should().Throw<BadRequestException>();
        }

        [Fact]
        public void Update_Should_Fail_Duplicate_Team()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionEdit);
            var repository = this._helper.GetService<Mock<IDispositionFileRepository>>();
            var dispFile = EntityHelper.CreateDispositionFile(1);

            dispFile.PimsDispositionFileTeams.Add(new PimsDispositionFileTeam() { PersonId = 1, DspFlTeamProfileTypeCode = "LISTAGENT" });
            dispFile.PimsDispositionFileTeams.Add(new PimsDispositionFileTeam() { PersonId = 2, DspFlTeamProfileTypeCode = "LISTAGENT" });

            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);

            // Act
            Action act = () => service.Update(1, dispFile, new List<UserOverrideCode>());

            // Assert
            act.Should().Throw<BadRequestException>();
        }

        [Fact]
        public void Update_Should_Fail_Region_Validation()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionEdit);
            var repository = this._helper.GetService<Mock<IDispositionFileRepository>>();
            var dispFile = EntityHelper.CreateDispositionFile(1);

            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetRegion(It.IsAny<long>())).Returns(2);
            repository.Setup(x => x.Update(It.IsAny<long>(), It.IsAny<PimsDispositionFile>())).Returns(dispFile);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(dispFile);

            // Act
            Action act = () => service.Update(1, dispFile, new List<UserOverrideCode>());

            // Assert
            var ex = act.Should().Throw<UserOverrideException>();
            ex.Which.UserOverride.Should().Be(UserOverrideCode.UpdateRegion);
        }

        [Fact]
        public void Update_UserOverride_Final_Validation()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionEdit);
            var repository = this._helper.GetService<Mock<IDispositionFileRepository>>();
            var dispFile = EntityHelper.CreateDispositionFile(1);
            dispFile.DispositionFileStatusTypeCode = EnumDispositionFileStatusTypeCode.COMPLETE.ToString();

            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetRegion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.Update(It.IsAny<long>(), It.IsAny<PimsDispositionFile>())).Returns(dispFile);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(dispFile);

            // Act
            Action act = () => service.Update(1, dispFile, new List<UserOverrideCode>());

            // Assert
            var ex = act.Should().Throw<UserOverrideException>();
            ex.Which.UserOverride.Should().Be(UserOverrideCode.DispositionFileFinalStatus);
        }

        [Fact]
        public void Update_NotAuthorized_IsContractor()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.AcquisitionFileEdit);
            var dispositionFile = EntityHelper.CreateDispositionFile(1);

            var repository = this._helper.GetService<Mock<IDispositionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.Update(It.IsAny<long>(),It.IsAny<PimsDispositionFile>())).Returns(dispositionFile);

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            var contractorUser = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: true);
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(contractorUser);

            // Act
            Action act = () => service.Update(1, dispositionFile, new List<UserOverrideCode>() { UserOverrideCode.UpdateRegion });

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void Update_IsContractor_SelfRemoved_ShouldFail()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionEdit);
            var dispositionFile = EntityHelper.CreateDispositionFile(1);
            dispositionFile.PimsDispositionFileTeams = new List<PimsDispositionFileTeam>()
            {
                new PimsDispositionFileTeam()
                {
                    DispositionFileTeamId = 100,
                    DispositionFileId = 1,
                    PersonId = 20,
                }
            };

            var repository = this._helper.GetService<Mock<IDispositionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(dispositionFile);
            repository.Setup(x => x.Update(It.IsAny<long>(), It.IsAny<PimsDispositionFile>())).Returns(dispositionFile);

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            var contractorUser = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: true);
            contractorUser.PersonId = 20;

            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(contractorUser);

            var updateDispositionFile = EntityHelper.CreateDispositionFile(1);

            // Act
            Action act = () => service.Update(1, updateDispositionFile, new List<UserOverrideCode>() { UserOverrideCode.UpdateRegion });

            // Assert
            var ex = act.Should().Throw<ContractorNotInTeamException>();
        }

        [Fact]
        public void Update_IsContractor_Success()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionEdit);
            var dispositionFile = EntityHelper.CreateDispositionFile(1);
            dispositionFile.PimsDispositionFileTeams = new List<PimsDispositionFileTeam>()
            {
                new PimsDispositionFileTeam()
                {
                    DispositionFileTeamId = 100,
                    DispositionFileId = 1,
                    PersonId = 20,
                }
            };

            var repository = this._helper.GetService<Mock<IDispositionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(dispositionFile);
            repository.Setup(x => x.Update(It.IsAny<long>(), It.IsAny<PimsDispositionFile>())).Returns(dispositionFile);

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            var contractorUser = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: true);
            contractorUser.PersonId = 20;

            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(contractorUser);


            // Act
            var result = service.Update(1, dispositionFile, new List<UserOverrideCode>() { UserOverrideCode.UpdateRegion });

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Update_Success()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionEdit);
            var repository = this._helper.GetService<Mock<IDispositionFileRepository>>();
            var dispFile = EntityHelper.CreateDispositionFile(1);

            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetRegion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.Update(It.IsAny<long>(), It.IsAny<PimsDispositionFile>())).Returns(dispFile);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(dispFile);

            // Act
            var result = service.Update(1, dispFile, new List<UserOverrideCode>() { UserOverrideCode.UpdateRegion });

            // Assert
            Assert.NotNull(result);
            repository.Verify(x => x.Update(It.IsAny<long>(), It.IsAny<PimsDispositionFile>()), Times.Once);
        }

        [Fact]
        public void Update_Success_AddsNote()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionEdit);

            var dspFile = EntityHelper.CreateDispositionFile();
            dspFile.ConcurrencyControlNumber = 1;
            dspFile.AppCreateUserid = "TESTER";

            var repository = this._helper.GetService<Mock<IDispositionFileRepository>>();
            var noteRepository = this._helper.GetService<Mock<IEntityNoteRepository>>();
            var lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();

            repository.Setup(x => x.Update(It.IsAny<long>(), It.IsAny<PimsDispositionFile>())).Returns(dspFile);
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(new PimsDispositionFile()
            {
                DispositionFileStatusTypeCode = "CLOSED",
                DispositionFileStatusTypeCodeNavigation = new PimsDispositionFileStatusType() { Description = "Closed" },
            });
            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });
            lookupRepository.Setup(x => x.GetAllDispositionFileStatusTypes()).Returns(new PimsDispositionFileStatusType[]{ new PimsDispositionFileStatusType() {
                Id = dspFile.DispositionFileStatusTypeCodeNavigation.Id,
                Description = dspFile.DispositionFileStatusTypeCodeNavigation.Description,
            },});

            // Act
            var result = service.Update(dspFile.Internal_Id, dspFile, new List<UserOverrideCode>() { UserOverrideCode.UpdateRegion });

            // Assert
            repository.Verify(x => x.Update(It.IsAny<long>(), It.IsAny<PimsDispositionFile>()), Times.Once);
            noteRepository.Verify(x => x.Add(It.Is<PimsDispositionFileNote>(x => x.DispositionFileId == 1
                    && x.Note.NoteTxt == "Disposition File status changed from Closed to Active")), Times.Once);
        }
        #endregion

        #region GetTeamMembers
        [Fact]
        public void GetTeamMembers_Success()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionView, Permissions.ContactView);

            var dispFile = EntityHelper.CreateDispositionFile();
            var person = EntityHelper.CreatePerson(1, "tester", "chester");
            var org = EntityHelper.CreateOrganization(1, "tester org");
            List<PimsDispositionFileTeam> allTeamMembers = new()
            {
                new() { DispositionFileId = dispFile.Internal_Id, PersonId = person.Internal_Id, Person = person },
                new() { DispositionFileId = dispFile.Internal_Id, OrganizationId = org.Internal_Id, Organization = org }
            };

            var repository = this._helper.GetService<Mock<IDispositionFileRepository>>();
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
            var service = this.CreateDispositionServiceWithPermissions();

            var dispFile = EntityHelper.CreateDispositionFile();

            // Act
            Action act = () => service.GetTeamMembers();

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        #endregion

        #region Checklist
        [Fact]
        public void GetChecklist_Success()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionView);

            var acqFile = EntityHelper.CreateDispositionFile();

            var repository = this._helper.GetService<Mock<IDispositionFileChecklistRepository>>();
            var acquisitionRepository = this._helper.GetService<Mock<IDispositionFileRepository>>();
            repository.Setup(x => x.GetAllChecklistItemsByDispositionFileId(It.IsAny<long>())).Returns(acqFile.PimsDispositionChecklistItems.ToList());
            acquisitionRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            // Act
            var result = service.GetChecklistItems(1);

            // Assert
            repository.Verify(x => x.GetAllChecklistItemsByDispositionFileId(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void GetChecklist_Append_Success()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionView);

            var acqFile = EntityHelper.CreateDispositionFile();

            var repository = this._helper.GetService<Mock<IDispositionFileChecklistRepository>>();
            var acquisitionRepository = this._helper.GetService<Mock<IDispositionFileRepository>>();
            repository.Setup(x => x.GetAllChecklistItemsByDispositionFileId(It.IsAny<long>())).Returns(new List<PimsDispositionChecklistItem>());
            repository.Setup(x => x.GetAllChecklistItemTypes()).Returns(new List<PimsDspChklstItemType>() { new PimsDspChklstItemType() { DspChklstItemTypeCode = "TEST" } });
            acquisitionRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            // Act
            var result = service.GetChecklistItems(1);

            // Assert
            repository.Verify(x => x.GetAllChecklistItemsByDispositionFileId(It.IsAny<long>()), Times.Once);
            result.Count().Should().Be(1);
            result.FirstOrDefault().DspChklstItemTypeCode.Should().Be("TEST");
            result.FirstOrDefault().DspChklstItemStatusTypeCode.Should().Be("INCOMP");
        }

        [Fact]
        public void GetChecklist_Append_IgnoreDspFileByStatus()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionView);

            var acqFile = EntityHelper.CreateDispositionFile(1);
            acqFile.DispositionFileStatusTypeCode = "COMPLT";

            var repository = this._helper.GetService<Mock<IDispositionFileChecklistRepository>>();
            var acquisitionRepository = this._helper.GetService<Mock<IDispositionFileRepository>>();
            repository.Setup(x => x.GetAllChecklistItemsByDispositionFileId(It.IsAny<long>())).Returns(new List<PimsDispositionChecklistItem>());
            repository.Setup(x => x.GetAllChecklistItemTypes()).Returns(new List<PimsDspChklstItemType>() { new PimsDspChklstItemType() { DspChklstItemTypeCode = "TEST" } });
            acquisitionRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            // Act
            var result = service.GetChecklistItems(1);

            // Assert
            repository.Verify(x => x.GetAllChecklistItemsByDispositionFileId(It.IsAny<long>()), Times.Once);
            result.Count().Should().Be(0);
        }

        [Fact]
        public void GetChecklist_Append_IgnoreItemByDate()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionView);

            var acqFile = EntityHelper.CreateDispositionFile(1);
            acqFile.AppCreateTimestamp = new DateTime(2023, 1, 1);

            var repository = this._helper.GetService<Mock<IDispositionFileChecklistRepository>>();
            var acquisitionRepository = this._helper.GetService<Mock<IDispositionFileRepository>>();
            repository.Setup(x => x.GetAllChecklistItemsByDispositionFileId(It.IsAny<long>())).Returns(new List<PimsDispositionChecklistItem>());
            repository.Setup(x => x.GetAllChecklistItemTypes()).Returns(new List<PimsDspChklstItemType>() { new PimsDspChklstItemType() { DspChklstItemTypeCode = "TEST", EffectiveDate = new DateOnly(2024, 1, 1) } });
            acquisitionRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            // Act
            var result = service.GetChecklistItems(1);

            // Assert
            repository.Verify(x => x.GetAllChecklistItemsByDispositionFileId(It.IsAny<long>()), Times.Once);
            result.Count().Should().Be(0);
        }

        [Fact]
        public void GetChecklist_NoPermission()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions();

            var acqFile = EntityHelper.CreateDispositionFile();

            // Act
            Action act = () => service.GetChecklistItems(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void UpdateChecklist_Success()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionEdit);

            var checklistItems = new List<PimsDispositionChecklistItem>() { new PimsDispositionChecklistItem() { Internal_Id = 1, DspChklstItemStatusTypeCode = "COMPLT" } };
            var acqFile = EntityHelper.CreateDispositionFile();

            var repository = this._helper.GetService<Mock<IDispositionFileRepository>>();
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var fileChecklistRepository = this._helper.GetService<Mock<IDispositionFileChecklistRepository>>();
            fileChecklistRepository.Setup(x => x.GetAllChecklistItemsByDispositionFileId(It.IsAny<long>()))
                .Returns(new List<PimsDispositionChecklistItem>() { new PimsDispositionChecklistItem() { Internal_Id = 1, DspChklstItemStatusTypeCode = "INCOMP" } });

            // Act
            service.UpdateChecklistItems(checklistItems);

            // Assert
            fileChecklistRepository.Verify(x => x.GetAllChecklistItemsByDispositionFileId(It.IsAny<long>()), Times.Once);
            fileChecklistRepository.Verify(x => x.Update(It.IsAny<PimsDispositionChecklistItem>()), Times.Once);
            repository.Verify(x => x.GetById(It.IsAny<long>()), Times.Exactly(1));
        }

        [Fact]
        public void UpdateChecklist_NoEmptyList()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionEdit);

            var acqFile = EntityHelper.CreateDispositionFile();
            acqFile.DispositionFileStatusTypeCode = "ACTIV";

            var acqRepository = this._helper.GetService<Mock<IDispositionFileRepository>>();
            acqRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var fileChecklistRepository = this._helper.GetService<Mock<IDispositionFileChecklistRepository>>();
            fileChecklistRepository.Setup(x => x.GetAllChecklistItemsByDispositionFileId(It.IsAny<long>()))
                .Returns(new List<PimsDispositionChecklistItem>() { new PimsDispositionChecklistItem() { Internal_Id = 1, DspChklstItemStatusTypeCode = "INCOMP" } });

            // Act
            Action act = () => service.UpdateChecklistItems(new List<PimsDispositionChecklistItem>());

            // Assert
            act.Should().Throw<BadRequestException>();

            fileChecklistRepository.Verify(x => x.GetAllChecklistItemsByDispositionFileId(It.IsAny<long>()), Times.Never);
            fileChecklistRepository.Verify(x => x.Update(It.IsAny<PimsDispositionChecklistItem>()), Times.Never);
        }

        [Fact]
        public void UpdateChecklist_ItemNotFound()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionEdit);

            var checklistItems = new List<PimsDispositionChecklistItem>() { new PimsDispositionChecklistItem() { Internal_Id = 999, DspChklstItemStatusTypeCode = "COMPLT" } };
            var acqFile = EntityHelper.CreateDispositionFile();
            acqFile.DispositionFileStatusTypeCode = "ACTIV";

            var acqRepository = this._helper.GetService<Mock<IDispositionFileRepository>>();
            acqRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var fileChecklistRepository = this._helper.GetService<Mock<IDispositionFileChecklistRepository>>();
            fileChecklistRepository.Setup(x => x.GetAllChecklistItemsByDispositionFileId(It.IsAny<long>()))
                .Returns(new List<PimsDispositionChecklistItem>() { new PimsDispositionChecklistItem() { Internal_Id = 1, DspChklstItemStatusTypeCode = "INCOMP" } });

            // Act
            Action act = () => service.UpdateChecklistItems(checklistItems);

            // Assert
            act.Should().Throw<BadRequestException>();

            fileChecklistRepository.Verify(x => x.GetAllChecklistItemsByDispositionFileId(It.IsAny<long>()), Times.Once);
            fileChecklistRepository.Verify(x => x.Update(It.IsAny<PimsDispositionChecklistItem>()), Times.Never);
        }

        [Fact]
        public void UpdateChecklist_NoPermission()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions();

            var checklistItems = new List<PimsDispositionChecklistItem>() { new PimsDispositionChecklistItem() { Internal_Id = 1, DspChklstItemStatusTypeCode = "COMPLT" } };
            var acqFile = EntityHelper.CreateDispositionFile();

            var repository = this._helper.GetService<Mock<IDispositionFileChecklistRepository>>();
            repository.Setup(x => x.GetAllChecklistItemsByDispositionFileId(It.IsAny<long>())).Returns(acqFile.PimsDispositionChecklistItems.ToList());

            // Act
            Action act = () => service.UpdateChecklistItems(checklistItems);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.GetAllChecklistItemsByDispositionFileId(It.IsAny<long>()), Times.Never);
        }
        #endregion

        #region Appraisal

        [Fact]
        public void GetDispositionAppraisal_Should_Fail_NoPermission()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions();

            // Act
            Action act = () => service.GetDispositionFileAppraisal(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void AddDispositionFileAppraisal_Should_Fail_NoPermission()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions();

            // Act
            Action act = () => service.AddDispositionFileAppraisal(1, new()
            {
                DispositionFileId = 1,
                DispositionAppraisalId = 0,
            });

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void AddDispositionFileAppraisal_Should_Fail_Invalid_DispositionFileId()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionEdit);
            var repository = this._helper.GetService<Mock<IDispositionFileRepository>>();

            repository.Setup(x => x.GetById(1)).Returns((PimsDispositionFile)null);


            // Act
            Action act = () => service.AddDispositionFileAppraisal(1, new()
            {
                DispositionFileId = 1,
                DispositionAppraisalId = 0,
            });

            // Assert
            act.Should().Throw<BadRequestException>();
        }

        [Fact]
        public void AddDispositionFileAppraisal_Should_Fail_Invalid_AppraisalId()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionEdit);
            var repository = this._helper.GetService<Mock<IDispositionFileRepository>>();

            repository.Setup(x => x.GetById(1)).Returns(new PimsDispositionFile()
            {
                DispositionFileId = 1,
            });


            // Act
            Action act = () => service.AddDispositionFileAppraisal(1, new()
            {
                DispositionFileId = 10,
                DispositionAppraisalId = 0,
            });

            // Assert
            act.Should().Throw<BadRequestException>();
        }

        [Fact]
        public void AddDispositionFileAppraisal_Should_Fail_Appraisal_Exists()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionEdit);
            var repository = this._helper.GetService<Mock<IDispositionFileRepository>>();

            repository.Setup(x => x.GetById(1)).Returns(new PimsDispositionFile()
            {
                DispositionFileId = 1,
                PimsDispositionAppraisals = new List<PimsDispositionAppraisal>() {
                    new PimsDispositionAppraisal()
                    {
                        DispositionAppraisalId = 100,
                        DispositionFileId = 1,
                    },
                },
            });

            // Act
            Action act = () => service.AddDispositionFileAppraisal(1, new()
            {
                DispositionFileId = 1,
            });

            // Assert
            act.Should().Throw<DuplicateEntityException>();
        }

        [Fact]
        public void AddDispositionFileAppraisal_Success()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionEdit);
            var repository = this._helper.GetService<Mock<IDispositionFileRepository>>();

            repository.Setup(x => x.GetById(1)).Returns(new PimsDispositionFile()
            {
                DispositionFileId = 1,
                PimsDispositionAppraisals = new List<PimsDispositionAppraisal>() { },
            });
            repository.Setup(x => x.AddDispositionFileAppraisal(It.IsAny<PimsDispositionAppraisal>())).Returns(new PimsDispositionAppraisal()
            {
                DispositionFileId = 1,
                DispositionAppraisalId = 100,
            });

            // Act
            var result = service.AddDispositionFileAppraisal(1, new()
            {
                DispositionFileId = 1,
                DispositionAppraisalId = 0,
            });

            // Assert
            Assert.NotNull(result);
            repository.Verify(x => x.AddDispositionFileAppraisal(It.IsAny<PimsDispositionAppraisal>()), Times.Once);
        }

        #endregion

        #region Offers

        [Fact]
        public void GetDispositionOfferById_Should_Fail_NoPermission()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions();

            // Act
            Action act = () => service.GetDispositionOfferById(1, 100);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void GetDispositionOfferById_Should_Fail_Invalid_DispositionFileId()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionView);
            var dispFile = EntityHelper.CreateDispositionFile(1);
            var repository = this._helper.GetService<Mock<IDispositionFileRepository>>();

            repository.Setup(x => x.GetById(1)).Returns((PimsDispositionFile)null);

            // Act
            Action act = () => service.GetDispositionOfferById(1, 100);

            // Assert
            act.Should().Throw<BadRequestException>();
        }

        [Fact]
        public void GetDispositionOfferById_Should_Fail_Invalid_OfferFileId()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionView);
            var repository = this._helper.GetService<Mock<IDispositionFileRepository>>();

            var dispFile = EntityHelper.CreateDispositionFile(1);
            dispFile.PimsDispositionOffers = new List<PimsDispositionOffer>()
            {
                new PimsDispositionOffer()
                {
                    DispositionOfferId = 10,
                    DispositionFileId = 1,
                },
            };

            repository.Setup(x => x.GetById(1)).Returns(dispFile);

            // Act
            Action act = () => service.GetDispositionOfferById(1, 100);

            // Assert
            act.Should().Throw<BadRequestException>();
        }

        [Fact]
        public void AddDispositionFileOffer_Should_Fail_NoPermission()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions();

            // Act
            Action act = () => service.AddDispositionFileOffer(1, new()
            {
                DispositionFileId = 1,
                DispositionOfferId = 0,
            });

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void AddDispositionFileOffer_Should_Fail_Invalid_DispositionFileId()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionEdit);
            var repository = this._helper.GetService<Mock<IDispositionFileRepository>>();

            repository.Setup(x => x.GetById(1)).Returns((PimsDispositionFile)null);


            // Act
            Action act = () => service.AddDispositionFileOffer(1, new()
            {
                DispositionFileId = 1,
                DispositionOfferId = 0,
            });

            // Assert
            act.Should().Throw<BadRequestException>();
        }

        [Fact]
        public void AddDispositionFileOffer_Should_Fail_Invalid_OfferId()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionEdit);
            var repository = this._helper.GetService<Mock<IDispositionFileRepository>>();

            repository.Setup(x => x.GetById(1)).Returns(new PimsDispositionFile()
            {
                DispositionFileId = 1,
            });


            // Act
            Action act = () => service.AddDispositionFileOffer(1, new()
            {
                DispositionFileId = 10,
                DispositionOfferId = 0,
            });

            // Assert
            act.Should().Throw<BadRequestException>();
        }

        [Fact]
        public void AddDispositionFileOffer_Should_Fail_Invalid_Accepted_Exists()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionEdit);
            var repository = this._helper.GetService<Mock<IDispositionFileRepository>>();

            repository.Setup(x => x.GetById(1)).Returns(new PimsDispositionFile()
            {
                DispositionFileId = 1,
                PimsDispositionOffers = new List<PimsDispositionOffer>()
                {
                    new PimsDispositionOffer()
                    {
                        DispositionOfferId = 10,
                        DispositionFileId = 1,
                        DispositionOfferStatusTypeCode = EnumDispositionOfferStatusTypeCode.ACCCEPTED.ToString(),
                    }
                }
            });

            // Act
            Action act = () => service.AddDispositionFileOffer(1, new()
            {
                DispositionFileId = 1,
                DispositionOfferId = 0,
                DispositionOfferStatusTypeCode = "ACCCEPTED"
            });

            // Assert
            act.Should().Throw<DuplicateEntityException>();
        }

        [Fact]
        public void AddDispositionFileOffer_Success()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionEdit);
            var repository = this._helper.GetService<Mock<IDispositionFileRepository>>();

            repository.Setup(x => x.GetById(1)).Returns(new PimsDispositionFile()
            {
                DispositionFileId = 1,
                PimsDispositionOffers = new List<PimsDispositionOffer>()
                {
                    new PimsDispositionOffer()
                    {
                        DispositionOfferId = 10,
                        DispositionFileId = 1,
                        DispositionOfferStatusTypeCode = EnumDispositionOfferStatusTypeCode.REJECTED.ToString(),
                    }
                }
            });
            repository.Setup(x => x.AddDispositionOffer(It.IsAny<PimsDispositionOffer>())).Returns(new PimsDispositionOffer()
            {
                DispositionFileId = 1,
                DispositionOfferId = 11
            });

            // Act
            var result = service.AddDispositionFileOffer(1, new()
            {
                DispositionFileId = 1,
                DispositionOfferId = 0,
                DispositionOfferStatusTypeCode = "OPEN"
            });

            // Assert
            Assert.NotNull(result);
            repository.Verify(x => x.AddDispositionOffer(It.IsAny<PimsDispositionOffer>()), Times.Once);
        }

        [Fact]
        public void UpdateDispositionFileOffer_Should_Fail_NoPermission()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions();

            // Act
            Action act = () => service.UpdateDispositionFileOffer(1, 10, new()
            {
                DispositionFileId = 1,
                DispositionOfferId = 10,
            });

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void UpdateDispositionFileOffer_Should_Fail_Invalid_DispositionFileId()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionEdit);
            var repository = this._helper.GetService<Mock<IDispositionFileRepository>>();

            repository.Setup(x => x.GetById(1)).Returns((PimsDispositionFile)null);

            // Act
            Action act = () => service.UpdateDispositionFileOffer(1, 10, new()
            {
                DispositionFileId = 1,
                DispositionOfferId = 0,
            });

            // Assert
            act.Should().Throw<BadRequestException>();
        }

        [Fact]
        public void UpdateDispositionFileOffer_Should_Fail_Invalid_OfferId()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionEdit);
            var repository = this._helper.GetService<Mock<IDispositionFileRepository>>();

            repository.Setup(x => x.GetById(1)).Returns(new PimsDispositionFile()
            {
                DispositionFileId = 1,
            });


            // Act
            Action act = () => service.AddDispositionFileOffer(1, new()
            {
                DispositionFileId = 10,
                DispositionOfferId = 0,
            });

            // Assert
            act.Should().Throw<BadRequestException>();
        }

        [Fact]
        public void UpdateDispositionFileOffer_Should_Fail_Invalid_Accepted_Exists()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionEdit);
            var repository = this._helper.GetService<Mock<IDispositionFileRepository>>();

            repository.Setup(x => x.GetById(1)).Returns(new PimsDispositionFile()
            {
                DispositionFileId = 1,
                PimsDispositionOffers = new List<PimsDispositionOffer>()
                {
                    new PimsDispositionOffer()
                    {
                        DispositionOfferId = 10,
                        DispositionFileId = 1,
                        DispositionOfferStatusTypeCode = EnumDispositionOfferStatusTypeCode.ACCCEPTED.ToString(),
                    },
                    new PimsDispositionOffer()
                    {
                        DispositionOfferId = 11,
                        DispositionFileId = 1,
                        DispositionOfferStatusTypeCode = EnumDispositionOfferStatusTypeCode.OPEN.ToString(),
                    }
                }
            });

            // Act
            Action act = () => service.UpdateDispositionFileOffer(1, 11, new()
            {
                DispositionFileId = 1,
                DispositionOfferId = 11,
                DispositionOfferStatusTypeCode = "ACCCEPTED"
            });

            // Assert
            act.Should().Throw<DuplicateEntityException>();
        }

        [Fact]
        public void UpdateDispositionFileOffer_Success()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionEdit);
            var repository = this._helper.GetService<Mock<IDispositionFileRepository>>();

            repository.Setup(x => x.GetById(1)).Returns(new PimsDispositionFile()
            {
                DispositionFileId = 1,
                PimsDispositionOffers = new List<PimsDispositionOffer>()
                {
                    new PimsDispositionOffer()
                    {
                        DispositionOfferId = 10,
                        DispositionFileId = 1,
                        DispositionOfferStatusTypeCode = EnumDispositionOfferStatusTypeCode.COLLAPSED.ToString(),
                    },
                    new PimsDispositionOffer()
                    {
                        DispositionOfferId = 11,
                        DispositionFileId = 1,
                        DispositionOfferStatusTypeCode = EnumDispositionOfferStatusTypeCode.OPEN.ToString(),
                    }
                }
            });
            repository.Setup(x => x.UpdateDispositionOffer(It.IsAny<PimsDispositionOffer>())).Returns(new PimsDispositionOffer()
            {
                DispositionFileId = 1,
                DispositionOfferId = 11
            });

            // Act
            var result = service.UpdateDispositionFileOffer(1, 11, new()
            {
                DispositionFileId = 1,
                DispositionOfferId = 11,
                DispositionOfferStatusTypeCode = "OPEN"
            });

            // Assert
            Assert.NotNull(result);
            repository.Verify(x => x.UpdateDispositionOffer(It.IsAny<PimsDispositionOffer>()), Times.Once);
        }

        [Fact]
        public void DeleteDispositionFileOffer_Fail_NoPermission()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions();

            // Act
            Action act = () => service.DeleteDispositionFileOffer(1, 10);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        #region Export

        [Fact]
        public void GetDispositionFileExport_NoPermissions()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions();
            var filter = new DispositionFilter();

            // Act
            Action act = () => service.GetDispositionFileExport(filter);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void GetDispositionFileExport_Success()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionView);
            var dispFilerepository = this._helper.GetService<Mock<IDispositionFileRepository>>();

            var filter = new DispositionFilter();
            var dispositionFile = EntityHelper.CreateDispositionFile(1);
            dispFilerepository.Setup(x => x.GetDispositionFileExportDeep(It.IsAny<DispositionFilter>()))
                        .Returns(new List<PimsDispositionFile>()
                        {
                            dispositionFile,
                        });

            // Act
            var result = service.GetDispositionFileExport(filter);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Count());
            dispFilerepository.Verify(x => x.GetDispositionFileExportDeep(It.IsAny<DispositionFilter>()), Times.Once);
        }

        [Fact]
        public void GetDispositionFileExport_Success_Properties()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionView);
            var dispFilerepository = this._helper.GetService<Mock<IDispositionFileRepository>>();

            var filter = new DispositionFilter();
            var dispositionFile = EntityHelper.CreateDispositionFile(1);
            dispositionFile.FileNumber = "10-25-2023";
            dispositionFile.PimsDispositionFileProperties = new List<PimsDispositionFileProperty>()
            {
                new PimsDispositionFileProperty()
                {
                    DispositionFileId = 1,
                    PropertyId = 100,
                    Property = new PimsProperty()
                    {
                        PropertyId = 100,
                        Pid = 8000,
                        Address = EntityHelper.CreateAddress(1)
                    },
                },
                new PimsDispositionFileProperty()
                {
                    DispositionFileId = 1,
                    PropertyId = 200,
                    Property = new PimsProperty()
                    {
                        PropertyId = 200,
                        Pid = 9000,
                    },
                },
            };

            dispFilerepository.Setup(x => x.GetDispositionFileExportDeep(It.IsAny<DispositionFilter>()))
                        .Returns(new List<PimsDispositionFile>()
                        {
                            dispositionFile,
                        });

            // Act
            var result = service.GetDispositionFileExport(filter);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Count());
            Assert.Equal("1234 St BC desc V9V9V9", result[0].CivicAddress);
            Assert.Equal("10-25-2023", result[0].FileNumber);
            Assert.Equal("8000|9000", result[0].Pid);
            dispFilerepository.Verify(x => x.GetDispositionFileExportDeep(It.IsAny<DispositionFilter>()), Times.Once);
        }

        [Fact]
        public void GetDispositionFileExport_Success_Team()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionView);
            var dispFilerepository = this._helper.GetService<Mock<IDispositionFileRepository>>();

            var filter = new DispositionFilter();
            var dispositionFile = EntityHelper.CreateDispositionFile(1);
            dispositionFile.FileNumber = "10-25-2023";
            dispositionFile.PimsDispositionFileTeams = new List<PimsDispositionFileTeam>()
            {
                new PimsDispositionFileTeam()
                {
                    Person = EntityHelper.CreatePerson(1, "first", "last"),
                    PersonId = 1,
                    DspFlTeamProfileTypeCodeNavigation = new PimsDspFlTeamProfileType() { Description = "person role"}
                },
                new PimsDispositionFileTeam()
                {
                    Organization = EntityHelper.CreateOrganization(1, "org"),
                    DspFlTeamProfileTypeCodeNavigation = new PimsDspFlTeamProfileType() { Description = "org role"}
                },
                new PimsDispositionFileTeam()
                {
                    DspFlTeamProfileTypeCodeNavigation = new PimsDspFlTeamProfileType() { Description = "primary role"},
                    Organization = EntityHelper.CreateOrganization(2, "org2"),
                    PrimaryContact = EntityHelper.CreatePerson(2, "primary", "contact")
                }
            };

            dispFilerepository.Setup(x => x.GetDispositionFileExportDeep(It.IsAny<DispositionFilter>()))
                        .Returns(new List<PimsDispositionFile>()
                        {
                            dispositionFile,
                        });

            // Act
            var result = service.GetDispositionFileExport(filter);

            // Assert
            Assert.NotNull(result);
            result.FirstOrDefault().TeamMembers.Should().Be("last first (person role)|org (Role: org role, Primary: N/A)|org2 (Role: primary role, Primary: contact primary)");
            dispFilerepository.Verify(x => x.GetDispositionFileExportDeep(It.IsAny<DispositionFilter>()), Times.Once);
        }

        [Fact]
        public void GetDispositionFileExport_Success_Appraisals_Empty()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionView);
            var dispFilerepository = this._helper.GetService<Mock<IDispositionFileRepository>>();

            var filter = new DispositionFilter();
            var dispositionFile = EntityHelper.CreateDispositionFile(1);
            dispositionFile.FileNumber = "10-25-2023";
            dispositionFile.PimsDispositionAppraisals = new List<PimsDispositionAppraisal>();

            dispFilerepository.Setup(x => x.GetDispositionFileExportDeep(It.IsAny<DispositionFilter>()))
                        .Returns(new List<PimsDispositionFile>()
                        {
                            dispositionFile,
                        });

            // Act
            var result = service.GetDispositionFileExport(filter);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Count());
            dispFilerepository.Verify(x => x.GetDispositionFileExportDeep(It.IsAny<DispositionFilter>()), Times.Once);
        }

        [Fact]
        public void GetDispositionFileExport_Success_Appraisals()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionView);
            var dispFilerepository = this._helper.GetService<Mock<IDispositionFileRepository>>();

            var filter = new DispositionFilter();
            var dispositionFile = EntityHelper.CreateDispositionFile(1);
            dispositionFile.FileNumber = "10-25-2023";
            dispositionFile.PimsDispositionAppraisals = new List<PimsDispositionAppraisal>() { new PimsDispositionAppraisal()
            {
                AppraisedAmt = 1,
                BcaRollYear = 2,
                ListPriceAmt = 3,
                BcaValueAmt = 4,
                AppraisalDt = new DateOnly(2000,1,1),
            } };

            dispFilerepository.Setup(x => x.GetDispositionFileExportDeep(It.IsAny<DispositionFilter>()))
                        .Returns(new List<PimsDispositionFile>()
                        {
                            dispositionFile,
                        });

            // Act
            var result = service.GetDispositionFileExport(filter);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Count());
            var row = result[0];
            row.AppraisalValue.Should().Be(1);
            row.RollYear.Should().Be("2");
            row.ListPrice.Should().Be(3);
            row.AssessmentValue.Should().Be(4);
            row.AppraisalDate.Should().Be("01-Jan-2000");

            dispFilerepository.Verify(x => x.GetDispositionFileExportDeep(It.IsAny<DispositionFilter>()), Times.Once);
        }

        [Fact]
        public void GetDispositionFileExport_Success_Sales_Empty()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionView);
            var dispFilerepository = this._helper.GetService<Mock<IDispositionFileRepository>>();

            var filter = new DispositionFilter();
            var dispositionFile = EntityHelper.CreateDispositionFile(1);
            dispositionFile.FileNumber = "10-25-2023";
            dispositionFile.PimsDispositionSales = new List<PimsDispositionSale>();

            dispFilerepository.Setup(x => x.GetDispositionFileExportDeep(It.IsAny<DispositionFilter>()))
                        .Returns(new List<PimsDispositionFile>()
                        {
                            dispositionFile,
                        });

            // Act
            var result = service.GetDispositionFileExport(filter);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Count());
            dispFilerepository.Verify(x => x.GetDispositionFileExportDeep(It.IsAny<DispositionFilter>()), Times.Once);
        }

        [Fact]
        public void GetDispositionFileExport_Success_Sales()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionView);
            var dispFilerepository = this._helper.GetService<Mock<IDispositionFileRepository>>();

            var filter = new DispositionFilter();
            var dispositionFile = EntityHelper.CreateDispositionFile(1);
            dispositionFile.FileNumber = "10-25-2023";
            dispositionFile.PimsDispositionSales = new List<PimsDispositionSale>() { new PimsDispositionSale()
            {
               NetBookAmt = 1,
               GstCollectedAmt = 2,
               RealtorCommissionAmt = 3,
               RemediationAmt = 4,
               SaleFinalAmt = 5,
               SppAmt = 6,
               TotalCostAmt = 7,
               SaleCompletionDt = new DateOnly(2000,1,1),
               SaleFiscalYear = 2001,
            } };

            dispFilerepository.Setup(x => x.GetDispositionFileExportDeep(It.IsAny<DispositionFilter>()))
                        .Returns(new List<PimsDispositionFile>()
                        {
                            dispositionFile,
                        });

            // Act
            var result = service.GetDispositionFileExport(filter);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Count());
            var row = result[0];
            row.NetBookValue.Should().Be(1);
            row.GstCollected.Should().Be(2);
            row.RealtorCommission.Should().Be(3);
            row.RemediationCost.Should().Be(4);
            row.FinalSalePrice.Should().Be(5);
            row.SppAmount.Should().Be(6);
            row.TotalCostOfSale.Should().Be(7);
            row.NetBeforeSpp.Should().Be(-8);
            row.NetAfterSpp.Should().Be(-14);
            row.TotalCostOfSale.Should().Be(7);
            row.FiscalYearOfSale.Should().Be("2001");
            row.SaleCompletionDate.Should().Be("01-Jan-2000");

            dispFilerepository.Verify(x => x.GetDispositionFileExportDeep(It.IsAny<DispositionFilter>()), Times.Once);
        }

        [Fact]
        public void GetDispositionFileExport_Success_Sales_Purchasers()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionView);
            var dispFilerepository = this._helper.GetService<Mock<IDispositionFileRepository>>();

            var filter = new DispositionFilter();
            var dispositionFile = EntityHelper.CreateDispositionFile(1);
            dispositionFile.FileNumber = "10-25-2023";
            dispositionFile.PimsDispositionSales = new List<PimsDispositionSale>() { new PimsDispositionSale()
            {
               PimsDispositionPurchasers = new List<PimsDispositionPurchaser>()
               {
                   new PimsDispositionPurchaser()
                   {
                       Person = EntityHelper.CreatePerson(1, "first", "last"),
                       PersonId = 1,
                   },
                   new PimsDispositionPurchaser()
                   {
                       Organization = EntityHelper.CreateOrganization(1, "org"),
                   },
                   new PimsDispositionPurchaser()
                   {
                       Organization = EntityHelper.CreateOrganization(1, "org2"),
                       PrimaryContact = EntityHelper.CreatePerson(2, "primary", "contact")
                   },
               }
            } };

            dispFilerepository.Setup(x => x.GetDispositionFileExportDeep(It.IsAny<DispositionFilter>()))
                        .Returns(new List<PimsDispositionFile>()
                        {
                            dispositionFile,
                        });

            // Act
            var result = service.GetDispositionFileExport(filter);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Count());
            var row = result[0];
            row.PurchaserNames.Should().Be("last first|org (Primary: N/A)|org2 (Primary: contact primary)");

            dispFilerepository.Verify(x => x.GetDispositionFileExportDeep(It.IsAny<DispositionFilter>()), Times.Once);
        }

        #endregion

        #endregion

        #region Sale

        [Fact]
        public void GetDisposition_Sale_Should_Fail_NoPermission()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions();

            // Act
            Action act = () => service.GetDispositionFileSale(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void AddDispositionFile_Sale_Should_Fail_NoPermission()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions();

            // Act
            Action act = () => service.AddDispositionFileSale(new()
            {
                DispositionFileId = 1,
                DispositionSaleId = 1,
            });

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void AddDispositionFile_Sale_Should_Fail_Sale_Exists()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionEdit);
            var repository = this._helper.GetService<Mock<IDispositionFileRepository>>();

            repository.Setup(x => x.GetById(1)).Returns(new PimsDispositionFile()
            {
                DispositionFileId = 1,
                PimsDispositionSales = new List<PimsDispositionSale>() {
                    new PimsDispositionSale()
                    {
                        DispositionSaleId = 10,
                        DispositionFileId = 1,
                    },
                },
            });

            // Act
            Action act = () => service.AddDispositionFileSale(new()
            {
                DispositionFileId = 1,
            });

            // Assert
            act.Should().Throw<DuplicateEntityException>();
        }

        [Fact]
        public void AddDispositionFile_Sale_Success()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionEdit);
            var repository = this._helper.GetService<Mock<IDispositionFileRepository>>();

            repository.Setup(x => x.GetById(1)).Returns(new PimsDispositionFile()
            {
                DispositionFileId = 1,
                PimsDispositionOffers = new List<PimsDispositionOffer>() { },
            });
            repository.Setup(x => x.AddDispositionFileSale(It.IsAny<PimsDispositionSale>())).Returns(new PimsDispositionSale()
            {
                DispositionFileId = 1,
                DispositionSaleId = 100,
            });

            // Act
            var result = service.AddDispositionFileSale(new()
            {
                DispositionFileId = 1,
                DispositionSaleId = 0,
            });

            // Assert
            Assert.NotNull(result);
            repository.Verify(x => x.AddDispositionFileSale(It.IsAny<PimsDispositionSale>()), Times.Once);
        }

        [Fact]
        public void UpdateDispositionFile_Sale_Should_Fail_NoPermission()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions();

            // Act
            Action act = () => service.UpdateDispositionFileSale(new()
            {
                DispositionFileId = 1,
                DispositionSaleId = 10,
            });

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void UpdateDispositionFile_Sale_Success()
        {
            // Arrange
            var service = this.CreateDispositionServiceWithPermissions(Permissions.DispositionEdit);
            var repository = this._helper.GetService<Mock<IDispositionFileRepository>>();

            repository.Setup(x => x.GetById(1)).Returns(new PimsDispositionFile()
            {
                DispositionFileId = 1,
                PimsDispositionSales = new List<PimsDispositionSale>() {
                    new PimsDispositionSale()
                    {
                        DispositionFileId = 1,
                        DispositionSaleId = 10
                    }
                },
            });
            repository.Setup(x => x.UpdateDispositionFileSale(It.IsAny<PimsDispositionSale>())).Returns(new PimsDispositionSale()
            {
                DispositionFileId = 1,
                DispositionSaleId = 10,
            });

            // Act
            var result = service.UpdateDispositionFileSale(new()
            {
                DispositionFileId = 1,
                DispositionSaleId = 10,
                SaleFinalAmt = 2000,
            });

            // Assert
            Assert.NotNull(result);
            repository.Verify(x => x.UpdateDispositionFileSale(It.IsAny<PimsDispositionSale>()), Times.Once);
        }

        #endregion
    }
}
