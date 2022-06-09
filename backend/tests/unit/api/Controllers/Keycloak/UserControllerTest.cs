using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Keycloak.Controllers;
using Pims.Core.Test;
using Pims.Dal.Keycloak;
using Pims.Dal.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;
using AdminModels = Pims.Api.Areas.Admin.Models.User;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Keycloak.Models.User;
using FluentAssertions;

namespace PimsApi.Test.Keycloak.Controllers
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("area", "keycloak")]
    [Trait("group", "user")]
    [ExcludeFromCodeCoverage]
    public class UserControllerTest
    {
        #region Variables
        #endregion

        #region Constructors
        public UserControllerTest()
        {
        }
        #endregion

        #region Tests
        #region SyncUserAsync
        [Fact]
        public async void SyncUserAsync_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<UserController>(Permissions.AdminUsers);

            var mapper = helper.GetService<IMapper>();
            var service = helper.GetService<Mock<IPimsKeycloakService>>();
            var user = EntityHelper.CreateUser("test");
            service.Setup(m => m.SyncUserAsync(It.IsAny<Guid>())).Returns(Task.FromResult(user));

            // Act
            var result = await controller.SyncUserAsync(user.GuidIdentifierValue.Value);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var data = Assert.IsType<Model.UserModel>(actionResult.Value);
            mapper.Map<Model.UserModel>(user).Should().BeEquivalentTo(data);
            service.Verify(m => m.SyncUserAsync(It.IsAny<Guid>()), Times.Once());
        }
        #endregion

        #region GetUsersAsync
        [Fact]
        public async void GetUsersAsync_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<UserController>(Permissions.AdminUsers);

            var mapper = helper.GetService<IMapper>();
            var service = helper.GetService<Mock<IPimsKeycloakService>>();
            var user = EntityHelper.CreateUser("test");
            var users = new[] { user };
            service.Setup(m => m.GetUsersAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(Task.FromResult((IEnumerable<Entity.PimsUser>)users));

            // Act
            var result = await controller.GetUsersAsync(1, 10);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var data = Assert.IsType<Model.UserModel[]>(actionResult.Value);
            mapper.Map<Model.UserModel[]>(users).Should().BeEquivalentTo(data);
            service.Verify(m => m.GetUsersAsync(1, 10, It.IsAny<string>()), Times.Once());
        }
        #endregion

        #region GetUserAsync
        [Fact]
        public async void GetUserAsync_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<UserController>(Permissions.AdminUsers);

            var mapper = helper.GetService<IMapper>();
            var service = helper.GetService<Mock<IPimsKeycloakService>>();
            var user = EntityHelper.CreateUser("test");
            service.Setup(m => m.GetUserAsync(It.IsAny<Guid>())).Returns(Task.FromResult(user));

            // Act
            var result = await controller.GetUserAsync(user.GuidIdentifierValue.Value);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var data = Assert.IsType<Model.UserModel>(actionResult.Value);
            mapper.Map<Model.UserModel>(user).Should().BeEquivalentTo(data);
            service.Verify(m => m.GetUserAsync(It.IsAny<Guid>()), Times.Once());
        }
        #endregion

        #region UpdateUserAsync
        [Fact]
        public async void UpdateUserAsync_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<UserController>(Permissions.AdminUsers);

            var mapper = helper.GetService<IMapper>();
            var service = helper.GetService<Mock<IPimsKeycloakService>>();
            var user = EntityHelper.CreateUser("test");
            service.Setup(m => m.UpdateUserAsync(It.IsAny<Entity.PimsUser>())).Returns(Task.FromResult(user));
            var model = mapper.Map<AdminModels.UserModel>(user);
            model.Email = "test@test.com";

            // Act
            var result = await controller.UpdateUserAsync(user.GuidIdentifierValue.Value, model);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var actualResult = Assert.IsType<AdminModels.UserModel>(actionResult.Value);
            var expectedResult = mapper.Map<AdminModels.UserModel>(user);
            Assert.Equal(expectedResult.Id, actualResult.Id);
            Assert.Equal(expectedResult.BusinessIdentifier, actualResult.BusinessIdentifier);
            Assert.Equal(expectedResult.FirstName, actualResult.FirstName);
            Assert.Equal(expectedResult.Surname, actualResult.Surname);
            Assert.Equal(expectedResult.Email, actualResult.Email);
            Assert.Equal(expectedResult.IsDisabled, actualResult.IsDisabled);
            expectedResult.Organizations.Should().BeEquivalentTo(actualResult.Organizations);
            expectedResult.Roles.Should().BeEquivalentTo(actualResult.Roles);
            service.Verify(m => m.UpdateUserAsync(It.IsAny<Entity.PimsUser>()), Times.Once());
        }
        #endregion
        #endregion
    }
}
