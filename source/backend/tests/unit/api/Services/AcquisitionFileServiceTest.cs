using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NetTopologySuite.Geometries;
using Pims.Core.Api.Exceptions;
using Pims.Api.Models.CodeTypes;
using Pims.Api.Services;
using Pims.Core.Exceptions;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Pims.Core.Security;
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

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser(1, Guid.NewGuid(), "Test", regionCode: 1));

            // Act
            var result = service.Add(acqFile, new List<UserOverrideCode>());

            // Assert
            repository.Verify(x => x.Add(It.IsAny<PimsAcquisitionFile>()), Times.Once);
        }

        [Fact]
        public void Add_Success_DefaultValues()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileAdd);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.AssignedDate = null;

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.Add(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);

            var lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();
            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser(1, Guid.NewGuid(), "Test", regionCode: 1));

            // Act
            var result = service.Add(acqFile, new List<UserOverrideCode>());

            // Assert
            repository.Verify(x => x.Add(It.IsAny<PimsAcquisitionFile>()), Times.Once);
            result.AssignedDate.Should().Be(DateTime.Today);
            result.AcquisitionFileStatusTypeCode.Should().Be(AcquisitionStatusTypes.ACTIVE.ToString());
        }

        [Fact]
        public void Add_Success_WithUserSuppliedAssignedDate()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileAdd);

            DateTime customDate = DateTime.Today.AddMonths(3);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.AssignedDate = customDate;

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.Add(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);

            var lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();
            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser(1, Guid.NewGuid(), "Test", regionCode: 1));

            // Act
            var result = service.Add(acqFile, new List<UserOverrideCode>());

            // Assert
            repository.Verify(x => x.Add(It.IsAny<PimsAcquisitionFile>()), Times.Once);
            result.AssignedDate.Should().Be(customDate);
            result.AcquisitionFileStatusTypeCode.Should().Be(AcquisitionStatusTypes.ACTIVE.ToString());
        }

        [Fact]
        public void Add_CannotDetermineRegion_Error()
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
        public void Add_NoPermission_Error()
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
        public void Add_Success_IsContractor_AssignedToProject()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileAdd);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.Project = new PimsProject() { Id = 1, PimsProjectPeople = new List<PimsProjectPerson>() { new PimsProjectPerson() { PersonId = 1, ProjectId = 1 } } };
            acqFile.ProjectId = 1;
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
            var projectRepository = this._helper.GetService<Mock<IProjectRepository>>();
            projectRepository.Setup(x => x.TryGet(It.IsAny<long>())).Returns(acqFile.Project);

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

        [Fact]
        public void Add_WithRetiredProperty_Should_Fail()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileAdd);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            PimsProperty property = new PimsProperty()
            {
                PropertyId = 100,
                Pid = 1000,
                IsRetired = true,
            };

            acqFile.PimsPropertyAcquisitionFiles.Add(new PimsPropertyAcquisitionFile()
            {
                Property = property,
            });

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.Add(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);

            var lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();
            lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser(1, Guid.NewGuid(), "Test", regionCode: 1));

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), true)).Returns(property);

            // Act
            Action act = () => service.Add(acqFile, new List<UserOverrideCode>());

            // Assert
            var ex = act.Should().Throw<BusinessRuleViolationException>();
            ex.WithMessage("Retired property can not be selected.");
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
        public void GetById_NoPermission_Error()
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
        public void Update_Drafts_TakesInProgress_Violation()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            var agreementRepository = this._helper.GetService<Mock<IAgreementRepository>>();
            agreementRepository.Setup(x => x.GetAgreementsByAcquisitionFile(It.IsAny<long>())).Returns(new List<PimsAgreement>() { });

            var compReqRepository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            compReqRepository.Setup(x => x.GetAllByAcquisitionFileId(It.IsAny<long>())).Returns(new List<PimsCompensationRequisition>());

            var takeRepository = this._helper.GetService<Mock<ITakeRepository>>();
            takeRepository.Setup(x => x.GetAllByAcquisitionFileId(It.IsAny<long>())).Returns(new List<PimsTake>() { new PimsTake() { TakeStatusTypeCode = AcquisitionTakeStatusTypes.INPROGRESS.ToString() } });

            // Act
            var pimsAcquisitionUpdate = EntityHelper.CreateAcquisitionFile(acqFile.AcquisitionFileId, acqFile.FileName);
            pimsAcquisitionUpdate.AcquisitionFileStatusTypeCode = AcquisitionStatusTypes.COMPLT.ToString();
            pimsAcquisitionUpdate.AcquisitionFileStatusTypeCodeNavigation = null;

            Action act = () => service.Update(pimsAcquisitionUpdate, new List<UserOverrideCode>());

            // Assert
            var exception = act.Should().Throw<BusinessRuleViolationException>();
            exception.WithMessage("Please ensure all in-progress property takes have been completed or canceled before completing an Acquisition File.");
        }

        [Fact]
        public void Update_Drafts_TakesNotInFile_Violation()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            var agreementRepository = this._helper.GetService<Mock<IAgreementRepository>>();
            agreementRepository.Setup(x => x.GetAgreementsByAcquisitionFile(It.IsAny<long>())).Returns(new List<PimsAgreement>() { });

            var compReqRepository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
            compReqRepository.Setup(x => x.GetAllByAcquisitionFileId(It.IsAny<long>())).Returns(new List<PimsCompensationRequisition>());

            var takeRepository = this._helper.GetService<Mock<ITakeRepository>>();
            takeRepository.Setup(x => x.GetAllByAcquisitionFileId(It.IsAny<long>())).Returns(new List<PimsTake>());

            // Act
            var pimsAcquisitionUpdate = EntityHelper.CreateAcquisitionFile(acqFile.AcquisitionFileId, acqFile.FileName);
            pimsAcquisitionUpdate.AcquisitionFileStatusTypeCode = AcquisitionStatusTypes.COMPLT.ToString();
            pimsAcquisitionUpdate.AcquisitionFileStatusTypeCodeNavigation = null;

            Action act = () => service.Update(pimsAcquisitionUpdate, new List<UserOverrideCode>());

            // Assert
            var exception = act.Should().Throw<BusinessRuleViolationException>();
            exception.WithMessage("You cannot complete an acquisition file that has no takes.");
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
        public void Update_SubFile_Violation()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.AcquisitionFileStatusTypeCode = AcquisitionStatusTypes.ACTIVE.ToString();
            acqFile.ProjectId = 1;
            acqFile.ProductId = 1;

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetRegion(It.IsAny<long>())).Returns((short)(acqFile.RegionCode));
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
            var pimsAcquisitionUpdate = EntityHelper.CreateAcquisitionFile(acqFile.AcquisitionFileId, acqFile.FileName);
            pimsAcquisitionUpdate.ProjectId = 99;
            pimsAcquisitionUpdate.ProductId = 88;
            Action act = () => service.Update(pimsAcquisitionUpdate, new List<UserOverrideCode>());

            // Assert
            var ex = act.Should().Throw<UserOverrideException>();
            ex.Which.UserOverride.Should().Be(UserOverrideCode.UpdateSubFilesProjectProduct);
            repository.Verify(x => x.Update(It.IsAny<PimsAcquisitionFile>()), Times.Never);
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
        public void Update_Success_SubFile_UserOverride()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.AcquisitionFileStatusTypeCode = AcquisitionStatusTypes.ACTIVE.ToString();
            acqFile.ProjectId = 1;
            acqFile.ProductId = 1;

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetRegion(It.IsAny<long>())).Returns((short)(acqFile.RegionCode));
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
            var pimsAcquisitionUpdate = EntityHelper.CreateAcquisitionFile(acqFile.AcquisitionFileId, acqFile.FileName);
            pimsAcquisitionUpdate.ProjectId = 99;
            pimsAcquisitionUpdate.ProductId = 88;
            var result = service.Update(pimsAcquisitionUpdate, new List<UserOverrideCode>() { UserOverrideCode.UpdateSubFilesProjectProduct });

            // Assert
            repository.Verify(x => x.Update(It.IsAny<PimsAcquisitionFile>()), Times.Once);
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
        public void Update_ProjectContractor_Removed()
        {
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var project = new PimsProject() { Id = 1, PimsProjectPeople = new List<PimsProjectPerson>() { new PimsProjectPerson() { PersonId = 1, ProjectId = 1 } } };
            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.ProjectId = 1;
            acqFile.Project = project;

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();

            var contractorUser = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: true);
            contractorUser.PersonId = 1;
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(contractorUser);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetRegion(It.IsAny<long>())).Returns(acqFile.RegionCode);

            var projectRepository = this._helper.GetService<Mock<IProjectRepository>>();
            projectRepository.Setup(x => x.TryGet(It.IsAny<long>())).Returns(project);

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

        //[Fact]
        //public void Update_FKException_Removed_AcqFileOwner()
        //{
        //    // Arrange
        //    var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

        //    var acqFile = EntityHelper.CreateAcquisitionFile();
        //    acqFile.PimsAcquisitionOwners = new List<PimsAcquisitionOwner>() {
        //        new PimsAcquisitionOwner() {
        //            AcquisitionOwnerId = 100,
        //        },
        //    };

        //    var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
        //    repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
        //    repository.Setup(x => x.Update(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);
        //    repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

        //    var compReqRepository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
        //    compReqRepository.Setup(x => x.GetAllByAcquisitionFileId(It.IsAny<long>()))
        //        .Returns(new List<PimsCompensationRequisition>() {
        //            new PimsCompensationRequisition() {
        //                CompensationRequisitionId = 1,
        //                AcquisitionFileId = acqFile.Internal_Id,
        //                AcquisitionOwnerId = 100,
        //            },
        //        });

        //    var lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();
        //    lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });
        //    var userRepository = this._helper.GetService<Mock<IUserRepository>>();
        //    userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

        //    var agreementRepository = this._helper.GetService<Mock<IAgreementRepository>>();
        //    agreementRepository.Setup(x => x.GetAgreementsByAcquisitionFile(It.IsAny<long>())).Returns(new List<PimsAgreement>());

        //    var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
        //    solver.Setup(x => x.CanEditDetails(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

        //    // Act
        //    var updatedAcqFile = EntityHelper.CreateAcquisitionFile();
        //    Action act = () => service.Update(updatedAcqFile, new List<UserOverrideCode>() { UserOverrideCode.UpdateRegion });

        //    // Assert
        //    act.Should().Throw<ForeignKeyDependencyException>();
        //    repository.Verify(x => x.Update(It.IsAny<PimsAcquisitionFile>()), Times.Never);
        //}

        //[Fact]
        //public void Update_FKException_Removed_OwnerSolicitor()
        //{
        //    // Arrange
        //    var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

        //    var acqFile = EntityHelper.CreateAcquisitionFile();
        //    acqFile.PimsInterestHolders = new List<PimsInterestHolder>() {
        //        new PimsInterestHolder() {
        //            InterestHolderTypeCode = "AOSLCTR",
        //            InterestHolderId = 100,
        //        },
        //    };

        //    var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
        //    repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
        //    repository.Setup(x => x.Update(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);
        //    repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

        //    var compReqRepository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
        //    compReqRepository.Setup(x => x.GetAllByAcquisitionFileId(It.IsAny<long>()))
        //        .Returns(new List<PimsCompensationRequisition>() {
        //            new PimsCompensationRequisition() {
        //                CompensationRequisitionId = 1,
        //                AcquisitionFileId = acqFile.Internal_Id,
        //                InterestHolderId = 100,
        //            },
        //        });

        //    var lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();
        //    lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });
        //    var userRepository = this._helper.GetService<Mock<IUserRepository>>();
        //    userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

        //    var agreementRepository = this._helper.GetService<Mock<IAgreementRepository>>();
        //    agreementRepository.Setup(x => x.GetAgreementsByAcquisitionFile(It.IsAny<long>())).Returns(new List<PimsAgreement>());

        //    var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
        //    solver.Setup(x => x.CanEditDetails(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

        //    // Act
        //    var updatedAcqFile = EntityHelper.CreateAcquisitionFile();
        //    Action act = () => service.Update(updatedAcqFile, new List<UserOverrideCode>() { UserOverrideCode.UpdateRegion });

        //    // Assert
        //    act.Should().Throw<ForeignKeyDependencyException>();
        //    repository.Verify(x => x.Update(It.IsAny<PimsAcquisitionFile>()), Times.Never);
        //}

        //[Fact]
        //public void Update_FKException_Removed_OwnerRepresentative()
        //{
        //    // Arrange
        //    var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

        //    var acqFile = EntityHelper.CreateAcquisitionFile();
        //    acqFile.PimsInterestHolders = new List<PimsInterestHolder>() {
        //        new PimsInterestHolder() {
        //            InterestHolderTypeCode = "AOREP",
        //            InterestHolderId = 100,
        //        },
        //    };

        //    var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
        //    repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
        //    repository.Setup(x => x.Update(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);
        //    repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

        //    var compReqRepository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
        //    compReqRepository.Setup(x => x.GetAllByAcquisitionFileId(It.IsAny<long>()))
        //        .Returns(new List<PimsCompensationRequisition>() {
        //            new PimsCompensationRequisition() {
        //                CompensationRequisitionId = 1,
        //                AcquisitionFileId = acqFile.Internal_Id,
        //                InterestHolderId = 100,
        //            },
        //        });

        //    var lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();
        //    lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });
        //    var userRepository = this._helper.GetService<Mock<IUserRepository>>();
        //    userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

        //    var agreementRepository = this._helper.GetService<Mock<IAgreementRepository>>();
        //    agreementRepository.Setup(x => x.GetAgreementsByAcquisitionFile(It.IsAny<long>())).Returns(new List<PimsAgreement>());

        //    var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
        //    solver.Setup(x => x.CanEditDetails(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

        //    // Act
        //    var updatedAcqFile = EntityHelper.CreateAcquisitionFile();
        //    Action act = () => service.Update(updatedAcqFile, new List<UserOverrideCode>() { UserOverrideCode.UpdateRegion });

        //    // Assert
        //    act.Should().Throw<ForeignKeyDependencyException>();
        //    repository.Verify(x => x.Update(It.IsAny<PimsAcquisitionFile>()), Times.Never);
        //}

        //[Fact]
        //public void Update_FKException_Removed_PersonOfInterest()
        //{
        //    // Arrange
        //    var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

        //    var acqFile = EntityHelper.CreateAcquisitionFile();
        //    acqFile.PimsAcquisitionFileTeams = new List<PimsAcquisitionFileTeam>() {
        //        new PimsAcquisitionFileTeam() {
        //            AcquisitionFileTeamId = 100,
        //        },
        //    };

        //    var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
        //    repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
        //    repository.Setup(x => x.Update(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);
        //    repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

        //    var compReqRepository = this._helper.GetService<Mock<ICompensationRequisitionRepository>>();
        //    compReqRepository.Setup(x => x.GetAllByAcquisitionFileId(It.IsAny<long>()))
        //        .Returns(new List<PimsCompensationRequisition>() {
        //            new PimsCompensationRequisition() {
        //                CompensationRequisitionId = 1,
        //                AcquisitionFileId = acqFile.Internal_Id,
        //                AcquisitionFileTeamId = 100,
        //            },
        //        });

        //    var lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();
        //    lookupRepository.Setup(x => x.GetAllRegions()).Returns(new List<PimsRegion>() { new PimsRegion() { Code = 4, RegionName = "Cannot determine" } });
        //    var userRepository = this._helper.GetService<Mock<IUserRepository>>();
        //    userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

        //    var agreementRepository = this._helper.GetService<Mock<IAgreementRepository>>();
        //    agreementRepository.Setup(x => x.GetAgreementsByAcquisitionFile(It.IsAny<long>())).Returns(new List<PimsAgreement>());

        //    var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
        //    solver.Setup(x => x.CanEditDetails(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

        //    // Act
        //    var updatedAcqFile = EntityHelper.CreateAcquisitionFile();
        //    Action act = () => service.Update(updatedAcqFile, new List<UserOverrideCode>() { UserOverrideCode.UpdateRegion });

        //    // Assert
        //    act.Should().Throw<ForeignKeyDependencyException>();
        //    repository.Verify(x => x.Update(It.IsAny<PimsAcquisitionFile>()), Times.Never);
        //}

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

        #region Properties
        [Fact]
        public void GetProperties_ByFileId_NoPermission()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileView);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = this._helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            repository.Setup(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>())).Returns(new List<PimsPropertyAcquisitionFile>());

            // Act
            Action act = () => service.GetProperties(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void GetProperties_ByFileId_Success()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileView, Permissions.PropertyView);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var propertyRepository = this._helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            propertyRepository.Setup(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>())).Returns(new List<PimsPropertyAcquisitionFile>());

            // Act
            var properties = service.GetProperties(1);

            // Assert
            propertyRepository.Verify(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void GetProperties_ByFileId_Success_Reproject()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileView, Permissions.PropertyView);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var propertyRepository = this._helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            propertyRepository.Setup(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>()))
                .Returns(new List<PimsPropertyAcquisitionFile>() { new() { Property = new() { Location = new Point(1, 1) } } });

            var propertyService = this._helper.GetService<Mock<IPropertyService>>();
            propertyService.Setup(x => x.TransformAllPropertiesToLatLong(It.IsAny<List<PimsPropertyAcquisitionFile>>()))
                .Returns<List<PimsPropertyAcquisitionFile>>(x => x);

            // Act
            var properties = service.GetProperties(1);

            // Assert
            propertyRepository.Verify(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>()), Times.Once);
            propertyService.Verify(x => x.TransformAllPropertiesToLatLong(It.IsAny<List<PimsPropertyAcquisitionFile>>()), Times.Once);
            properties.FirstOrDefault().Property.Location.Coordinates.Should().BeEquivalentTo(new Coordinate[] { new Coordinate(1, 1) });
        }

        #endregion

        #region UpdateProperties
        [Fact]
        public void UpdateProperties_Success()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit, Permissions.PropertyAdd, Permissions.PropertyView);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.ConcurrencyControlNumber = 1;

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var filePropertyRepository = this._helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>())).Returns(acqFile.PimsPropertyAcquisitionFiles.ToList());

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditProperties(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            service.UpdateProperties(acqFile, new List<UserOverrideCode>());

            // Assert
            filePropertyRepository.Verify(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void UpdateProperties_Success_Final_SystemAdmin()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit, Permissions.AcquisitionFileAdd, Permissions.AcquisitionFileView, Permissions.SystemAdmin, Permissions.PropertyView, Permissions.PropertyAdd);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.ConcurrencyControlNumber = 1;
            acqFile.AcquisitionFileStatusTypeCode = AcquisitionStatusTypes.COMPLT.ToString();

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
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit, Permissions.PropertyAdd, Permissions.PropertyView);

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
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), false)).Returns(property);

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
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit, Permissions.PropertyAdd, Permissions.PropertyView);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345, regionCode: 1);
            acqFile.PimsPropertyAcquisitionFiles = new List<PimsPropertyAcquisitionFile>() { new PimsPropertyAcquisitionFile() { Internal_Id = 1, Property = property } };

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), true)).Returns(property);
            propertyRepository.Setup(x => x.GetPropertyRegion(It.IsAny<long>())).Returns(1);

            var filePropertyRepository = this._helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>())).Returns(acqFile.PimsPropertyAcquisitionFiles.ToList());

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser(1, Guid.NewGuid(), "Test", regionCode: 1));

            var propertyService = this._helper.GetService<Mock<IPropertyService>>();
            propertyService.Setup(x => x.UpdateLocation(It.IsAny<PimsProperty>(), ref It.Ref<PimsProperty>.IsAny, It.IsAny<IEnumerable<UserOverrideCode>>(), false));

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditProperties(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            service.UpdateProperties(acqFile, new List<UserOverrideCode>() { UserOverrideCode.AddLocationToProperty });

            // Assert
            filePropertyRepository.Verify(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>()), Times.Once);
            filePropertyRepository.Verify(x => x.Update(It.IsAny<PimsPropertyAcquisitionFile>()), Times.Once);
            propertyService.Verify(x => x.UpdateLocation(It.IsAny<PimsProperty>(), ref It.Ref<PimsProperty>.IsAny, It.IsAny<IEnumerable<UserOverrideCode>>(), false), Times.Once);
            propertyService.Verify(x => x.UpdateFilePropertyLocation<PimsPropertyAcquisitionFile>(It.IsAny<PimsPropertyAcquisitionFile>(), It.IsAny<PimsPropertyAcquisitionFile>()), Times.Once);
        }

        [Fact]
        public void UpdateProperties_MatchProperties_Success_NoInternalId()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit, Permissions.PropertyAdd, Permissions.PropertyView);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(1, regionCode: 1);
            acqFile.PimsPropertyAcquisitionFiles = new List<PimsPropertyAcquisitionFile>() { new PimsPropertyAcquisitionFile() { Internal_Id = 0, Property = property, PropertyId = 1 } };
            var propertyAcquisitionFiles = new List<PimsPropertyAcquisitionFile>() { new PimsPropertyAcquisitionFile() { Internal_Id = 1, Property = property, PropertyId = 1 } };

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), true)).Returns(property);
            propertyRepository.Setup(x => x.GetPropertyRegion(It.IsAny<long>())).Returns(1);

            var filePropertyRepository = this._helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>())).Returns(propertyAcquisitionFiles);

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser(1, Guid.NewGuid(), "Test", regionCode: 1));

            var propertyService = this._helper.GetService<Mock<IPropertyService>>();
            propertyService.Setup(x => x.UpdateLocation(It.IsAny<PimsProperty>(), ref It.Ref<PimsProperty>.IsAny, It.IsAny<IEnumerable<UserOverrideCode>>(), false));

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditProperties(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            var response = service.UpdateProperties(acqFile, new List<UserOverrideCode>() { UserOverrideCode.AddLocationToProperty });

            // Assert
            var updatedProperty = response.PimsPropertyAcquisitionFiles.FirstOrDefault().Internal_Id.Should().Be(1);

        }

        [Fact]
        public void UpdateProperties_MatchProperties_NewProperty_Success()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit, Permissions.PropertyAdd, Permissions.PropertyView);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345, regionCode: 1);
            acqFile.PimsPropertyAcquisitionFiles = new List<PimsPropertyAcquisitionFile>() { new PimsPropertyAcquisitionFile() { Property = property } };

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            PimsPropertyAcquisitionFile updatedAcquisitionFileProperty = null;
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var filePropertyRepository = this._helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>())).Returns(acqFile.PimsPropertyAcquisitionFiles.ToList());
            filePropertyRepository.Setup(x => x.Add(It.IsAny<PimsPropertyAcquisitionFile>())).Callback<PimsPropertyAcquisitionFile>(x => updatedAcquisitionFileProperty = x).Returns(acqFile.PimsPropertyAcquisitionFiles.FirstOrDefault());

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), true)).Throws<KeyNotFoundException>();
            propertyRepository.Setup(x => x.GetPropertyRegion(It.IsAny<long>())).Returns(1);

            var propertyService = this._helper.GetService<Mock<IPropertyService>>();
            propertyService.Setup(x => x.PopulateNewProperty(It.IsAny<PimsProperty>(), It.IsAny<Boolean>(), It.IsAny<Boolean>())).Returns(
                new PimsProperty()
                {
                    PropertyDataSourceEffectiveDate = DateOnly.FromDateTime(System.DateTime.Now),
                    PropertyDataSourceTypeCode = "PMBC",
                    PropertyTypeCode = "UNKNOWN",
                    PropertyStatusTypeCode = "UNKNOWN",
                    SurplusDeclarationTypeCode = "UNKNOWN",
                    RegionCode = 1,
                }
            );
            propertyService.Setup(x => x.PopulateNewFileProperty(It.IsAny<PimsPropertyAcquisitionFile>())).Returns<PimsPropertyAcquisitionFile>(x => x);

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser(1, Guid.NewGuid(), "Test", regionCode: 1));

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditProperties(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            service.UpdateProperties(acqFile, new List<UserOverrideCode>());

            // Assert
            // since this is a new property, the following default fields should be set.
            var updatedProperty = updatedAcquisitionFileProperty.Property;
            updatedProperty.PropertyTypeCode.Should().Be("UNKNOWN");
            updatedProperty.PropertyStatusTypeCode.Should().Be("UNKNOWN");
            updatedProperty.SurplusDeclarationTypeCode.Should().Be("UNKNOWN");
            updatedProperty.PropertyDataSourceEffectiveDate.Should().Be(DateOnly.FromDateTime(DateTime.Now));
            updatedProperty.PropertyDataSourceTypeCode.Should().Be("PMBC");
            updatedProperty.IsOwned.Should().Be(false);

            filePropertyRepository.Verify(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>()), Times.Once);
            propertyService.Verify(x => x.PopulateNewProperty(It.IsAny<PimsProperty>(), It.IsAny<Boolean>(), It.IsAny<Boolean>()), Times.Once);
            propertyService.Verify(x => x.PopulateNewFileProperty(It.IsAny<PimsPropertyAcquisitionFile>()), Times.Once);
        }

        [Fact]
        public void UpdateProperties_UpdatePropertyFile_Success()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit, Permissions.PropertyAdd, Permissions.PropertyView);

            var property = EntityHelper.CreateProperty(12345, regionCode: 1);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.PimsPropertyAcquisitionFiles = new List<PimsPropertyAcquisitionFile>() { new PimsPropertyAcquisitionFile() { Internal_Id = 1, Property = property } };
            acqFile.ConcurrencyControlNumber = 1;

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var filePropertyRepository = this._helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>())).Returns(new List<PimsPropertyAcquisitionFile>() { new PimsPropertyAcquisitionFile() { Internal_Id = 1, Property = property, PropertyName = "updated" } });

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), true)).Throws<KeyNotFoundException>();
            propertyRepository.Setup(x => x.GetPropertyRegion(It.IsAny<long>())).Returns(1);

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser(1, Guid.NewGuid(), "Test", regionCode: 1));

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditProperties(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            service.UpdateProperties(acqFile, new List<UserOverrideCode>());

            // Assert
            filePropertyRepository.Verify(x => x.Update(It.IsAny<PimsPropertyAcquisitionFile>()), Times.Once);
        }

        [Fact]
        public void UpdateProperties_RemovePropertyFile_Success()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit, Permissions.PropertyAdd, Permissions.PropertyView);

            var property = EntityHelper.CreateProperty(12345);
            var acqFile = EntityHelper.CreateAcquisitionFile(1);
            acqFile.PimsPropertyAcquisitionFiles = new List<PimsPropertyAcquisitionFile>() { new() { Internal_Id = 1, Property = property } };

            var updatedAcqFile = EntityHelper.CreateAcquisitionFile(1);
            updatedAcqFile.PimsPropertyAcquisitionFiles.Clear();

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var filePropertyRepository = this._helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>())).Returns(acqFile.PimsPropertyAcquisitionFiles.ToList());

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), false)).Returns(property);
            propertyRepository.Setup(x => x.GetAllAssociationsCountById(It.IsAny<long>())).Returns(3);

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser(1, Guid.NewGuid(), "Test", regionCode: 1));

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditProperties(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            var propertyOperationService = this._helper.GetService<Mock<IPropertyOperationService>>();
            propertyOperationService.Setup(x => x.GetOperationsForProperty(It.IsAny<long>())).Returns(new List<PimsPropertyOperation>());

            // Act
            service.UpdateProperties(updatedAcqFile, new List<UserOverrideCode>());

            // Assert
            filePropertyRepository.Verify(x => x.Delete(It.IsAny<PimsPropertyAcquisitionFile>()), Times.Once);
            propertyRepository.Verify(x => x.Delete(property), Times.Never);
        }

        [Fact]
        public void UpdateProperties_RemoveProperty_Success()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit, Permissions.PropertyAdd, Permissions.PropertyView);

            var deletedProperty = EntityHelper.CreateProperty(12345);
            deletedProperty.PimsPropertyResearchFiles = new List<PimsPropertyResearchFile>();
            deletedProperty.PimsPropertyLeases = new List<PimsPropertyLease>();
            deletedProperty.PimsPropertyAcquisitionFiles = new List<PimsPropertyAcquisitionFile>() { new PimsPropertyAcquisitionFile() };

            var acqFile = EntityHelper.CreateAcquisitionFile(1);
            acqFile.PimsPropertyAcquisitionFiles = new List<PimsPropertyAcquisitionFile>() { new() { Internal_Id = 1, Property = deletedProperty } };

            var updatedAcqFile = EntityHelper.CreateAcquisitionFile(1);
            updatedAcqFile.PimsPropertyAcquisitionFiles.Clear();

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var filePropertyRepository = this._helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>())).Returns(new List<PimsPropertyAcquisitionFile>() { new PimsPropertyAcquisitionFile() { Property = deletedProperty } });
            filePropertyRepository.Setup(x => x.GetAcquisitionFilePropertyRelatedCount(It.IsAny<long>())).Returns(1);

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), true)).Returns(deletedProperty);
            propertyRepository.Setup(x => x.GetAllAssociationsById(It.IsAny<long>())).Returns(deletedProperty);
            propertyRepository.Setup(x => x.GetAllAssociationsCountById(It.IsAny<long>())).Returns(1);

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser(1, Guid.NewGuid(), "Test", regionCode: 1));

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditProperties(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            var propertyOperationService = this._helper.GetService<Mock<IPropertyOperationService>>();
            propertyOperationService.Setup(x => x.GetOperationsForProperty(It.IsAny<long>())).Returns(new List<PimsPropertyOperation>());

            // Act
            service.UpdateProperties(updatedAcqFile, new List<UserOverrideCode>());

            // Assert
            filePropertyRepository.Verify(x => x.Delete(It.IsAny<PimsPropertyAcquisitionFile>()), Times.Once);
            propertyRepository.Verify(x => x.Delete(deletedProperty), Times.Once);
        }

        [Fact]
        public void UpdateProperties_RemoveProperty_Fails_PropertyIsSubdividedOrConsolidated()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit, Permissions.PropertyAdd, Permissions.PropertyView);

            var deletedProperty = EntityHelper.CreateProperty(12345);
            deletedProperty.PimsPropertyResearchFiles = new List<PimsPropertyResearchFile>();
            deletedProperty.PimsPropertyLeases = new List<PimsPropertyLease>();
            deletedProperty.PimsPropertyAcquisitionFiles = new List<PimsPropertyAcquisitionFile>() { new PimsPropertyAcquisitionFile() };

            var acqFile = EntityHelper.CreateAcquisitionFile(1);
            acqFile.PimsPropertyAcquisitionFiles = new List<PimsPropertyAcquisitionFile>() { new() { Internal_Id = 1, Property = deletedProperty } };

            var updatedAcqFile = EntityHelper.CreateAcquisitionFile(1);
            updatedAcqFile.PimsPropertyAcquisitionFiles.Clear();

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var filePropertyRepository = this._helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>())).Returns(acqFile.PimsPropertyAcquisitionFiles.ToList());
            filePropertyRepository.Setup(x => x.GetAcquisitionFilePropertyRelatedCount(It.IsAny<long>())).Returns(1);

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), true)).Returns(deletedProperty);
            propertyRepository.Setup(x => x.GetAllAssociationsById(It.IsAny<long>())).Returns(deletedProperty);
            propertyRepository.Setup(x => x.GetAllAssociationsCountById(It.IsAny<long>())).Returns(1);

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser(1, Guid.NewGuid(), "Test", regionCode: 1));

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditProperties(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            var propertyOperationService = this._helper.GetService<Mock<IPropertyOperationService>>();
            propertyOperationService.Setup(x => x.GetOperationsForProperty(It.IsAny<long>())).Returns(new List<PimsPropertyOperation>() { new() { Internal_Id = 1, SourcePropertyId = deletedProperty.Internal_Id, SourceProperty = deletedProperty } });

            // Act
            Action act = () => service.UpdateProperties(updatedAcqFile, new List<UserOverrideCode>());

            // Assert
            act.Should().Throw<BusinessRuleViolationException>().WithMessage("This property cannot be deleted because it is part of a subdivision or consolidation");
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
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit, Permissions.PropertyAdd, Permissions.PropertyView);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.Update(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            var contractorUser = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: true);
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(contractorUser);

            var acqFileRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            acqFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditProperties(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            Action act = () => service.UpdateProperties(acqFile, new List<UserOverrideCode>());

            // Assert
            act.Should().Throw<NotAuthorizedException>().WithMessage("Contractor is not assigned to the Acquisition File's team");
        }

        [Fact]
        public void UpdateProperties_ExistingTakes()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit, Permissions.PropertyAdd, Permissions.PropertyView);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345);

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var filePropertyRepository = this._helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>())).Returns(new List<PimsPropertyAcquisitionFile>() { new PimsPropertyAcquisitionFile() { Property = property, PimsTakes = new List<PimsTake>() { new PimsTake() { TakeId = 1 } } } });
            filePropertyRepository.Setup(x => x.GetAcquisitionFilePropertyRelatedCount(It.IsAny<long>())).Returns(1);

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), false)).Throws<KeyNotFoundException>();
            propertyRepository.Setup(x => x.GetAllAssociationsById(It.IsAny<long>())).Returns(property);

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditProperties(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            Action act = () => service.UpdateProperties(acqFile, new List<UserOverrideCode>());

            // Assert
            act.Should().Throw<BusinessRuleViolationException>().WithMessage("You must remove all takes and interest holders from an acquisition file property before removing that property from an acquisition file");
        }

        [Fact]
        public void UpdateProperties_ExistingInterestHolders()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit, Permissions.PropertyAdd, Permissions.PropertyView);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345);

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var filePropertyRepository = this._helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>())).Returns(new List<PimsPropertyAcquisitionFile>() { new PimsPropertyAcquisitionFile() { Property = property, PimsInthldrPropInterests = new List<PimsInthldrPropInterest>() { new PimsInthldrPropInterest() { InterestHolderId = 1 } } } });
            filePropertyRepository.Setup(x => x.GetAcquisitionFilePropertyRelatedCount(It.IsAny<long>())).Returns(1);

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), false)).Throws<KeyNotFoundException>();
            propertyRepository.Setup(x => x.GetAllAssociationsById(It.IsAny<long>())).Returns(property);

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditProperties(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            Action act = () => service.UpdateProperties(acqFile, new List<UserOverrideCode>());

            // Assert
            act.Should().Throw<BusinessRuleViolationException>().WithMessage("You must remove all takes and interest holders from an acquisition file property before removing that property from an acquisition file");
        }

        [Fact]
        public void UpdateProperties_Final()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit, Permissions.AcquisitionFileAdd, Permissions.AcquisitionFileView, Permissions.PropertyAdd, Permissions.PropertyView);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.ConcurrencyControlNumber = 1;
            acqFile.AcquisitionFileStatusTypeCode = AcquisitionStatusTypes.COMPLT.ToString();

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var filePropertyRepository = this._helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>())).Returns(acqFile.PimsPropertyAcquisitionFiles.ToList());

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            Action act = () => service.UpdateProperties(acqFile, new List<UserOverrideCode>());

            // Assert
            act.Should().Throw<BusinessRuleViolationException>().WithMessage("The file you are editing is not active or hold, so you cannot save changes. Refresh your browser to see file state.");
        }

        [Fact]
        public void UpdateProperties_ValidatePropertyRegions_Success()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit, Permissions.PropertyAdd, Permissions.PropertyView);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345, regionCode: 1);
            acqFile.PimsPropertyAcquisitionFiles = new List<PimsPropertyAcquisitionFile>() { new() { Property = property } };

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), true)).Returns(property);
            propertyRepository.Setup(x => x.GetPropertyRegion(It.IsAny<long>())).Returns(1);

            var filePropertyRepository = this._helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>())).Returns(acqFile.PimsPropertyAcquisitionFiles.ToList());

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser(1, Guid.NewGuid(), "Test", regionCode: 1));

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditProperties(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            service.UpdateProperties(acqFile, new List<UserOverrideCode>());

            // Assert
            filePropertyRepository.Verify(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void UpdateProperties_ValidatePropertyRegions_Error()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit, Permissions.PropertyAdd, Permissions.PropertyView);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345, regionCode: 3);
            acqFile.PimsPropertyAcquisitionFiles = new List<PimsPropertyAcquisitionFile>() { new() { Property = property } };

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), true)).Returns(property);
            propertyRepository.Setup(x => x.GetPropertyRegion(It.IsAny<long>())).Returns(3);

            var filePropertyRepository = this._helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>())).Returns(acqFile.PimsPropertyAcquisitionFiles.ToList());

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser(1, Guid.NewGuid(), "Test", regionCode: 1));

            // Act
            Action act = () => service.UpdateProperties(acqFile, new List<UserOverrideCode>());

            // Assert
            var exception = act.Should().Throw<BadRequestException>();
            exception.WithMessage("You cannot add a property that is outside of your user account region(s)*"); // partial match on the error message - as documented by FluentAssertions
        }


        [Fact]
        public void UpdateProperties_WithRetiredProperty_Should_Fail()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit, Permissions.PropertyAdd, Permissions.PropertyView);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.ConcurrencyControlNumber = 1;

            PimsProperty property = EntityHelper.CreateProperty(12345, regionCode: 3);
            PimsProperty retiredProperty = new PimsProperty()
            {
                PropertyId = 100,
                Pid = 1000,
                IsRetired = true,
            };

            acqFile.PimsPropertyAcquisitionFiles = new List<PimsPropertyAcquisitionFile>() {
                new()
                {
                    Property = retiredProperty
                }
            };


            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), true)).Returns(retiredProperty);
            propertyRepository.Setup(x => x.GetPropertyRegion(It.IsAny<long>())).Returns(3);

            var filePropertyRepository = this._helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>())).Returns(acqFile.PimsPropertyAcquisitionFiles.ToList());

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser(1, Guid.NewGuid(), "Test", regionCode: 1));

            // Act
            Action act = () => service.UpdateProperties(acqFile, new List<UserOverrideCode>());

            // Assert
            var ex = act.Should().Throw<BusinessRuleViolationException>();
            ex.WithMessage("Retired property can not be selected.");
        }

        [Fact]
        public void UpdateProperties_WithProperty_SelectedForCompensation_Should_Fail()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit, Permissions.PropertyAdd, Permissions.PropertyView);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.ConcurrencyControlNumber = 1;

            var property = EntityHelper.CreateProperty(12345, regionCode: 1);
            acqFile.PimsPropertyAcquisitionFiles = new List<PimsPropertyAcquisitionFile>() {
                new PimsPropertyAcquisitionFile() { PropertyAcquisitionFileId=100, Property = property },
                new PimsPropertyAcquisitionFile() { PropertyAcquisitionFileId=101, Property = property },
            };

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var propertyRepository = this._helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>(), true)).Returns(property);
            propertyRepository.Setup(x => x.GetPropertyRegion(It.IsAny<long>())).Returns(1);

            var filePropertyRepository = this._helper.GetService<Mock<IAcquisitionFilePropertyRepository>>();
            filePropertyRepository.Setup(x => x.GetPropertiesByAcquisitionFileId(It.IsAny<long>())).Returns(acqFile.PimsPropertyAcquisitionFiles.ToList());
            filePropertyRepository.Setup(x => x.AcquisitionFilePropertyInCompensationReq(It.IsAny<long>())).Returns(true);

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser(1, Guid.NewGuid(), "Test", regionCode: 1));

            var propertyService = this._helper.GetService<Mock<IPropertyService>>();
            propertyService.Setup(x => x.UpdateLocation(It.IsAny<PimsProperty>(), ref It.Ref<PimsProperty>.IsAny, It.IsAny<IEnumerable<UserOverrideCode>>(), false));

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditProperties(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            var property2 = EntityHelper.CreateProperty(56789, regionCode: 1);

            acqFile.PimsPropertyAcquisitionFiles = new List<PimsPropertyAcquisitionFile>()
            {
                new()
                {
                    PropertyAcquisitionFileId = 100,
                    Property = property2,
                },
            };

            // Act
            Action act = () => service.UpdateProperties(acqFile, new List<UserOverrideCode>() { UserOverrideCode.AddLocationToProperty });

            // Assert
            var ex = act.Should().Throw<BusinessRuleViolationException>().WithMessage("Acquisition File property can not be removed since it's assigned as a property for a compensation requisition");
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
            result.FirstOrDefault().ChklstItemStatusTypeCode.Should().Be(ChecklistItemStatusTypes.INCOMP.ToString());
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

            var checklistItems = new List<PimsAcquisitionChecklistItem>() { new PimsAcquisitionChecklistItem() { Internal_Id = 1, ChklstItemStatusTypeCode = ChecklistItemStatusTypes.COMPLT.ToString() } };

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var fileChecklistRepository = this._helper.GetService<Mock<IAcquisitionFileChecklistRepository>>();
            fileChecklistRepository.Setup(x => x.GetAllChecklistItemsByAcquisitionFileId(It.IsAny<long>()))
                .Returns(new List<PimsAcquisitionChecklistItem>() { new PimsAcquisitionChecklistItem() { Internal_Id = 1, ChklstItemStatusTypeCode = ChecklistItemStatusTypes.INCOMP.ToString() } });

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditChecklists(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            service.UpdateChecklistItems(checklistItems);

            // Assert
            fileChecklistRepository.Verify(x => x.GetAllChecklistItemsByAcquisitionFileId(It.IsAny<long>()), Times.Once);
            fileChecklistRepository.Verify(x => x.Update(It.IsAny<PimsAcquisitionChecklistItem>()), Times.Once);
            repository.Verify(x => x.GetById(It.IsAny<long>()), Times.Exactly(3));
        }


        [Fact]
        public void UpdateChecklist_NoEmptyList()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions();

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = this._helper.GetService<Mock<IAcquisitionFileChecklistRepository>>();
            repository.Setup(x => x.GetAllChecklistItemsByAcquisitionFileId(It.IsAny<long>())).Returns(acqFile.PimsAcquisitionChecklistItems.ToList());

            // Act
            Action act = () => service.UpdateChecklistItems(new List<PimsAcquisitionChecklistItem>());

            // Assert
            act.Should().Throw<BadRequestException>();
            repository.Verify(x => x.GetAllChecklistItemsByAcquisitionFileId(It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public void UpdateChecklist_ItemNotFound()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var checklistItems = new List<PimsAcquisitionChecklistItem>() { new PimsAcquisitionChecklistItem() { Internal_Id = 999, ChklstItemStatusTypeCode = ChecklistItemStatusTypes.COMPLT.ToString() } };

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.AcquisitionFileStatusTypeCode = AcquisitionStatusTypes.ACTIVE.ToString();

            var acqRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            acqRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var fileChecklistRepository = this._helper.GetService<Mock<IAcquisitionFileChecklistRepository>>();
            fileChecklistRepository.Setup(x => x.GetAllChecklistItemsByAcquisitionFileId(It.IsAny<long>()))
                .Returns(new List<PimsAcquisitionChecklistItem>() { new PimsAcquisitionChecklistItem() { Internal_Id = 1, ChklstItemStatusTypeCode = ChecklistItemStatusTypes.INCOMP.ToString() } });

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditChecklists(It.IsAny<AcquisitionStatusTypes?>())).Returns(true);

            // Act
            Action act = () => service.UpdateChecklistItems(checklistItems);

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

            var checklistItems = new List<PimsAcquisitionChecklistItem>() { new PimsAcquisitionChecklistItem() { Internal_Id = 999, ChklstItemStatusTypeCode = ChecklistItemStatusTypes.COMPLT.ToString() } };
            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = this._helper.GetService<Mock<IAcquisitionFileChecklistRepository>>();
            repository.Setup(x => x.GetAllChecklistItemsByAcquisitionFileId(It.IsAny<long>())).Returns(acqFile.PimsAcquisitionChecklistItems.ToList());

            // Act
            Action act = () => service.UpdateChecklistItems(checklistItems);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.GetAllChecklistItemsByAcquisitionFileId(It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public void UpdateChecklist_NotAuthorized_Contractor()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var checklistItems = new List<PimsAcquisitionChecklistItem>() { new PimsAcquisitionChecklistItem() { Internal_Id = 999, ChklstItemStatusTypeCode = ChecklistItemStatusTypes.COMPLT.ToString() } };
            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = this._helper.GetService<Mock<IAcquisitionFileChecklistRepository>>();
            repository.Setup(x => x.GetAllChecklistItemsByAcquisitionFileId(It.IsAny<long>())).Returns(acqFile.PimsAcquisitionChecklistItems.ToList());

            var acqFileRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            acqFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            var contractorUser = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: true);
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(contractorUser);

            // Act
            Action act = () => service.UpdateChecklistItems(checklistItems);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void UpdateChecklist_InvalidStatus()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            var checklistItems = new List<PimsAcquisitionChecklistItem>() { new PimsAcquisitionChecklistItem() { Internal_Id = 1, ChklstItemStatusTypeCode = ChecklistItemStatusTypes.COMPLT.ToString() } };

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var fileChecklistRepository = this._helper.GetService<Mock<IAcquisitionFileChecklistRepository>>();
            fileChecklistRepository.Setup(x => x.GetAllChecklistItemsByAcquisitionFileId(It.IsAny<long>()))
                .Returns(new List<PimsAcquisitionChecklistItem>() { new PimsAcquisitionChecklistItem() { Internal_Id = 1, ChklstItemStatusTypeCode = ChecklistItemStatusTypes.INCOMP.ToString() } });

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            var solver = this._helper.GetService<Mock<IAcquisitionStatusSolver>>();
            solver.Setup(x => x.CanEditChecklists(It.IsAny<AcquisitionStatusTypes?>())).Returns(false);

            // Act
            Action act = () => service.UpdateChecklistItems(checklistItems);

            // Assert
            act.Should().Throw<BusinessRuleViolationException>();
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
        public void GetAgreementById_NoPermission()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions();

            // Act
            Action act = () => service.GetAgreementById(1, 10);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void GetAgreementById_NoPermission_IsContractor()
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
            Action act = () => service.GetAgreementById(1, 10);

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
        public void UpdateAgreement_Should_Fail_NoPermission()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions();

            // Act
            Action act = () => service.UpdateAgreement(1, new()
            {
                AcquisitionFileId = 1,
                AgreementId = 10,
            });

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void UpdateAgreement_NoPermission_IsContractor()
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
            Action act = () => service.UpdateAgreement(1, It.IsAny<PimsAgreement>());

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void DeleteAgreement_Fail_NoPermission()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions();

            // Act
            Action act = () => service.DeleteAgreement(1, 10);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void DeleteAgreement_Fail_NoPermission_IsContractor()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AgreementView);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var repository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acqFile);

            var contractorUser = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: true);
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(contractorUser);

            // Act
            Action act = () => service.DeleteAgreement(1, 10);

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
            acquisitionFile.FileNo = 2023;
            acquisitionFile.RegionCode = 1;
            acquisitionFile.FileNoSuffix = 1;
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
            Assert.Equal("01-2023-01", result[0].FileNumber);
            Assert.Equal("01-2023-01", result[1].FileNumber);
            Assert.Equal("8000", result[0].Pid);
            Assert.Equal("9000", result[1].Pid);
            acqFilerepository.Verify(x => x.GetAcquisitionFileExportDeep(It.IsAny<AcquisitionFilter>(), It.IsAny<HashSet<short>>(), It.IsAny<long?>()), Times.Once);
        }

        #endregion

        #region SubFiles

        [Fact]
        public void GetSubFiles_NoPermissions()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions();

            // Act
            Action act = () => service.GetAcquisitionSubFiles(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void GetSubFiles_NotAuthorized_Contractor()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileView);

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            var contractorUser = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: true);
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(contractorUser);

            var acqFileRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            acqFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(EntityHelper.CreateAcquisitionFile());

            // Act
            Action act = () => service.GetAcquisitionSubFiles(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void GetSubFiles_Fail_FileIsSubFile()
        {
            // Arrange
            var service = this.CreateAcquisitionServiceWithPermissions(Permissions.AcquisitionFileView);

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            var contractorUser = EntityHelper.CreateUser(1, Guid.NewGuid(), username: "Test", isContractor: false);
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(contractorUser);

            var mockCurrentAcquisitionFile = EntityHelper.CreateAcquisitionFile();
            mockCurrentAcquisitionFile.PrntAcquisitionFileId = 200;

            var acqFileRepository = this._helper.GetService<Mock<IAcquisitionFileRepository>>();
            acqFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(mockCurrentAcquisitionFile);

            // Act
            Action act = () => service.GetAcquisitionSubFiles(1);

            // Assert
            act.Should().Throw<BadRequestException>().WithMessage("Acquisition file should not be a sub-file.");
        }

        #endregion

        #endregion
    }
}
