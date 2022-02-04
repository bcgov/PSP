using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;
using Pims.Dal.Security;
using Pims.Dal.Services;
using Xunit;

namespace Pims.Dal.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("area", "admin")]
    [Trait("group", "lease")]
    [ExcludeFromCodeCoverage]
    public class LeaseServiceTermTest
    {

        #region Tests
        #region Add
        [Fact]
        public void AddTerm()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);

            var service = helper.Create<LeaseTermService>();
            var leaseService = helper.GetService<Mock<ILeaseService>>();
            var leaseRepository = helper.GetService<Mock<Repositories.ILeaseRepository>>();
            var leaseTermRepository = helper.GetService<Mock<Repositories.ILeaseTermRepository>>();
            leaseService.Setup(x => x.IsRowVersionEqual(It.IsAny<long>(), It.IsAny<long>())).Returns(true);
            leaseRepository.Setup(x => x.Get(It.IsAny<long>())).Returns(lease);

            // Act
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Id, Lease = lease };

            var updatedLease = service.AddTerm(lease.Id, 1, term);

            // Assert
            leaseTermRepository.Verify(x => x.Add(term), Times.Once);
            leaseRepository.Verify(x => x.Get(lease.Id), Times.Once);
        }

        [Fact]
        public void AddTerm_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);

            var service = helper.Create<LeaseTermService>();

            // Act
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Id, Lease = lease };

            Assert.Throws<NotAuthorizedException>(() => service.AddTerm(lease.Id, 1, term));
        }

        [Fact]
        public void AddTerm_InvalidRowVersion()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseView, Permissions.LeaseEdit);

            var lease = EntityHelper.CreateLease(1);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);

            var service = helper.Create<LeaseTermService>();

            // Act
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Id, Lease = lease };

            Assert.Throws<DbUpdateConcurrencyException>(() => service.AddTerm(lease.Id, 1, term));
        }

        [Fact]
        public void AddTerm_OverlappingDates()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit);

            var lease = EntityHelper.CreateLease(1);
            var originalTerm = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Id, Lease = lease };
            lease.PimsLeaseTerms = new List<PimsLeaseTerm>() { originalTerm };
            helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);

            var service = helper.Create<LeaseTermService>();
            var leaseService = helper.GetService<Mock<ILeaseService>>();
            var leaseTermRepository = helper.GetService<Mock<Repositories.ILeaseTermRepository>>();
            leaseService.Setup(x => x.IsRowVersionEqual(It.IsAny<long>(), It.IsAny<long>())).Returns(true);
            leaseTermRepository.Setup(x => x.GetByLeaseId(It.IsAny<long>())).Returns(lease.PimsLeaseTerms);

            // Act
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Id, Lease = lease };

            var ex = Assert.Throws<InvalidOperationException>(() => service.AddTerm(lease.Id, 1, term));
            ex.Message.Should().Be("A new term start and end date must not conflict with any existing terms.");
        }

        [Fact]
        public void AddTerm_PaymentsNotExercised()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit);

            var lease = EntityHelper.CreateLease(1);
            var payment = new PimsLeasePayment();
            helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);

            var service = helper.Create<LeaseTermService>();
            var leaseService = helper.GetService<Mock<ILeaseService>>();
            var leaseTermRepository = helper.GetService<Mock<Repositories.ILeaseTermRepository>>();
            leaseService.Setup(x => x.IsRowVersionEqual(It.IsAny<long>(), It.IsAny<long>())).Returns(true);
            leaseTermRepository.Setup(x => x.GetByLeaseId(It.IsAny<long>())).Returns(lease.PimsLeaseTerms);

            // Act
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.MaxValue, LeaseId = lease.Id, Lease = lease, LeaseTermStatusTypeCode = "NEXER", PimsLeasePayments = new List<PimsLeasePayment>() { payment } };

            var ex = Assert.Throws<InvalidOperationException>(() => service.AddTerm(lease.Id, 1, term));
            ex.Message.Should().Be("Term must be 'exercised' if payments have been made.");
        }
        #endregion

        #region Update
        [Fact]
        public void UpdateTerm()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            var originalTerm = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Id, Lease = lease };
            lease.PimsLeaseTerms = new List<PimsLeaseTerm>() { originalTerm };
            helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);

            var service = helper.Create<LeaseTermService>();
            var leaseService = helper.GetService<Mock<ILeaseService>>();
            var leaseRepository = helper.GetService<Mock<Repositories.ILeaseRepository>>();
            leaseService.Setup(x => x.IsRowVersionEqual(It.IsAny<long>(), It.IsAny<long>())).Returns(true);
            leaseRepository.Setup(x => x.Get(It.IsAny<long>())).Returns(lease);
            var leaseTermRepository = helper.GetService<Mock<Repositories.ILeaseTermRepository>>();
            leaseTermRepository.Setup(x => x.GetById(It.IsAny<long>(), It.IsAny<bool>())).Returns(originalTerm);

            // Act
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Id, Lease = lease };

            var updatedLease = service.UpdateTerm(lease.Id, 1, 1, term);

            // Assert
            leaseTermRepository.Verify(x => x.Update(term), Times.Once);
            leaseRepository.Verify(x => x.Get(lease.Id), Times.Once);
        }

        [Fact]
        public void UpdateTerm_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);

            var service = helper.Create<LeaseTermService>();

            // Act
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Id, Lease = lease };

            Assert.Throws<NotAuthorizedException>(() => service.UpdateTerm(lease.Id, 1, 1, term));
        }

        [Fact]
        public void UpdateTerm_InvalidRowVersion()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseView, Permissions.LeaseEdit);

            var lease = EntityHelper.CreateLease(1);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);

            var service = helper.Create<LeaseTermService>();

            // Act
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Id, Lease = lease };

            Assert.Throws<DbUpdateConcurrencyException>(() => service.UpdateTerm(lease.Id, 1, 1, term));
        }

        [Fact]
        public void UpdateTerm_OverlappingDates()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit);

            var lease = EntityHelper.CreateLease(1);
            var originalTerm = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Id, Lease = lease };
            lease.PimsLeaseTerms = new List<PimsLeaseTerm>() { originalTerm };
            helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);

            var service = helper.Create<LeaseTermService>();
            var leaseService = helper.GetService<Mock<ILeaseService>>();
            leaseService.Setup(x => x.IsRowVersionEqual(It.IsAny<long>(), It.IsAny<long>())).Returns(true);
            var leaseTermRepository = helper.GetService<Mock<Repositories.ILeaseTermRepository>>();
            leaseTermRepository.Setup(x => x.GetByLeaseId(It.IsAny<long>())).Returns(lease.PimsLeaseTerms);

            // Act
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Id, Lease = lease };

            var ex = Assert.Throws<InvalidOperationException>(() => service.UpdateTerm(lease.Id, 1, 1, term));
            ex.Message.Should().Be("A new term start and end date must not conflict with any existing terms.");
        }

        [Fact]
        public void UpdateTerm_PaymentsNotExercised()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit);

            var lease = EntityHelper.CreateLease(1);
            var payment = new PimsLeasePayment();
            var originalTerm = new PimsLeaseTerm() { TermStartDate = DateTime.Now, TermExpiryDate = DateTime.Now, LeaseId = lease.Id, Lease = lease, PimsLeasePayments = new List<PimsLeasePayment>() { payment } };
            lease.PimsLeaseTerms = new List<PimsLeaseTerm>() { originalTerm };
            helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);

            var service = helper.Create<LeaseTermService>();
            var leaseService = helper.GetService<Mock<ILeaseService>>();
            var leaseTermRepository = helper.GetService<Mock<Repositories.ILeaseTermRepository>>();
            leaseService.Setup(x => x.IsRowVersionEqual(It.IsAny<long>(), It.IsAny<long>())).Returns(true);
            leaseTermRepository.Setup(x => x.GetByLeaseId(It.IsAny<long>())).Returns(lease.PimsLeaseTerms);
            leaseTermRepository.Setup(x => x.GetById(It.IsAny<long>(), It.IsAny<bool>())).Returns(originalTerm);

            // Act
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.MaxValue, LeaseId = lease.Id, Lease = lease, LeaseTermStatusTypeCode = "NEXER" };

            var ex = Assert.Throws<InvalidOperationException>(() => service.UpdateTerm(lease.Id, 1, 1, term));
            ex.Message.Should().Be("Term must be 'exercised' if payments have been made.");
        }
        #endregion

        #region Delete
        [Fact]
        public void DeleteTerm()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            var originalTerm = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Id, Lease = lease };
            lease.PimsLeaseTerms = new List<PimsLeaseTerm>() { originalTerm };
            helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);

            var service = helper.Create<LeaseTermService>();
            var leaseService = helper.GetService<Mock<ILeaseService>>();
            var leaseRepository = helper.GetService<Mock<Repositories.ILeaseRepository>>();
            leaseService.Setup(x => x.IsRowVersionEqual(It.IsAny<long>(), It.IsAny<long>())).Returns(true);
            leaseRepository.Setup(x => x.Get(It.IsAny<long>())).Returns(lease);
            var leaseTermRepository = helper.GetService<Mock<Repositories.ILeaseTermRepository>>();
            leaseTermRepository.Setup(x => x.GetById(It.IsAny<long>(), It.IsAny<bool>())).Returns(originalTerm);

            // Act
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Id, Lease = lease };

            service.DeleteTerm(lease.Id, 1, term);

            // Assert
            leaseTermRepository.Verify(x => x.Delete(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void DeleteTerm_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);

            var service = helper.Create<LeaseTermService>();

            // Act
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Id, Lease = lease };

            Assert.Throws<NotAuthorizedException>(() => service.DeleteTerm(lease.Id, 1, term));
        }

        [Fact]
        public void DeleteTerm_InvalidRowVersion()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseView, Permissions.LeaseEdit);

            var lease = EntityHelper.CreateLease(1);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);

            var service = helper.Create<LeaseTermService>();

            // Act
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Id, Lease = lease };

            Assert.Throws<DbUpdateConcurrencyException>(() => service.DeleteTerm(lease.Id, 1, term));
        }

        [Fact]
        public void DeleteTerm_Payments()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit);

            var lease = EntityHelper.CreateLease(1);
            var payment = new PimsLeasePayment();
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Id, Lease = lease, PimsLeasePayments = new List<PimsLeasePayment>() { payment } };
            lease.PimsLeaseTerms = new List<PimsLeaseTerm>() { term };

            helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);

            var service = helper.Create<LeaseTermService>();
            var leaseService = helper.GetService<Mock<ILeaseService>>();
            var leaseTermRepository = helper.GetService<Mock<Repositories.ILeaseTermRepository>>();
            leaseService.Setup(x => x.IsRowVersionEqual(It.IsAny<long>(), It.IsAny<long>())).Returns(true);
            leaseTermRepository.Setup(x => x.GetByLeaseId(It.IsAny<long>())).Returns(lease.PimsLeaseTerms);
            leaseTermRepository.Setup(x => x.GetById(It.IsAny<long>(), It.IsAny<bool>())).Returns(term);

            // Act
            var ex = Assert.Throws<InvalidOperationException>(() => service.DeleteTerm(lease.Id, 1, term));
            ex.Message.Should().Be("A term with payments attached can not be deleted. If you intend to delete this term, you must delete each of the corresponding payments first.");
        }

        [Fact]
        public void DeleteTerm_Exer()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit);

            var lease = EntityHelper.CreateLease(1);
            var payment = new PimsLeasePayment();
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Id, Lease = lease, LeaseTermStatusTypeCode = "EXER" };
            lease.PimsLeaseTerms = new List<PimsLeaseTerm>() { term };

            helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);

            var service = helper.Create<LeaseTermService>();
            var leaseService = helper.GetService<Mock<ILeaseService>>();
            var leaseTermRepository = helper.GetService<Mock<Repositories.ILeaseTermRepository>>();
            leaseService.Setup(x => x.IsRowVersionEqual(It.IsAny<long>(), It.IsAny<long>())).Returns(true);
            leaseTermRepository.Setup(x => x.GetByLeaseId(It.IsAny<long>())).Returns(lease.PimsLeaseTerms);
            leaseTermRepository.Setup(x => x.GetById(It.IsAny<long>(), It.IsAny<bool>())).Returns(term);

            // Act
            var ex = Assert.Throws<InvalidOperationException>(() => service.DeleteTerm(lease.Id, 1, term));
            ex.Message.Should().Be("Exercised terms cannot be deleted. Remove all payments from this term and set this term to 'Not Exercised' to delete this term.");
        }

        [Fact]
        public void DeleteTerm_Initial()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseEdit);

            var lease = EntityHelper.CreateLease(1);
            var payment = new PimsLeasePayment();
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Id, Lease = lease };
            var term2 = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Id, Lease = lease };
            lease.PimsLeaseTerms = new List<PimsLeaseTerm>() { term, term2 };

            helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);

            var service = helper.Create<LeaseTermService>();
            var leaseService = helper.GetService<Mock<ILeaseService>>();
            var leaseTermRepository = helper.GetService<Mock<Repositories.ILeaseTermRepository>>();
            leaseService.Setup(x => x.IsRowVersionEqual(It.IsAny<long>(), It.IsAny<long>())).Returns(true);
            leaseTermRepository.Setup(x => x.GetByLeaseId(It.IsAny<long>())).Returns(lease.PimsLeaseTerms);
            leaseTermRepository.Setup(x => x.GetById(It.IsAny<long>(), It.IsAny<bool>())).Returns(term);

            // Act
            var ex = Assert.Throws<InvalidOperationException>(() => service.DeleteTerm(lease.Id, 1, term));
            ex.Message.Should().Be("You must delete all renewals before deleting the initial term.");

        }
        #endregion
        #endregion
    }
}
