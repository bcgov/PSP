using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Moq;
using Pims.Api.Services;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
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

        private LeasePeriodService CreateLeaseServicePeriodWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            this._helper.CreatePimsContext(user, true);
            return this._helper.Create<LeasePeriodService>();
        }

        #region Tests
        #region Add
        [Fact]
        public void AddPeriod()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);

            var service = this.CreateLeaseServicePeriodWithPermissions(Permissions.LeaseEdit, Permissions.LeaseView);
            var leasePeriodRepository = this._helper.GetService<Mock<ILeasePeriodRepository>>();

            // Act
            var period = new PimsLeasePeriod() { PeriodStartDate = DateTime.Now, LeaseId = lease.Internal_Id, Lease = lease };

            var updatedLease = service.AddPeriod(lease.Internal_Id, period);

            // Assert
            leasePeriodRepository.Verify(x => x.Add(period), Times.Once);
        }

        [Fact]
        public void AddPeriod_OverlappingDates()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            var originalTerm = new PimsLeasePeriod() { PeriodStartDate = DateTime.Now, LeaseId = lease.Internal_Id, Lease = lease, Internal_Id = 1 };
            lease.PimsLeasePeriods = new List<PimsLeasePeriod>() { originalTerm };

            var service = this.CreateLeaseServicePeriodWithPermissions(Permissions.LeaseEdit);
            var leasePeriodRepository = this._helper.GetService<Mock<ILeasePeriodRepository>>();
            leasePeriodRepository.Setup(x => x.GetAllByLeaseId(It.IsAny<long>())).Returns(lease.PimsLeasePeriods);

            // Act
            var period = new PimsLeasePeriod() { PeriodStartDate = DateTime.Now, LeaseId = lease.Internal_Id, Lease = lease };

            var ex = Assert.Throws<InvalidOperationException>(() => service.AddPeriod(lease.Internal_Id, period));
            ex.Message.Should().Be("A new period start and end date must not conflict with any existing periods.");
        }

        [Fact]
        public void AddPeriod_OverlappingDates_SameStartDateAsEndDate()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            var date = DateTime.Now;
            var originalTerm = new PimsLeasePeriod() { PeriodStartDate = date, PeriodExpiryDate = date.AddDays(1), LeaseId = lease.Internal_Id, Lease = lease, Internal_Id = 1 };
            lease.PimsLeasePeriods = new List<PimsLeasePeriod>() { originalTerm };

            var service = this.CreateLeaseServicePeriodWithPermissions(Permissions.LeaseEdit);
            var leasePeriodRepository = this._helper.GetService<Mock<ILeasePeriodRepository>>();
            leasePeriodRepository.Setup(x => x.GetAllByLeaseId(It.IsAny<long>())).Returns(lease.PimsLeasePeriods);

            // Act
            var period = new PimsLeasePeriod() { PeriodStartDate = date.AddDays(1), LeaseId = lease.Internal_Id, Lease = lease };

            var ex = Assert.Throws<InvalidOperationException>(() => service.AddPeriod(lease.Internal_Id, period));
            ex.Message.Should().Be("A new period start and end date must not conflict with any existing periods.");
        }

        [Fact]
        public void AddPeriod_OverlappingDates_SameStartDate()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            var originalTerm = new PimsLeasePeriod() { PeriodStartDate = DateTime.Now, PeriodExpiryDate = DateTime.Now.AddDays(10), LeaseId = lease.Internal_Id, Lease = lease, Internal_Id = 1 };
            lease.PimsLeasePeriods = new List<PimsLeasePeriod>() { originalTerm };

            var service = this.CreateLeaseServicePeriodWithPermissions(Permissions.LeaseEdit);
            var leasePeriodRepository = this._helper.GetService<Mock<ILeasePeriodRepository>>();
            leasePeriodRepository.Setup(x => x.GetAllByLeaseId(It.IsAny<long>())).Returns(lease.PimsLeasePeriods);

            // Act
            var period = new PimsLeasePeriod() { PeriodStartDate = originalTerm.PeriodStartDate, PeriodExpiryDate = originalTerm.PeriodExpiryDate = originalTerm.PeriodStartDate.AddDays(1), LeaseId = lease.Internal_Id, Lease = lease };

            var ex = Assert.Throws<InvalidOperationException>(() => service.AddPeriod(lease.Internal_Id, period));
            ex.Message.Should().Be("A new period start and end date must not conflict with any existing periods.");
        }

        [Fact]
        public void AddPeriod_PaymentsNotExercised()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            var payment = new PimsLeasePayment();

            var service = this.CreateLeaseServicePeriodWithPermissions(Permissions.LeaseEdit);
            var leasePeriodRepository = this._helper.GetService<Mock<ILeasePeriodRepository>>();
            leasePeriodRepository.Setup(x => x.GetAllByLeaseId(It.IsAny<long>())).Returns(lease.PimsLeasePeriods);

            // Act
            var period = new PimsLeasePeriod() { PeriodStartDate = DateTime.MaxValue, LeaseId = lease.Internal_Id, Lease = lease, LeasePeriodStatusTypeCode = "NEXER", PimsLeasePayments = new List<PimsLeasePayment>() { payment } };

            var ex = Assert.Throws<InvalidOperationException>(() => service.AddPeriod(lease.Internal_Id, period));
            ex.Message.Should().Be("Period must be 'exercised' if payments have been made.");
        }
        #endregion

        #region Update
        [Fact]
        public void UpdatePeriod()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            var originalTerm = new PimsLeasePeriod() { PeriodStartDate = DateTime.Now, LeaseId = lease.Internal_Id, Lease = lease };
            lease.PimsLeasePeriods = new List<PimsLeasePeriod>() { originalTerm };

            var service = this.CreateLeaseServicePeriodWithPermissions(Permissions.LeaseEdit, Permissions.LeaseView);
            var leasePeriodRepository = this._helper.GetService<Mock<ILeasePeriodRepository>>();
            leasePeriodRepository.Setup(x => x.GetById(It.IsAny<long>(), It.IsAny<bool>())).Returns(originalTerm);

            // Act
            var period = new PimsLeasePeriod() { PeriodStartDate = DateTime.Now, LeaseId = lease.Internal_Id, Lease = lease };

            var updatedLease = service.UpdatePeriod(lease.Internal_Id, 1, period);

            // Assert
            leasePeriodRepository.Verify(x => x.Update(period), Times.Once);
        }

        [Fact]
        public void UpdatePeriod_OverlappingDates()
        {
            // Arrange

            var lease = EntityHelper.CreateLease(1);
            var originalTerm = new PimsLeasePeriod() { PeriodStartDate = DateTime.Now, LeaseId = lease.Internal_Id, Lease = lease, Internal_Id = 1 };
            lease.PimsLeasePeriods = new List<PimsLeasePeriod>() { originalTerm };

            var service = this.CreateLeaseServicePeriodWithPermissions(Permissions.LeaseEdit);
            var leasePeriodRepository = this._helper.GetService<Mock<ILeasePeriodRepository>>();
            leasePeriodRepository.Setup(x => x.GetAllByLeaseId(It.IsAny<long>())).Returns(lease.PimsLeasePeriods);

            // Act
            var period = new PimsLeasePeriod() { PeriodStartDate = DateTime.Now, LeaseId = lease.Internal_Id, Lease = lease };

            var ex = Assert.Throws<InvalidOperationException>(() => service.UpdatePeriod(lease.Internal_Id, 1, period));
            ex.Message.Should().Be("A new period start and end date must not conflict with any existing periods.");
        }

        [Fact]
        public void UpdatePeriod_PaymentsNotExercised()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            var payment = new PimsLeasePayment();
            var originalTerm = new PimsLeasePeriod() { PeriodStartDate = DateTime.Now, PeriodExpiryDate = DateTime.Now, LeaseId = lease.Internal_Id, Lease = lease, PimsLeasePayments = new List<PimsLeasePayment>() { payment } };
            lease.PimsLeasePeriods = new List<PimsLeasePeriod>() { originalTerm };

            var service = this.CreateLeaseServicePeriodWithPermissions(Permissions.LeaseEdit);
            var leasePeriodRepository = this._helper.GetService<Mock<ILeasePeriodRepository>>();
            leasePeriodRepository.Setup(x => x.GetAllByLeaseId(It.IsAny<long>())).Returns(lease.PimsLeasePeriods);
            leasePeriodRepository.Setup(x => x.GetById(It.IsAny<long>(), It.IsAny<bool>())).Returns(originalTerm);

            // Act
            var period = new PimsLeasePeriod() { PeriodStartDate = DateTime.MaxValue, LeaseId = lease.Internal_Id, Lease = lease, LeasePeriodStatusTypeCode = "NEXER" };

            var ex = Assert.Throws<InvalidOperationException>(() => service.UpdatePeriod(lease.Internal_Id, 1, period));
            ex.Message.Should().Be("Period must be 'exercised' if payments have been made.");
        }
        #endregion

        #region Delete
        [Fact]
        public void DeletePeriod()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            var originalTerm = new PimsLeasePeriod() { PeriodStartDate = DateTime.Now, LeaseId = lease.Internal_Id, Lease = lease };
            lease.PimsLeasePeriods = new List<PimsLeasePeriod>() { originalTerm };

            var service = this.CreateLeaseServicePeriodWithPermissions(Permissions.LeaseEdit);
            var leasePeriodRepository = this._helper.GetService<Mock<ILeasePeriodRepository>>();
            leasePeriodRepository.Setup(x => x.GetById(It.IsAny<long>(), It.IsAny<bool>())).Returns(originalTerm);

            // Act
            var period = new PimsLeasePeriod() { PeriodStartDate = DateTime.Now, LeaseId = lease.Internal_Id, Lease = lease };

            service.DeletePeriod(lease.Internal_Id, period);

            // Assert
            leasePeriodRepository.Verify(x => x.Delete(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void DeletePeriod_Payments()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            var payment = new PimsLeasePayment();
            var period = new PimsLeasePeriod() { PeriodStartDate = DateTime.Now, LeaseId = lease.Internal_Id, Lease = lease, PimsLeasePayments = new List<PimsLeasePayment>() { payment } };
            lease.PimsLeasePeriods = new List<PimsLeasePeriod>() { period };

            var service = this.CreateLeaseServicePeriodWithPermissions(Permissions.LeaseEdit);
            var leasePeriodRepository = this._helper.GetService<Mock<ILeasePeriodRepository>>();
            leasePeriodRepository.Setup(x => x.GetAllByLeaseId(It.IsAny<long>())).Returns(lease.PimsLeasePeriods);
            leasePeriodRepository.Setup(x => x.GetById(It.IsAny<long>(), It.IsAny<bool>())).Returns(period);

            // Act
            var ex = Assert.Throws<InvalidOperationException>(() => service.DeletePeriod(lease.Internal_Id, period));
            ex.Message.Should().Be("A period with payments attached can not be deleted. If you intend to delete this period, you must delete each of the corresponding payments first.");
        }

        [Fact]
        public void DeletePeriod_Exer()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            var payment = new PimsLeasePayment();
            var period = new PimsLeasePeriod() { PeriodStartDate = DateTime.Now, LeaseId = lease.Internal_Id, Lease = lease, LeasePeriodStatusTypeCode = "EXER" };
            lease.PimsLeasePeriods = new List<PimsLeasePeriod>() { period };

            var service = this.CreateLeaseServicePeriodWithPermissions(Permissions.LeaseEdit);
            var leasePeriodRepository = this._helper.GetService<Mock<ILeasePeriodRepository>>();
            leasePeriodRepository.Setup(x => x.GetAllByLeaseId(It.IsAny<long>())).Returns(lease.PimsLeasePeriods);
            leasePeriodRepository.Setup(x => x.GetById(It.IsAny<long>(), It.IsAny<bool>())).Returns(period);

            // Act
            var ex = Assert.Throws<InvalidOperationException>(() => service.DeletePeriod(lease.Internal_Id, period));
            ex.Message.Should().Be("Exercised periods cannot be deleted. Remove all payments from this period and set this period to 'Not Exercised' to delete this period.");
        }

        [Fact]
        public void DeletePeriod_Initial()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            var payment = new PimsLeasePayment();
            var period = new PimsLeasePeriod() { PeriodStartDate = DateTime.Now, LeaseId = lease.Internal_Id, Lease = lease };
            var period2 = new PimsLeasePeriod() { PeriodStartDate = DateTime.Now, LeaseId = lease.Internal_Id, Lease = lease };
            lease.PimsLeasePeriods = new List<PimsLeasePeriod>() { period, period2 };

            var service = this.CreateLeaseServicePeriodWithPermissions(Permissions.LeaseEdit);
            var leasePeriodRepository = this._helper.GetService<Mock<ILeasePeriodRepository>>();
            leasePeriodRepository.Setup(x => x.GetAllByLeaseId(It.IsAny<long>())).Returns(lease.PimsLeasePeriods);
            leasePeriodRepository.Setup(x => x.GetById(It.IsAny<long>(), It.IsAny<bool>())).Returns(period);

            // Act
            var ex = Assert.Throws<InvalidOperationException>(() => service.DeletePeriod(lease.Internal_Id, period));
            ex.Message.Should().Be("You must delete all renewals before deleting the initial period.");
        }
        #endregion
        #endregion
    }
}
