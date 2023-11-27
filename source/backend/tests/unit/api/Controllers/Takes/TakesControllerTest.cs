using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Admin.Controllers;
using Pims.Api.Areas.Takes.Controllers;
using Pims.Api.Models.Concepts.Take;
using Pims.Api.Models.Concepts;
using Pims.Api.Services;
using Pims.Core.Exceptions;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Security;
using Xunit;

namespace Pims.Api.Test.Controllers
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("area", "takes")]
    [ExcludeFromCodeCoverage]
    public class TakesControllerTest
    {
        // xUnit.net creates a new instance of the test class for every test that is run,
        // so any code which is placed into the constructor of the test class will be run for every single test.
        private readonly TestHelper _helper;
        private readonly TakeController _controller;
        private readonly Mock<ITakeService> _service;
        private readonly IMapper _mapper;

        public TakesControllerTest()
        {
            this._helper = new TestHelper();
            this._controller = this._helper.CreateController<TakeController>(Permissions.PropertyView, Permissions.AcquisitionFileView);
            this._service = this._helper.GetService<Mock<ITakeService>>();
            this._mapper = this._helper.GetService<IMapper>();
        }

        [Fact]
        public void GetTakesByAcquisitionFileId_Success()
        {
            // Arrange
            this._service.Setup(m => m.GetByFileId(It.IsAny<long>()));

            // Act
            var result = this._controller.GetTakesByAcquisitionFileId(1);

            // Assert
            result.Should().BeOfType<JsonResult>();
            this._service.Verify(m => m.GetByFileId(It.IsAny<long>()), Times.Once());
        }

        [Fact]
        public void GetTakesByPropertyId_Success()
        {
            // Arrange
            this._service.Setup(m => m.GetByPropertyId(It.IsAny<long>(), It.IsAny<long>()));

            // Act
            var result = this._controller.GetTakesByPropertyId(1, 2);

            // Assert
            result.Should().BeOfType<JsonResult>();
            this._service.Verify(m => m.GetByPropertyId(It.IsAny<long>(), It.IsAny<long>()), Times.Once());
        }

        [Fact]
        public void GetTakesCountByPropertyId_Success()
        {
            // Arrange
            this._service.Setup(m => m.GetCountByPropertyId(It.IsAny<long>()));

            // Act
            var result = this._controller.GetTakesCountByPropertyId(1);

            // Assert
            result.Should().BeOfType<JsonResult>();
            this._service.Verify(m => m.GetCountByPropertyId(It.IsAny<long>()), Times.Once());
        }

        [Fact]
        public void UpdateAcquisitionPropertyTakes_Success()
        {
            // Arrange
            this._service.Setup(m => m.UpdateAcquisitionPropertyTakes(It.IsAny<long>(), It.IsAny<IEnumerable<PimsTake>>()));

            // Act
            var result = this._controller.UpdateAcquisitionPropertyTakes(1, new List<TakeModel>());

            // Assert
            result.Should().BeOfType<JsonResult>();
            this._service.Verify(m => m.UpdateAcquisitionPropertyTakes(It.IsAny<long>(), It.IsAny<IEnumerable<PimsTake>>()));
        }
    }
}
