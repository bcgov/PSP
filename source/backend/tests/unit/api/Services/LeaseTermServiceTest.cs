using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Pims.Api.Services;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Pims.Dal.Services;
using Xunit;

namespace Pims.Api.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("area", "admin")]
    [Trait("group", "lease")]
    [ExcludeFromCodeCoverage]
    public class LeaseServiceTermTest
    {

        private TestHelper _helper;

        public LeaseServiceTermTest()
        {
            _helper = new TestHelper();
        }

        private LeaseTermService CreateLeaseServiceTermWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            _helper.CreatePimsContext(user, true);
            return _helper.Create<LeaseTermService>();
        }

        #region Tests
        #region Add
        [Fact]
        public void AddTerm()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);

            var service = CreateLeaseServiceTermWithPermissions(Permissions.LeaseEdit, Permissions.LeaseView);
            var leaseService = _helper.GetService<Mock<ILeaseService>>();
            var leaseRepository = _helper.GetService<Mock<ILeaseRepository>>();
            var leaseTermRepository = _helper.GetService<Mock<ILeaseTermRepository>>();
            leaseService.Setup(x => x.IsRowVersionEqual(It.IsAny<long>(), It.IsAny<long>())).Returns(true);
            leaseRepository.Setup(x => x.Get(It.IsAny<long>())).Returns(lease);

            // Act
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Id, Lease = lease };

            var updatedLease = service.AddTerm(lease.Id, 1, term);

            // Assert
            leaseTermRepository.Verify(x => x.Add(term), Times.Once);
        }

        [Fact]
        public void AddTerm_NotAuthorized()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);

            var service = CreateLeaseServiceTermWithPermissions(Permissions.LeaseView);

            // Act
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Id, Lease = lease };

            Assert.Throws<NotAuthorizedException>(() => service.AddTerm(lease.Id, 1, term));
        }

        [Fact]
        public void AddTerm_InvalidRowVersion()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);

            var service = CreateLeaseServiceTermWithPermissions(Permissions.LeaseEdit, Permissions.LeaseView);

            // Act
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Id, Lease = lease };

            Assert.Throws<DbUpdateConcurrencyException>(() => service.AddTerm(lease.Id, 1, term));
        }

        [Fact]
        public void AddTerm_OverlappingDates()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            var originalTerm = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Id, Lease = lease, Id = 1 };
            lease.PimsLeaseTerms = new List<PimsLeaseTerm>() { originalTerm };

            var service = CreateLeaseServiceTermWithPermissions(Permissions.LeaseEdit);
            var leaseService = _helper.GetService<Mock<ILeaseService>>();
            var leaseTermRepository = _helper.GetService<Mock<ILeaseTermRepository>>();
            leaseService.Setup(x => x.IsRowVersionEqual(It.IsAny<long>(), It.IsAny<long>())).Returns(true);
            leaseTermRepository.Setup(x => x.GetAllByLeaseId(It.IsAny<long>())).Returns(lease.PimsLeaseTerms);

            // Act
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Id, Lease = lease };

            var ex = Assert.Throws<InvalidOperationException>(() => service.AddTerm(lease.Id, 1, term));
            ex.Message.Should().Be("A new term start and end date must not conflict with any existing terms.");
        }

        [Fact]
        public void AddTerm_OverlappingDates_SameStartDateAsEndDate()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            var date = DateTime.Now;
            var originalTerm = new PimsLeaseTerm() { TermStartDate = date, TermExpiryDate = date.AddDays(1), LeaseId = lease.Id, Lease = lease, Id = 1 };
            lease.PimsLeaseTerms = new List<PimsLeaseTerm>() { originalTerm };

            var service = CreateLeaseServiceTermWithPermissions(Permissions.LeaseEdit);
            var leaseService = _helper.GetService<Mock<ILeaseService>>();
            var leaseTermRepository = _helper.GetService<Mock<ILeaseTermRepository>>();
            leaseService.Setup(x => x.IsRowVersionEqual(It.IsAny<long>(), It.IsAny<long>())).Returns(true);
            leaseTermRepository.Setup(x => x.GetAllByLeaseId(It.IsAny<long>())).Returns(lease.PimsLeaseTerms);

            // Act
            var term = new PimsLeaseTerm() { TermStartDate = date.AddDays(1), LeaseId = lease.Id, Lease = lease };

            var ex = Assert.Throws<InvalidOperationException>(() => service.AddTerm(lease.Id, 1, term));
            ex.Message.Should().Be("A new term start and end date must not conflict with any existing terms.");
        }

        [Fact]
        public void AddTerm_OverlappingDates_SameStartDate()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            var originalTerm = new PimsLeaseTerm() { TermStartDate = DateTime.Now, TermExpiryDate = DateTime.Now.AddDays(10), LeaseId = lease.Id, Lease = lease, Id = 1 };
            lease.PimsLeaseTerms = new List<PimsLeaseTerm>() { originalTerm };

            var service = CreateLeaseServiceTermWithPermissions(Permissions.LeaseEdit);
            var leaseService = _helper.GetService<Mock<ILeaseService>>();
            var leaseTermRepository = _helper.GetService<Mock<ILeaseTermRepository>>();
            leaseService.Setup(x => x.IsRowVersionEqual(It.IsAny<long>(), It.IsAny<long>())).Returns(true);
            leaseTermRepository.Setup(x => x.GetAllByLeaseId(It.IsAny<long>())).Returns(lease.PimsLeaseTerms);

            // Act
            var term = new PimsLeaseTerm() { TermStartDate = originalTerm.TermStartDate, TermExpiryDate = originalTerm.TermExpiryDate = originalTerm.TermStartDate.AddDays(1), LeaseId = lease.Id, Lease = lease };

            var ex = Assert.Throws<InvalidOperationException>(() => service.AddTerm(lease.Id, 1, term));
            ex.Message.Should().Be("A new term start and end date must not conflict with any existing terms.");
        }

        [Fact]
        public void AddTerm_PaymentsNotExercised()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            var payment = new PimsLeasePayment();

            var service = CreateLeaseServiceTermWithPermissions(Permissions.LeaseEdit);
            var leaseService = _helper.GetService<Mock<ILeaseService>>();
            var leaseTermRepository = _helper.GetService<Mock<ILeaseTermRepository>>();
            leaseService.Setup(x => x.IsRowVersionEqual(It.IsAny<long>(), It.IsAny<long>())).Returns(true);
            leaseTermRepository.Setup(x => x.GetAllByLeaseId(It.IsAny<long>())).Returns(lease.PimsLeaseTerms);

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
            var lease = EntityHelper.CreateLease(1);
            var originalTerm = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Id, Lease = lease };
            lease.PimsLeaseTerms = new List<PimsLeaseTerm>() { originalTerm };

            var service = CreateLeaseServiceTermWithPermissions(Permissions.LeaseEdit, Permissions.LeaseView);
            var leaseService = _helper.GetService<Mock<ILeaseService>>();
            var leaseRepository = _helper.GetService<Mock<ILeaseRepository>>();
            leaseService.Setup(x => x.IsRowVersionEqual(It.IsAny<long>(), It.IsAny<long>())).Returns(true);
            leaseRepository.Setup(x => x.Get(It.IsAny<long>())).Returns(lease);
            var leaseTermRepository = _helper.GetService<Mock<ILeaseTermRepository>>();
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
            var service = CreateLeaseServiceTermWithPermissions(Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);

            // Act
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Id, Lease = lease };

            Assert.Throws<NotAuthorizedException>(() => service.UpdateTerm(lease.Id, 1, 1, term));
        }

        [Fact]
        public void UpdateTerm_InvalidRowVersion()
        {
            // Arrange
            var service = CreateLeaseServiceTermWithPermissions(Permissions.LeaseEdit, Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);

            // Act
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Id, Lease = lease };

            Assert.Throws<DbUpdateConcurrencyException>(() => service.UpdateTerm(lease.Id, 1, 1, term));
        }

        [Fact]
        public void UpdateTerm_OverlappingDates()
        {
            // Arrange

            var lease = EntityHelper.CreateLease(1);
            var originalTerm = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Id, Lease = lease, Id = 1 };
            lease.PimsLeaseTerms = new List<PimsLeaseTerm>() { originalTerm };

            var service = CreateLeaseServiceTermWithPermissions(Permissions.LeaseEdit);
            var leaseService = _helper.GetService<Mock<ILeaseService>>();
            leaseService.Setup(x => x.IsRowVersionEqual(It.IsAny<long>(), It.IsAny<long>())).Returns(true);
            var leaseTermRepository = _helper.GetService<Mock<ILeaseTermRepository>>();
            leaseTermRepository.Setup(x => x.GetAllByLeaseId(It.IsAny<long>())).Returns(lease.PimsLeaseTerms);

            // Act
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Id, Lease = lease };

            var ex = Assert.Throws<InvalidOperationException>(() => service.UpdateTerm(lease.Id, 1, 1, term));
            ex.Message.Should().Be("A new term start and end date must not conflict with any existing terms.");
        }

        [Fact]
        public void UpdateTerm_PaymentsNotExercised()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            var payment = new PimsLeasePayment();
            var originalTerm = new PimsLeaseTerm() { TermStartDate = DateTime.Now, TermExpiryDate = DateTime.Now, LeaseId = lease.Id, Lease = lease, PimsLeasePayments = new List<PimsLeasePayment>() { payment } };
            lease.PimsLeaseTerms = new List<PimsLeaseTerm>() { originalTerm };

            var service = CreateLeaseServiceTermWithPermissions(Permissions.LeaseEdit);
            var leaseService = _helper.GetService<Mock<ILeaseService>>();
            var leaseTermRepository = _helper.GetService<Mock<ILeaseTermRepository>>();
            leaseService.Setup(x => x.IsRowVersionEqual(It.IsAny<long>(), It.IsAny<long>())).Returns(true);
            leaseTermRepository.Setup(x => x.GetAllByLeaseId(It.IsAny<long>())).Returns(lease.PimsLeaseTerms);
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
            var lease = EntityHelper.CreateLease(1);
            var originalTerm = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Id, Lease = lease };
            lease.PimsLeaseTerms = new List<PimsLeaseTerm>() { originalTerm };

            var service = CreateLeaseServiceTermWithPermissions(Permissions.LeaseEdit);
            var leaseService = _helper.GetService<Mock<ILeaseService>>();
            var leaseRepository = _helper.GetService<Mock<ILeaseRepository>>();
            leaseService.Setup(x => x.IsRowVersionEqual(It.IsAny<long>(), It.IsAny<long>())).Returns(true);
            leaseRepository.Setup(x => x.Get(It.IsAny<long>())).Returns(lease);
            var leaseTermRepository = _helper.GetService<Mock<ILeaseTermRepository>>();
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
            var lease = EntityHelper.CreateLease(1);

            var service = CreateLeaseServiceTermWithPermissions();

            // Act
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Id, Lease = lease };

            Assert.Throws<NotAuthorizedException>(() => service.DeleteTerm(lease.Id, 1, term));
        }

        [Fact]
        public void DeleteTerm_InvalidRowVersion()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);

            var service = CreateLeaseServiceTermWithPermissions(Permissions.LeaseEdit);

            // Act
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Id, Lease = lease };

            Assert.Throws<DbUpdateConcurrencyException>(() => service.DeleteTerm(lease.Id, 1, term));
        }

        [Fact]
        public void DeleteTerm_Payments()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            var payment = new PimsLeasePayment();
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Id, Lease = lease, PimsLeasePayments = new List<PimsLeasePayment>() { payment } };
            lease.PimsLeaseTerms = new List<PimsLeaseTerm>() { term };

            var service = CreateLeaseServiceTermWithPermissions(Permissions.LeaseEdit);
            var leaseService = _helper.GetService<Mock<ILeaseService>>();
            var leaseTermRepository = _helper.GetService<Mock<ILeaseTermRepository>>();
            leaseService.Setup(x => x.IsRowVersionEqual(It.IsAny<long>(), It.IsAny<long>())).Returns(true);
            leaseTermRepository.Setup(x => x.GetAllByLeaseId(It.IsAny<long>())).Returns(lease.PimsLeaseTerms);
            leaseTermRepository.Setup(x => x.GetById(It.IsAny<long>(), It.IsAny<bool>())).Returns(term);

            // Act
            var ex = Assert.Throws<InvalidOperationException>(() => service.DeleteTerm(lease.Id, 1, term));
            ex.Message.Should().Be("A term with payments attached can not be deleted. If you intend to delete this term, you must delete each of the corresponding payments first.");
        }

        [Fact]
        public void DeleteTerm_Exer()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            var payment = new PimsLeasePayment();
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Id, Lease = lease, LeaseTermStatusTypeCode = "EXER" };
            lease.PimsLeaseTerms = new List<PimsLeaseTerm>() { term };

            var service = CreateLeaseServiceTermWithPermissions(Permissions.LeaseEdit);
            var leaseService = _helper.GetService<Mock<ILeaseService>>();
            var leaseTermRepository = _helper.GetService<Mock<ILeaseTermRepository>>();
            leaseService.Setup(x => x.IsRowVersionEqual(It.IsAny<long>(), It.IsAny<long>())).Returns(true);
            leaseTermRepository.Setup(x => x.GetAllByLeaseId(It.IsAny<long>())).Returns(lease.PimsLeaseTerms);
            leaseTermRepository.Setup(x => x.GetById(It.IsAny<long>(), It.IsAny<bool>())).Returns(term);

            // Act
            var ex = Assert.Throws<InvalidOperationException>(() => service.DeleteTerm(lease.Id, 1, term));
            ex.Message.Should().Be("Exercised terms cannot be deleted. Remove all payments from this term and set this term to 'Not Exercised' to delete this term.");
        }

        [Fact]
        public void DeleteTerm_Initial()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            var payment = new PimsLeasePayment();
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Id, Lease = lease };
            var term2 = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Id, Lease = lease };
            lease.PimsLeaseTerms = new List<PimsLeaseTerm>() { term, term2 };

            var service = CreateLeaseServiceTermWithPermissions(Permissions.LeaseEdit);
            var leaseService = _helper.GetService<Mock<ILeaseService>>();
            var leaseTermRepository = _helper.GetService<Mock<ILeaseTermRepository>>();
            leaseService.Setup(x => x.IsRowVersionEqual(It.IsAny<long>(), It.IsAny<long>())).Returns(true);
            leaseTermRepository.Setup(x => x.GetAllByLeaseId(It.IsAny<long>())).Returns(lease.PimsLeaseTerms);
            leaseTermRepository.Setup(x => x.GetById(It.IsAny<long>(), It.IsAny<bool>())).Returns(term);

            // Act
            var ex = Assert.Throws<InvalidOperationException>(() => service.DeleteTerm(lease.Id, 1, term));
            ex.Message.Should().Be("You must delete all renewals before deleting the initial term.");
        }
        #endregion
        #endregion
    }
}
