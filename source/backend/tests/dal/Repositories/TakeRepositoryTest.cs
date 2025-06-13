using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Pims.Core.Test;
using Pims.Dal.Repositories;
using Pims.Core.Security;
using Xunit;

namespace Pims.Dal.Test.Repositories
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("group", "property")]
    [ExcludeFromCodeCoverage]
    public class TakeRepositoryTest
    {
        private TestHelper _helper;

        public TakeRepositoryTest()
        {
            _helper = new TestHelper();
        }

        private TakeRepository CreateRepositoryWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            _helper.CreatePimsContext(user, true);
            return _helper.CreateRepository<TakeRepository>();
        }

        #region Tests


        [Fact]
        public void GetById_Throw_KeyNotFoundException()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.AcquisitionFileView);

            // Act
            Action act = () => repository.GetById(9999);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }

        [Fact]
        public void GetById_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.AcquisitionFileView);
            _helper.AddAndSaveChanges(EntityHelper.CreateTake(9999));

            // Act
            var result = repository.GetById(9999);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void GetAllByAcquisitionFileId_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.AcquisitionFileView);
            var take = EntityHelper.CreateTake(9999);
            take.PropertyAcquisitionFile.AcquisitionFileId = 5;
            _helper.AddAndSaveChanges(take);

            // Act
            var result = repository.GetAllByAcquisitionFileId(5);

            // Assert
            result.Should().NotBeEmpty();
        }

        [Fact]
        public void GetAllByAcquisitionFilePropertyId_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.AcquisitionFileView);
            var take = EntityHelper.CreateTake(9999);
            take.PropertyAcquisitionFile.AcquisitionFileId = 5;
            take.PropertyAcquisitionFile.PropertyId = 10;
            _helper.AddAndSaveChanges(take);

            // Act
            var result = repository.GetAllByAcqPropertyId(5, 10);

            // Assert
            result.Should().NotBeEmpty();
        }

        [Fact]
        public void GetAllByPropertyAcquisitionFileId_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.AcquisitionFileView);
            var take = EntityHelper.CreateTake(9999);
            _helper.AddAndSaveChanges(take);

            // Act
            var result = repository.GetAllByPropertyAcquisitionFileId(1);

            // Assert
            result.Should().NotBeEmpty();
        }

        [Fact]
        public void GetAllByPropertyId_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.AcquisitionFileView);
            var take = EntityHelper.CreateTake(9999);
            take.PropertyAcquisitionFile.PropertyId = 10;
            _helper.AddAndSaveChanges(take);

            // Act
            var result = repository.GetAllByPropertyId(10);

            // Assert
            result.Should().NotBeEmpty();
        }

        [Fact]
        public void GetCountByPropertyId_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.AcquisitionFileView);
            var take = EntityHelper.CreateTake(9999);
            take.PropertyAcquisitionFile.PropertyId = 10;
            _helper.AddAndSaveChanges(take);

            // Act
            var result = repository.GetCountByPropertyId(10);

            // Assert
            result.Should().Be(1);
        }

        [Fact]
        public void AddTake_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.AcquisitionFileView);
            var take = EntityHelper.CreateTake(9999);

            // Act
            var result = repository.AddTake(take);

            // Assert
            result.TakeId.Should().Be(9999);
        }

        [Fact]
        public void UpdateTake_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.AcquisitionFileView);
            var take = EntityHelper.CreateTake(9999);
            take.PropertyAcquisitionFile.PropertyId = 10;
            _helper.AddAndSaveChanges(take);

            var newTake = EntityHelper.CreateTake(9999);
            take.IsAcquiredForInventory = true;

            // Act
            var result = repository.UpdateTake(take);

            // Assert
            result.IsAcquiredForInventory.Should().BeTrue();
        }

        [Fact]
        public void UpdateTake_Success_PropertyAcquisitionFileNotChanged()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.AcquisitionFileView);
            var take = EntityHelper.CreateTake(9999);
            take.PropertyAcquisitionFileId = 10;
            _helper.AddAndSaveChanges(take);

            var newTake = EntityHelper.CreateTake(9999);
            take.PropertyAcquisitionFileId = 50;

            // Act
            var result = repository.UpdateTake(newTake);

            // Assert
            result.PropertyAcquisitionFileId.Should().Be(50);
        }

        [Fact]
        public void UpdateTake_KeyNotFoundException()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.AcquisitionFileView);
            var take = EntityHelper.CreateTake(9999);
            _helper.AddAndSaveChanges(take);

            var newTake = EntityHelper.CreateTake(1);

            // Act
            Action act = () => repository.UpdateTake(newTake);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }

        [Fact]
        public void TryDeleteTake_Success()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.AcquisitionFileView);
            var take = EntityHelper.CreateTake(9999);
            _helper.AddAndSaveChanges(take);

            // Act
            var result = repository.TryDeleteTake(9999);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void TryDeleteTake_NoTake()
        {
            // Arrange
            var repository = CreateRepositoryWithPermissions(Permissions.AcquisitionFileView);
            var take = EntityHelper.CreateTake(9999);
            _helper.AddAndSaveChanges(take);

            // Act
            var result = repository.TryDeleteTake(1);

            // Assert
            result.Should().BeFalse();
        }

        #endregion
    }
}
