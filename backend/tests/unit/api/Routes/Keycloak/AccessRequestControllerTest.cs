using System.Diagnostics.CodeAnalysis;
using Pims.Api.Areas.Keycloak.Controllers;
using Pims.Core.Extensions;
using Pims.Core.Test;
using Pims.Dal.Security;
using Xunit;
using Model = Pims.Api.Models.Concepts;

namespace Pims.Api.Test.Routes.Keycloak
{
    /// <summary>
    /// AccessRequestControllerTest class, provides a way to test endpoint routes.
    /// </summary>
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("area", "keycloak")]
    [Trait("group", "user")]
    [Trait("group", "route")]
    [ExcludeFromCodeCoverage]
    public class AccessRequestControllerTest
    {
        #region Tests
        [Fact]
        public void UpdateAccessRequestAsync_Route()
        {
            // Arrange
            var endpoint = typeof(AccessRequestController).FindMethod(nameof(AccessRequestController.UpdateAccessRequestAsync), typeof(Model.AccessRequestModel));

            // Act
            // Assert
            Assert.NotNull(endpoint);
            endpoint.HasPut("access/requests");
            endpoint.HasPermissions(Permissions.AdminUsers);
        }
        #endregion
    }
}
