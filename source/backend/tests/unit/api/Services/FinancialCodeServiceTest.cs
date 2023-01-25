using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Moq;
using Pims.Api.Models.Concepts;
using Pims.Api.Services;
using Pims.Core.Exceptions;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Xunit;

namespace Pims.Api.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("area", "admin")]
    [Trait("group", "financialcode")]
    [ExcludeFromCodeCoverage]
    public class FinancialCodeServiceTest
    {
        // xUnit.net creates a new instance of the test class for every test that is run,
        // so any code which is placed into the constructor of the test class will be run for every single test.
        private readonly TestHelper _helper = new();

        private FinancialCodeService CreateWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            return _helper.Create<FinancialCodeService>(user);
        }

        [Fact]
        public void GetAll_Success()
        {
            // Arrange
            var service = CreateWithPermissions(Permissions.SystemAdmin);
            var repo1 = _helper.GetService<Mock<IBusinessFunctionCodeRepository>>();
            var repo2 = _helper.GetService<Mock<IChartOfAccountsCodeRepository>>();
            var repo3 = _helper.GetService<Mock<IYearlyFinancialCodeRepository>>();
            var repo4 = _helper.GetService<Mock<ICostTypeCodeRepository>>();
            var repo5 = _helper.GetService<Mock<IFinancialActivityCodeRepository>>();
            var repo6 = _helper.GetService<Mock<IWorkActivityCodeRepository>>();
            var repo7 = _helper.GetService<Mock<IResponsibilityCodeRepository>>();

            repo1.Setup(x => x.GetAllBusinessFunctionCodes()).Returns(new List<PimsBusinessFunctionCode>());
            repo2.Setup(x => x.GetAllChartOfAccountCodes()).Returns(new List<PimsChartOfAccountsCode>());
            repo3.Setup(x => x.GetAllYearlyFinancialCodes()).Returns(new List<PimsYearlyFinancialCode>());
            repo4.Setup(x => x.GetAllCostTypeCodes()).Returns(new List<PimsCostTypeCode>());
            repo5.Setup(x => x.GetAllFinancialActivityCodes()).Returns(new List<PimsFinancialActivityCode>());
            repo6.Setup(x => x.GetAllWorkActivityCodes()).Returns(new List<PimsWorkActivityCode>());
            repo7.Setup(x => x.GetAllResponsibilityCodes()).Returns(new List<PimsResponsibilityCode>());

            // Act
            var result = service.GetAllFinancialCodes();

            // Assert
            result.Should().BeAssignableTo<IList<FinancialCodeModel>>();
            repo1.Verify(x => x.GetAllBusinessFunctionCodes(), Times.Once);
            repo2.Verify(x => x.GetAllChartOfAccountCodes(), Times.Once);
            repo3.Verify(x => x.GetAllYearlyFinancialCodes(), Times.Once);
            repo4.Verify(x => x.GetAllCostTypeCodes(), Times.Once);
            repo5.Verify(x => x.GetAllFinancialActivityCodes(), Times.Once);
            repo6.Verify(x => x.GetAllWorkActivityCodes(), Times.Once);
            repo7.Verify(x => x.GetAllResponsibilityCodes(), Times.Once);
        }

        [Fact]
        public void GetAll_NoPermission()
        {
            // Arrange
            var service = CreateWithPermissions();

            // Act
            Action act = () => service.GetAllFinancialCodes();

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void Add_Success()
        {
            // Arrange
            var service = CreateWithPermissions(Permissions.SystemAdmin);
            var repository = _helper.GetService<Mock<IBusinessFunctionCodeRepository>>();
            repository.Setup(x => x.Add(It.IsAny<PimsBusinessFunctionCode>()));

            // Act
            var result = service.Add(FinancialCodeTypes.BusinessFunction, new FinancialCodeModel());

            // Assert
            repository.Verify(x => x.Add(It.IsAny<PimsBusinessFunctionCode>()), Times.Once);
        }

        [Fact]
        public void Add_NoPermission()
        {
            // Arrange
            var service = CreateWithPermissions();

            // Act
            Action act = () => service.Add(FinancialCodeTypes.BusinessFunction, new FinancialCodeModel());

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void Add_ThrowIfNull()
        {
            // Arrange
            var service = CreateWithPermissions(Permissions.SystemAdmin);
            var repository = _helper.GetService<Mock<IBusinessFunctionCodeRepository>>();
            repository.Setup(x => x.Add(It.IsAny<PimsBusinessFunctionCode>()));

            // Act
            Action act = () => service.Add(FinancialCodeTypes.BusinessFunction, null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Add_ThrowIfDuplicateCodeFound()
        {
            // Arrange
            var service = CreateWithPermissions(Permissions.SystemAdmin);
            var repository = _helper.GetService<Mock<IBusinessFunctionCodeRepository>>();
            repository.Setup(x => x.Add(It.IsAny<PimsBusinessFunctionCode>())).Throws<DuplicateEntityException>();

            // Act
            var duplicate = new FinancialCodeModel()
            {
                Code = "FOO",
                Description = "Other description",
                EffectiveDate = DateTime.Now,
            };
            Action act = () => service.Add(FinancialCodeTypes.BusinessFunction, duplicate);

            // Assert
            act.Should().Throw<DuplicateEntityException>();
        }
    }
}
