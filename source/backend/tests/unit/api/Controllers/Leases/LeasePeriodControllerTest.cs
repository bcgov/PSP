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
    public class LeasePeriodControllerTest
    {
        private Mock<ILeasePeriodService> _service;
        private LeasePeriodController _controller;
        private IMapper _mapper;
        private TestHelper _helper;

        public LeasePeriodControllerTest()
        {
            this._helper = new TestHelper();
            this._controller = this._helper.CreateController<LeasePeriodController>(Permissions.LeaseView);
            this._mapper = this._helper.GetService<IMapper>();
            this._service = this._helper.GetService<Mock<ILeasePeriodService>>();
        }

        #region Tests
        /// <summary>
        /// Make a successful request.
        /// </summary>
        [Fact]
        public void UpdateLeasePeriods_Success()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            var leasePeriod = new Dal.Entities.PimsLeasePeriod() { LeasePeriodId = 1 };

            this._service.Setup(m => m.UpdatePeriod(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<Pims.Dal.Entities.PimsLeasePeriod>())).Returns(leasePeriod);

            // Act
            var result = this._controller.UpdatePeriod(lease.LeaseId, leasePeriod.LeasePeriodId, this._mapper.Map<LeasePeriodModel>(leasePeriod));

            // Assert
            this._service.Verify(m => m.UpdatePeriod(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<Pims.Dal.Entities.PimsLeasePeriod>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request.
        /// </summary>
        [Fact]
        public void DeleteLeasePeriods_Success()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            var leasePeriod = new Dal.Entities.PimsLeasePeriod();

            this._service.Setup(m => m.DeletePeriod(It.IsAny<long>(), It.IsAny<Pims.Dal.Entities.PimsLeasePeriod>())).Returns(true);

            // Act
            var result = this._controller.DeletePeriod(lease.LeaseId, this._mapper.Map<LeasePeriodModel>(leasePeriod));

            // Assert
            this._service.Verify(m => m.DeletePeriod(It.IsAny<long>(), It.IsAny<Pims.Dal.Entities.PimsLeasePeriod>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request.
        /// </summary>
        [Fact]
        public void AddLeasePeriods_Success()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            var leasePeriod = new Dal.Entities.PimsLeasePeriod();

            this._service.Setup(m => m.AddPeriod(It.IsAny<long>(), It.IsAny<Pims.Dal.Entities.PimsLeasePeriod>())).Returns(leasePeriod);

            // Act
            var result = this._controller.AddPeriod(lease.LeaseId, this._mapper.Map<LeasePeriodModel>(leasePeriod));

            // Assert
            this._service.Verify(m => m.AddPeriod(It.IsAny<long>(), It.IsAny<Pims.Dal.Entities.PimsLeasePeriod>()), Times.Once());
        }
        #endregion
    }
}
