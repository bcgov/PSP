using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Lease.Controllers;
using Pims.Api.Services;
using Pims.Core.Test;
using Pims.Dal;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Pims.Dal.Services;
using Xunit;
using Model = Pims.Api.Areas.Lease.Models.Lease;

namespace Pims.Api.Test.Controllers.Lease
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "lease")]
    [ExcludeFromCodeCoverage]
    public class LeasePaymentControllerTest
    {
        private Mock<ILeasePaymentService> _service;
        private LeasePaymentController _controller;
        private IMapper _mapper;
        private TestHelper _helper;

        public LeasePaymentControllerTest()
        {
            _helper = new TestHelper();
            _controller = _helper.CreateController<LeasePaymentController>(Permissions.LeaseView);
            _mapper = _helper.GetService<IMapper>();
            _service = _helper.GetService<Mock<ILeasePaymentService>>();
        }

        #region Tests
        /// <summary>
        /// Make a successful request.
        /// </summary>
        [Fact]
        public void UpdateLeasePayments_Success()
        {
            // Arrange

            var lease = EntityHelper.CreateLease(1);
            var leasePayment = new Dal.Entities.PimsLeasePayment() { LeasePaymentId = 1 };

            _service.Setup(m => m.UpdatePayment(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<long>(), It.IsAny<Pims.Dal.Entities.PimsLeasePayment>())).Returns(lease);

            // Act
            var result = _controller.UpdatePayment(lease.LeaseId, leasePayment.LeasePaymentId, _mapper.Map<Model.PaymentModel>(lease));

            // Assert
            _service.Verify(m => m.UpdatePayment(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<long>(), It.IsAny<Pims.Dal.Entities.PimsLeasePayment>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request.
        /// </summary>
        [Fact]
        public void DeleteLeasePayments_Success()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            var leasePayment = new Dal.Entities.PimsLeasePayment();

            _service.Setup(m => m.DeletePayment(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<Pims.Dal.Entities.PimsLeasePayment>())).Returns(lease);

            // Act
            var result = _controller.DeletePayment(lease.LeaseId, _mapper.Map<Model.PaymentModel>(lease));

            // Assert
            _service.Verify(m => m.DeletePayment(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<Pims.Dal.Entities.PimsLeasePayment>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request.
        /// </summary>
        [Fact]
        public void AddLeasePayments_Success()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            var leasePayment = new Dal.Entities.PimsLeasePayment();

            _service.Setup(m => m.AddPayment(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<Pims.Dal.Entities.PimsLeasePayment>())).Returns(lease);

            // Act
            var result = _controller.AddPayment(lease.LeaseId, _mapper.Map<Model.PaymentModel>(lease));

            // Assert
            _service.Verify(m => m.AddPayment(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<Pims.Dal.Entities.PimsLeasePayment>()), Times.Once());
        }
        #endregion
    }
}
