using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.CompensationRequisition.Controllers;
using Pims.Api.Services;
using Pims.Core.Test;
using Pims.Core.Security;
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
            this._helper = new TestHelper();
            this._controller = this._helper.CreateController<H120CategoryController>(Permissions.CompensationRequisitionView);
            this._service = this._helper.GetService<Mock<IH120CategoryService>>();
            this._mapper = this._helper.GetService<IMapper>();
        }

        [Fact]
        public void GetTakesByPropertyId_Success()
        {
            // Arrange
            this._service.Setup(m => m.GetAll());

            // Act
            var result = this._controller.GetH120Categories();

            // Assert
            result.Should().BeOfType<JsonResult>();
            this._service.Verify(m => m.GetAll(), Times.Once());
        }
    }
}
