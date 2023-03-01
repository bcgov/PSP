using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using MapsterMapper;
using Moq;
using Pims.Api.Constants;
using Pims.Api.Models.Concepts;
using Pims.Api.Services;
using Pims.Core.Test;
using Pims.Dal;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Xunit;

namespace Pims.Api.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "lease")]
    [ExcludeFromCodeCoverage]
    public class LeaseServiceTest
    {
        private TestHelper _helper;

        public LeaseServiceTest()
        {
            _helper = new TestHelper();
        }

        private LeaseService CreateLeaseService(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            _helper.CreatePimsContext(user, true);
            return _helper.Create<LeaseService>();
        }

        #region Tests
        #region Add
        [Fact]
        public void Update_WithoutStatusNote()
        {
            // Arrange
            var service = CreateLeaseService(Permissions.LeaseEdit);

            var currentLeaseEntity = new PimsLease()
            {
                LeaseId = 1,
                LeaseStatusTypeCode = "STATUS_A"
            };

            var leaseEntity = new PimsLease()
            {
                LeaseId = 1,
                LeaseStatusTypeCode = "STATUS_A"
            };

            var leaseRepository = _helper.GetService<Mock<ILeaseRepository>>();
            leaseRepository.Setup(x => x.Get(It.IsAny<long>())).Returns(currentLeaseEntity);

            var noteRepository = _helper.GetService<Mock<IEntityNoteRepository>>();

            // Act
            var result = service.Update(leaseEntity);

            // Assert
            noteRepository.Verify(x => x.Add(It.IsAny<PimsLeaseNote>()), Times.Never);
        }

        [Fact]
        public void Update_WithStatusNote()
        {
            // Arrange
            var service = CreateLeaseService(Permissions.LeaseEdit);

            var currentLeaseEntity = new PimsLease()
            {
                LeaseId = 1,
                LeaseStatusTypeCode = "STATUS_A"
            };

            var leaseEntity = new PimsLease()
            {
                LeaseId = 1,
                LeaseStatusTypeCode = "STATUS_B"
            };

            var leaseRepository = _helper.GetService<Mock<ILeaseRepository>>();
            leaseRepository.Setup(x => x.Get(It.IsAny<long>())).Returns(currentLeaseEntity);

            var noteRepository = _helper.GetService<Mock<IEntityNoteRepository>>();

            // Act
            var result = service.Update(leaseEntity);

            // Assert
            noteRepository.Verify(x => x.Add(It.IsAny<PimsLeaseNote>()), Times.Once);
        }

        [Fact]
        public void Update_Properties_Success()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);

            var service = CreateLeaseService(Permissions.LeaseEdit, Permissions.LeaseView);
            var leaseRepository = _helper.GetService<Mock<ILeaseRepository>>();
            var propertyLeaseRepository = _helper.GetService<Mock<IPropertyLeaseRepository>>();
            var propertyRepository = _helper.GetService<Mock<IPropertyRepository>>();
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>())).Returns(lease.PimsPropertyLeases.FirstOrDefault().Property);
            leaseRepository.Setup(x => x.Get(It.IsAny<long>())).Returns(lease);

            // Act

            var updatedLease = service.Update(lease);

            // Assert
            leaseRepository.Verify(x => x.Update(lease, false), Times.Once);
        }

        [Fact]
        public void Update_Properties_Delete_Success()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            var updatedLease = EntityHelper.CreateLease(2, addProperty: false);

            var service = CreateLeaseService(Permissions.LeaseEdit, Permissions.LeaseView);
            var leaseRepository = _helper.GetService<Mock<ILeaseRepository>>();
            var propertyLeaseRepository = _helper.GetService<Mock<IPropertyLeaseRepository>>();
            var propertyRepository = _helper.GetService<Mock<IPropertyRepository>>();
            propertyLeaseRepository.Setup(x => x.GetAllByLeaseId(It.IsAny<long>())).Returns(lease.PimsPropertyLeases);
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>())).Returns(lease.PimsPropertyLeases.FirstOrDefault().Property);
            leaseRepository.Setup(x => x.Get(It.IsAny<long>())).Returns(lease);

            // Act
            updatedLease = service.Update(updatedLease);

            // Assert
            propertyRepository.Verify(x => x.Delete(It.IsAny<PimsProperty>()), Times.Never());
        }

        [Fact]
        public void Update_Properties_Delete_POI_Success()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            var deletedProperty = lease.PimsPropertyLeases.FirstOrDefault().Property;
            deletedProperty.IsPropertyOfInterest = true;
            var updatedLease = EntityHelper.CreateLease(2, addProperty: false);

            var service = CreateLeaseService(Permissions.LeaseEdit, Permissions.LeaseView);
            var leaseRepository = _helper.GetService<Mock<ILeaseRepository>>();
            var propertyLeaseRepository = _helper.GetService<Mock<IPropertyLeaseRepository>>();
            var propertyRepository = _helper.GetService<Mock<IPropertyRepository>>();

            propertyLeaseRepository.Setup(x => x.GetAllByLeaseId(It.IsAny<long>())).Returns(lease.PimsPropertyLeases);
            propertyRepository.Setup(x => x.GetByPid(It.IsAny<int>())).Returns(deletedProperty);
            propertyRepository.Setup(x => x.GetAllAssociationsById(It.IsAny<long>())).Returns(lease.PimsPropertyLeases.FirstOrDefault().Property);
            leaseRepository.Setup(x => x.Get(It.IsAny<long>())).Returns(lease);

            // Act
            updatedLease = service.Update(updatedLease);

            // Assert
            propertyRepository.Verify(x => x.Delete(deletedProperty), Times.Once);
        }

        #endregion

        #endregion
    }
}
