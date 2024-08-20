using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using MapsterMapper;
using Moq;
using Pims.Api.Areas.Lease.Controllers;
using Pims.Api.Models.Concepts.Lease;
using Pims.Api.Services;
using Pims.Core.Test;
using Pims.Dal.Security;
using Xunit;

namespace Pims.Api.Test.Controllers.Lease
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "lease")]
    [ExcludeFromCodeCoverage]
    public class LeaseStakeholderControllerTest
    {
        private Mock<ILeaseService> _repository;
        private LeaseStakeholderController _controller;
        private IMapper _mapper;
        private TestHelper _helper;

        public LeaseStakeholderControllerTest()
        {
            this._helper = new TestHelper();
            this._controller = this._helper.CreateController<LeaseStakeholderController>(Permissions.LeaseView);
            this._mapper = this._helper.GetService<IMapper>();
            this._repository = this._helper.GetService<Mock<ILeaseService>>();
        }

        #region Tests
        #region GetProperties
        /// <summary>
        /// Make a successful request.
        /// </summary>
        [Fact]
        public void UpdateLeaseStakeholders_Success()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);

            this._repository.Setup(m => m.UpdateStakeholdersByLeaseId(It.IsAny<long>(), It.IsAny<ICollection<Pims.Dal.Entities.PimsLeaseStakeholder>>())).Returns(lease.PimsLeaseStakeholders);

            // Act
            var result = this._controller.UpdateStakeholders(lease.LeaseId, this._mapper.Map<IEnumerable<LeaseStakeholderModel>>(new List<LeaseStakeholderModel>()));

            // Assert
            this._repository.Verify(m => m.UpdateStakeholdersByLeaseId(It.IsAny<long>(), It.IsAny<ICollection<Pims.Dal.Entities.PimsLeaseStakeholder>>()), Times.Once());
        }
        #endregion
        #endregion
    }
}
