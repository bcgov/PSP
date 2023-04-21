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
    public class AcquisitionFileFormRepositoryTest
    {
        #region Data
        #endregion

        #region Tests

        #region Get Acquisition Properties By File Id
        [Fact]
        public void GetAllByAcquisitionFileId_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.FormView);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            var pimsAcquisitionFileForm = new PimsAcquisitionFileForm() { AcquisitionFileId = acqFile.AcquisitionFileId, FormTypeCodeNavigation = new PimsFormType() { Id = "H120" } };

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(pimsAcquisitionFileForm);
            var repository = helper.CreateRepository<AcquisitionFileFormRepository>(user);

            // Act
            var result = repository.GetAllByAcquisitionFileId(acqFile.AcquisitionFileId);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.FirstOrDefault().FormTypeCodeNavigation.Id.Should().Be(pimsAcquisitionFileForm.FormTypeCodeNavigation.Id);
        }

        [Fact]
        public void GetAllByAcquisitionFileId_NotFound()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.FormView);
            helper.CreatePimsContext(user, true);

            var repository = helper.CreateRepository<AcquisitionFileFormRepository>(user);

            // Act
            var result = repository.GetAllByAcquisitionFileId(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(0);
        }
        #endregion

        #region Get By File Id
        [Fact]
        public void GetByAcquisitionFileFormId_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.FormView);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            var pimsAcquisitionFileForm = new PimsAcquisitionFileForm() { AcquisitionFileId = acqFile.AcquisitionFileId, FormTypeCodeNavigation = new PimsFormType() { Id = "H120" }, AcquisitionFileFormId = 1 };

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(pimsAcquisitionFileForm);
            var repository = helper.CreateRepository<AcquisitionFileFormRepository>(user);

            // Act
            var result = repository.GetById(1);

            // Assert
            result.Should().NotBeNull();
            result.FormTypeCodeNavigation.Id.Should().Be(pimsAcquisitionFileForm.FormTypeCodeNavigation.Id);
        }

        [Fact]
        public void GetByAcquisitionFileFormId_NotFound()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.FormView);
            helper.CreatePimsContext(user, true);

            var repository = helper.CreateRepository<AcquisitionFileFormRepository>(user);

            // Act
            Action act = () => repository.GetById(1);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }
        #endregion

        #region Get Acquisition form By File Id
        [Fact]
        public void GetByAcquisitionFileId_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.FormView);

            var acqFile = EntityHelper.CreateAcquisitionFile();
            var pimsAcquisitionFileForm = new PimsAcquisitionFileForm() { AcquisitionFileId = acqFile.AcquisitionFileId, FormTypeCodeNavigation = new PimsFormType() { Id = "H120" } };

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(pimsAcquisitionFileForm);
            var repository = helper.CreateRepository<AcquisitionFileFormRepository>(user);

            // Act
            var result = repository.GetByAcquisitionFileFormId(pimsAcquisitionFileForm.Internal_Id);

            // Assert
            result.Should().NotBeNull();
            result.FormTypeCodeNavigation.Id.Should().Be(pimsAcquisitionFileForm.FormTypeCodeNavigation.Id);
        }

        [Fact]
        public void GetByAcquisitionFileId_NotFound()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.FormView);
            helper.CreatePimsContext(user, true);

            var repository = helper.CreateRepository<AcquisitionFileFormRepository>(user);

            // Act
            Action act = () => repository.GetByAcquisitionFileFormId(1);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }
        #endregion

        #region Add
        [Fact]
        public void Add_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.FormAdd);

            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<AcquisitionFileFormRepository>(user);

            var pimsAcquisitionFileForm = new PimsAcquisitionFileForm() { AcquisitionFileId = 1, FormTypeCodeNavigation = new PimsFormType() { Id = "H120" } };

            // Act
            var result = repository.Add(pimsAcquisitionFileForm);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void Add_ThrowIfNull()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.FormAdd);
            helper.CreatePimsContext(user, true);

            var repository = helper.CreateRepository<AcquisitionFileFormRepository>(user);

            // Act
            Action act = () => repository.Add(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
        #endregion

        #region Delete
        [Fact]
        public void Delete_AcquisitionFileForm_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.FormDelete);
            var pimsAcquisitionFileForm = new PimsAcquisitionFileForm() { AcquisitionFileId = 1, FormTypeCodeNavigation = new PimsFormType() { Id = "H120" } };

            var context = helper.CreatePimsContext(user, true);
            context.AddAndSaveChanges(pimsAcquisitionFileForm);

            var repository = helper.CreateRepository<AcquisitionFileFormRepository>(user);

            // Act
            repository.TryDelete(1);
            repository.CommitTransaction();

            // Assert
            var result = repository.GetAllByAcquisitionFileId(1);
            result.Should().BeEmpty();
        }

        [Fact]
        public void Delete_AcquisitionFileForm_InvalidId()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.FormDelete);
            var pimsAcquisitionFileForm = new PimsAcquisitionFileForm() { AcquisitionFileId = 1, FormTypeCodeNavigation = new PimsFormType() { Id = "H120" } };

            var context = helper.CreatePimsContext(user, true);
            context.AddAndSaveChanges(pimsAcquisitionFileForm);

            var repository = helper.CreateRepository<AcquisitionFileFormRepository>(user);

            // Act
            var result = repository.TryDelete(2);
            repository.CommitTransaction();

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        #endregion
    }
}
