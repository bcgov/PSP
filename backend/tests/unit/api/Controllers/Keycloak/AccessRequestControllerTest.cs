using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Keycloak.Controllers;
using Pims.Core.Comparers;
using Pims.Core.Test;
using Pims.Dal.Keycloak;
using Pims.Dal.Security;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Keycloak.Models.AccessRequest;

namespace PimsApi.Test.Keycloak.Controllers
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("area", "keycloak")]
    [Trait("group", "user")]
    [ExcludeFromCodeCoverage]
    public class AccessRequestControllerTest
    {
        #region Tests
        [Fact]
        public async void UpdateAccessRequestAsync_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<AccessRequestController>(Permissions.AdminUsers);

            var mapper = helper.GetService<IMapper>();
            var service = helper.GetService<Mock<IPimsKeycloakService>>();
            var accessRequest = EntityHelper.CreateAccessRequest(1);
            service.Setup(m => m.UpdateAccessRequestAsync(It.IsAny<Entity.PimsAccessRequest>())).Returns(Task.FromResult(accessRequest));
            var model = mapper.Map<Model.AccessRequestModel>(accessRequest);

            // Act
            var result = await controller.UpdateAccessRequestAsync(model);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var actualResult = Assert.IsType<Model.AccessRequestModel>(actionResult.Value);
            var expectedResult = mapper.Map<Model.AccessRequestModel>(accessRequest);
            Assert.Equal(expectedResult, actualResult, new DeepPropertyCompare());
            service.Verify(m => m.UpdateAccessRequestAsync(It.IsAny<Entity.PimsAccessRequest>()), Times.Once());
        }
        #endregion
    }
}
