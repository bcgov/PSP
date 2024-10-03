using System.Diagnostics.CodeAnalysis;
using Pims.Api.Controllers;
using Pims.Core.Extensions;
using Pims.Core.Test;
using Xunit;

namespace Pims.Api.Test.Routes.Project
{
    /// <summary>
    /// TenantControllerTest class, provides a way to test endpoint routes.
    /// </summary>
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("area", "tenants")]
    [Trait("group", "tenants")]
    [Trait("group", "route")]
    [ExcludeFromCodeCoverage]
    public class TenantControllerTest
    {
        #region Tests
        [Fact]
        public void Tenants_Route()
        {
            // Arrange
            // Act
            // Assert
            var type = typeof(TenantController);
            type.HasRoute("tenants");
            type.HasRoute("v{version:apiVersion}/tenants");
            type.HasApiVersion("1.0");
        }

        [Fact]
        public void Settings_Route()
        {
            // Arrange
            var endpoint = typeof(TenantController).FindMethod(nameof(TenantController.Settings));

            // Act
            // Assert
            Assert.NotNull(endpoint);
            endpoint.HasGet();
        }
        #endregion
    }
}
