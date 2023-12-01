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
        #region Constructors
        public DispositionFileRepositoryTest() { }
        #endregion

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
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DispositionView);

            var dispFile = EntityHelper.CreateDispositionFile();

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(dispFile);
            var repository = helper.CreateRepository<DispositionFileRepository>(user);

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
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DispositionView);
            helper.CreatePimsContext(user, true);

            var repository = helper.CreateRepository<DispositionFileRepository>(user);

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
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DispositionView);

            var dispFile = EntityHelper.CreateDispositionFile();

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(dispFile);
            var repository = helper.CreateRepository<DispositionFileRepository>(user);

            // Act
            var result = repository.GetRowVersion(1);

            // Assert
            result.Should().Be(2);
        }

        [Fact]
        public void GetRowVersion_NotFound()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DispositionView);
            helper.CreatePimsContext(user, true);

            var repository = helper.CreateRepository<DispositionFileRepository>(user);

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
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DispositionView);

            var dispFile = EntityHelper.CreateDispositionFile();
            dispFile.AppLastUpdateUserid = "test";
            dispFile.AppLastUpdateTimestamp = DateTime.Now;

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(dispFile);
            var repository = helper.CreateRepository<DispositionFileRepository>(user);

            // Act
            var result = repository.GetLastUpdateBy(1);

            // Assert
            result.AppLastUpdateUserid.Should().Be("service");
            result.AppLastUpdateTimestamp.Should().BeSameDateAs(dispFile.AppLastUpdateTimestamp);
        }

        #endregion

        #endregion
    }
}
