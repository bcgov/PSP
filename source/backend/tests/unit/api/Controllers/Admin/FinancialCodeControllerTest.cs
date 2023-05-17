using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Admin.Controllers;
using Pims.Api.Models.Concepts;
using Pims.Api.Services;
using Pims.Core.Exceptions;
using Pims.Core.Test;
using Pims.Dal.Security;
using Xunit;

namespace Pims.Api.Test.Admin.Controllers
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("area", "admin")]
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
            _controller = _helper.CreateController<FinancialCodeController>(Permissions.SystemAdmin);
            _service = _helper.GetService<Mock<IFinancialCodeService>>();
            _mapper = _helper.GetService<IMapper>();
        }

        [Fact]
        public void GetFinancialCodes_Success()
        {
            // Arrange
            _service.Setup(m => m.GetAllFinancialCodes());

            // Act
            var result = _controller.GetFinancialCodes();

            // Assert
            result.Should().BeOfType<JsonResult>();
            _service.Verify(m => m.GetAllFinancialCodes(), Times.Once());
        }

        [Fact]
        public void GetFinancialCodeById_Success()
        {
            // Arrange
            _service.Setup(m => m.GetById(It.IsAny<FinancialCodeTypes>(), It.IsAny<long>()));

            // Act
            var result = _controller.GetFinancialCode(FinancialCodeTypes.BusinessFunction, 1);

            // Assert
            result.Should().BeOfType<JsonResult>();
            _service.Verify(m => m.GetById(It.IsAny<FinancialCodeTypes>(), It.IsAny<long>()), Times.Once());
        }

        [Fact]
        public void AddFinancialCode_Success()
        {
            // Arrange
            _service.Setup(m => m.Add(It.IsAny<FinancialCodeTypes>(), It.IsAny<FinancialCodeModel>()));

            // Act
            var result = _controller.AddFinancialCode(FinancialCodeTypes.BusinessFunction, new FinancialCodeModel());

            // Assert
            result.Should().BeOfType<JsonResult>();
            _service.Verify(m => m.Add(It.IsAny<FinancialCodeTypes>(), It.IsAny<FinancialCodeModel>()), Times.Once());
        }

        [Fact]
        public void AddFinancialCode_DuplicateCode_ShouldThrow()
        {
            // Arrange
            _service.Setup(m => m.Add(It.IsAny<FinancialCodeTypes>(), It.IsAny<FinancialCodeModel>()))
                .Throws<DuplicateEntityException>();

            // Act
            var result = _controller.AddFinancialCode(FinancialCodeTypes.BusinessFunction, new FinancialCodeModel());

            // Assert
            result.Should().BeOfType<ConflictObjectResult>();
            _service.Verify(m => m.Add(It.IsAny<FinancialCodeTypes>(), It.IsAny<FinancialCodeModel>()), Times.Once());
        }

        [Fact]
        public void UpdateFinancialCode_Success()
        {
            // Arrange
            var code = EntityHelper.CreateFinancialCode(FinancialCodeTypes.BusinessFunction, 1, "CODE", "description");
            var model = _mapper.Map<FinancialCodeModel>(code);
            _service.Setup(m => m.Update(It.IsAny<FinancialCodeTypes>(), It.IsAny<FinancialCodeModel>())).Returns(model);

            // Act
            var result = _controller.UpdateFinancialCode(FinancialCodeTypes.BusinessFunction, model.Id, model);

            // Assert
            result.Should().BeOfType<JsonResult>();
            _service.Verify(m => m.Update(It.IsAny<FinancialCodeTypes>(), It.IsAny<FinancialCodeModel>()), Times.Once());
        }

        [Fact]
        public void UpdateFinancialCode_DuplicateCode_ShouldThrow()
        {
            // Arrange
            var code = EntityHelper.CreateFinancialCode(FinancialCodeTypes.BusinessFunction, 1, "CODE", "description");
            var model = _mapper.Map<FinancialCodeModel>(code);
            _service.Setup(m => m.Update(It.IsAny<FinancialCodeTypes>(), It.IsAny<FinancialCodeModel>()))
                .Throws<DuplicateEntityException>();

            // Act
            var result = _controller.UpdateFinancialCode(FinancialCodeTypes.BusinessFunction, model.Id, model);

            // Assert
            result.Should().BeOfType<ConflictObjectResult>();
            _service.Verify(m => m.Update(It.IsAny<FinancialCodeTypes>(), It.IsAny<FinancialCodeModel>()), Times.Once());
        }
    }
}
