using FluentAssertions;
using Moq;
using Pims.Api.Services;
using Pims.Core.Test;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Pims.Api.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("area", "compensation-requisition")]
    [ExcludeFromCodeCoverage]

    public class H120CategoryServiceTest
    {
        private readonly TestHelper _helper;

        public H120CategoryServiceTest()
        {
            _helper = new TestHelper();
        }

        [Fact]
        public void GetAll()
        {
            // Arrange
            var service = CreateWithPermissions(Permissions.CompensationRequisitionView);
            var repo = _helper.GetService<Mock<IH120CategoryRepository>>();
            repo.Setup(x => x.GetAll());

            // Act
            service.GetAll();

            // Assert
            repo.Verify(x => x.GetAll(), Times.Once);
        }


        private H120CategoryService CreateWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            return _helper.Create<H120CategoryService>(user);
        }
    }
}
