using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Pims.Dal.Services;
using Xunit;
using static Pims.Dal.Entities.PimsLeasePaymentStatusType;
using Entity = Pims.Dal.Entities;

namespace Pims.Dal.Test.Services
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
        public Mock<ILeaseService> leaseService;
        public Mock<ILeaseTermRepository> leaseTermRepository;
        public Mock<ILeasePaymentRepository> leasePaymentRepository;

        public LeaseServicePaymentTest()
        {
            this.helper = new TestHelper();
        }

        public void Dispose()
        {
            this.leaseService = null;
            this.leaseTermRepository = null;
            this.leasePaymentRepository = null;
        }

        private void MockCommonServices()
        {
            this.paymentService = this.helper.Create<LeasePaymentService>();
            this.leaseService = this.helper.GetService<Mock<ILeaseService>>();
            this.leaseTermRepository = this.helper.GetService<Mock<ILeaseTermRepository>>();
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
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, TermExpiryDate = DateTime.Now.AddDays(10) };

            this.MockCommonServices();
            this.leaseService.Setup(x => x.IsRowVersionEqual(It.IsAny<long>(), It.IsAny<long>())).Returns(true);
            this.leaseService.Setup(x => x.GetById(It.IsAny<long>())).Returns(lease);
            this.leaseTermRepository.Setup(x => x.GetById(It.IsAny<long>(), true)).Returns(term);

            // Act
            var payment = new PimsLeasePayment() { PaymentReceivedDate = DateTime.Now };

            var updatedLease = this.paymentService.AddPayment(lease.Id, 1, payment);

            // Assert
            this.leasePaymentRepository.Verify(x => x.Add(payment), Times.Once);
            this.leaseService.Verify(x => x.GetById(lease.Id), Times.Once);
        }

        [Fact]
        public void AddPayment_StatusPaymentType()
        {
            // Arrange
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            this.helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, TermExpiryDate = DateTime.Now.AddDays(10), GstAmount = 1, PaymentAmount = 1 };

            this.MockCommonServices();
            this.leaseService.Setup(x => x.IsRowVersionEqual(It.IsAny<long>(), It.IsAny<long>())).Returns(true);
            this.leaseService.Setup(x => x.GetById(It.IsAny<long>())).Returns(lease);
            this.leaseTermRepository.Setup(x => x.GetById(It.IsAny<long>(), true)).Returns(term);

            // Act
            var unpaidPayment = new PimsLeasePayment() { PaymentReceivedDate = DateTime.Now, PaymentAmountTotal = 0, };
            var overpaidPayment = new PimsLeasePayment() { PaymentReceivedDate = DateTime.Now, PaymentAmountTotal = 3 };
            var paidPayment = new PimsLeasePayment() { PaymentReceivedDate = DateTime.Now, PaymentAmountTotal = 2 };
            var partialPayment = new PimsLeasePayment() { PaymentReceivedDate = DateTime.Now, PaymentAmountTotal = 1 };

            this.paymentService.AddPayment(lease.Id, 1, unpaidPayment);
            this.paymentService.AddPayment(lease.Id, 1, overpaidPayment);
            this.paymentService.AddPayment(lease.Id, 1, paidPayment);
            this.paymentService.AddPayment(lease.Id, 1, partialPayment);

            // Assert
            this.leasePaymentRepository.Verify(x => x.Add(It.Is<PimsLeasePayment>(x => x.LeasePaymentStatusTypeCode == PimsLeasePaymentStatusTypes.UNPAID && x.PaymentAmountTotal == 0)));
            this.leasePaymentRepository.Verify(x => x.Add(It.Is<PimsLeasePayment>(x => x.LeasePaymentStatusTypeCode == PimsLeasePaymentStatusTypes.OVERPAID && x.PaymentAmountTotal == 3)));
            this.leasePaymentRepository.Verify(x => x.Add(It.Is<PimsLeasePayment>(x => x.LeasePaymentStatusTypeCode == PimsLeasePaymentStatusTypes.PAID && x.PaymentAmountTotal == 2)));
            this.leasePaymentRepository.Verify(x => x.Add(It.Is<PimsLeasePayment>(x => x.LeasePaymentStatusTypeCode == PimsLeasePaymentStatusTypes.PARTIAL && x.PaymentAmountTotal == 1)));
        }

        [Fact]
        public void AddPayment_NotAuthorized()
        {
            // Arrange
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            this.helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);
            this.MockCommonServices();

            // Act
            var payment = new PimsLeasePayment() { PaymentReceivedDate = DateTime.Now };

            Assert.Throws<NotAuthorizedException>(() => this.paymentService.AddPayment(lease.Id, 1, payment));
        }

        [Fact]
        public void AddPayment_InvalidRowVersion()
        {
            // Arrange
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseView, Permissions.LeaseEdit);

            var lease = EntityHelper.CreateLease(1);
            this.helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);
            this.MockCommonServices();

            // Act
            var payment = new PimsLeasePayment() { PaymentReceivedDate = DateTime.Now };

            Assert.Throws<DbUpdateConcurrencyException>(() => this.paymentService.AddPayment(lease.Id, 1, payment));
        }

        [Fact]
        public void AddPayment_ReceivedDateOutOfRange()
        {
            // Arrange
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit);

            var lease = EntityHelper.CreateLease(1);
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, TermExpiryDate = DateTime.Now.AddDays(10) };
            this.helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);

            this.MockCommonServices();
            this.leaseService.Setup(x => x.IsRowVersionEqual(It.IsAny<long>(), It.IsAny<long>())).Returns(true);
            this.leaseService.Setup(x => x.GetById(It.IsAny<long>())).Returns(lease);
            this.leaseTermRepository.Setup(x => x.GetById(It.IsAny<long>(), true)).Returns(term);

            // Act
            var addPayment = new PimsLeasePayment() { PaymentReceivedDate = DateTime.Now.AddDays(30) };

            var ex = Assert.Throws<InvalidOperationException>(() => this.paymentService.AddPayment(lease.Id, 1, addPayment));
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
            this.helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);

            this.MockCommonServices();
            this.leaseService.Setup(x => x.IsRowVersionEqual(It.IsAny<long>(), It.IsAny<long>())).Returns(true);
            this.leaseService.Setup(x => x.GetById(It.IsAny<long>())).Returns(lease);
            this.leaseTermRepository.Setup(x => x.GetById(It.IsAny<long>(), true)).Returns(term);

            // Act
            var payment = new PimsLeasePayment() { LeasePaymentId = originalPayment.LeasePaymentId, PaymentReceivedDate = DateTime.Now };

            var updatedLease = this.paymentService.UpdatePayment(lease.Id, 1, 1, payment);

            // Assert
            this.leasePaymentRepository.Verify(x => x.Update(payment), Times.Once);
            this.leaseService.Verify(x => x.GetById(lease.Id), Times.Once);
        }

        [Fact]
        public void UpdatePayment_NotAuthorized()
        {
            // Arrange
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            this.helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);
            this.MockCommonServices();

            // Act
            var payment = new PimsLeasePayment() { PaymentReceivedDate = DateTime.Now };

            Assert.Throws<NotAuthorizedException>(() => this.paymentService.UpdatePayment(lease.Id, 1, 1, payment));
        }

        [Fact]
        public void UpdatePayment_InvalidRowVersion()
        {
            // Arrange
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseView, Permissions.LeaseEdit);

            var lease = EntityHelper.CreateLease(1);
            this.helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);
            this.MockCommonServices();

            // Act
            var payment = new PimsLeasePayment() { PaymentReceivedDate = DateTime.Now };

            Assert.Throws<DbUpdateConcurrencyException>(() => this.paymentService.UpdatePayment(lease.Id, 1, 1, payment));
        }

        [Fact]
        public void UpdatePayment_ReceivedDateOutOfRange()
        {
            // Arrange
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit);

            var lease = EntityHelper.CreateLease(1);
            var originalPayment = new PimsLeasePayment() { PaymentReceivedDate = DateTime.Now };
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, TermExpiryDate = DateTime.Now.AddDays(10) };
            this.helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);

            this.MockCommonServices();
            this.leaseService.Setup(x => x.IsRowVersionEqual(It.IsAny<long>(), It.IsAny<long>())).Returns(true);
            this.leaseTermRepository.Setup(x => x.GetById(It.IsAny<long>(), It.IsAny<bool>())).Returns(term);

            // Act
            var payment = new PimsLeasePayment() { LeasePaymentId = originalPayment.LeasePaymentId, PaymentReceivedDate = DateTime.Now.AddDays(30) };

            var ex = Assert.Throws<InvalidOperationException>(() => this.paymentService.UpdatePayment(lease.Id, 1, 1, payment));
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
            this.helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);

            this.MockCommonServices();
            this.leaseService.Setup(x => x.IsRowVersionEqual(It.IsAny<long>(), It.IsAny<long>())).Returns(true);

            // Act
            var payment = new PimsLeasePayment();

            this.paymentService.DeletePayment(lease.Id, 1, payment);

            // Assert
            this.leasePaymentRepository.Verify(x => x.Delete(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void DeletePayment_NotAuthorized()
        {
            // Arrange
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseView, Permissions.LeaseDelete);

            var lease = EntityHelper.CreateLease(1);
            this.helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);
            this.MockCommonServices();

            // Act
            var payment = new PimsLeasePayment();

            Assert.Throws<NotAuthorizedException>(() => this.paymentService.DeletePayment(lease.Id, 1, payment));
        }

        [Fact]
        public void DeletePayment_InvalidRowVersion()
        {
            // Arrange
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseView, Permissions.LeaseEdit);

            var lease = EntityHelper.CreateLease(1);
            this.helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);
            this.MockCommonServices();

            // Act
            var payment = new PimsLeasePayment();

            Assert.Throws<DbUpdateConcurrencyException>(() => this.paymentService.DeletePayment(lease.Id, 1, payment));
        }
        #endregion
        #endregion
    }
}
