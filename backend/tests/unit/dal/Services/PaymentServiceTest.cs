using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Services;
using Pims.Dal.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;
using Entity = Pims.Dal.Entities;
using Moq;

namespace Pims.Dal.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("area", "admin")]
    [Trait("group", "lease")]
    [ExcludeFromCodeCoverage]
    public class LeaseServicePaymentTest: IDisposable
    {
        public TestHelper helper;
        public ILeasePaymentService paymentService;
        public Mock<ILeaseService> leaseService;
        public Mock<Repositories.ILeaseTermRepository> leaseTermRepository;
        public Mock<Repositories.ILeasePaymentRepository> leasePaymentRepository;

        public LeaseServicePaymentTest() {
            helper = new TestHelper();
        }

        public void Dispose()
        {
            leaseService = null;
            leaseTermRepository = null;
            leasePaymentRepository = null;
        }

        private  void MockCommonServices()
        {
            paymentService = helper.Create<LeasePaymentService>();
            leaseService = helper.GetService<Mock<ILeaseService>>();
            leaseTermRepository = helper.GetService<Mock<Repositories.ILeaseTermRepository>>();
            leasePaymentRepository = helper.GetService<Mock<Repositories.ILeasePaymentRepository>>();
        }

        #region Tests
        #region Add
        [Fact]
        public void AddPayment()
        {
            // Arrange
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, TermExpiryDate = DateTime.Now.AddDays(10) };

            MockCommonServices();
            leaseService.Setup(x => x.IsRowVersionEqual(It.IsAny<long>(), It.IsAny<long>())).Returns(true);
            leaseService.Setup(x => x.GetById(It.IsAny<long>())).Returns(lease);
            leaseTermRepository.Setup(x => x.GetById(It.IsAny<long>(), true)).Returns(term);

            // Act
            var payment = new PimsLeasePayment() { PaymentReceivedDate = DateTime.Now };

            var updatedLease = paymentService.AddPayment(lease.Id, 1, payment);

            // Assert
            leasePaymentRepository.Verify(x => x.Add(payment), Times.Once);
            leaseService.Verify(x => x.GetById(lease.Id), Times.Once);
        }

        [Fact]
        public void AddPayment_StatusPaymentType()
        {
            // Arrange
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, TermExpiryDate = DateTime.Now.AddDays(10), GstAmount = 1, PaymentAmount = 1 };

            MockCommonServices();
            leaseService.Setup(x => x.IsRowVersionEqual(It.IsAny<long>(), It.IsAny<long>())).Returns(true);
            leaseService.Setup(x => x.GetById(It.IsAny<long>())).Returns(lease);
            leaseTermRepository.Setup(x => x.GetById(It.IsAny<long>(), true)).Returns(term);

            // Act
            var unpaidPayment = new PimsLeasePayment() { PaymentReceivedDate = DateTime.Now, PaymentAmountTotal = 0,  };
            var overpaidPayment = new PimsLeasePayment() { PaymentReceivedDate = DateTime.Now, PaymentAmountTotal = 3 };
            var paidPayment = new PimsLeasePayment() { PaymentReceivedDate = DateTime.Now, PaymentAmountTotal = 2 };
            var partialPayment = new PimsLeasePayment() { PaymentReceivedDate = DateTime.Now, PaymentAmountTotal = 1 };

            paymentService.AddPayment(lease.Id, 1, unpaidPayment);
            paymentService.AddPayment(lease.Id, 1, overpaidPayment);
            paymentService.AddPayment(lease.Id, 1, paidPayment);
            paymentService.AddPayment(lease.Id, 1, partialPayment);

            // Assert
            leasePaymentRepository.Verify(x => x.Add(It.Is<PimsLeasePayment>(x => x.LeasePaymentStatusTypeCode == PimsLeaseStatusTypes.UNPAID && x.PaymentAmountTotal == 0)));
            leasePaymentRepository.Verify(x => x.Add(It.Is<PimsLeasePayment>(x => x.LeasePaymentStatusTypeCode == PimsLeaseStatusTypes.OVERPAID && x.PaymentAmountTotal == 3)));
            leasePaymentRepository.Verify(x => x.Add(It.Is<PimsLeasePayment>(x => x.LeasePaymentStatusTypeCode == PimsLeaseStatusTypes.PAID && x.PaymentAmountTotal == 2)));
            leasePaymentRepository.Verify(x => x.Add(It.Is<PimsLeasePayment>(x => x.LeasePaymentStatusTypeCode == PimsLeaseStatusTypes.PARTIAL && x.PaymentAmountTotal == 1)));
        }

        [Fact]
        public void AddPayment_NotAuthorized()
        {
            // Arrange
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);
            MockCommonServices();

            // Act
            var payment = new PimsLeasePayment() { PaymentReceivedDate = DateTime.Now };

            Assert.Throws<NotAuthorizedException>(() => paymentService.AddPayment(lease.Id, 1, payment));
        }

        [Fact]
        public void AddPayment_InvalidRowVersion()
        {
            // Arrange
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseView, Permissions.LeaseEdit);

            
            var lease = EntityHelper.CreateLease(1);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);
            MockCommonServices();

            // Act
            var payment = new PimsLeasePayment() { PaymentReceivedDate = DateTime.Now };

            Assert.Throws<DbUpdateConcurrencyException>(() => paymentService.AddPayment(lease.Id, 1, payment));
        }

        [Fact]
        public void AddPayment_ReceivedDateOutOfRange()
        {
            // Arrange
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit);

            var lease = EntityHelper.CreateLease(1);
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, TermExpiryDate = DateTime.Now.AddDays(10) };
            helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);

            MockCommonServices();
            leaseService.Setup(x => x.IsRowVersionEqual(It.IsAny<long>(), It.IsAny<long>())).Returns(true);
            leaseService.Setup(x => x.GetById(It.IsAny<long>())).Returns(lease);
            leaseTermRepository.Setup(x => x.GetById(It.IsAny<long>(), true)).Returns(term);

            // Act
            var addPayment = new PimsLeasePayment() { PaymentReceivedDate = DateTime.Now.AddDays(30) };

            var ex = Assert.Throws<InvalidOperationException>(() => paymentService.AddPayment(lease.Id, 1, addPayment));
            ex.Message.Should().Be("Payment received date must be within the start and expiry date of the term.");
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
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, TermExpiryDate = DateTime.Now.AddDays(10) };
            helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);

            MockCommonServices();
            leaseService.Setup(x => x.IsRowVersionEqual(It.IsAny<long>(), It.IsAny<long>())).Returns(true);
            leaseService.Setup(x => x.GetById(It.IsAny<long>())).Returns(lease);
            leaseTermRepository.Setup(x => x.GetById(It.IsAny<long>(), true)).Returns(term);

            // Act
            var payment = new PimsLeasePayment() { LeasePaymentId = originalPayment.LeasePaymentId, PaymentReceivedDate = DateTime.Now };

            var updatedLease = paymentService.UpdatePayment(lease.Id, 1, 1, payment);

            // Assert
            leasePaymentRepository.Verify(x => x.Update(payment), Times.Once);
            leaseService.Verify(x => x.GetById(lease.Id), Times.Once);
        }

        [Fact]
        public void UpdatePayment_NotAuthorized()
        {
            // Arrange
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);
            MockCommonServices();

            // Act
            var payment = new PimsLeasePayment() { PaymentReceivedDate = DateTime.Now };

            Assert.Throws<NotAuthorizedException>(() => paymentService.UpdatePayment(lease.Id, 1, 1, payment));
        }

        [Fact]
        public void UpdatePayment_InvalidRowVersion()
        {
            // Arrange
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseView, Permissions.LeaseEdit);

            var lease = EntityHelper.CreateLease(1);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);
            MockCommonServices();

            // Act
            var payment = new PimsLeasePayment() { PaymentReceivedDate = DateTime.Now };

            Assert.Throws<DbUpdateConcurrencyException>(() => paymentService.UpdatePayment(lease.Id, 1, 1, payment));
        }

        [Fact]
        public void UpdatePayment_ReceivedDateOutOfRange()
        {
            // Arrange
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit);

            var lease = EntityHelper.CreateLease(1);
            var originalPayment = new PimsLeasePayment() { PaymentReceivedDate = DateTime.Now };
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, TermExpiryDate = DateTime.Now.AddDays(10) };
            helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);

            MockCommonServices();
            leaseService.Setup(x => x.IsRowVersionEqual(It.IsAny<long>(), It.IsAny<long>())).Returns(true);
            leaseTermRepository.Setup(x => x.GetById(It.IsAny<long>(), It.IsAny<bool>())).Returns(term);

            // Act
            var payment = new PimsLeasePayment() { LeasePaymentId = originalPayment.LeasePaymentId, PaymentReceivedDate = DateTime.Now.AddDays(30) };

            var ex = Assert.Throws<InvalidOperationException>(() => paymentService.UpdatePayment(lease.Id, 1, 1, payment));
            ex.Message.Should().Be("Payment received date must be within the start and expiry date of the term.");
        }
        #endregion

        #region Delete
        [Fact]
        public void DeletePayment()
        {
            // Arrange
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, TermExpiryDate = DateTime.Now.AddDays(10) };
            helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);

            MockCommonServices();
            leaseService.Setup(x => x.IsRowVersionEqual(It.IsAny<long>(), It.IsAny<long>())).Returns(true);

            // Act
            var payment = new PimsLeasePayment();

            paymentService.DeletePayment(lease.Id, 1, payment);

            // Assert
            leasePaymentRepository.Verify(x => x.Delete(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void DeletePayment_NotAuthorized()
        {
            // Arrange
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);
            MockCommonServices();

            // Act
            var payment = new PimsLeasePayment();

            Assert.Throws<NotAuthorizedException>(() => paymentService.DeletePayment(lease.Id, 1, payment));
        }

        [Fact]
        public void DeletePayment_InvalidRowVersion()
        {
            // Arrange
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseView, Permissions.LeaseEdit);

            var lease = EntityHelper.CreateLease(1);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);
            MockCommonServices();

            // Act
            var payment = new PimsLeasePayment();

            Assert.Throws<DbUpdateConcurrencyException>(() => paymentService.DeletePayment(lease.Id, 1, payment));
        }
        #endregion
        #endregion
    }
}
