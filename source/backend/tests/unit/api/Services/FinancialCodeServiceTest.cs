using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
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
        private readonly TestHelper _helper;

        public FinancialCodeServiceTest()
        {
            this._helper = new TestHelper();
        }

        private FinancialCodeService CreateWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            return this._helper.Create<FinancialCodeService>(user);
        }

        [Fact]
        public void GetAll_Success()
        {
            // Arrange
            var service = this.CreateWithPermissions(Permissions.SystemAdmin);
            var repo1 = this._helper.GetService<Mock<IBusinessFunctionCodeRepository>>();
            var repo2 = this._helper.GetService<Mock<IChartOfAccountsCodeRepository>>();
            var repo3 = this._helper.GetService<Mock<IYearlyFinancialCodeRepository>>();
            var repo4 = this._helper.GetService<Mock<ICostTypeCodeRepository>>();
            var repo5 = this._helper.GetService<Mock<IFinancialActivityCodeRepository>>();
            var repo6 = this._helper.GetService<Mock<IWorkActivityCodeRepository>>();
            var repo7 = this._helper.GetService<Mock<IResponsibilityCodeRepository>>();

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
            var service = this.CreateWithPermissions();

            // Act
            Action act = () => service.GetAllFinancialCodes();

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void GetFinancialCodesByType_Success()
        {
            // Arrange
            var service = this.CreateWithPermissions(Permissions.ProjectView);
            var repo = this._helper.GetService<Mock<IBusinessFunctionCodeRepository>>();

            repo.Setup(x => x.GetAllBusinessFunctionCodes()).Returns(new List<PimsBusinessFunctionCode>());

            // Act
            var result = service.GetFinancialCodesByType(FinancialCodeTypes.BusinessFunction);

            // Assert
            result.Should().BeAssignableTo<IList<FinancialCodeModel>>();
            repo.Verify(x => x.GetAllBusinessFunctionCodes(), Times.Once);
        }

        [Fact]
        public void GetFinancialCodesByType_NoPermission()
        {
            // Arrange
            var service = this.CreateWithPermissions();

            // Act
            Action act = () => service.GetFinancialCodesByType(FinancialCodeTypes.BusinessFunction);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void GetById_Success()
        {
            // Arrange
            var service = this.CreateWithPermissions(Permissions.SystemAdmin);
            var repo = this._helper.GetService<Mock<IBusinessFunctionCodeRepository>>();
            repo.Setup(x => x.GetById(It.IsAny<long>()));

            // Act
            var result = service.GetById(FinancialCodeTypes.BusinessFunction, 1);

            // Assert
            repo.Verify(x => x.GetById(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void GetById_NoPermission()
        {
            // Arrange
            var service = this.CreateWithPermissions();

            // Act
            Action act = () => service.GetById(FinancialCodeTypes.BusinessFunction, 1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void Add_Success()
        {
            // Arrange
            var service = this.CreateWithPermissions(Permissions.SystemAdmin);
            var repository = this._helper.GetService<Mock<IBusinessFunctionCodeRepository>>();
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
            var service = this.CreateWithPermissions();

            // Act
            Action act = () => service.Add(FinancialCodeTypes.BusinessFunction, new FinancialCodeModel());

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void Add_ThrowIfNull()
        {
            // Arrange
            var service = this.CreateWithPermissions(Permissions.SystemAdmin);
            var repository = this._helper.GetService<Mock<IBusinessFunctionCodeRepository>>();
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
            var service = this.CreateWithPermissions(Permissions.SystemAdmin);
            var repository = this._helper.GetService<Mock<IBusinessFunctionCodeRepository>>();
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

        [Fact]
        public void Update_Success()
        {
            // Arrange
            var service = this.CreateWithPermissions(Permissions.SystemAdmin);
            var codeEntity = EntityHelper.CreateFinancialCode(FinancialCodeTypes.BusinessFunction, 1, "FOO", "Other description");
            var repository = this._helper.GetService<Mock<IBusinessFunctionCodeRepository>>();
            repository.Setup(x => x.Update(It.IsAny<PimsBusinessFunctionCode>())).Returns(codeEntity as PimsBusinessFunctionCode);
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            var _mapper = this._helper.GetService<IMapper>();

            // Act
            var model = _mapper.Map<FinancialCodeModel>(codeEntity);
            var result = service.Update(FinancialCodeTypes.BusinessFunction, model);

            // Assert
            repository.Verify(x => x.Update(It.IsAny<PimsBusinessFunctionCode>()), Times.Once);
        }

        [Fact]
        public void Update_NoPermission()
        {
            // Arrange
            var service = this.CreateWithPermissions();

            // Act
            Action act = () => service.Update(FinancialCodeTypes.BusinessFunction, new FinancialCodeModel());

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void Update_ThrowIf_Null()
        {
            // Arrange
            var service = this.CreateWithPermissions(Permissions.SystemAdmin);
            var repository = this._helper.GetService<Mock<IBusinessFunctionCodeRepository>>();
            repository.Setup(x => x.Update(It.IsAny<PimsBusinessFunctionCode>()));

            // Act
            Action act = () => service.Update(FinancialCodeTypes.BusinessFunction, null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Update_ThrowIf_DuplicateCodeFound()
        {
            // Arrange
            var service = this.CreateWithPermissions(Permissions.SystemAdmin);
            var repository = this._helper.GetService<Mock<IBusinessFunctionCodeRepository>>();
            repository.Setup(x => x.Update(It.IsAny<PimsBusinessFunctionCode>())).Throws<DuplicateEntityException>();
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);

            // Act
            var duplicate = new FinancialCodeModel()
            {
                Id = 1,
                RowVersion = 1,
                Code = "FOO",
                Description = "Other description",
                EffectiveDate = DateTime.Now,
            };
            Action act = () => service.Update(FinancialCodeTypes.BusinessFunction, duplicate);

            // Assert
            act.Should().Throw<DuplicateEntityException>();
        }

        [Fact]
        public void Update_ThrowIf_OlderVersion()
        {
            // Arrange
            var service = this.CreateWithPermissions(Permissions.SystemAdmin);
            var repository = this._helper.GetService<Mock<IBusinessFunctionCodeRepository>>();
            repository.Setup(x => x.Update(It.IsAny<PimsBusinessFunctionCode>()));
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(2);

            // Act
            var duplicate = new FinancialCodeModel()
            {
                Id = 1,
                RowVersion = 1,
                Code = "FOO",
                Description = "Other description",
                EffectiveDate = DateTime.Now,
            };
            Action act = () => service.Update(FinancialCodeTypes.BusinessFunction, duplicate);

            // Assert
            act.Should().Throw<DbUpdateConcurrencyException>();
        }
    }
}
