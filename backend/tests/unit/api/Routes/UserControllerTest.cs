using Pims.Api.Controllers;
using Pims.Core.Extensions;
using Pims.Core.Test;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Pims.Api.Test.Routes
{
    /// <summary>
    /// UserControllerTest class, provides a way to test endpoint routes.
    /// </summary>
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "user")]
    [Trait("group", "route")]
    [ExcludeFromCodeCoverage]
    public class UserControllerTest
    {
        #region Tests
        [Fact]
        public void User_Route()
        {
            // Arrange
            // Act
            // Assert
            var type = typeof(UserController);
            type.HasAuthorize();
            type.HasRoute("users");
            type.HasRoute("v{version:apiVersion}/users");
        }

        [Fact]
        public void UserInfo_Route()
        {
            // Arrange
            var endpoint = typeof(UserController).FindMethod(nameof(UserController.UserInfoAsync));

            // Act
            // Assert
            Assert.NotNull(endpoint);
            endpoint.HasGet("info");
        }
        #endregion
    }
}
