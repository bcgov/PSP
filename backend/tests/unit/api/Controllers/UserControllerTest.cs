using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using Pims.Api.Controllers;
using Pims.Core.Comparers;
using Pims.Core.Http;
using Pims.Core.Http.Configuration;
using Pims.Core.Test;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using KModel = Pims.Keycloak.Models;

namespace PimsApi.Test.Controllers
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "user")]
    [ExcludeFromCodeCoverage]
    public class UserControllerTest
    {
        #region Constructors
        public UserControllerTest()
        {
        }
        #endregion

        #region Tests
        #region UserInfo
        [Fact]
        public async Task UserInfo_Success()
        {
            // Arrange
            var user = PrincipalHelper.CreateForRole();
            var helper = new TestHelper();
            var options = new Pims.Keycloak.Configuration.KeycloakOptions()
            {
                Authority = "test",
                Audience = "test",
                Client = "test",
                OpenIdConnect = new OpenIdConnectOptions()
                {
                    Token = "test",
                    UserInfo = "test"
                }
            };
            var optionsMonitor = new Mock<IOptionsMonitor<Pims.Keycloak.Configuration.KeycloakOptions>>();
            optionsMonitor.Setup(m => m.CurrentValue).Returns(options);
            var controller = helper.CreateController<UserController>(user, optionsMonitor.Object);

            var service = helper.GetService<Mock<IProxyRequestClient>>();
            var model = new KModel.UserInfoModel()
            {
                Id = Guid.NewGuid()
            };
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(model))
            };
            service.Setup(m => m.ProxyGetAsync(It.IsAny<HttpRequest>(), It.IsAny<string>())).Returns(Task.FromResult(response));

            // Act
            var result = await controller.UserInfoAsync();

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<KModel.UserInfoModel>(actionResult.Value);
            Assert.Equal(model, actualResult, new ShallowPropertyCompare());
            service.Verify(m => m.ProxyGetAsync(It.IsAny<HttpRequest>(), It.IsAny<string>()), Times.Once());
        }
        #endregion
        #endregion
    }
}
