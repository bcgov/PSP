using Pims.Api.Controllers;
using Pims.Core.Extensions;
using Pims.Core.Test;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using Model = Pims.Api.Models.AccessRequest;

namespace Pims.Api.Test.Routes
{
    /// <summary>
    /// AccessRequestControllerTest class, provides a way to test endpoint routes.
    /// </summary>
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "user")]
    [Trait("group", "route")]
    [ExcludeFromCodeCoverage]
    public class AccessRequestControllerTest
    {
        #region Tests
        [Fact]
        public void GetAccessRequest_Current_Route()
        {
            // Arrange
            var endpoint = typeof(AccessRequestController).FindMethod(nameof(AccessRequestController.GetAccessRequest));

            // Act
            // Assert
            Assert.NotNull(endpoint);
            endpoint.HasGet("access/requests");
        }

        [Fact]
        public void GetAccessRequest_Route()
        {
            // Arrange
            var endpoint = typeof(AccessRequestController).FindMethod(nameof(AccessRequestController.GetAccessRequest), typeof(long));

            // Act
            // Assert
            Assert.NotNull(endpoint);
            endpoint.HasGet("access/requests/{id}");
        }

        [Fact]
        public void AddAccessRequest_Route()
        {
            // Arrange
            var endpoint = typeof(AccessRequestController).FindMethod(nameof(AccessRequestController.AddAccessRequest), typeof(Model.AccessRequestModel));

            // Act
            // Assert
            Assert.NotNull(endpoint);
            endpoint.HasPost("access/requests");
        }

        [Fact]
        public void UpdateAccessRequest_Route()
        {
            // Arrange
            var endpoint = typeof(AccessRequestController).FindMethod(nameof(AccessRequestController.UpdateAccessRequest), typeof(long), typeof(Model.AccessRequestModel));

            // Act
            // Assert
            Assert.NotNull(endpoint);
            endpoint.HasPut("access/requests/{id}");
        }
        #endregion
    }
}
