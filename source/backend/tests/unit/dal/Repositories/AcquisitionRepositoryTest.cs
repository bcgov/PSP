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
            var result = repository.GetPage(filter, new HashSet<short>() { 1 });

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
            var result = repository.GetPage(filter, new HashSet<short>() { 1 });

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
            var result = repository.GetPage(filter, new HashSet<short>() { 1 });

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
            var result = repository.GetAcquisitionFileExport(filter, new HashSet<short>() { 1 });

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
            var result = repository.GetAcquisitionFileExport(filter, new HashSet<short>() { 1 });

            // Assert
            result.Should().HaveCount(1);
        }

        #endregion

        #endregion
    }
}
