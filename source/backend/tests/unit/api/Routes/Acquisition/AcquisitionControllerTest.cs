using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Pims.Api.Areas.Acquisition.Controllers;
using Pims.Api.Constants;
using Pims.Api.Models.Concepts;
using Pims.Core.Extensions;
using Pims.Core.Test;
using Pims.Dal.Security;
using Xunit;

namespace Pims.Api.Test.Routes
{
    /// <summary>
    /// AcquisitionControllerTest class, provides a way to test endpoint routes.
    /// </summary>
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "acquisition")]
    [Trait("group", "route")]
    [ExcludeFromCodeCoverage]
    public class AcquisitionControllerTest
    {

        #region Constructors
        public AcquisitionControllerTest()
        {
        }
        #endregion

        #region Tests
        [Fact]
        public void Controller_Route()
        {
            // Arrange
            // Act
            // Assert
            var type = typeof(AcquisitionFileController);
            type.HasAuthorize();
            type.HasArea("acquisitionfiles");
            type.HasRoute("[area]");
            type.HasRoute("v{version:apiVersion}/[area]");
        }

        [Fact]
        public void AddAcquisitionFile_Route()
        {
            // Arrange
            var endpoint = typeof(AcquisitionFileController).FindMethod(nameof(AcquisitionFileController.AddAcquisitionFile), typeof(AcquisitionFileModel), typeof(string[]));

            // Act
            // Assert
            Assert.NotNull(endpoint);
            endpoint.HasPost();
            endpoint.HasPermissions(Permissions.AcquisitionFileAdd);
        }

        [Fact]
        public void GetAcquisitionFile_Route()
        {
            // Arrange
            var endpoint = typeof(AcquisitionFileController).FindMethod(nameof(AcquisitionFileController.GetAcquisitionFile), typeof(long));

            // Act
            // Assert
            Assert.NotNull(endpoint);
            endpoint.HasGet("{id:long}");
            endpoint.HasPermissions(Permissions.AcquisitionFileView);
        }

        [Fact]
        public void UpdateAcquisitionFile_Route()
        {
            // Arrange
            var endpoint = typeof(AcquisitionFileController).FindMethod(nameof(AcquisitionFileController.UpdateAcquisitionFile), typeof(long), typeof(AcquisitionFileModel), typeof(string[]));

            // Act
            // Assert
            Assert.NotNull(endpoint);
            endpoint.HasPut("{id:long}");
            endpoint.HasPermissions(Permissions.AcquisitionFileEdit);
        }
        #endregion
    }
}
