using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Pims.Core.Exceptions;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Repositories;
using Pims.Core.Security;
using Xunit;

namespace Pims.Dal.Test.Repositories
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("group", "acquisition")]
    [ExcludeFromCodeCoverage]
    public class AcquisitionRepositoryTest
    {
        #region Constructors
        public AcquisitionRepositoryTest() { }
        #endregion

        #region Tests

        #region GetPage
        [Fact]
        public void GetPage_AcquisitionName()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileAdd);
            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.FileName = "fileName";
            var filter = new AcquisitionFilter() { AcquisitionFileNameOrNumber = "fileName" };

            helper.CreatePimsContext(user, true).AddAndSaveChanges(acqFile);

            var repository = helper.CreateRepository<AcquisitionFileRepository>(user);

            // Act
            var result = repository.GetPageDeep(filter, new HashSet<short>() { 1 });

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetPage_AcquisitionNumber()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileAdd);
            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.FileNumber = "fileNumber";
            var filter = new AcquisitionFilter() { AcquisitionFileNameOrNumber = "fileNumber" };

            helper.CreatePimsContext(user, true).AddAndSaveChanges(acqFile);

            var repository = helper.CreateRepository<AcquisitionFileRepository>(user);

            // Act
            var result = repository.GetPageDeep(filter, new HashSet<short>() { 1 });

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetPage_AcquisitionHistoricalNumber()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileAdd);
            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.LegacyFileNumber = "legacy";
            var filter = new AcquisitionFilter() { AcquisitionFileNameOrNumber = "legacy" };

            helper.CreatePimsContext(user, true).AddAndSaveChanges(acqFile);

            var repository = helper.CreateRepository<AcquisitionFileRepository>(user);

            // Act
            var result = repository.GetPageDeep(filter, new HashSet<short>() { 1 });

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetPage_AlternateProject()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileAdd);
            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.PimsCompensationRequisitions = new List<PimsCompensationRequisition>() { new PimsCompensationRequisition() { AlternateProject = new PimsProject() { Code = "1", Description = "test", ProjectStatusTypeCode = "draft" } } };
            var filter = new AcquisitionFilter() { ProjectNameOrNumber = "test" };

            helper.CreatePimsContext(user, true).AddAndSaveChanges(acqFile);

            var repository = helper.CreateRepository<AcquisitionFileRepository>(user);

            // Act
            var result = repository.GetPageDeep(filter, new HashSet<short>() { 1 });

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetPage_Pid()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileAdd);
            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.PimsPropertyAcquisitionFiles = new List<PimsPropertyAcquisitionFile>() { new PimsPropertyAcquisitionFile() { Property = EntityHelper.CreateProperty(1, 2) } };
            var filter = new AcquisitionFilter() { Pid = "1" };

            helper.CreatePimsContext(user, true).AddAndSaveChanges(acqFile);

            var repository = helper.CreateRepository<AcquisitionFileRepository>(user);

            // Act
            var result = repository.GetPageDeep(filter, new HashSet<short>() { 1 });

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetPage_Pin()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileAdd);
            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.PimsPropertyAcquisitionFiles = new List<PimsPropertyAcquisitionFile>() { new PimsPropertyAcquisitionFile() { Property = EntityHelper.CreateProperty(1, 2) } };
            var filter = new AcquisitionFilter() { Pin = "2" };

            helper.CreatePimsContext(user, true).AddAndSaveChanges(acqFile);

            var repository = helper.CreateRepository<AcquisitionFileRepository>(user);

            // Act
            var result = repository.GetPageDeep(filter, new HashSet<short>() { 1 });

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetPage_Address()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileAdd);
            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.PimsPropertyAcquisitionFiles = new List<PimsPropertyAcquisitionFile>() { new PimsPropertyAcquisitionFile() { Property = EntityHelper.CreateProperty(1, 2) } };
            var filter = new AcquisitionFilter() { Address = "1234" };

            helper.CreatePimsContext(user, true).AddAndSaveChanges(acqFile);

            var repository = helper.CreateRepository<AcquisitionFileRepository>(user);

            // Act
            var result = repository.GetPageDeep(filter, new HashSet<short>() { 1 });

            // Assert
            result.Should().HaveCount(1);
        }
        #endregion

        #region Add
        [Fact]
        public void Add_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileAdd);
            var acqFile = EntityHelper.CreateAcquisitionFile();

            helper.CreatePimsContext(user, true);

            var mockSequenceRepo = new Mock<ISequenceRepository>();
            helper.AddSingleton(mockSequenceRepo.Object);

            var repository = helper.CreateRepository<AcquisitionFileRepository>(user);

            mockSequenceRepo.Setup(x => x.GetNextSequenceValue(It.IsAny<string>())).Returns(888999);

            // Act
            var result = repository.Add(acqFile);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsAcquisitionFile>();
            result.FileName.Should().Be("Test Acquisition File");
            result.AcquisitionFileId.Should().Be(1);
            result.FileNo.Should().Be(888999);
            result.FileNumber.Should().Be("01-888999-01");
        }

        [Fact]
        public void Add_SubFile_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileAdd);

            var acqMainFile = EntityHelper.CreateAcquisitionFile(1);
            acqMainFile.FileNo = 888999;

            var acqSubFile = EntityHelper.CreateAcquisitionFile(
                acqFileId: 2,
                statusType: acqMainFile.AcquisitionFileStatusTypeCodeNavigation,
                acquisitionType: acqMainFile.AcquisitionTypeCodeNavigation,
                region: acqMainFile.RegionCodeNavigation);
            acqSubFile.FileNo = 888999;
            acqSubFile.FileNumber = "01-888999-02";
            acqSubFile.PrntAcquisitionFileId = 1;
            acqSubFile.PrntAcquisitionFile = acqMainFile;

            var context = helper.CreatePimsContext(user, true);
            context.AddAndSaveChanges(acqMainFile, acqSubFile);

            var mockSequenceRepo = new Mock<ISequenceRepository>();
            mockSequenceRepo.Setup(x => x.GetNextSequenceValue(It.IsAny<string>())).Returns(888999);
            helper.AddSingleton(mockSequenceRepo.Object);

            var repository = helper.CreateRepository<AcquisitionFileRepository>(user);

            // Act
            var newSubFile = EntityHelper.CreateAcquisitionFile(
                acqFileId: 99,
                name: "Test sub-file",
                statusType: acqMainFile.AcquisitionFileStatusTypeCodeNavigation,
                acquisitionType: acqMainFile.AcquisitionTypeCodeNavigation,
                region: acqMainFile.RegionCodeNavigation);
            newSubFile.PrntAcquisitionFileId = 1;
            var result = repository.Add(newSubFile);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsAcquisitionFile>();
            result.FileName.Should().Be("Test sub-file");
            result.AcquisitionFileId.Should().Be(99);
            result.FileNo.Should().Be(888999);
            result.FileNumber.Should().Be("01-888999-03");
        }

        [Fact]
        public void Add_ThrowIfNull()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileAdd);
            helper.CreatePimsContext(user, true);

            var repository = helper.CreateRepository<AcquisitionFileRepository>(user);

            // Act
            Action act = () => repository.Add(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }


        [Fact]
        public void Add_WithRetiredProperty_Should_Fail()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileAdd);
            helper.CreatePimsContext(user, true);

            var repository = helper.CreateRepository<AcquisitionFileRepository>(user);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.PimsPropertyAcquisitionFiles = new List<PimsPropertyAcquisitionFile>()
            {
                new PimsPropertyAcquisitionFile()
                {
                    PropertyId = 100,
                    Property = new PimsProperty()
                    {
                        IsRetired = true,
                    }
                },
            };

            // Act
            Action act = () => repository.Add(acqFile);

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
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileView);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(acqFile);
            var repository = helper.CreateRepository<AcquisitionFileRepository>(user);

            // Act
            var result = repository.GetById(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsAcquisitionFile>();
            result.FileName.Should().Be("Test Acquisition File");
            result.AcquisitionFileId.Should().Be(acqFile.AcquisitionFileId);
        }

        [Fact]
        public void GetById_NotFound()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileView);
            helper.CreatePimsContext(user, true);

            var repository = helper.CreateRepository<AcquisitionFileRepository>(user);

            // Act
            Action act = () => repository.GetById(1);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }
        #endregion

        #region Update
        [Fact]
        public void Update_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileEdit);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(acqFile);

            var repository = helper.CreateRepository<AcquisitionFileRepository>(user);

            // Act
            var acquisitionUpdated = EntityHelper.CreateAcquisitionFile(acqFileId: 1, name: "updated");
            var result = repository.Update(acquisitionUpdated);

            // Assert
            result.Should().NotBeNull();
            result.FileName.Should().Be("updated");
        }

        [Fact]
        public void Update_Project_Propagate_Changes_To_SubFiles_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileEdit);

            var acqMainFile = EntityHelper.CreateAcquisitionFile(1);
            acqMainFile.ProjectId = 1;

            var subFileOne = EntityHelper.CreateAcquisitionFile(
                acqFileId: 2,
                name: "Sub-file 1",
                statusType: acqMainFile.AcquisitionFileStatusTypeCodeNavigation,
                acquisitionType: acqMainFile.AcquisitionTypeCodeNavigation,
                region: acqMainFile.RegionCodeNavigation);
            subFileOne.PrntAcquisitionFileId = 1;
            subFileOne.PrntAcquisitionFile = acqMainFile;
            subFileOne.ProjectId = 1;

            var subFileTwo = EntityHelper.CreateAcquisitionFile(
                acqFileId: 3,
                name: "Sub-file 2",
                statusType: acqMainFile.AcquisitionFileStatusTypeCodeNavigation,
                acquisitionType: acqMainFile.AcquisitionTypeCodeNavigation,
                region: acqMainFile.RegionCodeNavigation);
            subFileTwo.PrntAcquisitionFileId = 1;
            subFileTwo.PrntAcquisitionFile = acqMainFile;
            subFileTwo.ProjectId = 1;

            var project1 = EntityHelper.CreateProject(1, "0001", "One");
            var project2 = EntityHelper.CreateProject(2, "0002", "Two");
            project2.ProjectStatusTypeCodeNavigation = project1.ProjectStatusTypeCodeNavigation;

            var context = helper.CreatePimsContext(user, true);
            context.AddRange(project1, project2);
            context.AddAndSaveChanges(acqMainFile, subFileOne, subFileTwo);

            var repository = helper.CreateRepository<AcquisitionFileRepository>(user);

            // Act
            var acquisitionUpdated = EntityHelper.CreateAcquisitionFile(acqFileId: 1, name: "updated");
            acquisitionUpdated.ProjectId = 2;
            context.ChangeTracker.Clear();
            var result = repository.Update(acquisitionUpdated);
            context.SaveChanges();

            // Assert
            var subFiles = context.PimsAcquisitionFiles.AsNoTracking().Where(acq => acq.PrntAcquisitionFileId == 1);
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsAcquisitionFile>();
            result.FileName.Should().Be("updated");

            subFiles.Should().HaveCount(2);
            subFiles.Should().AllSatisfy(acq =>
                {
                    acq.ProjectId.Should().Be(2);
                });

        }

        [Fact]
        public void Update_Acquisition_OwnerRep_Add()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileEdit);

            var context = helper.CreatePimsContext(user, true);

            var person = EntityHelper.CreatePerson(1, "tester", "chester");
            context.AddAndSaveChanges(person);

            var acqFile = EntityHelper.CreateAcquisitionFile(region: context.PimsRegions.FirstOrDefault());
            context.AddAndSaveChanges(acqFile);

            var repository = helper.CreateRepository<AcquisitionFileRepository>(user);

            // Act
            var acquisitionUpdated = EntityHelper.CreateAcquisitionFile(acqFileId: 1, region: context.PimsRegions.FirstOrDefault());
            acquisitionUpdated.PimsInterestHolders.Add(
                new PimsInterestHolder() { AcquisitionFileId = acqFile.Internal_Id, PersonId = person.Internal_Id, Comment = "blah blah", InterestHolderTypeCode = "AOREP" });

            var result = repository.Update(acquisitionUpdated);

            // Assert
            result.Should().NotBeNull();
            result.PimsInterestHolders.Should().HaveCount(1);
            result.PimsInterestHolders.First().Person.Should().Be(person);
            result.PimsInterestHolders.First().Comment.Should().Be("blah blah");
        }

        [Fact]
        public void Update_Acquisition_OwnerRep_Update()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileEdit);

            var context = helper.CreatePimsContext(user, true);

            var person = EntityHelper.CreatePerson(1, "tester", "chester");
            var updatePerson = EntityHelper.CreatePerson(2, "tester", "two");
            updatePerson.PimsPersonAddresses = person.PimsPersonAddresses;
            context.AddAndSaveChanges(person, updatePerson);

            var acqFile = EntityHelper.CreateAcquisitionFile(region: context.PimsRegions.FirstOrDefault());
            acqFile.PimsInterestHolders.Add(
                new PimsInterestHolder() { AcquisitionFileId = acqFile.Internal_Id, PersonId = person.Internal_Id, Comment = "blah blah", InterestHolderTypeCode = "AOREP" });
            context.AddAndSaveChanges(acqFile);

            var repository = helper.CreateRepository<AcquisitionFileRepository>(user);

            // Act
            var acquisitionUpdated = EntityHelper.CreateAcquisitionFile(acqFileId: 1, region: context.PimsRegions.FirstOrDefault());
            acquisitionUpdated.PimsInterestHolders.Add(
                new PimsInterestHolder() { AcquisitionFileId = acqFile.Internal_Id, PersonId = updatePerson.Internal_Id, Comment = "updated comment", InterestHolderTypeCode = "AOREP" });

            var result = repository.Update(acquisitionUpdated);

            // Assert
            result.Should().NotBeNull();
            result.PimsInterestHolders.Should().HaveCount(1);
            result.PimsInterestHolders.First().Person.Should().Be(updatePerson);
            result.PimsInterestHolders.First().Comment.Should().Be("updated comment");
        }

        [Fact]
        public void Update_Acquisition_OwnerRep_Remove()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileEdit);

            var context = helper.CreatePimsContext(user, true);

            var person = EntityHelper.CreatePerson(1, "tester", "chester");
            context.AddAndSaveChanges(person);

            var acqFile = EntityHelper.CreateAcquisitionFile(region: context.PimsRegions.FirstOrDefault());
            acqFile.PimsInterestHolders.Add(
                new PimsInterestHolder() { AcquisitionFileId = acqFile.Internal_Id, PersonId = person.Internal_Id, Comment = "blah blah", InterestHolderTypeCode = "AOREP" });
            context.AddAndSaveChanges(acqFile);

            var repository = helper.CreateRepository<AcquisitionFileRepository>(user);

            // Act
            var acquisitionUpdated = EntityHelper.CreateAcquisitionFile(acqFileId: 1, region: context.PimsRegions.FirstOrDefault());
            var result = repository.Update(acquisitionUpdated);

            // Assert
            result.Should().NotBeNull();
            result.PimsInterestHolders.Should().BeEmpty();
        }

        [Fact]
        public void Update_Null()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileEdit);

            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<AcquisitionFileRepository>(user);

            // Act
            Action act = () => repository.Update(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Update_KeyNotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileEdit);

            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<AcquisitionFileRepository>(user);
            var acqFile = EntityHelper.CreateAcquisitionFile();

            // Act
            Action act = () => repository.Update(acqFile);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }
        #endregion

        #region Export

        [Fact]
        public void GetAcquisitionFileExport_Filter_AcquisitionName()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileAdd, Permissions.AcquisitionFileView);
            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.FileName = "fileName";
            var filter = new AcquisitionFilter() { AcquisitionFileNameOrNumber = "fileName" };

            helper.CreatePimsContext(user, true).AddAndSaveChanges(acqFile);

            var repository = helper.CreateRepository<AcquisitionFileRepository>(user);

            // Act
            var result = repository.GetAcquisitionFileExportDeep(filter, new HashSet<short>() { 1 });

            // Assert
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetAcquisitionFileExport_Filter_AcquisitionNumber()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileAdd);
            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.FileNumber = "fileNumber";
            var filter = new AcquisitionFilter() { AcquisitionFileNameOrNumber = "fileNumber" };

            helper.CreatePimsContext(user, true).AddAndSaveChanges(acqFile);

            var repository = helper.CreateRepository<AcquisitionFileRepository>(user);

            // Act
            var result = repository.GetAcquisitionFileExportDeep(filter, new HashSet<short>() { 1 });

            // Assert
            result.Should().HaveCount(1);
        }

        #endregion

        #region GetLastUpdateBy
        [Fact]
        public void GetLastUpdateBy_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileView);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.AppLastUpdateUserid = "test";
            acqFile.AppLastUpdateTimestamp = DateTime.Now;

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(acqFile);
            var repository = helper.CreateRepository<AcquisitionFileRepository>(user);

            // Act
            var result = repository.GetLastUpdateBy(1);

            // Assert
            result.AppLastUpdateUserid.Should().Be("service");
            result.AppLastUpdateTimestamp.Should().BeSameDateAs(acqFile.AppLastUpdateTimestamp);
        }

        #endregion

        #region GetOwnersByAcquisitionFileId
        [Fact]
        public void GetOwnersByAcquisitionFileId_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileView);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.PimsAcquisitionOwners = new List<PimsAcquisitionOwner>() { new PimsAcquisitionOwner() { } };

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(acqFile);
            var repository = helper.CreateRepository<AcquisitionFileRepository>(user);

            // Act
            var result = repository.GetOwnersByAcquisitionFileId(1);

            // Assert
            result.Should().HaveCount(1);
        }

        #endregion

        #region GetTeamMembers
        [Fact]
        public void GetTeamMembers_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileView);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.PimsAcquisitionFileTeams = new List<PimsAcquisitionFileTeam>() { new PimsAcquisitionFileTeam() { } };

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(acqFile);
            var repository = helper.CreateRepository<AcquisitionFileRepository>(user);

            // Act
            var result = repository.GetTeamMembers(new HashSet<short>() { 1 });

            // Assert
            result.Should().HaveCount(1);
        }

        #endregion

        #region GetRowVersion
        [Fact]
        public void GetRowVersion_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileView);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(acqFile);
            var repository = helper.CreateRepository<AcquisitionFileRepository>(user);

            // Act
            var result = repository.GetRowVersion(1);

            // Assert
            result.Should().Be(2);
        }

        #endregion

        #region GetRegion
        [Fact]
        public void GetRegion_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileView);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(acqFile);
            var repository = helper.CreateRepository<AcquisitionFileRepository>(user);

            // Act
            var result = repository.GetRegion(1);

            // Assert
            result.Should().Be(1);
        }

        #endregion

        #endregion
    }
}
