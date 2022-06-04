using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using Pims.Api.Controllers;
using Pims.Core.Http;
using Pims.Core.Http.Configuration;
using Pims.Core.Test;
using Pims.Dal.Repositories;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using KModel = Pims.Keycloak.Models;
using Model = Pims.Api.Models.Concepts;

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

            var optionsMonitor = new Mock<IOptionsMonitor<Pims.Keycloak.Configuration.KeycloakOptions>>();
            optionsMonitor.Setup(m => m.CurrentValue).Returns(GetKeycloakOptions());
            var userService = new Mock<IUserService>();

            var controller = helper.CreateController<UserController>(user, optionsMonitor.Object, userService.Object);

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
            model.Should().BeEquivalentTo(actualResult);
            service.Verify(m => m.ProxyGetAsync(It.IsAny<HttpRequest>(), It.IsAny<string>()), Times.Once());
        }

        [Fact(Skip = "skip")]
        public void UserBasicInfo_Success()
        {
            // Arrange
            var user = PrincipalHelper.CreateForRole();
            var helper = new TestHelper();


            var userEntity = new Pims.Dal.Entities.PimsUser();
            userEntity.Person = new Pims.Dal.Entities.PimsPerson()
            {
                FirstName = "John",
                Surname = "Carry"
            };

            var expectedResult = new Model.UserModel()
            {
                Person = new Model.PersonModel()
                {
                    FirstName = "John",
                    Surname = "Carry"
                }
            };

            var optionsMonitor = new Mock<IOptionsMonitor<Pims.Keycloak.Configuration.KeycloakOptions>>();
            optionsMonitor.Setup(m => m.CurrentValue).Returns(GetKeycloakOptions());
            var userService = new Mock<IUserService>();
            var mapper = new Mock<IMapper>();

            userService.Setup(x => x.GetUserInfo(It.IsAny<Guid>())).Returns(userEntity);

            var controller = helper.CreateController<UserController>(user, optionsMonitor.Object, userService.Object, mapper.Object);

            // Act
            var result = controller.UserBasicInfo(Guid.NewGuid());

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<Model.UserModel>(actionResult.Value);
            expectedResult.Should().BeEquivalentTo(actualResult);
            userService.Verify(m => m.GetUserInfo(It.IsAny<Guid>()), Times.Once());
        }

        [Fact]
        public void UserBasicInfo_BadRequest()
        {
            // Arrange
            var user = PrincipalHelper.CreateForRole();
            var helper = new TestHelper();

            var userEntity = new Pims.Dal.Entities.PimsUser();
            userEntity.Person = new Pims.Dal.Entities.PimsPerson()
            {
                FirstName = "John",
                Surname = "Carry"
            };

            var optionsMonitor = new Mock<IOptionsMonitor<Pims.Keycloak.Configuration.KeycloakOptions>>();
            optionsMonitor.Setup(m => m.CurrentValue).Returns(GetKeycloakOptions());
            var userService = new Mock<IUserService>();
            var mapper = new Mock<IMapper>();

            userService.Setup(x => x.GetUserInfo(It.IsAny<Guid>())).Returns(userEntity);

            var controller = helper.CreateController<UserController>(user, optionsMonitor.Object, userService.Object, mapper.Object);

            var expectedResult = new Pims.Api.Models.ErrorResponseModel("Invalid keycloakUserId", "keycloakUserId should be a valid non empty guid");

            // Act
            var result = controller.UserBasicInfo(Guid.Empty);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<Pims.Api.Models.ErrorResponseModel>(actionResult.Value);
            expectedResult.Should().BeEquivalentTo(actualResult);
            userService.Verify(m => m.GetUserInfo(It.IsAny<Guid>()), Times.Never());
        }
        #endregion

        #region Helpers
        private Pims.Keycloak.Configuration.KeycloakOptions GetKeycloakOptions()
        {
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

            return options;
        }
        #endregion

        #endregion
    }
}
