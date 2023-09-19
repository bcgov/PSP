using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Notes.Controllers;
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
        private Mock<IPropertyService> _service;
        private PropertyController _controller;
        private IMapper _mapper;
        private TestHelper _helper;

        public PropertyControllerTest()
        {
            this._helper = new TestHelper();
            this._controller = this._helper.CreateController<PropertyController>(Permissions.PropertyView);
            this._mapper = this._helper.GetService<IMapper>();
            this._service = this._helper.GetService<Mock<IPropertyService>>();
        }

        #region Get
        /// <summary>
        /// Make a successful request to fetch property by PID.
        /// </summary>
        [Fact]
        public void GetConceptPropertyById_Success()
        {
            // Arrange
            var pid = 12345;
            var property = EntityHelper.CreateProperty(pid);

            this._service.Setup(m => m.GetById(It.IsAny<long>())).Returns(property);

            // Act
            var result = this._controller.GetConceptPropertyWithId(property.Internal_Id);

            // Assert
            this._service.Verify(m => m.GetById(It.IsAny<long>()), Times.Once());
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
            var property = EntityHelper.CreateProperty(12345);

            this._service.Setup(m => m.Update(It.IsAny<Pims.Dal.Entities.PimsProperty>())).Returns(property);

            // Act
            var result = this._controller.UpdateConceptProperty(this._mapper.Map<Models.Concepts.PropertyModel>(property));

            // Assert
            this._service.Verify(m => m.Update(It.IsAny<Pims.Dal.Entities.PimsProperty>()), Times.Once());
        }
        #endregion
    }
}
