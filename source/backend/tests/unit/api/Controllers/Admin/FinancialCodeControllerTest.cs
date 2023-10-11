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
            this._helper = new TestHelper();
            this._controller = this._helper.CreateController<FinancialCodeController>(Permissions.SystemAdmin);
            this._service = this._helper.GetService<Mock<IFinancialCodeService>>();
            this._mapper = this._helper.GetService<IMapper>();
        }

        [Fact]
        public void GetFinancialCodes_Success()
        {
            // Arrange
            this._service.Setup(m => m.GetAllFinancialCodes());

            // Act
            var result = this._controller.GetFinancialCodes();

            // Assert
            result.Should().BeOfType<JsonResult>();
            this._service.Verify(m => m.GetAllFinancialCodes(), Times.Once());
        }

        [Fact]
        public void GetFinancialCodeById_Success()
        {
            // Arrange
            this._service.Setup(m => m.GetById(It.IsAny<FinancialCodeTypes>(), It.IsAny<long>()));

            // Act
            var result = this._controller.GetFinancialCode(FinancialCodeTypes.BusinessFunction, 1);

            // Assert
            result.Should().BeOfType<JsonResult>();
            this._service.Verify(m => m.GetById(It.IsAny<FinancialCodeTypes>(), It.IsAny<long>()), Times.Once());
        }

        [Fact]
        public void AddFinancialCode_Success()
        {
            // Arrange
            this._service.Setup(m => m.Add(It.IsAny<FinancialCodeTypes>(), It.IsAny<FinancialCodeModel>()));

            // Act
            var result = this._controller.AddFinancialCode(FinancialCodeTypes.BusinessFunction, new FinancialCodeModel());

            // Assert
            result.Should().BeOfType<JsonResult>();
            this._service.Verify(m => m.Add(It.IsAny<FinancialCodeTypes>(), It.IsAny<FinancialCodeModel>()), Times.Once());
        }

        [Fact]
        public void AddFinancialCode_DuplicateCode_ShouldThrow()
        {
            // Arrange
            this._service.Setup(m => m.Add(It.IsAny<FinancialCodeTypes>(), It.IsAny<FinancialCodeModel>()))
                .Throws<DuplicateEntityException>();

            // Act
            var result = this._controller.AddFinancialCode(FinancialCodeTypes.BusinessFunction, new FinancialCodeModel());

            // Assert
            result.Should().BeOfType<ConflictObjectResult>();
            this._service.Verify(m => m.Add(It.IsAny<FinancialCodeTypes>(), It.IsAny<FinancialCodeModel>()), Times.Once());
        }

        [Fact]
        public void UpdateFinancialCode_Success()
        {
            // Arrange
            var code = EntityHelper.CreateFinancialCode(FinancialCodeTypes.BusinessFunction, 1, "CODE", "description");
            var model = this._mapper.Map<FinancialCodeModel>(code);
            this._service.Setup(m => m.Update(It.IsAny<FinancialCodeTypes>(), It.IsAny<FinancialCodeModel>())).Returns(model);

            // Act
            var result = this._controller.UpdateFinancialCode(FinancialCodeTypes.BusinessFunction, model.Id, model);

            // Assert
            result.Should().BeOfType<JsonResult>();
            this._service.Verify(m => m.Update(It.IsAny<FinancialCodeTypes>(), It.IsAny<FinancialCodeModel>()), Times.Once());
        }

        [Fact]
        public void UpdateFinancialCode_DuplicateCode_ShouldThrow()
        {
            // Arrange
            var code = EntityHelper.CreateFinancialCode(FinancialCodeTypes.BusinessFunction, 1, "CODE", "description");
            var model = this._mapper.Map<FinancialCodeModel>(code);
            this._service.Setup(m => m.Update(It.IsAny<FinancialCodeTypes>(), It.IsAny<FinancialCodeModel>()))
                .Throws<DuplicateEntityException>();

            // Act
            var result = this._controller.UpdateFinancialCode(FinancialCodeTypes.BusinessFunction, model.Id, model);

            // Assert
            result.Should().BeOfType<ConflictObjectResult>();
            this._service.Verify(m => m.Update(It.IsAny<FinancialCodeTypes>(), It.IsAny<FinancialCodeModel>()), Times.Once());
        }
    }
}
