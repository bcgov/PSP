using System;
using System.Diagnostics.CodeAnalysis;
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

        #endregion

        #endregion
    }
}
