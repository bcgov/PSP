using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Controllers;
using Pims.Api.Models.Concepts;
using Pims.Api.Services;
using Pims.Core.Exceptions;
using Pims.Core.Test;
using Pims.Dal.Security;
using Xunit;

namespace Pims.Api.Test.Controllers
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "financialcode")]
    [ExcludeFromCodeCoverage]
    public class FinancialCodeControllerTest
    {
        // xUnit.net creates a new instance of the test class for every test that is run,
        // so any code which is placed into the constructor of the test class will be run for every single test.
        private readonly TestHelper _helper;
        private readonly FinancialCodeController _controller;
        private readonly Mock<IFinancialCodeService> _service;
        private readonly IMapper _mapper;

        public FinancialCodeControllerTest()
        {
            _helper = new TestHelper();
            _controller = _helper.CreateController<FinancialCodeController>(Permissions.ProjectView);
            _service = _helper.GetService<Mock<IFinancialCodeService>>();
            _mapper = _helper.GetService<IMapper>();
        }

        [Fact]
        public void GetFinancialCodesByType_Success()
        {
            // Arrange
            _service.Setup(m => m.GetFinancialCodesByType(It.IsAny<FinancialCodeTypes>()));

            // Act
            var result = _controller.GetFinancialCodesByType(FinancialCodeTypes.BusinessFunction);

            // Assert
            result.Should().BeOfType<JsonResult>();
            _service.Verify(m => m.GetFinancialCodesByType(It.IsAny<FinancialCodeTypes>()), Times.Once());
        }
    }
}
