using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Pims.Core.Test;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Xunit;

namespace Pims.Dal.Test.Repositories
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("area", "admin")]
    [Trait("group", "province")]
    [ExcludeFromCodeCoverage]
    public class ProvinceRepositoryTest
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

            var service = helper.CreateRepository<ProvinceRepository>(user);

            // Act
            var result = service.GetAll();

            // Assert
            Assert.True(result.Any());
        }
        #endregion
        #endregion
    }
}
