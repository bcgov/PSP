using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
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
            var result = repository.AddAcquisition(acquisitionFileDocument);

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
            var result = repository.DeleteAcquisition(acquisitionFileDocument);

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
            Action act = () => repository.DeleteAcquisition(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
