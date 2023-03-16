using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Admin.Controllers;
using Pims.Core.Test;
using Pims.Dal;
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
    [Trait("group", "role")]
    [ExcludeFromCodeCoverage]
    public class RoleControllerTest
    {
        #region Constructors
        public RoleControllerTest()
        {
        }
        #endregion

        #region Tests
        #region GetRoles
        [Fact]
        public void GetRoles_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<RoleController>(Permissions.AdminRoles);

            var mapper = helper.GetService<IMapper>();
            var repository = helper.GetService<Mock<IRoleRepository>>();
            var roles = new Entity.PimsRole[] { EntityHelper.CreateRole("role1"), EntityHelper.CreateRole("role2") };
            var paged = new Entity.Models.Paged<Entity.PimsRole>(roles);
            repository.Setup(m => m.GetPage(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(paged);

            // Act
            var result = controller.GetRoles(1, 10);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var actualResult = Assert.IsType<Pims.Api.Models.PageModel<Model.RoleModel>>(actionResult.Value);
            mapper.Map<Model.RoleModel[]>(roles).Should().BeEquivalentTo(actualResult.Items);
            repository.Verify(m => m.GetPage(1, 10, null), Times.Once());
        }

        [Fact]
        public void GetRoles_Success_MinPage()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<RoleController>(Permissions.AdminRoles);

            var mapper = helper.GetService<IMapper>();
            var repository = helper.GetService<Mock<IRoleRepository>>();
            var roles = new Entity.PimsRole[] { EntityHelper.CreateRole("role1"), EntityHelper.CreateRole("role2") };
            var paged = new Entity.Models.Paged<Entity.PimsRole>(roles);
            repository.Setup(m => m.GetPage(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(paged);

            // Act
            var result = controller.GetRoles(0, 10);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var actualResult = Assert.IsType<Pims.Api.Models.PageModel<Model.RoleModel>>(actionResult.Value);
            mapper.Map<Model.RoleModel[]>(roles).Should().BeEquivalentTo(actualResult.Items);
            repository.Verify(m => m.GetPage(1, 10, null), Times.Once());
        }

        [Fact]
        public void GetRoles_Success_MinQuantity()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<RoleController>(Permissions.AdminRoles);

            var mapper = helper.GetService<IMapper>();
            var repository = helper.GetService<Mock<IRoleRepository>>();
            var roles = new Entity.PimsRole[] { EntityHelper.CreateRole("role1"), EntityHelper.CreateRole("role2") };
            var paged = new Entity.Models.Paged<Entity.PimsRole>(roles);
            repository.Setup(m => m.GetPage(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(paged);

            // Act
            var result = controller.GetRoles(1, 0);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var actualResult = Assert.IsType<Pims.Api.Models.PageModel<Model.RoleModel>>(actionResult.Value);
            mapper.Map<Model.RoleModel[]>(roles).Should().BeEquivalentTo(actualResult.Items);
            repository.Verify(m => m.GetPage(1, 1, null), Times.Once());
        }

        [Fact]
        public void GetRoles_Success_MaxQuantity()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<RoleController>(Permissions.AdminRoles);

            var mapper = helper.GetService<IMapper>();
            var repository = helper.GetService<Mock<IRoleRepository>>();
            var roles = new Entity.PimsRole[] { EntityHelper.CreateRole("role1"), EntityHelper.CreateRole("role2") };
            var paged = new Entity.Models.Paged<Entity.PimsRole>(roles);
            repository.Setup(m => m.GetPage(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(paged);

            // Act
            var result = controller.GetRoles(1, 51);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var actualResult = Assert.IsType<Pims.Api.Models.PageModel<Model.RoleModel>>(actionResult.Value);
            mapper.Map<Model.RoleModel[]>(roles).Should().BeEquivalentTo(actualResult.Items);
            repository.Verify(m => m.GetPage(1, 50, null), Times.Once());
        }

        [Fact]
        public void GetRoles_Filtered_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<RoleController>(Permissions.AdminRoles);

            var mapper = helper.GetService<IMapper>();
            var repository = helper.GetService<Mock<IRoleRepository>>();
            var roles = new Entity.PimsRole[] { EntityHelper.CreateRole("role1"), EntityHelper.CreateRole("role2") };
            var paged = new Entity.Models.Paged<Entity.PimsRole>(roles);
            repository.Setup(m => m.GetPage(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(paged);

            // Act
            var result = controller.GetRoles(1, 10, "test");

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var actualResult = Assert.IsType<Pims.Api.Models.PageModel<Model.RoleModel>>(actionResult.Value);
            mapper.Map<Model.RoleModel[]>(roles).Should().BeEquivalentTo(actualResult.Items);
            repository.Verify(m => m.GetPage(1, 10, "test"), Times.Once());
        }
        #endregion

        #region GetRole
        [Fact]
        public void GetRole()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<RoleController>(Permissions.AdminRoles);

            var mapper = helper.GetService<IMapper>();
            var repository = helper.GetService<Mock<IRoleRepository>>();
            var role = EntityHelper.CreateRole("role1");
            repository.Setup(m => m.GetByKey(It.IsAny<Guid>())).Returns(role);

            // Act
            var result = controller.GetRole(role.RoleUid);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var actualResult = Assert.IsType<Model.RoleModel>(actionResult.Value);
            mapper.Map<Model.RoleModel>(role).Should().BeEquivalentTo(actualResult);
            repository.Verify(m => m.GetByKey(role.RoleUid), Times.Once());
        }
        #endregion

        #endregion
    }
}
