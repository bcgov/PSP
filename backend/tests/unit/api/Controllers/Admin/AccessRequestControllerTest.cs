using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Admin.Controllers;
using Pims.Api.Models;
using Pims.Core.Comparers;
using Pims.Core.Test;
using Pims.Dal;
using Pims.Dal.Entities.Models;
using Pims.Dal.Security;
using Xunit;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Models.Concepts;
using FluentAssertions;
using System.Linq;

namespace PimsApi.Test.Admin.Controllers
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
            var service = helper.GetService<Mock<IPimsRepository>>();
            var accessRequest1 = EntityHelper.CreateAccessRequest(1);
            var accessRequest2 = EntityHelper.CreateAccessRequest(2);
            var accessRequests = new[] { accessRequest1, accessRequest2 };
            var paged = new Entity.Models.Paged<Entity.PimsAccessRequest>(accessRequests);

            service.Setup(m => m.AccessRequest.Get(It.IsAny<AccessRequestFilter>())).Returns(paged);

            // Act
            var result = controller.GetPage(1, 10);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var actualResult = Assert.IsType<PageModel<Model.AccessRequestModel>>(actionResult.Value);
            service.Verify(m => m.AccessRequest.Get(It.IsAny<AccessRequestFilter>()), Times.Once());
        }

        [Fact]
        public void GetAccessRequests_PageMin_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<AccessRequestController>(Permissions.AdminUsers);

            var mapper = helper.GetService<IMapper>();
            var service = helper.GetService<Mock<IPimsRepository>>();
            var accessRequest1 = EntityHelper.CreateAccessRequest(1);
            var accessRequest2 = EntityHelper.CreateAccessRequest(2);
            var accessRequests = new[] { accessRequest1, accessRequest2 };
            var paged = new Entity.Models.Paged<Entity.PimsAccessRequest>(accessRequests);
            service.Setup(m => m.AccessRequest.Get(It.IsAny<AccessRequestFilter>())).Returns(paged);

            // Act
            var result = controller.GetPage(-1, -10);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var actualResult = Assert.IsType<PageModel<Model.AccessRequestModel>>(actionResult.Value);
            service.Verify(m => m.AccessRequest.Get(It.IsAny<AccessRequestFilter>()), Times.Once());
        }

        [Fact]
        public void GetAccessRequests_PageMax_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<AccessRequestController>(Permissions.AdminUsers);

            var mapper = helper.GetService<IMapper>();
            var service = helper.GetService<Mock<IPimsRepository>>();
            var accessRequest1 = EntityHelper.CreateAccessRequest(1);
            var accessRequest2 = EntityHelper.CreateAccessRequest(2);
            var accessRequests = new[] { accessRequest1, accessRequest2 };
            var paged = new Entity.Models.Paged<Entity.PimsAccessRequest>(accessRequests);
            service.Setup(m => m.AccessRequest.Get(It.IsAny<AccessRequestFilter>())).Returns(paged);

            // Act
            var result = controller.GetPage(2, 100);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var actualResult = Assert.IsType<PageModel<Pims.Api.Models.Concepts.AccessRequestModel>>(actionResult.Value);
            service.Verify(m => m.AccessRequest.Get(It.IsAny<AccessRequestFilter>()), Times.Once());
        }
        #endregion
    }
}

