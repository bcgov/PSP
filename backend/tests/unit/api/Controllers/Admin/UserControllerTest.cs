using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Admin.Controllers;
using Pims.Core.Comparers;
using Pims.Core.Test;
using Pims.Dal;
using Pims.Dal.Security;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Admin.Models.User;

namespace PimsApi.Test.Admin.Controllers
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("area", "admin")]
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
        #region GetUsers
        [Fact]
        public void GetUsers_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<UserController>(Permissions.AdminUsers);

            var mapper = helper.GetService<IMapper>();
            var service = helper.GetService<Mock<IPimsRepository>>();
            var users = new Entity.PimsUser[] { EntityHelper.CreateUser("user1"), EntityHelper.CreateUser("user2") };
            var paged = new Entity.Models.Paged<Entity.PimsUser>(users);
            service.Setup(m => m.User.Get(It.IsAny<Entity.Models.UserFilter>())).Returns(paged);

            // Act
            var result = controller.GetUsers();

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var actualResult = Assert.IsType<Pims.Api.Models.PageModel<Model.UserModel>>(actionResult.Value);
            Assert.Equal(mapper.Map<Model.UserModel[]>(users), actualResult.Items, new DeepPropertyCompare());
            service.Verify(m => m.User.Get(It.IsAny<Entity.Models.UserFilter>()), Times.Once());
        }

        [Fact]
        public void GetUsers_Filtered_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<UserController>(Permissions.AdminUsers);

            var mapper = helper.GetService<IMapper>();
            var service = helper.GetService<Mock<IPimsRepository>>();
            var users = new Entity.PimsUser[] { EntityHelper.CreateUser("user1"), EntityHelper.CreateUser("user2") };
            var paged = new Entity.Models.Paged<Entity.PimsUser>(users);
            var filter = new Entity.Models.UserFilter(1, 1, "organization", "username", "lastname", "firstname", "email", false, "role", null, "position");
            service.Setup(m => m.User.Get(It.IsAny<Entity.Models.UserFilter>())).Returns(paged);

            // Act
            var result = controller.GetUsers(filter);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var actualResult = Assert.IsType<Pims.Api.Models.PageModel<Model.UserModel>>(actionResult.Value);
            Assert.Equal(mapper.Map<Model.UserModel[]>(users), actualResult.Items, new DeepPropertyCompare());
            service.Verify(m => m.User.Get(It.IsAny<Entity.Models.UserFilter>()), Times.Once());
        }
        #endregion

        #region GetMyUsers
        [Fact]
        public void GetMyUsers_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<UserController>(Permissions.AdminUsers);

            var mapper = helper.GetService<IMapper>();
            var service = helper.GetService<Mock<IPimsRepository>>();
            var users = new Entity.PimsUser[] { EntityHelper.CreateUser("user1"), EntityHelper.CreateUser("user2") };
            var paged = new Entity.Models.Paged<Entity.PimsUser>(users);
            var filter = new Entity.Models.UserFilter(1, 1, "organization", "username", "lastname", "firstname", "email", false, "role", null, "position");
            service.Setup(m => m.User.Get(It.IsAny<Entity.Models.UserFilter>())).Returns(paged);

            // Act
            var result = controller.GetMyUsers(filter);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var actualResult = Assert.IsType<Pims.Api.Models.PageModel<Model.UserModel>>(actionResult.Value);
            Assert.Equal(mapper.Map<Model.UserModel[]>(users), actualResult.Items, new DeepPropertyCompare());
            service.Verify(m => m.User.Get(It.IsAny<Entity.Models.UserFilter>()), Times.Once());
        }
        #endregion

        #region GetUser
        [Fact]
        public void GetUser()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<UserController>(Permissions.AdminUsers);

            var mapper = helper.GetService<IMapper>();
            var service = helper.GetService<Mock<IPimsRepository>>();
            var user = EntityHelper.CreateUser("user1");
            service.Setup(m => m.User.Get(It.IsAny<Guid>())).Returns(user);

            // Act
            var result = controller.GetUser(user.GuidIdentifierValue.Value);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var actualResult = Assert.IsType<Model.UserModel>(actionResult.Value);
            Assert.Equal(mapper.Map<Model.UserModel>(user), actualResult, new DeepPropertyCompare());
            service.Verify(m => m.User.Get(user.GuidIdentifierValue.Value), Times.Once());
        }
        #endregion

        #region AddUser
        [Fact]
        public void AddUser()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<UserController>(Permissions.AdminUsers);

            var mapper = helper.GetService<IMapper>();
            var service = helper.GetService<Mock<IPimsRepository>>();
            var user = EntityHelper.CreateUser("user1");
            var organization = user.GetOrganizations().First();
            service.Setup(m => m.User.Add(It.IsAny<Entity.PimsUser>())).Callback<Entity.PimsUser>(u => { });
            var model = mapper.Map<Model.UserModel>(user);
            model.Email = "test@test.com";

            // Act
            var result = controller.AddUser(model);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(201, actionResult.StatusCode);
            var actualResult = Assert.IsType<Model.UserModel>(actionResult.Value);
            // actualResult.Email.Should().Be(user.Email);
            actualResult.FirstName.Should().Be(user.Person.FirstName);
            actualResult.Surname.Should().Be(user.Person.Surname);
            actualResult.RowVersion.Should().Be(user.ConcurrencyControlNumber);
            actualResult.BusinessIdentifier.Should().Be(user.BusinessIdentifierValue);
            service.Verify(m => m.User.Add(It.IsAny<Entity.PimsUser>()), Times.Once());
        }
        #endregion

        #region UpdateUser
        [Fact]
        public void UpdateUser()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<UserController>(Permissions.AdminUsers);

            var mapper = helper.GetService<IMapper>();
            var service = helper.GetService<Mock<IPimsRepository>>();
            var user = EntityHelper.CreateUser("user1");
            service.Setup(m => m.User.Update(It.IsAny<Entity.PimsUser>()));
            var model = mapper.Map<Model.UserModel>(user);
            model.Email = "test@test.com";

            // Act
            var result = controller.UpdateUser(user.GuidIdentifierValue.Value, model);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var actualResult = Assert.IsType<Model.UserModel>(actionResult.Value);
            // actualResult.Email.Should().Be(user.Email);
            actualResult.FirstName.Should().Be(user.Person.FirstName);
            actualResult.Surname.Should().Be(user.Person.Surname);
            actualResult.RowVersion.Should().Be(user.ConcurrencyControlNumber);
            actualResult.BusinessIdentifier.Should().Be(user.BusinessIdentifierValue);
            service.Verify(m => m.User.Update(It.IsAny<Entity.PimsUser>()), Times.Once());
        }
        #endregion

        #region DeleteUser
        [Fact]
        public void DeleteUser()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<UserController>(Permissions.AdminUsers);

            var mapper = helper.GetService<IMapper>();
            var service = helper.GetService<Mock<IPimsRepository>>();
            var user = EntityHelper.CreateUser("user1");
            service.Setup(m => m.User.Delete(It.IsAny<Entity.PimsUser>()));
            var model = mapper.Map<Model.UserModel>(user);
            model.Email = "test@.test.com";

            // Act
            var result = controller.DeleteUser(user.GuidIdentifierValue.Value, model);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var actualResult = Assert.IsType<Model.UserModel>(actionResult.Value);
            Assert.Equal(model, actualResult, new DeepPropertyCompare());
            service.Verify(m => m.User.Delete(It.IsAny<Entity.PimsUser>()), Times.Once());
        }
        #endregion
        #endregion
    }
}
