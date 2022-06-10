using Pims.Api.Areas.Admin.Controllers;
using Pims.Core.Extensions;
using Pims.Core.Test;
using Pims.Dal.Security;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using Model = Pims.Api.Models.Concepts;

namespace Pims.Api.Test.Routes.Admin
{
    /// <summary>
    /// RoleControllerTest class, provides a way to test endpoint routes.
    /// </summary>
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("area", "admin")]
    [Trait("group", "role")]
    [Trait("group", "route")]
    [ExcludeFromCodeCoverage]
    public class RoleControllerTest
    {
        #region Variables
        #endregion

        #region Constructors
        public RoleControllerTest()
        {
        }
        #endregion

        #region Tests
        [Fact]
        public void Role_Route()
        {
            // Arrange
            // Act
            // Assert
            var type = typeof(RoleController);
            type.HasPermissions(Permissions.AdminRoles);
            type.HasArea("admin");
            type.HasRoute("[area]/roles");
            type.HasRoute("v{version:apiVersion}/[area]/roles");
            type.HasApiVersion("1.0");
        }

        [Fact]
        public void GetRoles_Route()
        {
            // Arrange
            var endpoint = typeof(RoleController).FindMethod(nameof(RoleController.GetRoles), typeof(int), typeof(int), typeof(string));

            // Act
            // Assert
            Assert.NotNull(endpoint);
            endpoint.HasGet();
        }

        [Fact]
        public void GetRole_Route()
        {
            // Arrange
            var endpoint = typeof(RoleController).FindMethod(nameof(RoleController.GetRole), typeof(Guid));

            // Act
            // Assert
            Assert.NotNull(endpoint);
            endpoint.HasGet("{key}");
        }

        [Fact]
        public void GetRoleByName_Route()
        {
            // Arrange
            var endpoint = typeof(RoleController).FindMethod(nameof(RoleController.GetRoleByName), typeof(string));

            // Act
            // Assert
            Assert.NotNull(endpoint);
            endpoint.HasGet("name/{name}");
        }

        [Fact]
        public void AddRole_Route()
        {
            // Arrange
            var endpoint = typeof(RoleController).FindMethod(nameof(RoleController.AddRole), typeof(Model.RoleModel));

            // Act
            // Assert
            Assert.NotNull(endpoint);
            endpoint.HasPost();
        }

        [Fact]
        public void UpdateRole_Route()
        {
            // Arrange
            var endpoint = typeof(RoleController).FindMethod(nameof(RoleController.UpdateRole), typeof(Guid), typeof(Model.RoleModel));

            // Act
            // Assert
            Assert.NotNull(endpoint);
            endpoint.HasPut("{key}");
        }

        [Fact]
        public void DeleteRole_Route()
        {
            // Arrange
            var endpoint = typeof(RoleController).FindMethod(nameof(RoleController.DeleteRole), typeof(Guid), typeof(Model.RoleModel));

            // Act
            // Assert
            Assert.NotNull(endpoint);
            endpoint.HasDelete("{key}");
        }
        #endregion
    }
}
