using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Admin.Controllers;
using Pims.Api.Areas.CompensationRequisition.Controllers;
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
    public class H120CategoryControllerTest
    {
        // xUnit.net creates a new instance of the test class for every test that is run,
        // so any code which is placed into the constructor of the test class will be run for every single test.
        private readonly TestHelper _helper;
        private readonly H120CategoryController _controller;
        private readonly Mock<IH120CategoryService> _service;
        private readonly IMapper _mapper;

        public H120CategoryControllerTest()
        {
            _helper = new TestHelper();
            _controller = _helper.CreateController<H120CategoryController>(Permissions.CompensationRequisitionView);
            _service = _helper.GetService<Mock<IH120CategoryService>>();
            _mapper = _helper.GetService<IMapper>();
        }

        [Fact]
        public void GetTakesByPropertyId_Success()
        {
            // Arrange
            _service.Setup(m => m.GetAll());

            // Act
            var result = _controller.GetH120Categories();

            // Assert
            result.Should().BeOfType<JsonResult>();
            _service.Verify(m => m.GetAll(), Times.Once());
        }
    }
}
