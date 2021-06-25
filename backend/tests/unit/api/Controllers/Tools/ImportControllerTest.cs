using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Tools.Controllers;
using Pims.Core.Test;
using Pims.Dal.Security;
using Pims.Dal.Services.Admin;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Tools.Models.Import;

namespace Pims.Api.Test.Controllers.Tools
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("area", "tools")]
    [Trait("group", "import")]
    [ExcludeFromCodeCoverage]
    public class ImportControllerTest
    {
        #region Variables
        #endregion

        #region Constructors
        public ImportControllerTest() { }
        #endregion

        #region Tests
        #region ImportProperties
        [Fact]
        public void ImportProperties_BadRequest()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<ImportController>(Permissions.SystemAdmin);

            var properties = Enumerable.Range(0, 101).Select(i => new Model.ImportPropertyModel()).ToArray();

            // Act
            var result = controller.ImportProperties(properties);

            // Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void ImportProperties_UpdateParcel_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<ImportController>(Permissions.SystemAdmin);

            var properties = new[]
            {
                new Model.ImportPropertyModel()
                {
                    ParcelId = "123-123-123",
                    LocalId = "test",
                    PropertyType = "Land",
                    AgencyCode = "AEST",
                    SubAgency = "School",
                    FiscalYear = 2020,
                    Assessed = 0,
                    Classification = "Classification",
                    Status = "Active",
                    CivicAddress = "test",
                    City = "test",
                    Postal = "T9T9T9",
                    LandArea = 45.55f
                }
            };

            var parcel = new Entity.Parcel()
            {
                Id = 123123123
            };

            var service = helper.GetService<Mock<IPimsAdminService>>();
            service.Setup(m => m.BuildingConstructionType.GetAll()).Returns(Array.Empty<Entity.BuildingConstructionType>());
            service.Setup(m => m.BuildingPredominateUse.GetAll()).Returns(Array.Empty<Entity.BuildingPredominateUse>());
            service.Setup(m => m.PropertyClassification.GetAll()).Returns(new[] { new Entity.PropertyClassification("Classification") { Id = 1 } });
            service.Setup(m => m.Province.GetAll()).Returns(new[] { new Entity.Province("BC", "British Columbia") });
            service.Setup(m => m.Agency.GetAll()).Returns(new[] { new Entity.Agency("AEST", "Advanced Education, Skills & Training") });
            service.Setup(m => m.Parcel.GetByPidWithoutTracking(It.IsAny<int>())).Returns(parcel);
            service.Setup(m => m.AdministrativeArea.Get(It.IsAny<string>())).Returns(new Entity.AdministrativeArea("test"));

            // Act
            var result = controller.ImportProperties(properties);

            // Assert
            JsonResult actionResult = Assert.IsType<JsonResult>(result);
            var data = Assert.IsAssignableFrom<IEnumerable<Model.ParcelModel>>(actionResult.Value);
            Assert.Equal(properties.First().ParcelId, data.First().PID);
            service.Verify(m => m.BuildingConstructionType.GetAll(), Times.Once());
            service.Verify(m => m.BuildingPredominateUse.GetAll(), Times.Once());
            service.Verify(m => m.PropertyClassification.GetAll(), Times.Once());
            service.Verify(m => m.Province.GetAll(), Times.Once());
            service.Verify(m => m.AdministrativeArea.Get(It.IsAny<string>()), Times.Once());
            service.Verify(m => m.Agency.GetAll(), Times.Once());
            service.Verify(m => m.Agency.Add(It.IsAny<Entity.Agency>()), Times.Once());
            service.Verify(m => m.Parcel.Update(It.IsAny<Entity.Parcel>()), Times.Once());
        }

        #endregion
        #endregion
    }
}
