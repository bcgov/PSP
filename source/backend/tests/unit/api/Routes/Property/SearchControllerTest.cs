using System.Diagnostics.CodeAnalysis;
using Pims.Api.Areas.Property.Controllers;
using Pims.Api.Areas.Property.Models.Search;
using Pims.Core.Extensions;
using Pims.Core.Test;
using Pims.Dal.Security;
using Xunit;

namespace Pims.Api.Test.Routes
{
    /// <summary>
    /// SearchControllerTest class, provides a way to test endpoint routes.
    /// </summary>
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "property")]
    [Trait("group", "route")]
    [ExcludeFromCodeCoverage]
    public class SearchControllerTest
    {
        #region Variables
        #endregion

        #region Constructors
        public SearchControllerTest()
        {
        }
        #endregion

        #region Tests
        [Fact]
        public void Property_Route()
        {
            // Arrange
            // Act
            // Assert
            var type = typeof(SearchController);
            type.HasAuthorize();
            type.HasArea("properties");
            type.HasRoute("[area]/search");
            type.HasRoute("v{version:apiVersion}/[area]/search");
        }

        [Fact]
        public void GetProperties_Query_Route()
        {
            // Arrange
            var endpoint = typeof(SearchController).FindMethod(nameof(SearchController.GetProperties));

            // Act
            // Assert
            Assert.NotNull(endpoint);
            endpoint.HasGet();
            endpoint.HasPermissions(Permissions.PropertyView);
        }

        [Fact]
        public void GetProperties_Filter_Route()
        {
            // Arrange
            var endpoint = typeof(SearchController).FindMethod(nameof(SearchController.GetProperties), typeof(PropertyFilterModel));

            // Act
            // Assert
            Assert.NotNull(endpoint);
            endpoint.HasPost("filter");
            endpoint.HasPermissions(Permissions.PropertyView);
        }
        #endregion
    }
}
