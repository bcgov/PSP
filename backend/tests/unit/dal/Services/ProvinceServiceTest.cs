using Pims.Core.Test;
using Pims.Dal.Security;
using Pims.Dal.Services;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;

namespace Pims.Dal.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("area", "admin")]
    [Trait("group", "province")]
    [ExcludeFromCodeCoverage]
    public class ProvinceServiceTest
    {
        #region Tests
        #region Get
        [Fact]
        public void Get()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);
            using var init = helper.InitializeDatabase(user);

            var service = helper.CreateService<ProvinceService>(user);

            // Act
            var result = service.Get();

            // Assert
            Assert.True(result.Any());
        }
        #endregion
        #endregion
    }
}
