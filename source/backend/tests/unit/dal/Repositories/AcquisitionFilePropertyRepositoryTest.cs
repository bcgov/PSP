using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
    public class AcquisitionFilePropertyRepositoryTest
    {
        #region Data
        #endregion

        #region Tests

        #region Get Acquisition Properties By File Id
        [Fact]
        public void GetByAcquisitionFileId_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileView);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            var pimsPropertyAcquisitionFile = new PimsPropertyAcquisitionFile() { AcquisitionFileId = acqFile.AcquisitionFileId, Property = EntityHelper.CreateProperty(1) };

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(pimsPropertyAcquisitionFile);
            var repository = helper.CreateRepository<AcquisitionFilePropertyRepository>(user);

            // Act
            var result = repository.GetPropertiesByAcquisitionFileId(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<List<PimsPropertyAcquisitionFile>>();
            result.Should().HaveCount(1);
            result.FirstOrDefault().PropertyAcquisitionFileId.Should().Be(pimsPropertyAcquisitionFile.PropertyAcquisitionFileId);
        }

        [Fact]
        public void GetByAcquisitionFileId_NotFound()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteView);
            helper.CreatePimsContext(user, true);

            var repository = helper.CreateRepository<AcquisitionFilePropertyRepository>(user);

            // Act
            var result = repository.GetPropertiesByAcquisitionFileId(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<List<PimsPropertyAcquisitionFile>>();
            result.Should().HaveCount(0);
        }
        #endregion

        #region Get Acquisition Properties By File Id
        [Fact]
        public void GetAcquisitionFilePropertyRelatedCount_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileView);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            var pimsPropertyAcquisitionFile = new PimsPropertyAcquisitionFile() { AcquisitionFileId = acqFile.AcquisitionFileId, Property = new PimsProperty() { RegionCode = 1, PropertyId = 1 } };
            var pimsPropertyAcquisitionFile2 = new PimsPropertyAcquisitionFile() { AcquisitionFileId = acqFile.AcquisitionFileId, Property = new PimsProperty() { RegionCode = 1, PropertyId = 2 } };

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(pimsPropertyAcquisitionFile);
            var repository = helper.CreateRepository<AcquisitionFilePropertyRepository>(user);

            // Act
            var result = repository.GetAcquisitionFilePropertyRelatedCount(1);

            // Assert
            result.Should().Be(1);
        }

        [Fact]
        public void GetAcquisitionFilePropertyRelatedCount_NotFound()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteView);
            helper.CreatePimsContext(user, true);

            var repository = helper.CreateRepository<AcquisitionFilePropertyRepository>(user);

            // Act
            var result = repository.GetAcquisitionFilePropertyRelatedCount(1);

            // Assert
            result.Should().Be(0);
        }
        #endregion

        #region Add
        [Fact]
        public void Add_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileAdd);

            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<AcquisitionFilePropertyRepository>(user);

            var pimsPropertyAcquisitionFile = new PimsPropertyAcquisitionFile() { AcquisitionFileId = 1, PropertyId = 1, Property = new PimsProperty() { RegionCode = 1, PropertyId = 1 } };

            // Act
            var result = repository.Add(pimsPropertyAcquisitionFile);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsPropertyAcquisitionFile>();
        }

        [Fact]
        public void Add_ThrowIfNull()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileAdd);
            helper.CreatePimsContext(user, true);

            var repository = helper.CreateRepository<AcquisitionFilePropertyRepository>(user);

            // Act
            Action act = () => repository.Add(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
        #endregion

        #region Add
        [Fact]
        public void Update_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileAdd);

            var pimsPropertyAcquisitionFile = new PimsPropertyAcquisitionFile() { AcquisitionFileId = 1, Property = new PimsProperty() { RegionCode = 1, PropertyId = 1 } };

            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<AcquisitionFilePropertyRepository>(user);
            context.AddAndSaveChanges(pimsPropertyAcquisitionFile);

            // Act
            var result = repository.Update(new PimsPropertyAcquisitionFile() { AcquisitionFileId = 1, PropertyName = "updated", PropertyId = 1, Property = new PimsProperty() { RegionCode = 1, PropertyId = 2 } });

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsPropertyAcquisitionFile>();
            result.PropertyName.Should().Be("updated");
        }

        [Fact]
        public void Update_ThrowIfNull()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileAdd);
            helper.CreatePimsContext(user, true);

            var repository = helper.CreateRepository<AcquisitionFilePropertyRepository>(user);

            // Act
            Action act = () => repository.Update(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
        #endregion

        #region Delete
        [Fact]
        public void Delete_PropertyAcquisitionFile()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileDelete);
            var pimsPropertyAcquisitionFile = new PimsPropertyAcquisitionFile() { Property = new PimsProperty() { RegionCode = 1 } };

            var context = helper.CreatePimsContext(user, true);
            context.AddAndSaveChanges(pimsPropertyAcquisitionFile);

            var repository = helper.CreateRepository<AcquisitionFilePropertyRepository>(user);

            // Act
            repository.Delete(pimsPropertyAcquisitionFile);

            // Assert
            var result = repository.GetPropertiesByAcquisitionFileId(pimsPropertyAcquisitionFile.PropertyAcquisitionFileId);
            result.Should().BeEmpty();
        }

        [Fact]
        public void Delete_ThrowIfNull()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AcquisitionFileDelete);

            var context = helper.CreatePimsContext(user, true);

            var repository = helper.CreateRepository<AcquisitionFilePropertyRepository>(user);

            // Act
            Action act = () => repository.Delete(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        #endregion

        #endregion
    }
}
