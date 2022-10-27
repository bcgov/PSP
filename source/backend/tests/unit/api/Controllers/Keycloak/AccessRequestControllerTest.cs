using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Keycloak.Controllers;
using Pims.Api.Models.Concepts;
using Pims.Core.Test;
using Pims.Dal.Keycloak;
using Pims.Dal.Security;
using Xunit;
using Entity = Pims.Dal.Entities;

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
            var model = mapper.Map<AccessRequestModel>(accessRequest);

            // Act
            var result = await controller.UpdateAccessRequestAsync(model);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var actualResult = Assert.IsType<AccessRequestModel>(actionResult.Value);
            var expectedResult = mapper.Map<AccessRequestModel>(accessRequest);
            expectedResult.Should().BeEquivalentTo(actualResult, options => options.Excluding(c => c.User));
            service.Verify(m => m.UpdateAccessRequestAsync(It.IsAny<Entity.PimsAccessRequest>()), Times.Once());
        }
        #endregion
    }
}
