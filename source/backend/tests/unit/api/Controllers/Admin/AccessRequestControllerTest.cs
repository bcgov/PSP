using System.Linq;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Admin.Controllers;
using Pims.Api.Models;
using Pims.Core.Test;
using Pims.Dal;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Xunit;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Models.Concepts;

namespace Pims.Api.Test.Admin.Controllers
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("area", "admin")]
    [Trait("group", "accessRequest")]
    public class AccessRequestControllerTest
    {
        #region Constructors
        public AccessRequestControllerTest()
        {
        }
        #endregion

        #region Tests
        [Fact]
        public void GetAccessRequests_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<AccessRequestController>(Permissions.AdminUsers);

            var mapper = helper.GetService<IMapper>();
            var repository = helper.GetService<Mock<IAccessRequestRepository>>();
            var accessRequest1 = EntityHelper.CreateAccessRequest(1);
            var accessRequest2 = EntityHelper.CreateAccessRequest(2);
            var accessRequests = new[] { accessRequest1, accessRequest2 };
            var paged = new Entity.Models.Paged<Entity.PimsAccessRequest>(accessRequests);

            repository.Setup(m => m.GetAll(It.IsAny<AccessRequestFilter>())).Returns(paged);

            // Act
            var result = controller.GetPage(1, 10);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var actualResult = Assert.IsType<PageModel<Model.AccessRequestModel>>(actionResult.Value);
            repository.Verify(m => m.GetAll(It.IsAny<AccessRequestFilter>()), Times.Once());
        }

        [Fact]
        public void GetAccessRequests_PageMin_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<AccessRequestController>(Permissions.AdminUsers);

            var mapper = helper.GetService<IMapper>();
            var repository = helper.GetService<Mock<IAccessRequestRepository>>();
            var accessRequest1 = EntityHelper.CreateAccessRequest(1);
            var accessRequest2 = EntityHelper.CreateAccessRequest(2);
            var accessRequests = new[] { accessRequest1, accessRequest2 };
            var paged = new Entity.Models.Paged<Entity.PimsAccessRequest>(accessRequests);
            repository.Setup(m => m.GetAll(It.IsAny<AccessRequestFilter>())).Returns(paged);

            // Act
            var result = controller.GetPage(-1, -10);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var actualResult = Assert.IsType<PageModel<Model.AccessRequestModel>>(actionResult.Value);
            repository.Verify(m => m.GetAll(It.IsAny<AccessRequestFilter>()), Times.Once());
        }

        [Fact]
        public void GetAccessRequests_PageMax_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<AccessRequestController>(Permissions.AdminUsers);

            var mapper = helper.GetService<IMapper>();
            var repository = helper.GetService<Mock<IAccessRequestRepository>>();
            var accessRequest1 = EntityHelper.CreateAccessRequest(1);
            var accessRequest2 = EntityHelper.CreateAccessRequest(2);
            var accessRequests = new[] { accessRequest1, accessRequest2 };
            var paged = new Entity.Models.Paged<Entity.PimsAccessRequest>(accessRequests);
            repository.Setup(m => m.GetAll(It.IsAny<AccessRequestFilter>())).Returns(paged);

            // Act
            var result = controller.GetPage(2, 100);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var actualResult = Assert.IsType<PageModel<Pims.Api.Models.Concepts.AccessRequestModel>>(actionResult.Value);
            repository.Verify(m => m.GetAll(It.IsAny<AccessRequestFilter>()), Times.Once());
        }

        [Fact]
        public void GetAccessRequest_ById_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<AccessRequestController>(Permissions.AdminUsers);

            var mapper = helper.GetService<IMapper>();
            var repository = helper.GetService<Mock<IAccessRequestRepository>>();
            var accessRequest1 = EntityHelper.CreateAccessRequest(1);
            repository.Setup(m => m.GetById(It.IsAny<long>())).Returns(accessRequest1);

            // Act
            var result = controller.GetAccessRequest(accessRequest1.AccessRequestId);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var actualResult = Assert.IsType<Pims.Api.Models.Concepts.AccessRequestModel>(actionResult.Value);
            repository.Verify(m => m.GetById(It.IsAny<long>()), Times.Once());
        }

        [Fact]
        public void GetAccessRequests_Delete_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<AccessRequestController>(Permissions.AdminUsers);

            var mapper = helper.GetService<IMapper>();
            var repository = helper.GetService<Mock<IAccessRequestRepository>>();
            var accessRequest1 = EntityHelper.CreateAccessRequest(1);
            repository.Setup(m => m.Delete(It.IsAny<Entity.PimsAccessRequest>())).Returns(accessRequest1);

            // Act
            var result = controller.Delete(accessRequest1.AccessRequestId, mapper.Map<Model.AccessRequestModel>(accessRequest1));

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var actualResult = Assert.IsType<Model.AccessRequestModel>(actionResult.Value);
            mapper.Map<Model.AccessRequestModel>(accessRequest1).Should().BeEquivalentTo(actualResult);
            repository.Verify(m => m.Delete(It.IsAny<Entity.PimsAccessRequest>()), Times.Once());
        }
        #endregion
    }
}
