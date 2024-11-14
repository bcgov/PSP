using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Moq;
using Pims.Api.Models.CodeTypes;
using Pims.Api.Services;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;
using Pims.Core.Security;
using Xunit;
using static Pims.Dal.Entities.PimsLeasePaymentStatusType;

namespace Pims.Api.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("area", "admin")]
    [Trait("group", "lease")]
    [ExcludeFromCodeCoverage]
    public class LeaseServicePaymentTest : IDisposable
    {
        public TestHelper helper;
        public ILeasePaymentService paymentService;
        public Mock<ILeasePeriodRepository> LeasePeriodRepository;
        public Mock<ILeasePaymentRepository> leasePaymentRepository;

        public LeaseServicePaymentTest()
        {
            this.helper = new TestHelper();
        }

        public void Dispose()
        {
            this.LeasePeriodRepository = null;
            this.leasePaymentRepository = null;
        }

        private void MockCommonServices()
        {
            this.paymentService = this.helper.Create<LeasePaymentService>();
            this.LeasePeriodRepository = this.helper.GetService<Mock<ILeasePeriodRepository>>();
            this.leasePaymentRepository = this.helper.GetService<Mock<ILeasePaymentRepository>>();
        }

        #region Tests
        #region Add
        [Fact]
        public void AddPayment()
        {
            // Arrange
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            this.helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);
            var period = new PimsLeasePeriod() { PeriodStartDate = DateTime.Now, PeriodExpiryDate = DateTime.Now.AddDays(10) };

            this.MockCommonServices();
            this.LeasePeriodRepository.Setup(x => x.GetById(It.IsAny<long>(), true)).Returns(period);

            // Act
            var payment = new PimsLeasePayment() { PaymentReceivedDate = DateTime.Now, LeasePaymentCategoryTypeCode = LeasePaymentCategoryTypes.BASE.ToString() };

            var updatedLease = this.paymentService.AddPayment(lease.Internal_Id, payment);

            // Assert
            this.leasePaymentRepository.Verify(x => x.Add(payment), Times.Once);
        }

        [Fact]
        public void AddPayment_StatusPaymentType()
        {
            // Arrange
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            this.helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);
            var period = new PimsLeasePeriod() { PeriodStartDate = DateTime.Now, PeriodExpiryDate = DateTime.Now.AddDays(10), GstAmount = 1, PaymentAmount = 1 };

            this.MockCommonServices();
            this.LeasePeriodRepository.Setup(x => x.GetById(It.IsAny<long>(), true)).Returns(period);

            // Act
            var unpaidPayment = new PimsLeasePayment() { PaymentReceivedDate = DateTime.Now, PaymentAmountTotal = 0, LeasePaymentCategoryTypeCode = LeasePaymentCategoryTypes.BASE.ToString() };
            var overpaidPayment = new PimsLeasePayment() { PaymentReceivedDate = DateTime.Now, PaymentAmountTotal = 3, LeasePaymentCategoryTypeCode = LeasePaymentCategoryTypes.BASE.ToString() };
            var paidPayment = new PimsLeasePayment() { PaymentReceivedDate = DateTime.Now, PaymentAmountTotal = 2, LeasePaymentCategoryTypeCode = LeasePaymentCategoryTypes.BASE.ToString() };
            var partialPayment = new PimsLeasePayment() { PaymentReceivedDate = DateTime.Now, PaymentAmountTotal = 1, LeasePaymentCategoryTypeCode = LeasePaymentCategoryTypes.BASE.ToString() };

            this.paymentService.AddPayment(lease.Internal_Id, unpaidPayment);
            this.paymentService.AddPayment(lease.Internal_Id, overpaidPayment);
            this.paymentService.AddPayment(lease.Internal_Id, paidPayment);
            this.paymentService.AddPayment(lease.Internal_Id, partialPayment);

            // Assert
            this.leasePaymentRepository.Verify(x => x.Add(It.Is<PimsLeasePayment>(x => x.LeasePaymentStatusTypeCode == PimsLeasePaymentStatusTypes.UNPAID && x.PaymentAmountTotal == 0)));
            this.leasePaymentRepository.Verify(x => x.Add(It.Is<PimsLeasePayment>(x => x.LeasePaymentStatusTypeCode == PimsLeasePaymentStatusTypes.OVERPAID && x.PaymentAmountTotal == 3)));
            this.leasePaymentRepository.Verify(x => x.Add(It.Is<PimsLeasePayment>(x => x.LeasePaymentStatusTypeCode == PimsLeasePaymentStatusTypes.PAID && x.PaymentAmountTotal == 2)));
            this.leasePaymentRepository.Verify(x => x.Add(It.Is<PimsLeasePayment>(x => x.LeasePaymentStatusTypeCode == PimsLeasePaymentStatusTypes.PARTIAL && x.PaymentAmountTotal == 1)));
        }

        [Fact]
        public void AddPayment_ReceivedDateOutOfRange()
        {
            // Arrange
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit);

            var lease = EntityHelper.CreateLease(1);
            var period = new PimsLeasePeriod() { PeriodStartDate = DateTime.Now, PeriodExpiryDate = DateTime.Now.AddDays(10) };
            this.helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);

            this.MockCommonServices();
            this.LeasePeriodRepository.Setup(x => x.GetById(It.IsAny<long>(), true)).Returns(period);

            // Act
            var addPayment = new PimsLeasePayment() { PaymentReceivedDate = DateTime.Now.AddDays(30) };

            var result = this.paymentService.AddPayment(lease.Internal_Id, addPayment);
            this.leasePaymentRepository.Verify(x => x.Add(It.IsAny<PimsLeasePayment>()));
        }
        #endregion

        #region Update
        [Fact]
        public void UpdatePayment()
        {
            // Arrange
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            var originalPayment = new PimsLeasePayment() { PaymentReceivedDate = DateTime.Now };
            var period = new PimsLeasePeriod() { PeriodStartDate = DateTime.Now, PeriodExpiryDate = DateTime.Now.AddDays(10) };
            this.helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);

            this.MockCommonServices();
            this.LeasePeriodRepository.Setup(x => x.GetById(It.IsAny<long>(), true)).Returns(period);

            // Act
            var payment = new PimsLeasePayment() { LeasePaymentId = originalPayment.LeasePaymentId, PaymentReceivedDate = DateTime.Now, LeasePaymentCategoryTypeCode = LeasePaymentCategoryTypes.BASE.ToString() };

            var updatedLease = this.paymentService.UpdatePayment(lease.Internal_Id, 1, payment);

            // Assert
            this.leasePaymentRepository.Verify(x => x.Update(payment), Times.Once);
        }

        [Fact]
        public void UpdatePayment_Variable_Gst()
        {
            // Arrange
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            var originalPayment = new PimsLeasePayment() { PaymentReceivedDate = DateTime.Now };
            var period = new PimsLeasePeriod() { PeriodStartDate = DateTime.Now, PeriodExpiryDate = DateTime.Now.AddDays(10), VblRentGstAmount = 1 };
            this.helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);

            this.MockCommonServices();
            this.LeasePeriodRepository.Setup(x => x.GetById(It.IsAny<long>(), true)).Returns(period);
            PimsLeasePayment response = null;
            this.leasePaymentRepository.Setup(x => x.Update(It.IsAny<PimsLeasePayment>())).Callback<PimsLeasePayment>(x => response = x);

            // Act
            var payment = new PimsLeasePayment() { LeasePaymentId = originalPayment.LeasePaymentId, PaymentReceivedDate = DateTime.Now, LeasePaymentCategoryTypeCode = LeasePaymentCategoryTypes.VBL.ToString(), PaymentAmountTotal = 1 };

            var updatedLease = this.paymentService.UpdatePayment(lease.Internal_Id, 1, payment);

            // Assert
            response.LeasePaymentStatusTypeCode.Should().Be(PimsLeasePaymentStatusTypes.PAID);
        }

        [Fact]
        public void UpdatePayment_Addition_Gst()
        {
            // Arrange
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            var originalPayment = new PimsLeasePayment() { PaymentReceivedDate = DateTime.Now };
            var period = new PimsLeasePeriod() { PeriodStartDate = DateTime.Now, PeriodExpiryDate = DateTime.Now.AddDays(10), AddlRentGstAmount = 1 };
            this.helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);

            this.MockCommonServices();
            this.LeasePeriodRepository.Setup(x => x.GetById(It.IsAny<long>(), true)).Returns(period);
            PimsLeasePayment response = null;
            this.leasePaymentRepository.Setup(x => x.Update(It.IsAny<PimsLeasePayment>())).Callback<PimsLeasePayment>(x => response = x);

            // Act
            var payment = new PimsLeasePayment() { LeasePaymentId = originalPayment.LeasePaymentId, PaymentReceivedDate = DateTime.Now, LeasePaymentCategoryTypeCode = LeasePaymentCategoryTypes.ADDL.ToString(), PaymentAmountTotal = 1 };

            var updatedLease = this.paymentService.UpdatePayment(lease.Internal_Id, 1, payment);

            // Assert
            response.LeasePaymentStatusTypeCode.Should().Be(PimsLeasePaymentStatusTypes.PAID);
        }

        [Fact]
        public void UpdatePayment_ReceivedDateOutOfRange()
        {
            // Arrange
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit);

            var lease = EntityHelper.CreateLease(1);
            var originalPayment = new PimsLeasePayment() { PaymentReceivedDate = DateTime.Now };
            var period = new PimsLeasePeriod() { PeriodStartDate = DateTime.Now, PeriodExpiryDate = DateTime.Now.AddDays(10) };
            this.helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);

            this.MockCommonServices();
            this.LeasePeriodRepository.Setup(x => x.GetById(It.IsAny<long>(), It.IsAny<bool>())).Returns(period);

            // Act
            var payment = new PimsLeasePayment() { LeasePaymentId = originalPayment.LeasePaymentId, PaymentReceivedDate = DateTime.Now.AddDays(30) };

            var result = this.paymentService.UpdatePayment(lease.Internal_Id, 1, payment);
            this.leasePaymentRepository.Verify(x => x.Update(It.IsAny<PimsLeasePayment>()));
        }
        #endregion

        #region Delete
        [Fact]
        public void DeletePayment()
        {
            // Arrange
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            var period = new PimsLeasePeriod() { PeriodStartDate = DateTime.Now, PeriodExpiryDate = DateTime.Now.AddDays(10) };
            this.helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);

            this.MockCommonServices();

            // Act
            var payment = new PimsLeasePayment();

            this.paymentService.DeletePayment(lease.Internal_Id, payment);

            // Assert
            this.leasePaymentRepository.Verify(x => x.Delete(It.IsAny<long>()), Times.Once);
        }
        #endregion
        #endregion
    }
}
