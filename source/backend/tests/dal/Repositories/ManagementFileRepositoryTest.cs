using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Moq;
using Pims.Core.Exceptions;
using Pims.Core.Security;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Repositories;
using Xunit;

namespace Pims.Dal.Test.Repositories
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("group", "management")]
    [ExcludeFromCodeCoverage]
    public class ManagementFileRepositoryTest
    {
        private readonly TestHelper _helper;

        #region Constructors
        public ManagementFileRepositoryTest()
        {
            _helper = new TestHelper();
        }
        #endregion

        private ManagementFileRepository CreateRepositoryWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            _helper.CreatePimsContext(user, true);
            return _helper.CreateRepository<ManagementFileRepository>(user);
        }

        #region Tests

        #region Add
        [Fact]
        public void Add_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ManagementAdd);
            var managementFile = EntityHelper.CreateManagementFile();

            var repository = helper.CreateRepository<ManagementFileRepository>(user);

            var mockSequenceRepo = new Mock<ISequenceRepository>();
            mockSequenceRepo.Setup(x => x.GetNextSequenceValue(It.IsAny<string>())).Returns(100);

            // Act
            var result = repository.Add(managementFile);

            // Assert

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsManagementFile>();
            result.ManagementFileId.Should().Be(1);

        }

        [Fact]
        public void Add_WithRetiredProperty_Should_Fail()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ManagementAdd);
            var managementFile = EntityHelper.CreateManagementFile();

            managementFile.PimsManagementFileProperties = new List<PimsManagementFileProperty>()
            {
                new PimsManagementFileProperty()
                {
                    PropertyId = 100,
                    Property = new PimsProperty()
                    {
                        IsRetired = true,
                    }
                },
            };

            var repository = helper.CreateRepository<ManagementFileRepository>(user);

            // Act
            Action act = () => repository.Add(managementFile);

            // Assert
            var ex = act.Should().Throw<BusinessRuleViolationException>();
            ex.WithMessage("Retired property can not be selected.");
        }
        #endregion

        #region Update

        [Fact]
        public void Update_Success()
        {
            // Arrange
            var managementFile = EntityHelper.CreateManagementFile();

            var repository = CreateRepositoryWithPermissions(Permissions.ManagementView);
            _helper.AddAndSaveChanges(managementFile);

            // Act
            var result = repository.Update(managementFile.Internal_Id, managementFile);

            // Assert

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsManagementFile>();
            result.ManagementFileId.Should().Be(1);
        }

        [Fact]
        public void Update_KeyNotFound()
        {
            // Arrange
            var managementFile = EntityHelper.CreateManagementFile();

            var repository = CreateRepositoryWithPermissions(Permissions.ManagementView);

            // Act
            Action act = () => repository.Update(managementFile.Internal_Id, managementFile);

            // Assert

            act.Should().Throw<KeyNotFoundException>();
        }
        #endregion

        #region GetById
        [Fact]
        public void GetById_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.ManagementView);
            var managementFile = EntityHelper.CreateManagementFile();
            _helper.AddAndSaveChanges(managementFile);

            // Act
            var result = repository.GetById(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsManagementFile>();
            result.FileName.Should().Be("Test Management File");
            result.ManagementFileId.Should().Be(managementFile.ManagementFileId);
        }

        [Fact]
        public void GetById_NotFound()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.ManagementView);

            // Act
            Action act = () => repository.GetById(1);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }
        #endregion

        #region GetTeamMembers
        [Fact]
        public void GetTeamMembers_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.ManagementView);
            var managementFile = EntityHelper.CreateManagementFile();
            managementFile.PimsManagementFileTeams = new List<PimsManagementFileTeam>() { new PimsManagementFileTeam() { ManagementFileProfileTypeCode = "PROPCOORD" } };
            _helper.AddAndSaveChanges(managementFile);

            // Act
            var userContext = UserContextModel.FromPimsUser(EntityHelper.CreateUser("Test", regionCode: 1));
            var result = repository.GetTeamMembers(userContext);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<List<PimsManagementFileTeam>>();
            result.Should().HaveCount(1);
        }
        #endregion

        #region GetRowVersion
        [Fact]
        public void GetRowVersion_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.ManagementView);
            var managementFile = EntityHelper.CreateManagementFile();
            _helper.AddAndSaveChanges(managementFile);

            // Act
            var result = repository.GetRowVersion(1);

            // Assert
            result.Should().Be(2);
        }

        [Fact]
        public void GetRowVersion_NotFound()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.ManagementView);

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
            var repository = CreateRepositoryWithPermissions(Permissions.ManagementView);
            var managementFile = EntityHelper.CreateManagementFile();
            managementFile.AppLastUpdateUserid = "test";
            managementFile.AppLastUpdateTimestamp = DateTime.Now;
            _helper.AddAndSaveChanges(managementFile);

            // Act
            var result = repository.GetLastUpdateBy(1);

            // Assert
            result.AppLastUpdateUserid.Should().Be("service");
            result.AppLastUpdateTimestamp.Should().BeSameDateAs(managementFile.AppLastUpdateTimestamp);
        }

        #endregion

        #region GetPageDeep
        [Fact]
        public void GetPageDeep_NoFilter_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.ManagementView);
            var managementFile = EntityHelper.CreateManagementFile();
            _helper.AddAndSaveChanges(managementFile);

            // Act
            var userContext = UserContextModel.FromPimsUser(EntityHelper.CreateUser("Test", regionCode: 1));
            var result = repository.GetPageDeep(new ManagementFilter(), userContext);

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetPageDeep_ManagementName_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.ManagementView);
            var managementFile = EntityHelper.CreateManagementFile();
            managementFile.FileName = "fileName";
            _helper.AddAndSaveChanges(managementFile);

            // Act
            var userContext = UserContextModel.FromPimsUser(EntityHelper.CreateUser("Test", regionCode: 1));
            var result = repository.GetPageDeep(new ManagementFilter() { FileNameOrNumberOrReference = "fileName" }, userContext);

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetPageDeep_ManagementName_NotFound()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.ManagementView);
            var managementFile = EntityHelper.CreateManagementFile();
            managementFile.FileName = "fileName";
            _helper.AddAndSaveChanges(managementFile);

            // Act
            var userContext = UserContextModel.FromPimsUser(EntityHelper.CreateUser("Test", regionCode: 1));
            var result = repository.GetPageDeep(new ManagementFilter() { FileNameOrNumberOrReference = "notFound" }, userContext);

            // Assert
            result.Should().HaveCount(0);
        }

        [Fact]
        public void GetPageDeep_ManagementNumber_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.ManagementView);
            var managementFile = EntityHelper.CreateManagementFile();
            managementFile.ManagementFileId = 100;
            _helper.AddAndSaveChanges(managementFile);

            // Act
            var userContext = UserContextModel.FromPimsUser(EntityHelper.CreateUser("Test", regionCode: 1));
            var result = repository.GetPageDeep(new ManagementFilter() { FileNameOrNumberOrReference = "M-100" }, userContext);

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetPageDeep_ManagementHistoricalNumber_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.ManagementView);
            var managementFile = EntityHelper.CreateManagementFile();
            managementFile.LegacyFileNum = "legacy";
            _helper.AddAndSaveChanges(managementFile);

            // Act
            var userContext = UserContextModel.FromPimsUser(EntityHelper.CreateUser("Test", regionCode: 1));
            var result = repository.GetPageDeep(new ManagementFilter() { FileNameOrNumberOrReference = "legacy" }, userContext);

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetPageDeep_ManagementTeamMemberPerson_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.ManagementView);
            var managementFile = EntityHelper.CreateManagementFile();
            var person = EntityHelper.CreatePerson(1, "tester", "chester");
            managementFile.PimsManagementFileTeams.Add(new PimsManagementFileTeam() { ManagementFileId = managementFile.Internal_Id, PersonId = person.Internal_Id, Person = person, ManagementFileProfileTypeCode = "COORD" });
            _helper.AddAndSaveChanges(managementFile);

            // Act
            var userContext = UserContextModel.FromPimsUser(EntityHelper.CreateUser("Test", regionCode: 1));
            var result = repository.GetPageDeep(new ManagementFilter() { TeamMemberPersonId = 1 }, userContext);

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetPageDeep_ManagementTeamMemberOrganization_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.ManagementView);
            var managementFile = EntityHelper.CreateManagementFile();
            var org = EntityHelper.CreateOrganization(1, "tester org");
            managementFile.PimsManagementFileTeams.Add(new PimsManagementFileTeam() { ManagementFileId = managementFile.Internal_Id, OrganizationId = org.Internal_Id, Organization = org, ManagementFileProfileTypeCode = "COORD" });
            _helper.AddAndSaveChanges(managementFile);

            // Act
            var userContext = UserContextModel.FromPimsUser(EntityHelper.CreateUser("Test", regionCode: 1));
            var result = repository.GetPageDeep(new ManagementFilter() { TeamMemberOrganizationId = 1 }, userContext);

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetPageDeep_ManagementTeamMember_NotFound()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.ManagementView);
            var managementFile = EntityHelper.CreateManagementFile();
            var person = EntityHelper.CreatePerson(1, "tester", "chester");
            managementFile.PimsManagementFileTeams.Add(new PimsManagementFileTeam() { ManagementFileId = managementFile.Internal_Id, PersonId = person.Internal_Id, Person = person, ManagementFileProfileTypeCode = "COORD" });
            _helper.AddAndSaveChanges(managementFile);

            // Act
            var userContext = UserContextModel.FromPimsUser(EntityHelper.CreateUser("Test", regionCode: 1));
            var result = repository.GetPageDeep(new ManagementFilter() { TeamMemberPersonId = 2 }, userContext);

            // Assert
            result.Should().HaveCount(0);
        }

        [Fact]
        public void GetPageDeep_Pid_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.ManagementView);
            var managementFile = EntityHelper.CreateManagementFile();
            managementFile.PimsManagementFileProperties = new List<PimsManagementFileProperty>() { new PimsManagementFileProperty() { Property = EntityHelper.CreateProperty(1, 2) } };
            _helper.AddAndSaveChanges(managementFile);

            // Act
            var userContext = UserContextModel.FromPimsUser(EntityHelper.CreateUser("Test", regionCode: 1));
            var result = repository.GetPageDeep(new ManagementFilter() { Pid = "1" }, userContext);

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetPageDeep_Pin_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.ManagementView);
            var managementFile = EntityHelper.CreateManagementFile();
            managementFile.PimsManagementFileProperties = new List<PimsManagementFileProperty>() { new PimsManagementFileProperty() { Property = EntityHelper.CreateProperty(1, 2) } };
            _helper.AddAndSaveChanges(managementFile);

            // Act
            var userContext = UserContextModel.FromPimsUser(EntityHelper.CreateUser("Test", regionCode: 1));
            var result = repository.GetPageDeep(new ManagementFilter() { Pin = "2" }, userContext);

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetPageDeep_Address_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.ManagementView);
            var managementFile = EntityHelper.CreateManagementFile();
            managementFile.PimsManagementFileProperties = new List<PimsManagementFileProperty>() { new PimsManagementFileProperty() { Property = EntityHelper.CreateProperty(1, 2) } };
            _helper.AddAndSaveChanges(managementFile);

            // Act
            var userContext = UserContextModel.FromPimsUser(EntityHelper.CreateUser("Test", regionCode: 1));
            var result = repository.GetPageDeep(new ManagementFilter() { Address = "1234" }, userContext);

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetPageDeep_FileStatus_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.ManagementView);
            var managementFile = EntityHelper.CreateManagementFile();
            _helper.AddAndSaveChanges(managementFile);

            // Act
            var userContext = UserContextModel.FromPimsUser(EntityHelper.CreateUser("Test", regionCode: 1));
            var result = repository.GetPageDeep(new ManagementFilter() { ManagementFileStatusCode = "ACTIVE" }, userContext);

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetPageDeep_Project_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.ManagementView);
            var managementFile = EntityHelper.CreateManagementFile();
            managementFile.Project = EntityHelper.CreateProject(1, "TEST", "");
            _helper.AddAndSaveChanges(managementFile);

            // Act
            var userContext = UserContextModel.FromPimsUser(EntityHelper.CreateUser("Test", regionCode: 1));
            var result = repository.GetPageDeep(new ManagementFilter() { ProjectNameOrNumber = "TEST" }, userContext);

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetPageDeep_Type_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.ManagementView);
            var managementFile = EntityHelper.CreateManagementFile();

            _helper.AddAndSaveChanges(managementFile);

            // Act
            var userContext = UserContextModel.FromPimsUser(EntityHelper.CreateUser("Test", regionCode: 1));
            var result = repository.GetPageDeep(new ManagementFilter() { ManagementFilePurposeCode = "Purpose" }, userContext);

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetPageDeep_Contractor_ManagementTeamMember_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.ManagementView);
            var contractorFile = EntityHelper.CreateManagementFile();
            contractorFile.RegionCodeNavigation = EntityHelper.CreateRegion(1, "Northern");
            contractorFile.RegionCode = 1;
            contractorFile.PimsManagementFileTeams.Add(
                new() { ManagementFileId = contractorFile.Internal_Id, ManagementFileProfileTypeCode = "COORD", PersonId = 1 } // contractor Id
            );
            _helper.AddAndSaveChanges(contractorFile);

            // Act
            var userContext = UserContextModel.FromPimsUser(EntityHelper.CreateUser("Test", regionCode: 1, isContractor: true));
            var result = repository.GetPageDeep(new ManagementFilter(), userContext);

            // Assert
            result.Should().HaveCount(1);
            result.First().ManagementFileId.Should().Be(contractorFile.Internal_Id);
        }

        [Fact]
        public void GetPageDeep_Contractor_ProjectTeamMember_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.ManagementView);
            var contractorFile = EntityHelper.CreateManagementFile();
            contractorFile.RegionCodeNavigation = EntityHelper.CreateRegion(1, "Northern");
            contractorFile.RegionCode = 1;
            contractorFile.Project = EntityHelper.CreateProject(1, "PRJ", "Mock Project");
            contractorFile.Project.PimsProjectPeople.Add(new() { ProjectPersonRoleTypeCode = "COORD", PersonId = 1 }); // contractor Id
            _helper.AddAndSaveChanges(contractorFile);

            // Act
            var userContext = UserContextModel.FromPimsUser(EntityHelper.CreateUser("Test", regionCode: 1, isContractor: true));
            var result = repository.GetPageDeep(new ManagementFilter(), userContext);

            // Assert
            result.Should().HaveCount(1);
            result.First().ManagementFileId.Should().Be(contractorFile.Internal_Id);
        }
        #endregion

        #endregion
    }
}
