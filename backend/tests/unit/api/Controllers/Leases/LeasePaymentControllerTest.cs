using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Lease.Controllers;
using Pims.Core.Comparers;
using Pims.Core.Test;
using Pims.Dal;
using Pims.Dal.Security;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using System.Collections.Generic;
using Model = Pims.Api.Areas.Lease.Models.Lease;
using Pims.Dal.Services;

namespace Pims.Api.Test.Controllers.Lease
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "lease")]
    [ExcludeFromCodeCoverage]
    public class LeasePaymentControllerTest
    {
        #region Tests
        /// <summary>
        /// Make a successful request.
        /// </summary>
        [Fact]
        public void UpdateLeasePayments_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<LeasePaymentController>(Permissions.LeaseEdit);

            var lease = EntityHelper.CreateLease(1);
            var leasePayment = new Dal.Entities.PimsLeasePayment() { LeasePaymentId = 1};

            var service = helper.GetService<Mock<IPimsService>>();
            var mapper = helper.GetService<IMapper>();

            service.Setup(m => m.LeasePaymentService.UpdatePayment(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<long>(), It.IsAny<Pims.Dal.Entities.PimsLeasePayment>())).Returns(lease);

            // Act
            var result = controller.UpdatePayment(lease.LeaseId, leasePayment.LeasePaymentId, mapper.Map<Model.PaymentModel>(lease));

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<Model.LeaseModel>(actionResult.Value);
            var expectedResult = mapper.Map<Model.LeaseModel>(lease);
            Assert.Equal(expectedResult, actualResult, new DeepPropertyCompare());
            service.Verify(m => m.LeasePaymentService.UpdatePayment(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<long>(), It.IsAny<Pims.Dal.Entities.PimsLeasePayment>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request.
        /// </summary>
        [Fact]
        public void DeleteLeasePayments_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<LeasePaymentController>(Permissions.LeaseEdit);

            var lease = EntityHelper.CreateLease(1);
            var leasePayment = new Dal.Entities.PimsLeasePayment();

            var service = helper.GetService<Mock<IPimsService>>();
            var mapper = helper.GetService<IMapper>();

            service.Setup(m => m.LeasePaymentService.DeletePayment(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<Pims.Dal.Entities.PimsLeasePayment>())).Returns(lease);

            // Act
            var result = controller.DeletePayment(lease.LeaseId, mapper.Map<Model.PaymentModel>(lease));

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<Model.LeaseModel>(actionResult.Value);
            var expectedResult = mapper.Map<Model.LeaseModel>(lease);
            Assert.Equal(expectedResult, actualResult, new DeepPropertyCompare());
            service.Verify(m => m.LeasePaymentService.DeletePayment(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<Pims.Dal.Entities.PimsLeasePayment>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request.
        /// </summary>
        [Fact]
        public void AddLeasePayments_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<LeasePaymentController>(Permissions.LeaseEdit);

            var lease = EntityHelper.CreateLease(1);
            var leasePayment = new Dal.Entities.PimsLeasePayment();

            var service = helper.GetService<Mock<IPimsService>>();
            var mapper = helper.GetService<IMapper>();

            service.Setup(m => m.LeasePaymentService.AddPayment(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<Pims.Dal.Entities.PimsLeasePayment>())).Returns(lease);

            // Act
            var result = controller.AddPayment(lease.LeaseId, mapper.Map<Model.PaymentModel>(lease));

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<Model.LeaseModel>(actionResult.Value);
            var expectedResult = mapper.Map<Model.LeaseModel>(lease);
            Assert.Equal(expectedResult, actualResult, new DeepPropertyCompare());
            service.Verify(m => m.LeasePaymentService.AddPayment(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<Pims.Dal.Entities.PimsLeasePayment>()), Times.Once());
        }
        #endregion
    }
}
