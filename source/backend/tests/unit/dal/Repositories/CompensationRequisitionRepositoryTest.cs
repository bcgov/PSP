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

namespace Pims.Dal.Test.Repositories
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("group", "compensation-requisition")]
    [ExcludeFromCodeCoverage]
    public class CompensationRequisitionRepositoryTest
    {
        private readonly TestHelper _helper = new();

        private CompensationRequisitionRepository CreateWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            _helper.CreatePimsContext(user, true);
            return _helper.CreateRepository<CompensationRequisitionRepository>(user);
        }

        [Fact]
        public void GetById_Success()
        {
           throw new NotImplementedException();
        }

        [Fact]
        public void Update_Success()
        {
            throw new NotImplementedException();
        }
    }
}
