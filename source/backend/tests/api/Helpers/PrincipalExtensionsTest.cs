using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using FluentAssertions;
using k8s.KubeConfigModels;
using Moq;
using Pims.Api.Services;
using Pims.Core.Exceptions;
using Pims.Core.Extensions;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Xunit;

namespace Pims.Api.Test.Helpers
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("area", "helpers")]
    [Trait("group", "permissons")]
    [ExcludeFromCodeCoverage]
    public class PrincipalExtensionsTest
    {
        private TestHelper _helper;

        public PrincipalExtensionsTest()
        {
            this._helper = new TestHelper();
        }

        #region Tests
        [Fact]
        public void ThrowInvalidAccessToLeaseFile_Success()
        {
            // Arrange
            this._helper.Create<LeaseService>();

            var principal = new ClaimsPrincipal();
            var lease = EntityHelper.CreateLease(1);
            lease.RegionCode = 1;
            var pimsUser = EntityHelper.CreateUser("testuser", regionCode: 1);

            var leaseRepository = _helper.GetService<Mock<ILeaseRepository>>();
            leaseRepository.Setup(x => x.GetNoTracking(It.IsAny<long>())).Returns(lease);

            var userRepository = _helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(pimsUser);

            var projectRepository = _helper.GetService<Mock<IProjectRepository>>();

            // Act
            Action act = () => principal.ThrowInvalidAccessToLeaseFile(userRepository.Object, leaseRepository.Object, projectRepository.Object, 1);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void ThrowInvalidAccessToLeaseFile_ContractorNotAssigned_Error()
        {
            // Arrange
            this._helper.Create<LeaseService>();

            var principal = new ClaimsPrincipal();
            var lease = EntityHelper.CreateLease(1);
            lease.RegionCode = 1;
            var pimsUser = EntityHelper.CreateUser("testuser", regionCode: 1, isContractor: true);

            var leaseRepository = _helper.GetService<Mock<ILeaseRepository>>();
            leaseRepository.Setup(x => x.GetNoTracking(It.IsAny<long>())).Returns(lease);

            var userRepository = _helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(pimsUser);

            var projectRepository = _helper.GetService<Mock<IProjectRepository>>();

            // Act
            Action act = () => principal.ThrowInvalidAccessToLeaseFile(userRepository.Object, leaseRepository.Object, projectRepository.Object, 1);

            // Assert
            act.Should().Throw<NotAuthorizedException>().WithMessage("Contractor is not assigned to the Lease File's team or the associated Project's team");
        }

        [Fact]
        public void ThrowInvalidAccessToLeaseFile_ContractorInTeam_Success()
        {
            // Arrange
            this._helper.Create<LeaseService>();

            var principal = new ClaimsPrincipal();
            var lease = EntityHelper.CreateLease(1);
            lease.RegionCode = 1;
            lease.PimsLeaseLicenseTeams.Add(new() { PersonId = 1 });
            var pimsUser = EntityHelper.CreateUser("testuser", regionCode: 1, isContractor: true);
            pimsUser.PersonId = 1;

            var leaseRepository = _helper.GetService<Mock<ILeaseRepository>>();
            leaseRepository.Setup(x => x.GetNoTracking(It.IsAny<long>())).Returns(lease);

            var userRepository = _helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(pimsUser);

            var projectRepository = _helper.GetService<Mock<IProjectRepository>>();

            // Act
            Action act = () => principal.ThrowInvalidAccessToLeaseFile(userRepository.Object, leaseRepository.Object, projectRepository.Object, 1);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void ThrowInvalidAccessToLeaseFile_ContractorInProject_Success()
        {
            // Arrange
            this._helper.Create<LeaseService>();

            var principal = new ClaimsPrincipal();
            var lease = EntityHelper.CreateLease(1);
            lease.RegionCode = 1;
            lease.Project = EntityHelper.CreateProject(1, "TESTPROJ", "Test Project");
            lease.Project.PimsProjectPeople.Add(new PimsProjectPerson() { PersonId = 1 });
            lease.ProjectId = lease.Project.Internal_Id;

            var pimsUser = EntityHelper.CreateUser("testuser", regionCode: 1, isContractor: true);
            pimsUser.PimsRegionUsers.Add(new PimsRegionUser() { RegionCode = 1 });
            pimsUser.PersonId = 1;

            var leaseRepository = _helper.GetService<Mock<ILeaseRepository>>();
            leaseRepository.Setup(x => x.GetNoTracking(It.IsAny<long>())).Returns(lease);

            var userRepository = _helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(pimsUser);

            var projectRepository = _helper.GetService<Mock<IProjectRepository>>();
            projectRepository.Setup(x => x.TryGet(It.IsAny<long>())).Returns(lease.Project);

            // Act
            Action act = () => principal.ThrowInvalidAccessToLeaseFile(userRepository.Object, leaseRepository.Object, projectRepository.Object, 1);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void ThrowInvalidAccessToAcquisitionFile_ContractorNotAssigned_Error()
        {
            // Arrange
            this._helper.Create<AcquisitionFileService>();

            var principal = new ClaimsPrincipal();
            var userRepository = _helper.GetService<Mock<IUserRepository>>();
            var acquisitionFileRepository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
            var pimsUser = EntityHelper.CreateUser(1, Guid.NewGuid(), "testuser", regionCode: 1, isContractor: true);
            var acquisitionFile = EntityHelper.CreateAcquisitionFile(1);

            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(pimsUser);
            acquisitionFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acquisitionFile);

            // Act
            Action act = () => principal.ThrowInvalidAccessToAcquisitionFile(userRepository.Object, acquisitionFileRepository.Object, 1);

            // Assert
            act.Should().Throw<NotAuthorizedException>().WithMessage("Contractor is not assigned to the Acquisition File's team or the associated Project's team");
        }

        [Fact]
        public void ThrowInvalidAccessToAcquisitionFile_ContractorInTeam_Success()
        {
            // Arrange
            this._helper.Create<AcquisitionFileService>();

            var principal = new ClaimsPrincipal();
            var userRepository = _helper.GetService<Mock<IUserRepository>>();
            var acquisitionFileRepository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
            var pimsUser = EntityHelper.CreateUser(1, Guid.NewGuid(), "testuser", regionCode: 1, isContractor: true);
            var acquisitionFile = EntityHelper.CreateAcquisitionFile(1);
            acquisitionFile.PimsAcquisitionFileTeams.Add(new PimsAcquisitionFileTeam { PersonId = pimsUser.PersonId });

            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(pimsUser);
            acquisitionFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acquisitionFile);

            // Act
            Action act = () => principal.ThrowInvalidAccessToAcquisitionFile(userRepository.Object, acquisitionFileRepository.Object, 1);

            // Assert
            act.Should().NotThrow<NotAuthorizedException>();
        }

        [Fact]
        public void ThrowInvalidAccessToAcquisitionFile_ContractorInProject_Success()
        {
            // Arrange
            this._helper.Create<AcquisitionFileService>();

            var principal = new ClaimsPrincipal();
            var userRepository = _helper.GetService<Mock<IUserRepository>>();
            var acquisitionFileRepository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
            var pimsUser = EntityHelper.CreateUser(1, Guid.NewGuid(), "testuser", regionCode: 1, isContractor: true);
            pimsUser.PersonId = 1;
            var acquisitionFile = EntityHelper.CreateAcquisitionFile(1);
            acquisitionFile.Project = EntityHelper.CreateProject(1, "TESTPROJ", "Test Project");
            acquisitionFile.Project.PimsProjectPeople.Add(new PimsProjectPerson() { PersonId = 1 });
            acquisitionFile.ProjectId = acquisitionFile.Project.Internal_Id;

            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(pimsUser);
            acquisitionFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acquisitionFile);

            // Act
            Action act = () => principal.ThrowInvalidAccessToAcquisitionFile(userRepository.Object, acquisitionFileRepository.Object, 1);

            // Assert
            act.Should().NotThrow<NotAuthorizedException>();
        }

        [Fact]
        public void ThrowInvalidAccessToDispositionFile_ContractorNotAssigned_Error()
        {
            // Arrange
            this._helper.Create<DispositionFileService>();

            var principal = new ClaimsPrincipal();
            var userRepository = _helper.GetService<Mock<IUserRepository>>();
            var dispositionFileRepository = _helper.GetService<Mock<IDispositionFileRepository>>();
            var pimsUser = EntityHelper.CreateUser(1, Guid.NewGuid(), "testuser", regionCode: 1, isContractor: true);
            var dispositionFile = EntityHelper.CreateDispositionFile(1);

            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(pimsUser);
            dispositionFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(dispositionFile);

            // Act
            Action act = () => principal.ThrowInvalidAccessToDispositionFile(userRepository.Object, dispositionFileRepository.Object, 1);

            // Assert
            act.Should().Throw<NotAuthorizedException>().WithMessage("Contractor is not assigned to the Disposition File's team");
        }

        [Fact]
        public void ThrowInvalidAccessToDispositionFile_ContractorInTeam_Success()
        {
            // Arrange
            this._helper.Create<DispositionFileService>();

            var principal = new ClaimsPrincipal();
            var userRepository = _helper.GetService<Mock<IUserRepository>>();
            var dispositionFileRepository = _helper.GetService<Mock<IDispositionFileRepository>>();
            var pimsUser = EntityHelper.CreateUser(1, Guid.NewGuid(), "testuser", regionCode: 1, isContractor: true);
            var dispositionFile = EntityHelper.CreateDispositionFile(1);
            dispositionFile.PimsDispositionFileTeams.Add(new PimsDispositionFileTeam { PersonId = 1 });

            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(pimsUser);
            dispositionFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(dispositionFile);

            // Act
            Action act = () => principal.ThrowInvalidAccessToDispositionFile(userRepository.Object, dispositionFileRepository.Object, 1);

            // Assert
            act.Should().NotThrow<NotAuthorizedException>();
        }

        [Fact]
        public void ThrowInvalidAccessToDispositionFile_ContractorInProject_Error()
        {
            // Arrange
            this._helper.Create<DispositionFileService>();

            var principal = new ClaimsPrincipal();
            var userRepository = _helper.GetService<Mock<IUserRepository>>();
            var dispositionFileRepository = _helper.GetService<Mock<IDispositionFileRepository>>();
            var dispositionFile = EntityHelper.CreateDispositionFile(1);
            var pimsUser = EntityHelper.CreateUser(1, Guid.NewGuid(), "testuser", regionCode: 1, isContractor: true);
            pimsUser.PersonId = 1;
            dispositionFile.Project = EntityHelper.CreateProject(1, "TESTPROJ", "Test Project");
            dispositionFile.Project.PimsProjectPeople.Add(new PimsProjectPerson() { PersonId = 1 });
            dispositionFile.ProjectId = dispositionFile.Project.Internal_Id;

            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(pimsUser);
            dispositionFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(dispositionFile);

            // Act
            Action act = () => principal.ThrowInvalidAccessToDispositionFile(userRepository.Object, dispositionFileRepository.Object, 1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }
        #endregion
    }
}
