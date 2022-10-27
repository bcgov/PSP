using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Property.Controllers;
using Pims.Api.Services;
using Pims.Core.Test;
using Pims.Dal.Security;
using Pims.Dal.Services;
using Xunit;

namespace Pims.Api.Test.Controllers.Property
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "property")]
    [ExcludeFromCodeCoverage]
    public class PropertyControllerTest
    {
        #region Get
        /// <summary>
        /// Make a successful request to fetch property by PID.
        /// </summary>
        [Fact]
        public void GetConceptPropertyById_Success()
        {
            // Arrange
            var pid = 12345;
            var helper = new TestHelper();
            var controller = helper.CreateController<PropertyController>(Permissions.PropertyView);
            var property = EntityHelper.CreateProperty(pid);

            var service = helper.GetService<Mock<IPropertyService>>();
            var mapper = helper.GetService<IMapper>();

            service.Setup(m => m.GetById(It.IsAny<long>())).Returns(property);

            // Act
            var result = controller.GetConceptPropertyWithId(property.Id);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<Models.Concepts.PropertyModel>(actionResult.Value);
            var expectedResult = mapper.Map<Models.Concepts.PropertyModel>(property);
            expectedResult.Should().BeEquivalentTo(actualResult);
            service.Verify(m => m.GetById(It.IsAny<long>()), Times.Once());
        }
        #endregion
        #region Update
        /// <summary>
        /// Make a successful request to update a property.
        /// </summary>
        [Fact]
        public void UpdateConceptProperty_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<PropertyController>(Permissions.PropertyEdit);
            var property = EntityHelper.CreateProperty(12345);

            var service = helper.GetService<Mock<IPropertyService>>();
            var mapper = helper.GetService<IMapper>();

            service.Setup(m => m.Update(It.IsAny<Pims.Dal.Entities.PimsProperty>())).Returns(property);

            // Act
            var result = controller.UpdateConceptProperty(mapper.Map<Models.Concepts.PropertyModel>(property));

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<Models.Concepts.PropertyModel>(actionResult.Value);
            var expectedResult = mapper.Map<Models.Concepts.PropertyModel>(property);
            expectedResult.Should().BeEquivalentTo(actualResult);
            service.Verify(m => m.Update(It.IsAny<Pims.Dal.Entities.PimsProperty>()), Times.Once());
        }
        #endregion
    }
}
