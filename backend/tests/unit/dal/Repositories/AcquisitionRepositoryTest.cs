using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Pims.Core.Test;
using Pims.Dal.Entities;
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

        #region Add Note
        [Fact]
        public void Add_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileAdd);

            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<AcquisitionFileRepository>(user);

            var acqFile = EntityHelper.CreateAcquisitionFile();

            // Act
            var result = repository.Add(acqFile);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsAcquisitionFile>();
            result.FileName.Should().Be("Test Acquisition File");
            result.AcquisitionFileId.Should().Be(1);
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

            // TODO: Update test when Update gets implemented
            Action act = () => repository.Update(acquisitionUpdated);

            // Assert
            act.Should().Throw<System.NotImplementedException>();
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

        #endregion
    }
}
