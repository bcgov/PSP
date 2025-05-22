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
        public void LeaseRegionUserAccess_NoRegion_Success()
        {
            var pimsUser = EntityHelper.CreateUser("testuser");
            pimsUser.ThrowInvalidAccessToLeaseFile(null);
        }

        [Fact]
        public void LeaseRegionUserAccess_HasRegion_Failure()
        {
            var pimsUser = EntityHelper.CreateUser(1, Guid.NewGuid(), "testuser", regionCode: 1);
            Action act = () => pimsUser.ThrowInvalidAccessToLeaseFile(2);
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void LeaseRegionUserAccess_HasRegion_Success()
        {
            var pimsUser = EntityHelper.CreateUser(1, Guid.NewGuid(), "testuser", regionCode: 1);
            Action act = () => pimsUser.ThrowInvalidAccessToLeaseFile(1);
            act.Should().NotThrow<NotAuthorizedException>();
        }

        #region Tests
        [Fact]
        public void ThrowInvalidAccessToAcquisitionFile_ContractorNotAssigned_ThrowsNotAuthorizedException()
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
            act.Should().Throw<NotAuthorizedException>().WithMessage("Contractor is not assigned to the Acquisition File's team");
        }

        [Fact]
        public void ThrowInvalidAccessToAcquisitionFile_ContractorAssigned_DoesNotThrow()
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
        public void ThrowInvalidAccessToAcquisitionFile_ContractorProject_DoesNotThrow()
        {
            // Arrange
            this._helper.Create<AcquisitionFileService>();

            var principal = new ClaimsPrincipal();
            var userRepository = _helper.GetService<Mock<IUserRepository>>();
            var acquisitionFileRepository = _helper.GetService<Mock<IAcquisitionFileRepository>>();
            var pimsUser = EntityHelper.CreateUser(1, Guid.NewGuid(), "testuser", regionCode: 1, isContractor: true);
            pimsUser.PersonId = 1;
            var acquisitionFile = EntityHelper.CreateAcquisitionFile(1);
            acquisitionFile.Project = new PimsProject() { PimsProjectPeople = new List<PimsProjectPerson>() { new PimsProjectPerson() { PersonId = 1 } } };

            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(pimsUser);
            acquisitionFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(acquisitionFile);

            // Act
            Action act = () => principal.ThrowInvalidAccessToAcquisitionFile(userRepository.Object, acquisitionFileRepository.Object, 1);

            // Assert
            act.Should().NotThrow<NotAuthorizedException>();
        }

        [Fact]
        public void ThrowInvalidAccessToDispositionFile_ContractorNotAssigned_ThrowsNotAuthorizedException()
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
        public void ThrowInvalidAccessToDispositionFile_ContractorAssigned_DoesNotThrow()
        {
            // Arrange
            this._helper.Create<DispositionFileService>();

            var principal = new ClaimsPrincipal();
            var userRepository = _helper.GetService<Mock<IUserRepository>>();
            var dispositionFileRepository = _helper.GetService<Mock<IDispositionFileRepository>>();
            var pimsUser = EntityHelper.CreateUser(1, Guid.NewGuid(), "testuser", regionCode: 1, isContractor: true);
            var dispositionFile = EntityHelper.CreateDispositionFile(1);
            dispositionFile.PimsDispositionFileTeams.Add(new PimsDispositionFileTeam { PersonId = pimsUser.PersonId });

            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(pimsUser);
            dispositionFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(dispositionFile);

            // Act
            Action act = () => principal.ThrowInvalidAccessToDispositionFile(userRepository.Object, dispositionFileRepository.Object, 1);

            // Assert
            act.Should().NotThrow<NotAuthorizedException>();
        }

        [Fact]
        public void ThrowInvalidAccessToDispositionFile_ContractorProject_DoesThrow()
        {
            // Arrange
            this._helper.Create<DispositionFileService>();

            var principal = new ClaimsPrincipal();
            var userRepository = _helper.GetService<Mock<IUserRepository>>();
            var dispositionFileRepository = _helper.GetService<Mock<IDispositionFileRepository>>();
            var dispositionFile = EntityHelper.CreateDispositionFile(1);
            var pimsUser = EntityHelper.CreateUser(1, Guid.NewGuid(), "testuser", regionCode: 1, isContractor: true);
            pimsUser.PersonId = 1;
            dispositionFile.Project = new PimsProject() { PimsProjectPeople = new List<PimsProjectPerson>() { new PimsProjectPerson() { PersonId = 1 } } };

            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(pimsUser);
            dispositionFileRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(dispositionFile);

            // Act
            Action act = () => principal.ThrowInvalidAccessToDispositionFile(userRepository.Object, dispositionFileRepository.Object, 1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }
        #endregion
        #endregion
    }
}
