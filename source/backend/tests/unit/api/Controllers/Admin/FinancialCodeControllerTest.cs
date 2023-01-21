using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Admin.Controllers;
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
    [Trait("area", "admin")]
    [Trait("group", "financialcode")]
    [ExcludeFromCodeCoverage]
    public class FinancialCodeControllerTest
    {
        // xUnit.net creates a new instance of the test class for every test that is run,
        // so any code which is placed into the constructor of the test class will be run for every single test.
        readonly TestHelper helper = new();

        [Fact]
        public void GetFinancialCodes_Success()
        {
            // Arrange
            var controller = helper.CreateController<FinancialCodeController>(Permissions.SystemAdmin);
            var service = helper.GetService<Mock<IFinancialCodeService>>();
            service.Setup(m => m.GetAllFinancialCodes());

            // Act
            var result = controller.GetFinancialCodes();

            // Assert
            result.Should().BeOfType<JsonResult>();
            service.Verify(m => m.GetAllFinancialCodes(), Times.Once());
        }

        [Fact]
        public void AddFinancialCode_Success()
        {
            // Arrange
            var controller = helper.CreateController<FinancialCodeController>(Permissions.SystemAdmin);
            var service = helper.GetService<Mock<IFinancialCodeService>>();
            service.Setup(m => m.Add(It.IsAny<FinancialCodeTypes>(), It.IsAny<FinancialCodeModel>()));

            // Act
            var result = controller.AddFinancialCode(FinancialCodeTypes.BusinessFunction, new FinancialCodeModel());

            // Assert
            result.Should().BeOfType<JsonResult>();
            service.Verify(m => m.Add(It.IsAny<FinancialCodeTypes>(), It.IsAny<FinancialCodeModel>()), Times.Once());
        }

        [Fact]
        public void AddFinancialCode_DuplicateCode_ShouldThrow()
        {
            // Arrange
            var controller = helper.CreateController<FinancialCodeController>(Permissions.SystemAdmin);
            var service = helper.GetService<Mock<IFinancialCodeService>>();
            service.Setup(m => m.Add(It.IsAny<FinancialCodeTypes>(), It.IsAny<FinancialCodeModel>()))
                .Throws<DuplicateEntityException>();

            // Act
            var result = controller.AddFinancialCode(FinancialCodeTypes.BusinessFunction, new FinancialCodeModel());

            // Assert
            result.Should().BeOfType<ConflictObjectResult>();
            service.Verify(m => m.Add(It.IsAny<FinancialCodeTypes>(), It.IsAny<FinancialCodeModel>()), Times.Once());
        }
    }
}
