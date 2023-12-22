using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Moq;
using Pims.Core.Exceptions;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Xunit;

namespace Pims.Dal.Test.Repositories
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("group", "disposition")]
    [ExcludeFromCodeCoverage]
    public class DispositionFileRepositoryTest
    {
        private readonly TestHelper _helper;

        #region Constructors
        public DispositionFileRepositoryTest()
        {
            _helper = new TestHelper();
        }
        #endregion

        private DispositionFileRepository CreateRepositoryWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            _helper.CreatePimsContext(user, true);
            return _helper.CreateRepository<DispositionFileRepository>(user);
        }

        #region Tests

        [Fact]
        public void Add_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DispositionAdd);
            var dispositionFile = EntityHelper.CreateDispositionFile();

            var repository = helper.CreateRepository<DispositionFileRepository>(user);

            var mockSequenceRepo = new Mock<ISequenceRepository>();
            mockSequenceRepo.Setup(x => x.GetNextSequenceValue(It.IsAny<string>())).Returns(100);

            // Act
            var result = repository.Add(dispositionFile);

            // Assert

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsDispositionFile>();
            result.DispositionFileId.Should().Be(1);
            result.FileNumber.Equals("D-100");
        }


        #region GetById
        [Fact]
        public void GetById_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);
            var dispFile = EntityHelper.CreateDispositionFile();
            _helper.AddAndSaveChanges(dispFile);

            // Act
            var result = repository.GetById(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsDispositionFile>();
            result.FileName.Should().Be("Test Disposition File");
            result.DispositionFileId.Should().Be(dispFile.DispositionFileId);
        }

        [Fact]
        public void GetById_NotFound()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);

            // Act
            Action act = () => repository.GetById(1);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }
        #endregion

        #region GetById
        [Fact]
        public void GetRowVersion_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);
            var dispFile = EntityHelper.CreateDispositionFile();
            _helper.AddAndSaveChanges(dispFile);

            // Act
            var result = repository.GetRowVersion(1);

            // Assert
            result.Should().Be(2);
        }

        [Fact]
        public void GetRowVersion_NotFound()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);

            // Act
            Action act = () => repository.GetRowVersion(1);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }

        #endregion

        #region GetLastUpdateBy
        [Fact]
        public void GetLastUpdateBy_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);
            var dispFile = EntityHelper.CreateDispositionFile();
            dispFile.AppLastUpdateUserid = "test";
            dispFile.AppLastUpdateTimestamp = DateTime.Now;
            _helper.AddAndSaveChanges(dispFile);

            // Act
            var result = repository.GetLastUpdateBy(1);

            // Assert
            result.AppLastUpdateUserid.Should().Be("service");
            result.AppLastUpdateTimestamp.Should().BeSameDateAs(dispFile.AppLastUpdateTimestamp);
        }

        #endregion

        #region GetPageDeep
        [Fact]
        public void GetPageDeep_NoFilter_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);
            var dispFile = EntityHelper.CreateDispositionFile();
            _helper.AddAndSaveChanges(dispFile);

            // Act
            var result = repository.GetPageDeep(new DispositionFilter());

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetPageDeep_DispositionName_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);
            var dispFile = EntityHelper.CreateDispositionFile();
            dispFile.FileName = "fileName";
            _helper.AddAndSaveChanges(dispFile);

            // Act
            var result = repository.GetPageDeep(new DispositionFilter() { FileNameOrNumberOrReference = "fileName" });

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetPageDeep_DispositionName_NotFound()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);
            var dispFile = EntityHelper.CreateDispositionFile();
            dispFile.FileName = "fileName";
            _helper.AddAndSaveChanges(dispFile);

            // Act
            var result = repository.GetPageDeep(new DispositionFilter() { FileNameOrNumberOrReference = "notFound" });

            // Assert
            result.Should().HaveCount(0);
        }

        [Fact]
        public void GetPageDeep_DispositionNumber_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);
            var dispFile = EntityHelper.CreateDispositionFile();
            dispFile.FileNumber = "fileNumber";
            _helper.AddAndSaveChanges(dispFile);

            // Act
            var result = repository.GetPageDeep(new DispositionFilter() { FileNameOrNumberOrReference = "fileNumber" });

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetPageDeep_DispositionHistoricalNumber_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);
            var dispFile = EntityHelper.CreateDispositionFile();
            dispFile.FileReference = "legacy";
            _helper.AddAndSaveChanges(dispFile);

            // Act
            var result = repository.GetPageDeep(new DispositionFilter() { FileNameOrNumberOrReference = "legacy" });

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetPageDeep_DispositionTeamMemberPerson_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);
            var dispFile = EntityHelper.CreateDispositionFile();
            var person = EntityHelper.CreatePerson(1, "tester", "chester");
            dispFile.PimsDispositionFileTeams.Add(new PimsDispositionFileTeam() { DispositionFileId = dispFile.Internal_Id, PersonId = person.Internal_Id, Person = person, DspFlTeamProfileTypeCode = "COORD" });
            _helper.AddAndSaveChanges(dispFile);

            // Act
            var result = repository.GetPageDeep(new DispositionFilter() { TeamMemberPersonId = 1 });

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetPageDeep_DispositionTeamMemberOrganization_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);
            var dispFile = EntityHelper.CreateDispositionFile();
            var org = EntityHelper.CreateOrganization(1, "tester org");
            dispFile.PimsDispositionFileTeams.Add(new PimsDispositionFileTeam() { DispositionFileId = dispFile.Internal_Id, OrganizationId = org.Internal_Id, Organization = org, DspFlTeamProfileTypeCode = "COORD" });
            _helper.AddAndSaveChanges(dispFile);

            // Act
            var result = repository.GetPageDeep(new DispositionFilter() { TeamMemberOrganizationId = 1 });

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetPageDeep_DispositionTeamMember_NotFound()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.DispositionView);
            var dispFile = EntityHelper.CreateDispositionFile();
            var person = EntityHelper.CreatePerson(1, "tester", "chester");
            dispFile.PimsDispositionFileTeams.Add(new PimsDispositionFileTeam() { DispositionFileId = dispFile.Internal_Id, PersonId = person.Internal_Id, Person = person, DspFlTeamProfileTypeCode = "COORD" });
            _helper.AddAndSaveChanges(dispFile);

            // Act
            var result = repository.GetPageDeep(new DispositionFilter() { TeamMemberPersonId = 2 });

            // Assert
            result.Should().HaveCount(0);
        }
        #endregion

        #endregion
    }
}
