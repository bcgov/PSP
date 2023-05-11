using FluentAssertions;
using Moq;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Services;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Pims.Api.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("area", "compensation-requisition")]
    [ExcludeFromCodeCoverage]

    public class CompensationRequisitionServiceTest
    {
        private readonly TestHelper _helper;

        public CompensationRequisitionServiceTest()
        {
            _helper = new TestHelper();
        }

        [Fact]
        public void GetById_NoPermission()
        {
            // Arrange
            var service = CreateCompRequisitionServiceWithPermissions();

            // Act
            Action act = () => service.GetById(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void GetById_Success()
        {
            // Arrange
            var service = CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionView);
            var repo = _helper.GetService<Mock<ICompensationRequisitionRepository>>();
            repo.Setup(x => x.GetById(It.IsAny<long>()));

            // Act
            var result = service.GetById(1);

            // Assert
            repo.Verify(x => x.GetById(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void Update_NoPermission()
        {
            // Arrange
            var service = CreateCompRequisitionServiceWithPermissions();

            // Act
            Action act = () => service.Update(1, new PimsCompensationRequisition());

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void Update_BadRequest_IdMissmatch()
        {
            // Arrange
            var service = CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionEdit);

            // Act
            Action act = () => service.Update(1, new PimsCompensationRequisition() { Internal_Id = 2 });

            // Assert
            act.Should().Throw<BadRequestException>();
        }

        [Fact]
        public void Update_BadRequest_EntityIsNull()
        {
            // Arrange
            var service = CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionEdit);

            // Act
            Action act = () => service.Update(1, null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Update_Success()
        {
            // Arrange
            var service = CreateCompRequisitionServiceWithPermissions(Permissions.CompensationRequisitionEdit);
            var repository = _helper.GetService<Mock<ICompensationRequisitionRepository>>();
            repository.Setup(x => x.Update(It.IsAny<PimsCompensationRequisition>())).Returns(new PimsCompensationRequisition { Internal_Id = 1 });

            // Act
            var result = service.Update(1, new PimsCompensationRequisition { Internal_Id = 1, ConcurrencyControlNumber = 2 });

            // Assert
            result.Should().NotBeNull();
            repository.Verify(x => x.Update(It.IsAny<PimsCompensationRequisition>()), Times.Once);
        }


        private CompensationRequisitionService CreateCompRequisitionServiceWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            _helper.CreatePimsContext(user, true);
            return _helper.Create<CompensationRequisitionService>(user);
        }
    }
}
