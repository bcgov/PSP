using System.Diagnostics.CodeAnalysis;
using Pims.Api.Areas.Keycloak.Controllers;
using Pims.Api.Models.Concepts.AccessRequest;
using Pims.Core.Extensions;
using Pims.Core.Test;
using Pims.Core.Security;
using Xunit;

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
            var endpoint = typeof(AccessRequestController).FindMethod(nameof(AccessRequestController.UpdateAccessRequestAsync), typeof(AccessRequestModel));

            // Act
            // Assert
            Assert.NotNull(endpoint);
            endpoint.HasPut("access/requests");
            endpoint.HasPermissions(Permissions.AdminUsers);
        }
        #endregion
    }
}
