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
            this._helper = new TestHelper();
        }

        private LeaseTermService CreateLeaseServiceTermWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            this._helper.CreatePimsContext(user, true);
            return this._helper.Create<LeaseTermService>();
        }

        #region Tests
        #region Add
        [Fact]
        public void AddTerm()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);

            var service = this.CreateLeaseServiceTermWithPermissions(Permissions.LeaseEdit, Permissions.LeaseView);
            var leaseTermRepository = this._helper.GetService<Mock<ILeaseTermRepository>>();

            // Act
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Internal_Id, Lease = lease };

            var updatedLease = service.AddTerm(lease.Internal_Id, term);

            // Assert
            leaseTermRepository.Verify(x => x.Add(term), Times.Once);
        }

        [Fact]
        public void AddTerm_OverlappingDates()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            var originalTerm = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Internal_Id, Lease = lease, Internal_Id = 1 };
            lease.PimsLeaseTerms = new List<PimsLeaseTerm>() { originalTerm };

            var service = this.CreateLeaseServiceTermWithPermissions(Permissions.LeaseEdit);
            var leaseTermRepository = this._helper.GetService<Mock<ILeaseTermRepository>>();
            leaseTermRepository.Setup(x => x.GetAllByLeaseId(It.IsAny<long>())).Returns(lease.PimsLeaseTerms);

            // Act
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Internal_Id, Lease = lease };

            var ex = Assert.Throws<InvalidOperationException>(() => service.AddTerm(lease.Internal_Id, term));
            ex.Message.Should().Be("A new term start and end date must not conflict with any existing terms.");
        }

        [Fact]
        public void AddTerm_OverlappingDates_SameStartDateAsEndDate()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            var date = DateTime.Now;
            var originalTerm = new PimsLeaseTerm() { TermStartDate = date, TermExpiryDate = date.AddDays(1), LeaseId = lease.Internal_Id, Lease = lease, Internal_Id = 1 };
            lease.PimsLeaseTerms = new List<PimsLeaseTerm>() { originalTerm };

            var service = this.CreateLeaseServiceTermWithPermissions(Permissions.LeaseEdit);
            var leaseTermRepository = this._helper.GetService<Mock<ILeaseTermRepository>>();
            leaseTermRepository.Setup(x => x.GetAllByLeaseId(It.IsAny<long>())).Returns(lease.PimsLeaseTerms);

            // Act
            var term = new PimsLeaseTerm() { TermStartDate = date.AddDays(1), LeaseId = lease.Internal_Id, Lease = lease };

            var ex = Assert.Throws<InvalidOperationException>(() => service.AddTerm(lease.Internal_Id, term));
            ex.Message.Should().Be("A new term start and end date must not conflict with any existing terms.");
        }

        [Fact]
        public void AddTerm_OverlappingDates_SameStartDate()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            var originalTerm = new PimsLeaseTerm() { TermStartDate = DateTime.Now, TermExpiryDate = DateTime.Now.AddDays(10), LeaseId = lease.Internal_Id, Lease = lease, Internal_Id = 1 };
            lease.PimsLeaseTerms = new List<PimsLeaseTerm>() { originalTerm };

            var service = this.CreateLeaseServiceTermWithPermissions(Permissions.LeaseEdit);
            var leaseTermRepository = this._helper.GetService<Mock<ILeaseTermRepository>>();
            leaseTermRepository.Setup(x => x.GetAllByLeaseId(It.IsAny<long>())).Returns(lease.PimsLeaseTerms);

            // Act
            var term = new PimsLeaseTerm() { TermStartDate = originalTerm.TermStartDate, TermExpiryDate = originalTerm.TermExpiryDate = originalTerm.TermStartDate.AddDays(1), LeaseId = lease.Internal_Id, Lease = lease };

            var ex = Assert.Throws<InvalidOperationException>(() => service.AddTerm(lease.Internal_Id, term));
            ex.Message.Should().Be("A new term start and end date must not conflict with any existing terms.");
        }

        [Fact]
        public void AddTerm_PaymentsNotExercised()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            var payment = new PimsLeasePayment();

            var service = this.CreateLeaseServiceTermWithPermissions(Permissions.LeaseEdit);
            var leaseTermRepository = this._helper.GetService<Mock<ILeaseTermRepository>>();
            leaseTermRepository.Setup(x => x.GetAllByLeaseId(It.IsAny<long>())).Returns(lease.PimsLeaseTerms);

            // Act
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.MaxValue, LeaseId = lease.Internal_Id, Lease = lease, LeaseTermStatusTypeCode = "NEXER", PimsLeasePayments = new List<PimsLeasePayment>() { payment } };

            var ex = Assert.Throws<InvalidOperationException>(() => service.AddTerm(lease.Internal_Id, term));
            ex.Message.Should().Be("Term must be 'exercised' if payments have been made.");
        }
        #endregion

        #region Update
        [Fact]
        public void UpdateTerm()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            var originalTerm = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Internal_Id, Lease = lease };
            lease.PimsLeaseTerms = new List<PimsLeaseTerm>() { originalTerm };

            var service = this.CreateLeaseServiceTermWithPermissions(Permissions.LeaseEdit, Permissions.LeaseView);
            var leaseTermRepository = this._helper.GetService<Mock<ILeaseTermRepository>>();
            leaseTermRepository.Setup(x => x.GetById(It.IsAny<long>(), It.IsAny<bool>())).Returns(originalTerm);

            // Act
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Internal_Id, Lease = lease };

            var updatedLease = service.UpdateTerm(lease.Internal_Id, 1, term);

            // Assert
            leaseTermRepository.Verify(x => x.Update(term), Times.Once);
        }

        [Fact]
        public void UpdateTerm_OverlappingDates()
        {
            // Arrange

            var lease = EntityHelper.CreateLease(1);
            var originalTerm = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Internal_Id, Lease = lease, Internal_Id = 1 };
            lease.PimsLeaseTerms = new List<PimsLeaseTerm>() { originalTerm };

            var service = this.CreateLeaseServiceTermWithPermissions(Permissions.LeaseEdit);
            var leaseTermRepository = this._helper.GetService<Mock<ILeaseTermRepository>>();
            leaseTermRepository.Setup(x => x.GetAllByLeaseId(It.IsAny<long>())).Returns(lease.PimsLeaseTerms);

            // Act
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Internal_Id, Lease = lease };

            var ex = Assert.Throws<InvalidOperationException>(() => service.UpdateTerm(lease.Internal_Id, 1, term));
            ex.Message.Should().Be("A new term start and end date must not conflict with any existing terms.");
        }

        [Fact]
        public void UpdateTerm_PaymentsNotExercised()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            var payment = new PimsLeasePayment();
            var originalTerm = new PimsLeaseTerm() { TermStartDate = DateTime.Now, TermExpiryDate = DateTime.Now, LeaseId = lease.Internal_Id, Lease = lease, PimsLeasePayments = new List<PimsLeasePayment>() { payment } };
            lease.PimsLeaseTerms = new List<PimsLeaseTerm>() { originalTerm };

            var service = this.CreateLeaseServiceTermWithPermissions(Permissions.LeaseEdit);
            var leaseTermRepository = this._helper.GetService<Mock<ILeaseTermRepository>>();
            leaseTermRepository.Setup(x => x.GetAllByLeaseId(It.IsAny<long>())).Returns(lease.PimsLeaseTerms);
            leaseTermRepository.Setup(x => x.GetById(It.IsAny<long>(), It.IsAny<bool>())).Returns(originalTerm);

            // Act
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.MaxValue, LeaseId = lease.Internal_Id, Lease = lease, LeaseTermStatusTypeCode = "NEXER" };

            var ex = Assert.Throws<InvalidOperationException>(() => service.UpdateTerm(lease.Internal_Id, 1, term));
            ex.Message.Should().Be("Term must be 'exercised' if payments have been made.");
        }
        #endregion

        #region Delete
        [Fact]
        public void DeleteTerm()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            var originalTerm = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Internal_Id, Lease = lease };
            lease.PimsLeaseTerms = new List<PimsLeaseTerm>() { originalTerm };

            var service = this.CreateLeaseServiceTermWithPermissions(Permissions.LeaseEdit);
            var leaseTermRepository = this._helper.GetService<Mock<ILeaseTermRepository>>();
            leaseTermRepository.Setup(x => x.GetById(It.IsAny<long>(), It.IsAny<bool>())).Returns(originalTerm);

            // Act
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Internal_Id, Lease = lease };

            service.DeleteTerm(lease.Internal_Id, term);

            // Assert
            leaseTermRepository.Verify(x => x.Delete(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void DeleteTerm_Payments()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            var payment = new PimsLeasePayment();
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Internal_Id, Lease = lease, PimsLeasePayments = new List<PimsLeasePayment>() { payment } };
            lease.PimsLeaseTerms = new List<PimsLeaseTerm>() { term };

            var service = this.CreateLeaseServiceTermWithPermissions(Permissions.LeaseEdit);
            var leaseTermRepository = this._helper.GetService<Mock<ILeaseTermRepository>>();
            leaseTermRepository.Setup(x => x.GetAllByLeaseId(It.IsAny<long>())).Returns(lease.PimsLeaseTerms);
            leaseTermRepository.Setup(x => x.GetById(It.IsAny<long>(), It.IsAny<bool>())).Returns(term);

            // Act
            var ex = Assert.Throws<InvalidOperationException>(() => service.DeleteTerm(lease.Internal_Id, term));
            ex.Message.Should().Be("A term with payments attached can not be deleted. If you intend to delete this term, you must delete each of the corresponding payments first.");
        }

        [Fact]
        public void DeleteTerm_Exer()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            var payment = new PimsLeasePayment();
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Internal_Id, Lease = lease, LeaseTermStatusTypeCode = "EXER" };
            lease.PimsLeaseTerms = new List<PimsLeaseTerm>() { term };

            var service = this.CreateLeaseServiceTermWithPermissions(Permissions.LeaseEdit);
            var leaseTermRepository = this._helper.GetService<Mock<ILeaseTermRepository>>();
            leaseTermRepository.Setup(x => x.GetAllByLeaseId(It.IsAny<long>())).Returns(lease.PimsLeaseTerms);
            leaseTermRepository.Setup(x => x.GetById(It.IsAny<long>(), It.IsAny<bool>())).Returns(term);

            // Act
            var ex = Assert.Throws<InvalidOperationException>(() => service.DeleteTerm(lease.Internal_Id, term));
            ex.Message.Should().Be("Exercised terms cannot be deleted. Remove all payments from this term and set this term to 'Not Exercised' to delete this term.");
        }

        [Fact]
        public void DeleteTerm_Initial()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            var payment = new PimsLeasePayment();
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Internal_Id, Lease = lease };
            var term2 = new PimsLeaseTerm() { TermStartDate = DateTime.Now, LeaseId = lease.Internal_Id, Lease = lease };
            lease.PimsLeaseTerms = new List<PimsLeaseTerm>() { term, term2 };

            var service = this.CreateLeaseServiceTermWithPermissions(Permissions.LeaseEdit);
            var leaseTermRepository = this._helper.GetService<Mock<ILeaseTermRepository>>();
            leaseTermRepository.Setup(x => x.GetAllByLeaseId(It.IsAny<long>())).Returns(lease.PimsLeaseTerms);
            leaseTermRepository.Setup(x => x.GetById(It.IsAny<long>(), It.IsAny<bool>())).Returns(term);

            // Act
            var ex = Assert.Throws<InvalidOperationException>(() => service.DeleteTerm(lease.Internal_Id, term));
            ex.Message.Should().Be("You must delete all renewals before deleting the initial term.");
        }
        #endregion
        #endregion
    }
}
