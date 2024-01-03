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
    [Trait("group", "disposition")]
    [ExcludeFromCodeCoverage]
    public class DispositionFilePropertyRepositoryTest
    {
        #region Data
        #endregion

        #region Tests

        #region Get Disposition Properties By File Id
        [Fact]
        public void GetByDispositionFileId_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DispositionView);

            var acqFile = EntityHelper.CreateDispositionFile();
            var pimsPropertyDispositionFile = new PimsDispositionFileProperty() { DispositionFileId = acqFile.DispositionFileId, Property = EntityHelper.CreateProperty(1) };

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(pimsPropertyDispositionFile);
            var repository = helper.CreateRepository<DispositionFilePropertyRepository>(user);

            // Act
            var result = repository.GetPropertiesByDispositionFileId(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<List<PimsDispositionFileProperty>>();
            result.Should().HaveCount(1);
            result.FirstOrDefault().DispositionFilePropertyId.Should().Be(pimsPropertyDispositionFile.DispositionFilePropertyId);
        }

        [Fact]
        public void GetByDispositionFileId_NotFound()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteView);
            helper.CreatePimsContext(user, true);

            var repository = helper.CreateRepository<DispositionFilePropertyRepository>(user);

            // Act
            var result = repository.GetPropertiesByDispositionFileId(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<List<PimsDispositionFileProperty>>();
            result.Should().HaveCount(0);
        }
        #endregion

        #region Get Disposition Properties Count By File Id
        [Fact]
        public void GetDispositionFilePropertyRelatedCount_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DispositionView);

            var acqFile = EntityHelper.CreateDispositionFile();
            var pimsPropertyDispositionFile = new PimsDispositionFileProperty() { DispositionFileId = acqFile.DispositionFileId, Property = EntityHelper.CreateProperty(1) };
            var pimsPropertyDispositionFile2 = new PimsDispositionFileProperty() { DispositionFileId = acqFile.DispositionFileId, Property = EntityHelper.CreateProperty(2) };

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(pimsPropertyDispositionFile);
            var repository = helper.CreateRepository<DispositionFilePropertyRepository>(user);

            // Act
            var result = repository.GetDispositionFilePropertyRelatedCount(1);

            // Assert
            result.Should().Be(1);
        }

        [Fact]
        public void GetDispositionFilePropertyRelatedCount_NotFound()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteView);
            helper.CreatePimsContext(user, true);

            var repository = helper.CreateRepository<DispositionFilePropertyRepository>(user);

            // Act
            var result = repository.GetDispositionFilePropertyRelatedCount(1);

            // Assert
            result.Should().Be(0);
        }
        #endregion

        #endregion
    }
}
