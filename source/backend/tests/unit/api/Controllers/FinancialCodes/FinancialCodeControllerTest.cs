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
            this._helper = new TestHelper();
            this._controller = this._helper.CreateController<FinancialCodeController>(Permissions.ProjectView);
            this._service = this._helper.GetService<Mock<IFinancialCodeService>>();
            this._mapper = this._helper.GetService<IMapper>();
        }

        [Fact]
        public void GetFinancialCodesByType_Success()
        {
            // Arrange
            this._service.Setup(m => m.GetFinancialCodesByType(It.IsAny<FinancialCodeTypes>()));

            // Act
            var result = this._controller.GetFinancialCodesByType(FinancialCodeTypes.BusinessFunction);

            // Assert
            result.Should().BeOfType<JsonResult>();
            this._service.Verify(m => m.GetFinancialCodesByType(It.IsAny<FinancialCodeTypes>()), Times.Once());
        }
    }
}
