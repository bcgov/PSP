using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DocumentFormat.OpenXml.Office2010.Excel;
using FluentAssertions;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
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
using Pims.Dal.Entities.Models;
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
            this._helper = new TestHelper();
        }

        private AcquisitionFileService CreateAcquisitionServiceWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            return this._helper.Create<AcquisitionFileService>(user);
        }

        #region Add
        [Fact]
        public void Add_Success()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileAdd);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.Add(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);
            var lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();
            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });

            // Act
            var result = service.Add(acqFile, new List<UserOverrideCode>());

            // Assert
            repository.Verify(x => x.Add(It.IsAny<PimsAcquisitionFile>()), Times.Once);
        }

        [Fact]
        public void Add_CannotDetermineRegion()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileAdd);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.RegionCode = 4;

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.Add(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);

            var lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();
            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });

            // Act
            Action act = () => service.Add(acqFile, new List<UserOverrideCode>());

            // Assert
            act.Should().Throw<BadRequestException>();
        }

        [Fact]
        public void Add_NoPermission()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions();

            var acqFile = EntityHelper.CreateAcquisitionFile();

            // Act
            Action act = () => service.Add(acqFile, new List<UserOverrideCode>());

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.Add(It.IsAny<PimsAcquisitionFile>()), Times.Never);
        }

        [Fact]
        public void Add_ContractorNotInTeamException_Fail_IsContractor()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileAdd);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.ConcurrencyControlNumber = 1;

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            var contractorUser = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: true);
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(contractorUser);

            // Act
            Action act = () => service.Add(acqFile, new List<UserOverrideCode>() { UserOverrideCode.UpdateRegion });

            // Assert
            act.Should().Throw<ContractorNotInTeamException>();
        }

        [Fact]
        public void Add_Success_IsContractor_AssignedToTeam()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileAdd);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.PimsAcquisitionFileTeams.Add(new PimsAcquisitionFileTeam() { PersonId = 1, AcqFlTeamProfileTypeCode = "test" });
            acqFile.ConcurrencyControlNumber = 1;

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();

            var newGuid = Guid.NewGuid();
            var contractorUser = EntityHelper.CreateUser(1, newGuid, username: "Test", isContractor: true);
            contractorUser.PersonId = 1;
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(contractorUser);

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.Add(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);
            var lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();
            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });

            // Act
            var result = service.Add(acqFile, new List<UserOverrideCode>());

            // Assert
            repository.Verify(x => x.Add(It.IsAny<PimsAcquisitionFile>()), Times.Once);
        }

        [Fact]
        public void Add_ThrowIfNull()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileAdd);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();

            // Act
            Action act = () => service.Add(null, new List<UserOverrideCode>());

            // Assert
            act.Should().Throw<ArgumentNullException>();
            repository.Verify(x => x.Add(It.IsAny<PimsAcquisitionFile>()), Times.Never);
        }

        [Fact]
        public void Add_DuplicateTeam()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileAdd);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.PimsAcquisitionFileTeams.Add(new PimsAcquisitionFileTeam() { PersonId = 1, AcqFlTeamProfileTypeCode = "test" });
            acqFile.PimsAcquisitionFileTeams.Add(new PimsAcquisitionFileTeam() { PersonId = 1, AcqFlTeamProfileTypeCode = "test" });

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            var lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();
            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });

            // Act
            Action act = () => service.Add(acqFile, new List<UserOverrideCode>());

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
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileView);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
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
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileView);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
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
            var service = this.CreateAcquisitionServiceWithPermissions();

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
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.ConcurrencyControlNumber = 1;

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.Update(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var compReqRepository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            compReqRepository.Setup(x => x.GetAllByAcquisitionFileId(It.IsAny<long>())).Returns(new List<PimsCompensationRequisition>());

            var lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();
            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            var agreementRepository = this._helper.GetService<Mock<IAgreementRepository>>();
            agreementRepository.Setup(x => x.GetAgreementsByAcquisitionFile(It.IsAny<long>())).Returns(new List<PimsAgreement>());

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditDetails(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            var result = service.Update(acqFile, new List<UserOverrideCode>() { UserOverrideCode.UpdateRegion });

            // Assert
            repository.Verify(x => x.Update(It.IsAny<PimsAcquisitionFile>()), Times.Once);
        }

        [Fact]
        public void Update_NotAuthorized_IsContractor()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.ConcurrencyControlNumber = 1;

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.Update(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);
            var lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();
            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(new PimsAcquisitionFile()
            {
                AcquisitionFileStatusTypeCode = acqFile.AcquisitionFileStatusTypeCode,
            });
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            var contractorUser = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: true);
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(contractorUser);

            // Act
            Action act = () => service.Update(acqFile, new List<UserOverrideCode>() { UserOverrideCode.UpdateRegion });

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void Update_CannotDetermineRegion()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.RegionCode = 4;
            acqFile.ConcurrencyControlNumber = 1;
            acqFile.AcquisitionFileStatusTypeCode = AcquisitionStatusTypes.ACTIVE.ToString();

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.Update(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetRegion(It.IsAny<long>())).Returns(acqFile.RegionCode);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var compReqRepository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            compReqRepository.Setup(x => x.GetAllByAcquisitionFileId(It.IsAny<long>())).Returns(new List<PimsCompensationRequisition>());

            var lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();
            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            var agreementRepository = this._helper.GetService<Mock<IAgreementRepository>>();
            agreementRepository.Setup(x => x.GetAgreementsByAcquisitionFile(It.IsAny<long>())).Returns(new List<PimsAgreement>());

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditDetails(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            Action act = () => service.Update(acqFile, new List<UserOverrideCode>() { UserOverrideCode.AddLocationToProperty });

            // Assert
            act.Should().Throw<BadRequestException>();
        }

        [Fact]
        public void Update_NoPermission()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions();

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            // Act
            Action act = () => service.Update(acqFile, new List<UserOverrideCode>() { UserOverrideCode.AddLocationToProperty });

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.Update(It.IsAny<PimsAcquisitionFile>()), Times.Never);
        }

        [Fact]
        public void Update_ThrowIf_Null()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();

            // Act
            Action act = () => service.Update(null, new List<UserOverrideCode>());

            // Assert
            act.Should().Throw<ArgumentNullException>();
            repository.Verify(x => x.Update(It.IsAny<PimsAcquisitionFile>()), Times.Never);
        }

        [Fact]
        public void Update_Region_Violation()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.AcquisitionFileStatusTypeCode = AcquisitionStatusTypes.ACTIVE.ToString();

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetRegion(It.IsAny<long>())).Returns((short)(acqFile.RegionCode + 100));
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            var agreementRepository = this._helper.GetService<Mock<IAgreementRepository>>();
            agreementRepository.Setup(x => x.GetAgreementsByAcquisitionFile(It.IsAny<long>())).Returns(new List<PimsAgreement>());

            var compReqRepository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            compReqRepository.Setup(x => x.GetAllByAcquisitionFileId(It.IsAny<long>())).Returns(new List<PimsCompensationRequisition>());

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditDetails(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            Action act = () => service.Update(acqFile, new List<UserOverrideCode>());

            // Assert
            var ex = act.Should().Throw<UserOverrideException>();
            ex.Which.UserOverride.Should().Be(UserOverrideCode.UpdateRegion);
            repository.Verify(x => x.Update(It.IsAny<PimsAcquisitionFile>()), Times.Never);
        }

        [Fact]
        public void Update_Version_Violation()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(2);

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            Action act = () => service.Update(acqFile, new List<UserOverrideCode>());

            // Assert
            var ex = act.Should().Throw<DbUpdateConcurrencyException>();
        }

        [Fact]
        public void Update_Drafts_Violation()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var agreementRepository = this._helper.GetService<Mock<IAgreementRepository>>();
            agreementRepository.Setup(x => x.GetAgreementsByAcquisitionFile(It.IsAny<long>())).Returns(new List<PimsAgreement>() { new PimsAgreement() { AgreementStatusTypeCode = "DRAFT" } });

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            Action act = () => service.Update(acqFile, new List<UserOverrideCode>());

            // Assert
            act.Should().Throw<BusinessRuleViolationException>();
        }

        [Fact]
        public void Update_DraftsNotComplete_NoViolation()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var agreementRepository = this._helper.GetService<Mock<IAgreementRepository>>();
            agreementRepository.Setup(x => x.GetAgreementsByAcquisitionFile(It.IsAny<long>())).Returns(new List<PimsAgreement>() { new PimsAgreement() { AgreementStatusTypeCode = "DRAFT" } });

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            var statusSolver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            statusSolver.Setup(x => x.CanEditDetails(It.IsAny<AcquisitionStatusTypes>())).Returns(true);

            // Act
            Action act = () => service.Update(acqFile, new List<UserOverrideCode>());

            // Assert
            act.Should().NotThrow<BusinessRuleViolationException>();
        }

        [Fact]
        public void Update_Success_Region_UserOverride()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.Update(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();
            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            var compReqRepository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            compReqRepository.Setup(x => x.GetAllByAcquisitionFileId(It.IsAny<long>())).Returns(new List<PimsCompensationRequisition>());

            var agreementRepository = this._helper.GetService<Mock<IAgreementRepository>>();
            agreementRepository.Setup(x => x.GetAgreementsByAcquisitionFile(It.IsAny<long>())).Returns(new List<PimsAgreement>());

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditDetails(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            var result = service.Update(acqFile, new List<UserOverrideCode>() { UserOverrideCode.UpdateRegion });

            // Assert
            repository.Verify(x => x.Update(It.IsAny<PimsAcquisitionFile>()), Times.Once);
        }

        [Fact]
        public void Update_PropertyOfInterest_Violation_Owned()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.AcquisitionFileStatusTypeCode = "COMPLT";
            acqFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345);
            property.IsPropertyOfInterest = true;
            var propertyAcqFile = new PimsPropertyAcquisitionFile() { Property = property };
            acqFile.PimsPropertyAcquisitionFiles = new List<PimsPropertyAcquisitionFile>() { propertyAcqFile };

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var filePropertyRepository = this._helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>())).Returns(acqFile.PimsPropertyAcquisitionFiles.ToList());

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            var takeRepository = this._helper.GetService<Mock<ITakeRepository>>();
            takeRepository.Setup(x => x.GetAllByPropertyAcquisitionFileId(It.IsAny<long>())).Returns(new List<PimsTake>() { new PimsTake() { IsNewHighwayDedication = true, IsAcquiredForInventory = true } });

            var agreementRepository = this._helper.GetService<Mock<IAgreementRepository>>();
            agreementRepository.Setup(x => x.GetAgreementsByAcquisitionFile(It.IsAny<long>())).Returns(new List<PimsAgreement>());

            var compensationRequisitionRepository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            compensationRequisitionRepository.Setup(x => x.GetAllByAcquisitionFileId(It.IsAny<long>())).Returns(new List<PimsCompensationRequisition>());

            var lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();
            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditDetails(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            Action act = () => service.Update(acqFile, new List<UserOverrideCode>() { UserOverrideCode.UpdateRegion });

            // Assert
            var ex = act.Should().Throw<UserOverrideException>();
            ex.Which.UserOverride.Should().Be(UserOverrideCode.PoiToInventory);
            repository.Verify(x => x.Update(It.IsAny<PimsAcquisitionFile>()), Times.Never);
        }

        [Fact]
        public void Update_PropertyOfInterest_Violation_Other()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.AcquisitionFileStatusTypeCode = "COMPLT";
            acqFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345);
            property.IsPropertyOfInterest = true;
            var propertyAcqFile = new PimsPropertyAcquisitionFile() { Property = property };
            acqFile.PimsPropertyAcquisitionFiles = new List<PimsPropertyAcquisitionFile>() { propertyAcqFile };

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var filePropertyRepository = this._helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>())).Returns(acqFile.PimsPropertyAcquisitionFiles.ToList());

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            var takeRepository = this._helper.GetService<Mock<ITakeRepository>>();
            takeRepository.Setup(x => x.GetAllByPropertyAcquisitionFileId(It.IsAny<long>())).Returns(new List<PimsTake>() { new PimsTake() { IsNewInterestInSrw = true } });

            var agreementRepository = this._helper.GetService<Mock<IAgreementRepository>>();
            agreementRepository.Setup(x => x.GetAgreementsByAcquisitionFile(It.IsAny<long>())).Returns(new List<PimsAgreement>());

            var compReqRepository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            compReqRepository.Setup(x => x.GetAllByAcquisitionFileId(It.IsAny<long>())).Returns(new List<PimsCompensationRequisition>());

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditDetails(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            Action act = () => service.Update(acqFile, new List<UserOverrideCode>() { UserOverrideCode.UpdateRegion });

            // Assert
            var ex = act.Should().Throw<UserOverrideException>();
            ex.Which.UserOverride.Should().Be(UserOverrideCode.PoiToInventory);
            repository.Verify(x => x.Update(It.IsAny<PimsAcquisitionFile>()), Times.Never);
        }

        [Fact]
        public void Update_Success_PropertyOfInterest_UserOverride()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.AcquisitionFileStatusTypeCode = "COMPLT";
            acqFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345);
            property.IsPropertyOfInterest = true;
            var propertyAcqFile = new PimsPropertyAcquisitionFile() { Property = property };
            acqFile.PimsPropertyAcquisitionFiles = new List<PimsPropertyAcquisitionFile>() { propertyAcqFile };

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);
            repository.Setup(x => x.Update(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);

            var filePropertyRepository = this._helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>())).Returns(acqFile.PimsPropertyAcquisitionFiles.ToList());

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.TransferFileProperty(It.IsAny<PimsProperty>(), It.IsAny<Boolean>(), It.IsAny<Boolean>(), It.IsAny<Boolean>()));

            var lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();
            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            var compReqRepository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            compReqRepository.Setup(x => x.GetAllByAcquisitionFileId(It.IsAny<long>())).Returns(new List<PimsCompensationRequisition>());

            var agreementRepository = this._helper.GetService<Mock<IAgreementRepository>>();
            agreementRepository.Setup(x => x.GetAgreementsByAcquisitionFile(It.IsAny<long>())).Returns(new List<PimsAgreement>());

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditDetails(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            var result = service.Update(acqFile, new List<UserOverrideCode>() { UserOverrideCode.UpdateRegion, UserOverrideCode.PoiToInventory });

            // Assert
            repository.Verify(x => x.Update(It.IsAny<PimsAcquisitionFile>()), Times.Once);
            propertyRepository.Verify(x => x.TransferFileProperty(It.IsAny<PimsProperty>(), It.IsAny<Boolean>(), It.IsAny<Boolean>(), It.IsAny<Boolean>()), Times.Once);
        }

        public static IEnumerable<object[]> takesTestParameters = new List<object[]>() {
            new object[] { new List<PimsTake>() { new PimsTake() { IsNewHighwayDedication = true }, new PimsTake() { IsNewLicenseToConstruct = true } }, true, false },
            new object[] { new List<PimsTake>() { new PimsTake() { IsNewLandAct = true }, new PimsTake() { IsNewLicenseToConstruct = true } }, true, false }, // core inventory takes priority over other interest
            new object[] { new List<PimsTake>() { new PimsTake() { IsNewLandAct = true, LandActEndDt = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)) }, new PimsTake() { IsNewLicenseToConstruct = true } }, false, false }, // should ignore any expired takes
            new object[] { new List<PimsTake>(), false, true }, // No takes should be core inventory
            new object[] { new List<PimsTake>() { new PimsTake() { IsNewLandAct = true, LandActEndDt = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)) } }, true, false }, // only expired takes is the same as no takes
            new object[] { new List<PimsTake>() { new PimsTake() { IsNewHighwayDedication = true }, new PimsTake() { IsNewLicenseToConstruct = true } }, true, false },
            new object[] { new List<PimsTake>() { new PimsTake() { IsNewHighwayDedication = true }, new PimsTake() { IsNewLicenseToConstruct = true } }, true, false },
            new object[] { new List<PimsTake>() { new PimsTake() { IsNewHighwayDedication = true }, new PimsTake() { IsNewLicenseToConstruct = true } }, true, false },
            new object[] { new List<PimsTake>() { new PimsTake() { IsNewLandAct = true, LandActTypeCode = "Section 15" } }, false, false },
            new object[] { new List<PimsTake>() { new PimsTake() { IsNewLandAct = true, LandActTypeCode = "Section 16" } }, true, false },
            new object[] { new List<PimsTake>() { new PimsTake() { IsNewLandAct = true, LandActTypeCode = "Section 17" } }, false, false },
            new object[] { new List<PimsTake>() { new PimsTake() { IsNewLandAct = true, LandActTypeCode = "NOI" } }, false, false },
            new object[] { new List<PimsTake>() { new PimsTake() { IsNewLandAct = true, LandActTypeCode = "Section 66" } }, false, false },
            new object[] { new List<PimsTake>() { new PimsTake() { IsNewLandAct = true, LandActTypeCode = "Crown Grant (New)" } }, true, false },
            new object[] { new List<PimsTake>() { new PimsTake() { IsNewInterestInSrw = true } }, false, false },
            new object[] { new List<PimsTake>() { new PimsTake() { IsNewLicenseToConstruct = true } }, false, false },
            new object[] { new List<PimsTake>() { new PimsTake() { IsThereSurplus = true } }, true, false },
            new object[] { new List<PimsTake>() { new PimsTake() { IsNewHighwayDedication = true } }, false, true },
            new object[] { new List<PimsTake>() { new PimsTake() { IsNewHighwayDedication = true, IsAcquiredForInventory = false } }, false, true },
            new object[] { new List<PimsTake>() { new PimsTake() { IsNewHighwayDedication = false, IsAcquiredForInventory = true } }, true, false },
            new object[] { new List<PimsTake>() { new PimsTake() { IsNewLandAct = true, LandActTypeCode = "Section 16", IsNewHighwayDedication = true, IsAcquiredForInventory = false } }, false, true },
        }.ToArray();

        [Theory, MemberData(nameof(takesTestParameters))]
        public void Update_Success_Transfer_MultipleTakes_Core(List<PimsTake> takes, bool isOwned, bool isPropertyOfInterest)
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.AcquisitionFileStatusTypeCode = "COMPLT";
            acqFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345);
            property.IsPropertyOfInterest = true;
            var propertyAcqFile = new PimsPropertyAcquisitionFile() { Property = property };
            acqFile.PimsPropertyAcquisitionFiles = new List<PimsPropertyAcquisitionFile>() { propertyAcqFile };

            var repository = mockForMultipleTakes(acqFile);

            var captureProperty = new PimsProperty();

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.TransferFileProperty(captureProperty, It.IsAny<Boolean>(), It.IsAny<Boolean>(), It.IsAny<Boolean>()));

            var takeRepository = this._helper.GetService<Mock<ITakeRepository>>();
            takeRepository.Setup(x => x.GetAllByPropertyAcquisitionFileId(It.IsAny<long>())).Returns(takes);

            var agreementRepository = this._helper.GetService<Mock<IAgreementRepository>>();
            agreementRepository.Setup(x => x.GetAgreementsByAcquisitionFile(It.IsAny<long>())).Returns(new List<PimsAgreement>());

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditDetails(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            var result = service.Update(acqFile, new List<UserOverrideCode>() { UserOverrideCode.UpdateRegion, UserOverrideCode.PoiToInventory });

            // Assert
            repository.Verify(x => x.Update(It.IsAny<PimsAcquisitionFile>()), Times.Once);
            propertyRepository.Verify(x => x.TransferFileProperty(It.IsAny<PimsProperty>(), isOwned, isPropertyOfInterest, It.IsAny<Boolean>()), Times.Once);
        }

        [Fact]
        public void Update_Success_AddsNote()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.ConcurrencyControlNumber = 1;
            acqFile.AppCreateUserid = "TESTER";

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            var noteRepository = this._helper.GetService<Mock<IEntityNoteRepository>>();
            var lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();

            var compReqRepository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            compReqRepository.Setup(x => x.GetAllByAcquisitionFileId(It.IsAny<long>())).Returns(new List<PimsCompensationRequisition>());

            repository.Setup(x => x.Update(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(new PimsAcquisitionFile()
            {
                AcquisitionFileStatusTypeCode = "CLOSED",
                AcquisitionFileStatusTypeCodeNavigation = new PimsAcquisitionFileStatusType() { Description = "Closed" },
            });
            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });
            lookupRepository.Setup(x => x.GetAllAcquisitionFileStatusTypes()).Returns(new PimsAcquisitionFileStatusType[]{ new PimsAcquisitionFileStatusType() {
                Id = acqFile.AcquisitionFileStatusTypeCodeNavigation.Id,
                Description = acqFile.AcquisitionFileStatusTypeCodeNavigation.Description,
            },});

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            var agreementRepository = this._helper.GetService<Mock<IAgreementRepository>>();
            agreementRepository.Setup(x => x.GetAgreementsByAcquisitionFile(It.IsAny<long>())).Returns(new List<PimsAgreement>());

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditDetails(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            var result = service.Update(acqFile, new List<UserOverrideCode>() { UserOverrideCode.UpdateRegion });

            // Assert
            repository.Verify(x => x.Update(It.IsAny<PimsAcquisitionFile>()), Times.Once);
            noteRepository.Verify(x => x.Add(It.Is<PimsAcquisitionFileNote>(x => x.AcquisitionFileId == 1
                    && x.Note.NoteTxt == "Acquisition File status changed from Closed to Active")), Times.Once);
        }

        [Fact]
        public void Update_InvalidStatus()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.ConcurrencyControlNumber = 1;

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.Update(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var compReqRepository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            compReqRepository.Setup(x => x.GetAllByAcquisitionFileId(It.IsAny<long>())).Returns(new List<PimsCompensationRequisition>());

            var lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();
            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            var agreementRepository = this._helper.GetService<Mock<IAgreementRepository>>();
            agreementRepository.Setup(x => x.GetAgreementsByAcquisitionFile(It.IsAny<long>())).Returns(new List<PimsAgreement>());

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditDetails(It.IsAny<AcquisitionStatusTypes?>())).Returns(false);

            // Act
            Action act = () => service.Update(acqFile, new List<UserOverrideCode>() { UserOverrideCode.UpdateRegion });

            // Assert
            act.Should().Throw<BusinessRuleViolationException>();
        }

        [Fact]
        public void UpdateProperties_Success()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit, Permissions.AcquisitionFileAdd, Permissions.AcquisitionFileView);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.ConcurrencyControlNumber = 1;

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var filePropertyRepository = this._helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>())).Returns(acqFile.PimsPropertyAcquisitionFiles.ToList());

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            service.UpdateProperties(acqFile, new List<UserOverrideCode>());

            // Assert
            filePropertyRepository.Verify(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void UpdateProperties_Takes_Violation()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit, Permissions.AcquisitionFileAdd, Permissions.AcquisitionFileView);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345);
            var propertyAcqFile = new PimsPropertyAcquisitionFile() { Property = property };
            propertyAcqFile.PimsTakes = new List<PimsTake>() { new PimsTake() };
            acqFile.PimsPropertyAcquisitionFiles = new List<PimsPropertyAcquisitionFile>() { propertyAcqFile };

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>())).Returns(property);

            var filePropertyRepository = this._helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>())).Returns(acqFile.PimsPropertyAcquisitionFiles.ToList());

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            Action act = () => service.UpdateProperties(EntityHelper.CreateAcquisitionFile(), new List<UserOverrideCode>());

            // Assert
            act.Should().Throw<BusinessRuleViolationException>();
        }

        [Fact]
        public void UpdateProperties_MatchProperties_Success()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345);
            acqFile.PimsPropertyAcquisitionFiles = new List<PimsPropertyAcquisitionFile>() { new PimsPropertyAcquisitionFile() { Property = property } };

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>())).Returns(property);

            var filePropertyRepository = this._helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>())).Returns(acqFile.PimsPropertyAcquisitionFiles.ToList());

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            service.UpdateProperties(acqFile, new List<UserOverrideCode>() { UserOverrideCode.AddLocationToProperty });

            // Assert
            filePropertyRepository.Verify(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void UpdateProperties_MatchProperties_NewProperty_Success()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345);
            acqFile.PimsPropertyAcquisitionFiles = new List<PimsPropertyAcquisitionFile>() { new PimsPropertyAcquisitionFile() { Property = property } };

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            PimsPropertyAcquisitionFile updatedAcquisitionFileProperty = null;
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var filePropertyRepository = this._helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>())).Returns(acqFile.PimsPropertyAcquisitionFiles.ToList());
            filePropertyRepository.Setup(x => x.Add(It.IsAny<PimsPropertyAcquisitionFile>())).Callback<PimsPropertyAcquisitionFile>(x => updatedAcquisitionFileProperty = x).Returns(acqFile.PimsPropertyAcquisitionFiles.FirstOrDefault());

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>())).Throws<KeyNotFoundException>();

            var coordinateService = this._helper.GetService<Mock<ICoordinateTransformService>>();
            coordinateService.Setup(x => x.TransformCoordinates(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Coordinate>())).Returns(new Coordinate(924046.3314288399, 1088892.9140135897));

            var propertyService = this._helper.GetService<Mock<IPropertyService>>();
            propertyService.Setup(x => x.PopulateNewProperty(It.IsAny<PimsProperty>(), It.IsAny<Boolean>(), It.IsAny<Boolean>())).Returns(new PimsProperty()
            {
                PropertyClassificationTypeCode = "UNKNOWN",
                PropertyDataSourceEffectiveDate = DateOnly.FromDateTime(System.DateTime.Now),
                PropertyDataSourceTypeCode = "PMBC",
                PropertyTypeCode = "UNKNOWN",
                PropertyStatusTypeCode = "UNKNOWN",
                SurplusDeclarationTypeCode = "UNKNOWN",
                IsPropertyOfInterest = true,
            });

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            service.UpdateProperties(acqFile, new List<UserOverrideCode>());

            // Assert
            // since this is a new property, the following default fields should be set.
            var updatedProperty = updatedAcquisitionFileProperty.Property;
            updatedProperty.PropertyClassificationTypeCode.Should().Be("UNKNOWN");
            updatedProperty.PropertyTypeCode.Should().Be("UNKNOWN");
            updatedProperty.PropertyStatusTypeCode.Should().Be("UNKNOWN");
            updatedProperty.SurplusDeclarationTypeCode.Should().Be("UNKNOWN");
            updatedProperty.PropertyDataSourceEffectiveDate.Should().Be(DateOnly.FromDateTime(DateTime.Now));
            updatedProperty.PropertyDataSourceTypeCode.Should().Be("PMBC");
            updatedProperty.IsPropertyOfInterest.Should().Be(true);

            filePropertyRepository.Verify(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void UpdateProperties_UpdatePropertyFile_Success()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var property = EntityHelper.CreateProperty(12345);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.PimsPropertyAcquisitionFiles = new List<PimsPropertyAcquisitionFile>() { new PimsPropertyAcquisitionFile() { Internal_Id = 1, Property = property } };
            acqFile.ConcurrencyControlNumber = 1;

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var filePropertyRepository = this._helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>())).Returns(new List<PimsPropertyAcquisitionFile>() { new PimsPropertyAcquisitionFile() { Internal_Id = 1, Property = property, PropertyName = "updated" } });

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>())).Throws<KeyNotFoundException>();

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            service.UpdateProperties(acqFile, new List<UserOverrideCode>());

            // Assert
            filePropertyRepository.Verify(x => x.Update(It.IsAny<PimsPropertyAcquisitionFile>()), Times.Once);
        }

        [Fact]
        public void UpdateProperties_RemovePropertyFile_Success()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345);

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var filePropertyRepository = this._helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>())).Returns(new List<PimsPropertyAcquisitionFile>() { new PimsPropertyAcquisitionFile() { Property = property } });

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>())).Throws<KeyNotFoundException>();

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            service.UpdateProperties(acqFile, new List<UserOverrideCode>());

            // Assert
            filePropertyRepository.Verify(x => x.Delete(It.IsAny<PimsPropertyAcquisitionFile>()), Times.Once);
        }

        [Fact]
        public void UpdateProperties_RemoveProperty_Success()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345);
            property.IsPropertyOfInterest = true;
            property.PimsPropertyResearchFiles = new List<PimsPropertyResearchFile>();
            property.PimsPropertyLeases = new List<PimsPropertyLease>();
            property.PimsPropertyAcquisitionFiles = new List<PimsPropertyAcquisitionFile>() { new PimsPropertyAcquisitionFile() };

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var filePropertyRepository = this._helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>())).Returns(new List<PimsPropertyAcquisitionFile>() { new PimsPropertyAcquisitionFile() { Property = property } });
            filePropertyRepository.Setup(x => x.GetAcquisitionFilePropertyRelatedCount(It.IsAny<long>())).Returns(1);

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>())).Throws<KeyNotFoundException>();
            propertyRepository.Setup(x => x.GetAllAssociationsById(It.IsAny<long>())).Returns(property);

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            service.UpdateProperties(acqFile, new List<UserOverrideCode>());

            // Assert
            filePropertyRepository.Verify(x => x.Delete(It.IsAny<PimsPropertyAcquisitionFile>()), Times.Once);
            propertyRepository.Verify(x => x.Delete(It.IsAny<PimsProperty>()), Times.Once);
        }

        [Fact]
        public void UpdateProperties_NoPermission()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions();

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.Update(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);

            // Act
            Action act = () => service.UpdateProperties(acqFile, new List<UserOverrideCode>());

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.Update(It.IsAny<PimsAcquisitionFile>()), Times.Never);
        }

        [Fact]
        public void UpdateProperties_NotAuthorized_Contractor()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.Update(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            var contractorUser = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: true);
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(contractorUser);

            var acqFileRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            acqFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            // Act
            Action act = () => service.UpdateProperties(acqFile, new List<UserOverrideCode>());

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void UpdateProperties_ExistingTakes()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345);
            property.IsPropertyOfInterest = true;

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var filePropertyRepository = this._helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>())).Returns(new List<PimsPropertyAcquisitionFile>() { new PimsPropertyAcquisitionFile() { Property = property, PimsTakes = new List<PimsTake>() { new PimsTake() { TakeId = 1 } } } });
            filePropertyRepository.Setup(x => x.GetAcquisitionFilePropertyRelatedCount(It.IsAny<long>())).Returns(1);

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>())).Throws<KeyNotFoundException>();
            propertyRepository.Setup(x => x.GetAllAssociationsById(It.IsAny<long>())).Returns(property);

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            Action act = () => service.UpdateProperties(acqFile, new List<UserOverrideCode>());

            // Assert
            act.Should().Throw<BusinessRuleViolationException>();
        }

        [Fact]
        public void UpdateProperties_ExistingInterestHolders()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345);
            property.IsPropertyOfInterest = true;

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var filePropertyRepository = this._helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>())).Returns(new List<PimsPropertyAcquisitionFile>() { new PimsPropertyAcquisitionFile() { Property = property, PimsInthldrPropInterests = new List<PimsInthldrPropInterest>() { new PimsInthldrPropInterest() { InterestHolderId = 1 } } } });
            filePropertyRepository.Setup(x => x.GetAcquisitionFilePropertyRelatedCount(It.IsAny<long>())).Returns(1);

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>())).Throws<KeyNotFoundException>();
            propertyRepository.Setup(x => x.GetAllAssociationsById(It.IsAny<long>())).Returns(property);

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            Action act = () => service.UpdateProperties(acqFile, new List<UserOverrideCode>());

            // Assert
            act.Should().Throw<BusinessRuleViolationException>();
        }

        [Fact]
        public void Update_DuplicateTeam()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.PimsAcquisitionFileTeams.Add(new PimsAcquisitionFileTeam() { PersonId = 1, AcqFlTeamProfileTypeCode = "test" });
            acqFile.PimsAcquisitionFileTeams.Add(new PimsAcquisitionFileTeam() { PersonId = 1, AcqFlTeamProfileTypeCode = "test" });
            acqFile.ConcurrencyControlNumber = 1;
            acqFile.AcquisitionFileStatusTypeCode = AcquisitionStatusTypes.ACTIVE.ToString();

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.Update(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetRegion(It.IsAny<long>())).Returns(acqFile.RegionCode);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();
            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            var agreementRepository = this._helper.GetService<Mock<IAgreementRepository>>();
            agreementRepository.Setup(x => x.GetAgreementsByAcquisitionFile(It.IsAny<long>())).Returns(new List<PimsAgreement>());

            var compReqRepository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            compReqRepository.Setup(x => x.GetAllByAcquisitionFileId(It.IsAny<long>())).Returns(new List<PimsCompensationRequisition>());

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditDetails(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            Action act = () => service.Update(acqFile, new List<UserOverrideCode>() { UserOverrideCode.AddPropertyToInventory });

            // Assert
            act.Should().Throw<BadRequestException>();
        }

        [Fact]
        public void Update_Contractor_Removed()
        {
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.PimsAcquisitionFileTeams.Add(new PimsAcquisitionFileTeam() { PersonId = 1, AcqFlTeamProfileTypeCode = EnumUserTypeCodes.CONTRACT.ToString(), });

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();

            var contractorUser = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: true);
            contractorUser.PersonId = 1;
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(contractorUser);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetRegion(It.IsAny<long>())).Returns(acqFile.RegionCode);

            var agreementRepository = this._helper.GetService<Mock<IAgreementRepository>>();
            agreementRepository.Setup(x => x.GetAgreementsByAcquisitionFile(It.IsAny<long>())).Returns(new List<PimsAgreement>());

            var compReqRepository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            compReqRepository.Setup(x => x.GetAllByAcquisitionFileId(It.IsAny<long>())).Returns(new List<PimsCompensationRequisition>());

            var updatedFile = EntityHelper.CreateAcquisitionFile();
            updatedFile.ConcurrencyControlNumber = 1;

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditDetails(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            Action act = () => service.Update(updatedFile, new List<UserOverrideCode>() { UserOverrideCode.AddPropertyToInventory });

            // Assert
            act.Should().Throw<UserOverrideException>();
        }

        [Fact]
        public void Update_FKExeption_Removed_AcqFileOwner()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.PimsAcquisitionOwners = new List<PimsAcquisitionOwner>() {
                new PimsAcquisitionOwner() {
                    AcquisitionOwnerId = 100,
                },
            };

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.Update(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var compReqRepository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            compReqRepository.Setup(x => x.GetAllByAcquisitionFileId(It.IsAny<long>()))
                .Returns(new List<PimsCompensationRequisition>() {
                    new PimsCompensationRequisition() {
                        CompensationRequisitionId = 1,
                        AcquisitionFileId = acqFile.Internal_Id,
                        AcquisitionOwnerId = 100,
                    },
                });

            var lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();
            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            var agreementRepository = this._helper.GetService<Mock<IAgreementRepository>>();
            agreementRepository.Setup(x => x.GetAgreementsByAcquisitionFile(It.IsAny<long>())).Returns(new List<PimsAgreement>());

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditDetails(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            var updatedAcqFile = EntityHelper.CreateAcquisitionFile();
            Action act = () => service.Update(updatedAcqFile, new List<UserOverrideCode>() { UserOverrideCode.UpdateRegion });

            // Assert
            act.Should().Throw<ForeignKeyDependencyException>();
            repository.Verify(x => x.Update(It.IsAny<PimsAcquisitionFile>()), Times.Never);
        }

        [Fact]
        public void Update_FKExeption_Removed_OwnerSolicitor()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.PimsInterestHolders = new List<PimsInterestHolder>() {
                new PimsInterestHolder() {
                    InterestHolderTypeCode = "AOSLCTR",
                    InterestHolderId = 100,
                },
            };

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.Update(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var compReqRepository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            compReqRepository.Setup(x => x.GetAllByAcquisitionFileId(It.IsAny<long>()))
                .Returns(new List<PimsCompensationRequisition>() {
                    new PimsCompensationRequisition() {
                        CompensationRequisitionId = 1,
                        AcquisitionFileId = acqFile.Internal_Id,
                        InterestHolderId = 100,
                    },
                });

            var lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();
            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            var agreementRepository = this._helper.GetService<Mock<IAgreementRepository>>();
            agreementRepository.Setup(x => x.GetAgreementsByAcquisitionFile(It.IsAny<long>())).Returns(new List<PimsAgreement>());

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditDetails(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            var updatedAcqFile = EntityHelper.CreateAcquisitionFile();
            Action act = () => service.Update(updatedAcqFile, new List<UserOverrideCode>() { UserOverrideCode.UpdateRegion });

            // Assert
            act.Should().Throw<ForeignKeyDependencyException>();
            repository.Verify(x => x.Update(It.IsAny<PimsAcquisitionFile>()), Times.Never);
        }

        [Fact]
        public void Update_FKExeption_Removed_OwnerRepresentative()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.PimsInterestHolders = new List<PimsInterestHolder>() {
                new PimsInterestHolder() {
                    InterestHolderTypeCode = "AOREP",
                    InterestHolderId = 100,
                },
            };

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.Update(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var compReqRepository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            compReqRepository.Setup(x => x.GetAllByAcquisitionFileId(It.IsAny<long>()))
                .Returns(new List<PimsCompensationRequisition>() {
                    new PimsCompensationRequisition() {
                        CompensationRequisitionId = 1,
                        AcquisitionFileId = acqFile.Internal_Id,
                        InterestHolderId = 100,
                    },
                });

            var lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();
            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            var agreementRepository = this._helper.GetService<Mock<IAgreementRepository>>();
            agreementRepository.Setup(x => x.GetAgreementsByAcquisitionFile(It.IsAny<long>())).Returns(new List<PimsAgreement>());

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditDetails(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            var updatedAcqFile = EntityHelper.CreateAcquisitionFile();
            Action act = () => service.Update(updatedAcqFile, new List<UserOverrideCode>() { UserOverrideCode.UpdateRegion });

            // Assert
            act.Should().Throw<ForeignKeyDependencyException>();
            repository.Verify(x => x.Update(It.IsAny<PimsAcquisitionFile>()), Times.Never);
        }

        [Fact]
        public void Update_FKExeption_Removed_PersonOfInterest()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.PimsAcquisitionFileTeams = new List<PimsAcquisitionFileTeam>() {
                new PimsAcquisitionFileTeam() {
                    AcquisitionFileTeamId = 100,
                },
            };

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.Update(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var compReqRepository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            compReqRepository.Setup(x => x.GetAllByAcquisitionFileId(It.IsAny<long>()))
                .Returns(new List<PimsCompensationRequisition>() {
                    new PimsCompensationRequisition() {
                        CompensationRequisitionId = 1,
                        AcquisitionFileId = acqFile.Internal_Id,
                        AcquisitionFileTeamId = 100,
                    },
                });

            var lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();
            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            var agreementRepository = this._helper.GetService<Mock<IAgreementRepository>>();
            agreementRepository.Setup(x => x.GetAgreementsByAcquisitionFile(It.IsAny<long>())).Returns(new List<PimsAgreement>());

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditDetails(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            var updatedAcqFile = EntityHelper.CreateAcquisitionFile();
            Action act = () => service.Update(updatedAcqFile, new List<UserOverrideCode>() { UserOverrideCode.UpdateRegion });

            // Assert
            act.Should().Throw<ForeignKeyDependencyException>();
            repository.Verify(x => x.Update(It.IsAny<PimsAcquisitionFile>()), Times.Never);
        }

        [Fact]
        public void Update_NewTotalAllowableCompensation_Success()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.PimsAcquisitionFileTeams = new List<PimsAcquisitionFileTeam>() {
                new PimsAcquisitionFileTeam() {
                    AcquisitionFileTeamId = 100,
                },
            };

            var values = new List<PimsAcquisitionFile>();
            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.Update(Capture.In(values))).Returns(acqFile);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);
            repository.Setup(x => x.GetRegion(It.IsAny<long>())).Returns(1);

            var compReqRepository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            compReqRepository.Setup(x => x.GetAllByAcquisitionFileId(It.IsAny<long>()))
                .Returns(new List<PimsCompensationRequisition>() {
                    new PimsCompensationRequisition() {
                        CompensationRequisitionId = 1,
                        AcquisitionFileId = acqFile.Internal_Id,
                    },
                });

            var lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();
            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });

            var agreementRepository = this._helper.GetService<Mock<IAgreementRepository>>();
            agreementRepository.Setup(x => x.GetAgreementsByAcquisitionFile(It.IsAny<long>())).Returns(new List<PimsAgreement>());

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditDetails(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            var updatedAcqFile = EntityHelper.CreateAcquisitionFile();
            updatedAcqFile.TotalAllowableCompensation = 100;
            var response = service.Update(updatedAcqFile, new List<UserOverrideCode>() { });

            // Assert
            values.FirstOrDefault().TotalAllowableCompensation.Should().Be(100);
        }

        [Fact]
        public void Update_NewTotalAllowableCompensation_Failure_LessThenCurrentFinancials()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.PimsAcquisitionFileTeams = new List<PimsAcquisitionFileTeam>() {
                new PimsAcquisitionFileTeam() {
                    AcquisitionFileTeamId = 100,
                },
            };

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.Update(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);
            repository.Setup(x => x.GetRegion(It.IsAny<long>())).Returns(1);

            var compReqRepository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            compReqRepository.Setup(x => x.GetAllByAcquisitionFileId(It.IsAny<long>()))
                .Returns(new List<PimsCompensationRequisition>() {
                    new PimsCompensationRequisition() {
                        CompensationRequisitionId = 1,
                        AcquisitionFileId = acqFile.Internal_Id,
                    },
                });

            var compReqService = this._helper.GetService<Mock<ICompReqFinancialService>>();
            compReqService.Setup(c => c.GetAllByAcquisitionFileId(It.IsAny<long>(), true)).Returns(
                new List<PimsCompReqFinancial>() { new PimsCompReqFinancial() { TotalAmt = 1000 } });

            var agreementRepository = this._helper.GetService<Mock<IAgreementRepository>>();
            agreementRepository.Setup(x => x.GetAgreementsByAcquisitionFile(It.IsAny<long>())).Returns(new List<PimsAgreement>());

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditDetails(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            var updatedAcqFile = EntityHelper.CreateAcquisitionFile();
            updatedAcqFile.TotalAllowableCompensation = 100;
            Action act = () => service.Update(updatedAcqFile, new List<UserOverrideCode>() { });

            // Assert
            act.Should().Throw<BusinessRuleViolationException>();
        }
        #endregion

        #region Checklist
        [Fact]
        public void GetChecklist_Success()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileView);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = this._helper.GetService<Mock<IAcquisitionFileChecklistRepository>>();
            var acquisitionRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetAllChecklistItemsByAcquisitionFileId(It.IsAny<long>())).Returns(acqFile.PimsAcquisitionChecklistItems.ToList());
            acquisitionRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
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
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileView);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = this._helper.GetService<Mock<IAcquisitionFileChecklistRepository>>();
            var acquisitionRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
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
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileView);

            var acqFile = EntityHelper.CreateAcquisitionFile(1);
            acqFile.AcquisitionFileStatusTypeCode = "COMPLT";

            var repository = this._helper.GetService<Mock<IAcquisitionFileChecklistRepository>>();
            var acquisitionRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
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
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileView);

            var acqFile = EntityHelper.CreateAcquisitionFile(1);
            acqFile.AppCreateTimestamp = new DateTime(2023, 1, 1);

            var repository = this._helper.GetService<Mock<IAcquisitionFileChecklistRepository>>();
            var acquisitionRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetAllChecklistItemsByAcquisitionFileId(It.IsAny<long>())).Returns(new List<PimsAcquisitionChecklistItem>());
            repository.Setup(x => x.GetAllChecklistItemTypes()).Returns(new List<PimsAcqChklstItemType>() { new PimsAcqChklstItemType() { AcqChklstItemTypeCode = "TEST", EffectiveDate = new DateOnly(2024, 1, 1) } });
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
            var service = this.CreateAcquisitionServiceWithPermissions();

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
            var service = this.CreateAcquisitionServiceWithPermissions();

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var acqFileRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            acqFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
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
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.PimsAcquisitionChecklistItems = new List<PimsAcquisitionChecklistItem>() { new PimsAcquisitionChecklistItem() { Internal_Id = 1, AcqChklstItemStatusTypeCode = "COMPLT" } };

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var fileChecklistRepository = this._helper.GetService<Mock<IAcquisitionFileChecklistRepository>>();
            fileChecklistRepository.Setup(x => x.GetAllChecklistItemsByAcquisitionFileId(It.IsAny<long>()))
                .Returns(new List<PimsAcquisitionChecklistItem>() { new PimsAcquisitionChecklistItem() { Internal_Id = 1, AcqChklstItemStatusTypeCode = "INCOMP" } });

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditChecklists(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            service.UpdateChecklistItems(acqFile);

            // Assert
            fileChecklistRepository.Verify(x => x.GetAllChecklistItemsByAcquisitionFileId(It.IsAny<long>()), Times.Once);
            fileChecklistRepository.Verify(x => x.Update(It.IsAny<PimsAcquisitionChecklistItem>()), Times.Once);
            repository.Verify(x => x.GetById(It.IsAny<long>()), Times.Exactly(3));
        }

        [Fact]
        public void UpdateChecklist_ItemNotFound()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.PimsAcquisitionChecklistItems = new List<PimsAcquisitionChecklistItem>() { new PimsAcquisitionChecklistItem() { Internal_Id = 999, AcqChklstItemStatusTypeCode = "COMPLT" } };
            acqFile.AcquisitionFileStatusTypeCode = AcquisitionStatusTypes.ACTIVE.ToString();

            var acqRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            acqRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var fileChecklistRepository = this._helper.GetService<Mock<IAcquisitionFileChecklistRepository>>();
            fileChecklistRepository.Setup(x => x.GetAllChecklistItemsByAcquisitionFileId(It.IsAny<long>()))
                .Returns(new List<PimsAcquisitionChecklistItem>() { new PimsAcquisitionChecklistItem() { Internal_Id = 1, AcqChklstItemStatusTypeCode = "INCOMP" } });

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditChecklists(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            Action act = () => service.UpdateChecklistItems(acqFile);

            // Assert
            act.Should().Throw<BadRequestException>();

            fileChecklistRepository.Verify(x => x.GetAllChecklistItemsByAcquisitionFileId(It.IsAny<long>()), Times.Once);
            fileChecklistRepository.Verify(x => x.Update(It.IsAny<PimsAcquisitionChecklistItem>()), Times.Never);
            acqRepository.Verify(x => x.GetById(It.IsAny<long>()), Times.Exactly(2));
        }

        [Fact]
        public void UpdateChecklist_NoPermission()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions();

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = this._helper.GetService<Mock<IAcquisitionFileChecklistRepository>>();
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
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = this._helper.GetService<Mock<IAcquisitionFileChecklistRepository>>();
            repository.Setup(x => x.GetAllChecklistItemsByAcquisitionFileId(It.IsAny<long>())).Returns(acqFile.PimsAcquisitionChecklistItems.ToList());

            var acqFileRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            acqFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            var contractorUser = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: true);
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(contractorUser);

            // Act
            Action act = () => service.UpdateChecklistItems(acqFile);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void UpdateChecklist_InvalidStatus()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.PimsAcquisitionChecklistItems = new List<PimsAcquisitionChecklistItem>() { new PimsAcquisitionChecklistItem() { Internal_Id = 1, AcqChklstItemStatusTypeCode = "COMPLT" } };

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var fileChecklistRepository = this._helper.GetService<Mock<IAcquisitionFileChecklistRepository>>();
            fileChecklistRepository.Setup(x => x.GetAllChecklistItemsByAcquisitionFileId(It.IsAny<long>()))
                .Returns(new List<PimsAcquisitionChecklistItem>() { new PimsAcquisitionChecklistItem() { Internal_Id = 1, AcqChklstItemStatusTypeCode = "INCOMP" } });

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditChecklists(It.IsAny<AcquisitionStatusTypes?>())).Returns(false);

            // Act
            Action act = () => service.UpdateChecklistItems(acqFile);

            // Assert
            act.Should().Throw<BusinessRuleViolationException>();
        }
        #endregion

        #region CompensationRequisition

        [Fact]
        public void GetCompensationsRequisitions_NoPermissions()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions();

            // Act
            Action act = () => service.GetAcquisitionCompensations(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void GetCompensationsRequisitions_NotAuthorized_Contractor()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileView, Permissions.CompensationRequisitionView);

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            var contractorUser = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: true);
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(contractorUser);

            var acqFileRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
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
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileView, Permissions.CompensationRequisitionView);

            var repository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            repository.Setup(x => x.GetAllByAcquisitionFileId(It.IsAny<long>()))
                .Returns(new List<PimsCompensationRequisition>()
                {
                    new PimsCompensationRequisition(),
                });

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
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
            var service = this.CreateAcquisitionServiceWithPermissions();

            // Act
            Action act = () => service.AddCompensationRequisition(1, new PimsCompensationRequisition());

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void AddCompensationsRequisitions_NullException()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.CompensationRequisitionAdd);
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();

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
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.CompensationRequisitionAdd);
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();

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
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.CompensationRequisitionAdd);
            var repository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            var acqFilerepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            var newCompensationReq = EntityHelper.CreateCompensationRequisition(1, 1);
            var acquisitionFile = EntityHelper.CreateAcquisitionFile(1);

            acqFilerepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acquisitionFile);
            repository.Setup(x => x.Add(It.IsAny<PimsCompensationRequisition>())).Returns(newCompensationReq);

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
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
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.CompensationRequisitionAdd);
            var repository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            var acqFilerepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            var newCompensationReq = EntityHelper.CreateCompensationRequisition(1, 1);
            var acquisitionFile = EntityHelper.CreateAcquisitionFile(1);

            acqFilerepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acquisitionFile);
            repository.Setup(x => x.Add(It.IsAny<PimsCompensationRequisition>())).Returns(newCompensationReq);

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            var result = service.AddCompensationRequisition(1, newCompensationReq);

            // Assert
            repository.Verify(x => x.Add(It.IsAny<PimsCompensationRequisition>()), Times.Once);
        }
        #endregion

        #region InterestHolders

        [Fact]
        public void GetInterestHolders_NoPermissions()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions();

            // Act
            Action act = () => service.GetInterestHolders(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void GetInterestHolders_NotAuthorized_Contractor()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileView);

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            var contractorUser = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: true);
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(contractorUser);

            var acqFileRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            acqFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(EntityHelper.CreateAcquisitionFile());

            // Act
            Action act = () => service.GetInterestHolders(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void GetInterestHolders_Success()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileView);

            var repository = this._helper.GetService<Mock<IInterestHolderRepository>>();
            repository.Setup(x => x.GetInterestHoldersByAcquisitionFile(It.IsAny<long>()))
                .Returns(new List<PimsInterestHolder>()
                {
                    new PimsInterestHolder(),
                });

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            var result = service.GetInterestHolders(1);

            // Assert
            repository.Verify(x => x.GetInterestHoldersByAcquisitionFile(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void UpdateInterestHolders_NoPermissions()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions();

            // Act
            Action act = () => service.UpdateInterestHolders(1, new List<PimsInterestHolder>()
                {
                    new PimsInterestHolder(),
                });

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void UpdateInterestHolders_NotAuthorized_Contractor()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.CompensationRequisitionEdit);
            var repository = this._helper.GetService<Mock<IInterestHolderRepository>>();
            var acqFilerepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            var acquisitionFile = EntityHelper.CreateAcquisitionFile(1);

            acqFilerepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acquisitionFile);
            repository.Setup(x => x.UpdateAllForAcquisition(It.IsAny<long>(), It.IsAny<List<PimsInterestHolder>>())).Returns(new List<PimsInterestHolder>() { new PimsInterestHolder() });

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            var contractorUser = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: true);
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(contractorUser);

            // Act
            Action act = () => service.UpdateInterestHolders(1, new List<PimsInterestHolder>() { new PimsInterestHolder() });

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        // TODO: fix
        /*
                [Fact]
                public void UpdateInterestHolders_Success()
                {
                    // Arrange
                    var service = CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);
                    var repository = _helper.GetService<Mock<IInterestHolderRepository>>();
                    var acqFilerepository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
                    var acquisitionFile = EntityHelper.CreateAcquisitionFile(1);

                    acquisitionFile.PimsInterestHolders = new List<PimsInterestHolder>() {
                        new PimsInterestHolder() {
                            InterestHolderId = 100,
                        },
                    };

                    acqFilerepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acquisitionFile);
                    repository.Setup(x => x.UpdateAllForAcquisition(It.IsAny<long>(), It.IsAny<List<PimsInterestHolder>>())).Returns(new List<PimsInterestHolder>() { new PimsInterestHolder() });

                    var userRepository = _helper.GetService<Mock<IUserRepository>>();
                    userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

                    var compReqRepository = _helper.GetService<Mock<ICompensationRequisitionRepository>>();
                    compReqRepository.Setup(x => x.GetAllByAcquisitionFileId(It.IsAny<long>()))
                        .Returns(new List<PimsCompensationRequisition>() {
                            new PimsCompensationRequisition() {
                                CompensationRequisitionId = 1,
                                AcquisitionFileId = acquisitionFile.Internal_Id,
                                PimsAcquisitionPayees = new List<PimsAcquisitionPayee>()
                                {
                                    new PimsAcquisitionPayee()
                                    {
                                        Internal_Id = 1,
                                        CompensationRequisitionId = 1,
                                        AcquisitionOwnerId = null,
                                        InterestHolderId = null,
                                        AcquisitionFileTeamId = null
                                    },
                                },
                            },
                        });

                    // Act
                    var result = service.UpdateInterestHolders(1, new List<PimsInterestHolder>() { new PimsInterestHolder() });

                    // Assert
                    repository.Verify(x => x.UpdateAllForAcquisition(It.IsAny<long>(), It.IsAny<List<PimsInterestHolder>>()), Times.Once);
                }



                [Fact]
                public void UpdateInterestHolders_FKExeption_Removed_InterestHolder()
                {
                    // Arrange
                    var service = CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);
                    var acqFile = EntityHelper.CreateAcquisitionFile();
                    acqFile.PimsInterestHolders = new List<PimsInterestHolder>() {
                        new PimsInterestHolder() {
                            InterestHolderId = 100,
                        },
                    };

                    var repository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
                    repository.Setup(x => x.Update(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);
                    repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

                    var compReqRepository = _helper.GetService<Mock<ICompensationRequisitionRepository>>();
                    compReqRepository.Setup(x => x.GetAllByAcquisitionFileId(It.IsAny<long>()))
                        .Returns(new List<PimsCompensationRequisition>() {
                            new PimsCompensationRequisition() {
                                CompensationRequisitionId = 1,
                                AcquisitionFileId = acqFile.Internal_Id,
                                PimsAcquisitionPayees = new List<PimsAcquisitionPayee>()
                                {
                                    new PimsAcquisitionPayee()
                                    {
                                        Internal_Id = 1,
                                        CompensationRequisitionId = 1,
                                        AcquisitionOwnerId = null,
                                        InterestHolderId = 100,
                                        AcquisitionFileTeamId = null
                                    },
                                },
                            },
                        });

                    // Act
                    var updatedAcqFile = EntityHelper.CreateAcquisitionFile();
                    Action act = () => service.UpdateInterestHolders(1, new List<PimsInterestHolder>() { new PimsInterestHolder() });

                    // Assert
                    act.Should().Throw<ForeignKeyDependencyException>();
                    repository.Verify(x => x.Update(It.IsAny<PimsAcquisitionFile>()), Times.Never);
                }*/
        #endregion

        #region Agreements

        [Fact]
        public void GetAgreementsByAcquisitionFileId_NoPermission_IsContractor()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileView, Permissions.AgreementView);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var contractorUser = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: true);
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(contractorUser);

            // Act
            Action act = () => service.GetAgreements(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void SearchAgreementsByAcquisitionFileId_Success()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileView, Permissions.AgreementView);

            var filter = new AcquisitionReportFilterModel();

            var repository = this._helper.GetService<Mock<IAgreementRepository>>();
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            repository.Setup(x => x.SearchAgreements(It.IsAny<AcquisitionReportFilterModel>())).Returns(new List<PimsAgreement>());

            var contractorUser = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: true);
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(contractorUser);

            // Act
            var agreements = service.SearchAgreements(filter);

            // Assert
            agreements.Should().BeEmpty();
        }

        [Fact]
        public void SearchAgreementsByAcquisitionFileId_Region_Success()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileView, Permissions.AgreementView);

            var filter = new AcquisitionReportFilterModel();

            var repository = this._helper.GetService<Mock<IAgreementRepository>>();
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            var matchingAgreement = new PimsAgreement() { AcquisitionFile = new PimsAcquisitionFile() { RegionCode = 1 } };
            var nonMatchingAgreement = new PimsAgreement() { AcquisitionFile = new PimsAcquisitionFile() { RegionCode = 2 } };
            repository.Setup(x => x.SearchAgreements(It.IsAny<AcquisitionReportFilterModel>())).Returns(new List<PimsAgreement>() { matchingAgreement, nonMatchingAgreement });

            var user = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: false);
            user.PimsRegionUsers = new List<PimsRegionUser>() { new PimsRegionUser() { RegionCode = 1 } };
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(user);

            // Act
            var agreements = service.SearchAgreements(filter);

            // Assert
            agreements.Should().HaveCount(1);
            agreements.FirstOrDefault().AcquisitionFile.RegionCode.Should().Be(1);
        }

        [Fact]
        public void SearchAgreementsByAcquisitionFileId_Contractor_Filter()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileView, Permissions.AgreementView);

            var filter = new AcquisitionReportFilterModel();

            var user = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: true);

            var repository = this._helper.GetService<Mock<IAgreementRepository>>();
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            var matchingPerson = new PimsAcquisitionFileTeam() { PersonId = user.PersonId };
            var matchingAgreement = new PimsAgreement() { AcquisitionFile = new PimsAcquisitionFile() { RegionCode = 1, PimsAcquisitionFileTeams = new List<PimsAcquisitionFileTeam>() { matchingPerson } } };
            var nonMatchingAgreement = new PimsAgreement() { AcquisitionFile = new PimsAcquisitionFile() { } };
            repository.Setup(x => x.SearchAgreements(It.IsAny<AcquisitionReportFilterModel>())).Returns(new List<PimsAgreement>() { matchingAgreement, nonMatchingAgreement });

            user.PimsRegionUsers = new List<PimsRegionUser>() { new PimsRegionUser() { RegionCode = 1 } };
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(user);

            // Act
            var agreements = service.SearchAgreements(filter);

            // Assert
            agreements.Should().HaveCount(1);
            agreements.FirstOrDefault().AcquisitionFile.RegionCode.Should().Be(1);
            agreements.FirstOrDefault().AcquisitionFile.PimsAcquisitionFileTeams.FirstOrDefault().PersonId.Should().Be(user.PersonId);
        }

        [Fact]
        public void SearchAgreementsByAcquisitionFileId_NoPermission()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileView);

            var filter = new AcquisitionReportFilterModel();

            var repository = this._helper.GetService<Mock<IAgreementRepository>>();
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            repository.Setup(x => x.SearchAgreements(It.IsAny<AcquisitionReportFilterModel>())).Returns(new List<PimsAgreement>());

            var contractorUser = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: true);
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(contractorUser);

            // Act
            Action act = () => service.SearchAgreements(filter);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void UpdateAgreementsByAcquisitionFileId_NoPermission_IsContractor()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileView, Permissions.AgreementView);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
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
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileView, Permissions.AgreementView);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var contractorUser = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: true);
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(contractorUser);

            // Act
            Action act = () => service.GetOwners(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        #endregion

        #region Team Members

        [Fact]
        public void GetTeamMembers_NoPermission()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions();

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetTeamMembers(It.IsAny<HashSet<short>>(), null)).Returns(new List<PimsAcquisitionFileTeam>());

            // Act
            Action act = () => service.GetOwners(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void GetTeamMembers_Success()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileView, Permissions.AgreementView);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            repository.Setup(x => x.GetTeamMembers(It.IsAny<HashSet<short>>(), It.IsAny<long>())).Returns(new List<PimsAcquisitionFileTeam>());

            var contractorUser = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: true);
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(contractorUser);

            // Act
            var teamMembers = service.GetTeamMembers();

            // Assert
            repository.Verify(x => x.GetTeamMembers(It.IsAny<HashSet<short>>(), contractorUser.PersonId), Times.Once);
        }

        #endregion

        #region Properties

        [Fact]
        public void GetProperties_ByAcquisitionFileId_NoPermission_IsContractor()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileView, Permissions.AgreementView);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var contractorUser = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: true);
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(contractorUser);

            // Act
            Action act = () => service.GetProperties(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }
        #endregion

        #region ExpPayment

        [Fact]
        public void GetExpPayments_NoPermissions()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions();

            // Act
            Action act = () => service.GetAcquisitionExpropriationPayments(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void GetExpPayments_NotAuthorized_Contractor()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileView, Permissions.CompensationRequisitionView);

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            var contractorUser = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: true);
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(contractorUser);

            var acqFileRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            acqFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(EntityHelper.CreateAcquisitionFile());

            // Act
            Action act = () => service.GetAcquisitionExpropriationPayments(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void GetExpPayments_Success()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileView, Permissions.CompensationRequisitionView);

            var repository = this._helper.GetService<Mock<IExpropriationPaymentRepository>>();
            repository.Setup(x => x.GetAllByAcquisitionFileId(It.IsAny<long>()))
                .Returns(new List<PimsExpropriationPayment>()
                {
                    new PimsExpropriationPayment(),
                });

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            var result = service.GetAcquisitionExpropriationPayments(1);

            // Assert
            repository.Verify(x => x.GetAllByAcquisitionFileId(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void AddExpPayment_NoPermissions()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions();

            // Act
            Action act = () => service.AddExpropriationPayment(1, new PimsExpropriationPayment());

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void AddExpPayment_NullException()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();

            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            Action act = () => service.AddExpropriationPayment(1, null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void AddExpPayment_BadRequest_IdMissmatch()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();

            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            Action act = () => service.AddExpropriationPayment(1, new PimsExpropriationPayment() { Internal_Id = 2 });

            // Assert
            act.Should().Throw<BadRequestException>();
        }

        [Fact]
        public void AddExpPayment_NotAuthorized_Contractor()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.CompensationRequisitionAdd);
            var repository = this._helper.GetService<Mock<IExpropriationPaymentRepository>>();
            var acqFilerepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            var newExpPayment = EntityHelper.CreateExpropriationPayment(1, 1);
            var acquisitionFile = EntityHelper.CreateAcquisitionFile(1);

            acqFilerepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acquisitionFile);
            repository.Setup(x => x.Add(It.IsAny<PimsExpropriationPayment>())).Returns(newExpPayment);

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            var contractorUser = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: true);
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(contractorUser);

            // Act
            Action act = () => service.AddExpropriationPayment(1, newExpPayment);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void AddExpPayment_Success()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);
            var repository = this._helper.GetService<Mock<IExpropriationPaymentRepository>>();
            var acqFilerepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            var newExpPayment = EntityHelper.CreateExpropriationPayment(1, 1);
            var acquisitionFile = EntityHelper.CreateAcquisitionFile(1);

            acqFilerepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acquisitionFile);
            repository.Setup(x => x.Add(It.IsAny<PimsExpropriationPayment>())).Returns(newExpPayment);

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            var result = service.AddExpropriationPayment(1, newExpPayment);

            // Assert
            repository.Verify(x => x.Add(It.IsAny<PimsExpropriationPayment>()), Times.Once);
        }

        #endregion

        #region Export

        [Fact]
        public void GetAcquisitionFileExport_NoPermissions()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions();
            var filter = new AcquisitionFilter();

            // Act
            Action act = () => service.GetAcquisitionFileExport(filter);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void GetAcquisitionFileExport_Success()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileView);
            var acqFilerepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();

            var filter = new AcquisitionFilter();
            var acquisitionFile = EntityHelper.CreateAcquisitionFile(1);
            acqFilerepository.Setup(x => x.GetAcquisitionFileExportDeep(It.IsAny<AcquisitionFilter>(), It.IsAny<HashSet<short>>(), It.IsAny<long?>()))
                        .Returns(new List<PimsAcquisitionFile>()
                        {
                            acquisitionFile,
                        });

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            var result = service.GetAcquisitionFileExport(filter);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Count());
            acqFilerepository.Verify(x => x.GetAcquisitionFileExportDeep(It.IsAny<AcquisitionFilter>(), It.IsAny<HashSet<short>>(), It.IsAny<long?>()), Times.Once);
        }

        [Fact]
        public void GetAcquisitionFileExport_Success_FlatProperties()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileView);
            var acqFilerepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();

            var filter = new AcquisitionFilter();
            var acquisitionFile = EntityHelper.CreateAcquisitionFile(1);
            acquisitionFile.FileNumber = "10-25-2023";
            acquisitionFile.PimsPropertyAcquisitionFiles = new List<PimsPropertyAcquisitionFile>()
            {
                new PimsPropertyAcquisitionFile()
                {
                    AcquisitionFileId = 1,
                    PropertyId = 100,
                    Property = new PimsProperty()
                    {
                        PropertyId = 100,
                        Pid = 8000,
                    },
                },
                new PimsPropertyAcquisitionFile()
                {
                    AcquisitionFileId = 1,
                    PropertyId = 200,
                    Property = new PimsProperty()
                    {
                        PropertyId = 200,
                        Pid = 9000,
                    },
                },
            };

            acqFilerepository.Setup(x => x.GetAcquisitionFileExportDeep(It.IsAny<AcquisitionFilter>(), It.IsAny<HashSet<short>>(), It.IsAny<long?>()))
                        .Returns(new List<PimsAcquisitionFile>()
                        {
                            acquisitionFile,
                        });

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            var result = service.GetAcquisitionFileExport(filter);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("10-25-2023", result[0].FileNumber);
            Assert.Equal("10-25-2023", result[1].FileNumber);
            Assert.Equal("8000", result[0].Pid);
            Assert.Equal("9000", result[1].Pid);
            acqFilerepository.Verify(x => x.GetAcquisitionFileExportDeep(It.IsAny<AcquisitionFilter>(), It.IsAny<HashSet<short>>(), It.IsAny<long?>()), Times.Once);
        }

        #endregion

        #endregion

        private Mock<IAcquisitionFileRepository> mockForMultipleTakes(PimsAcquisitionFile acqFile)
        {
            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);
            repository.Setup(x => x.Update(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);

            var filePropertyRepository = this._helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>())).Returns(acqFile.PimsPropertyAcquisitionFiles.ToList());

            var lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();
            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            var compReqRepository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            compReqRepository.Setup(x => x.GetAllByAcquisitionFileId(It.IsAny<long>())).Returns(new List<PimsCompensationRequisition>());

            return repository;
        }
    }
}
