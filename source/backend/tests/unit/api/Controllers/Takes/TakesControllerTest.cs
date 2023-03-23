using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Admin.Controllers;
using Pims.Api.Areas.Takes.Controllers;
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
            _helper = new TestHelper();
            _controller = _helper.CreateController<TakeController>(Permissions.PropertyView, Permissions.AcquisitionFileView);
            _service = _helper.GetService<Mock<ITakeService>>();
            _mapper = _helper.GetService<IMapper>();
        }

        [Fact]
        public void GetTakesByAcquisitionFileId_Success()
        {
            // Arrange
            _service.Setup(m => m.GetByFileId(It.IsAny<long>()));

            // Act
            var result = _controller.GetTakesByAcquisitionFileId(1);

            // Assert
            result.Should().BeOfType<JsonResult>();
            _service.Verify(m => m.GetByFileId(It.IsAny<long>()), Times.Once());
        }

        [Fact]
        public void GetTakesCountByPropertyId_Success()
        {
            // Arrange
            _service.Setup(m => m.GetCountByPropertyId(It.IsAny<long>()));

            // Act
            var result = _controller.GetTakesCountByPropertyId(1);

            // Assert
            result.Should().BeOfType<JsonResult>();
            _service.Verify(m => m.GetCountByPropertyId(It.IsAny<long>()), Times.Once());
        }

        [Fact]
        public void UpdateAcquisitionPropertyTakes_Success()
        {
            // Arrange
            _service.Setup(m => m.UpdateAcquisitionPropertyTakes(It.IsAny<long>(), It.IsAny<IEnumerable<PimsTake>>()));

            // Act
            var result = _controller.UpdateAcquisitionPropertyTakes(1, new List<TakeModel>());

            // Assert
            result.Should().BeOfType<JsonResult>();
            _service.Verify(m => m.UpdateAcquisitionPropertyTakes(It.IsAny<long>(), It.IsAny<IEnumerable<PimsTake>>()));
        }
    }
}
