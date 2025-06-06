using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using Pims.Api.Controllers;
using Pims.Core.Test;
using Pims.Dal;
using Pims.Dal.Repositories;
using Xunit;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Test.Controllers
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "tenants")]
    [ExcludeFromCodeCoverage]
    public class TenantControllerTest
    {
        #region Variables
        private TestHelper _helper;
        private ClaimsPrincipal _user;
        #endregion

        public TenantControllerTest()
        {
            this._helper = new TestHelper();
            this._user = PrincipalHelper.CreateForPermission();
        }

        #region Tests
        #region Settings
        [Fact]
        public void Settings_200Response()
        {
            // Arrange
            var mockOptions = new Mock<IOptionsMonitor<PimsOptions>>();
            var options = new PimsOptions()
            {
                Tenant = "TEST",
            };
            mockOptions.Setup(m => m.CurrentValue).Returns(options);
            var controller = this._helper.CreateController<TenantController>(this._user, mockOptions.Object);

            var mapper = this._helper.GetService<IMapper>();
            var repository = this._helper.GetService<Mock<ITenantRepository>>();

            var tenant = EntityHelper.CreateTenant(1, "TEST");
            repository.Setup(m => m.TryGetTenantByCode(tenant.Code)).Returns(tenant);

            // Act
            var result = (JsonResult)controller.Settings();

            // Assert
            Assert.Null(result.StatusCode);
            repository.Verify(m => m.TryGetTenantByCode(tenant.Code), Times.Once());
        }

        [Fact]
        public void Settings_NoTenantFound_204Response()
        {
            // Arrange
            var mockOptions = new Mock<IOptionsMonitor<PimsOptions>>();
            var options = new PimsOptions()
            {
                Tenant = "TEST",
            };
            mockOptions.Setup(m => m.CurrentValue).Returns(options);
            var controller = this._helper.CreateController<TenantController>(this._user, mockOptions.Object);

            var mapper = this._helper.GetService<IMapper>();
            var repository = this._helper.GetService<Mock<ITenantRepository>>();

            var tenant = EntityHelper.CreateTenant(1, "TEST");
            repository.Setup(m => m.TryGetTenantByCode(tenant.Code)).Returns<Entity.PimsTenant>(null);

            // Act
            var result = controller.Settings();

            // Assert
            var actionResult = Assert.IsType<NoContentResult>(result);
            actionResult.StatusCode.Should().Be(204);
            repository.Verify(m => m.TryGetTenantByCode(tenant.Code), Times.Once());
        }

        [Fact]
        public void Settings_TenantOptions_204Response()
        {
            // Arrange
            var mockOptions = new Mock<IOptionsMonitor<PimsOptions>>();
            var options = new PimsOptions();
            mockOptions.Setup(m => m.CurrentValue).Returns(options);
            var controller = this._helper.CreateController<TenantController>(this._user, mockOptions.Object);

            var mapper = this._helper.GetService<IMapper>();
            var repository = this._helper.GetService<Mock<ITenantRepository>>();

            repository.Setup(m => m.TryGetTenantByCode(null)).Returns<Entity.PimsTenant>(null);

            // Act
            var result = controller.Settings();

            // Assert
            var actionResult = Assert.IsType<NoContentResult>(result);
            actionResult.StatusCode.Should().Be(204);
            repository.Verify(m => m.TryGetTenantByCode(null), Times.Once());
        }
        #endregion
        #endregion
    }
}
