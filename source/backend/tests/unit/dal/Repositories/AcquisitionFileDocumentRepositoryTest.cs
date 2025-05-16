using System;
using FluentAssertions;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;
using Pims.Core.Security;
using Xunit;

namespace Pims.Dal.Test.Repositories
{
    public class AcquisitionFileDocumentRepositoryTest
    {
        [Fact]
        public void AddAcquisition_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentView);

            var acquisitionFileDocument = new PimsAcquisitionFileDocument();

            var repository = helper.CreateRepository<AcquisitionFileDocumentRepository>(user);

            // Act
            var result = repository.AddDocument(acquisitionFileDocument);

            // Assert
            result.AcquisitionFileDocumentId.Should().Be(1);
        }

        [Fact]
        public void DeleteAcquisition_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentView);

            var acquisitionFileDocument = new PimsAcquisitionFileDocument();

            var repository = helper.CreateRepository<AcquisitionFileDocumentRepository>(user);

            // Act
            var result = repository.DeleteDocument(acquisitionFileDocument);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void DeleteAcquisition_Null()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentView);

            var repository = helper.CreateRepository<AcquisitionFileDocumentRepository>(user);

            // Act
            Action act = () => repository.DeleteDocument(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
