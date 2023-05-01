using Moq;
using Pims.Api.Services;
using Pims.Core.Test;
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

    public class CompensationRequisitionTest
    {
        private readonly TestHelper _helper;

        public CompensationRequisitionTest()
        {
            _helper = new TestHelper();
        }

        [Fact]
        public void GetById_Success()
        {
            // Arrange
            var service = CreateWithPermissions(Permissions.CompensationRequisitionView);
            var repo = _helper.GetService<Mock<ICompensationRequisitionRepository>>();
            repo.Setup(x => x.GetById(It.IsAny<long>()));

            // Act
            var result = service.GetById(1);

            // Assert
            repo.Verify(x => x.GetById(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void Update_Success()
        {
            // Arrange
            var service = CreateWithPermissions(Permissions.CompensationRequisitionEdit);
            var repo = _helper.GetService<Mock<ICompensationRequisitionRepository>>();

            throw new NotImplementedException();
        }


        private CompensationRequisitionService CreateWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            return _helper.Create<CompensationRequisitionService>(user);
        }
    }
}
