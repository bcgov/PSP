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

namespace Pims.Dal.Test.Repositories
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("area", "admin")]
    [Trait("group", "lease")]
    [ExcludeFromCodeCoverage]
    public class LeaseReportsServiceTest : IDisposable
    {
        public TestHelper helper;
        public Mock<ILeaseRepository> leaseRepository;
        public ILeaseReportsService leaseReportsService;

        public LeaseReportsServiceTest()
        {
            this.helper = new TestHelper();
        }

        public void Dispose()
        {
            this.leaseRepository = null;
        }

        private void MockCommonServices()
        {
            this.leaseReportsService = this.helper.Create<LeaseReportsService>();
            this.leaseRepository = this.helper.GetService<Mock<ILeaseRepository>>();
        }

        #region Tests
        #region GetAggregated
        [Fact]
        public void GetAggregatedLeases_NotAuthorized()
        {
            // Arrange
            var user = PrincipalHelper.CreateForPermission();

            var lease = EntityHelper.CreateLease(1);
            this.helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, TermExpiryDate = DateTime.Now.AddDays(10) };

            this.MockCommonServices();
            this.leaseRepository.Setup(x => x.Get(It.IsAny<LeaseFilter>(), true)).Returns(new List<PimsLease>() { lease });

            // Act
            // Assert
            Assert.Throws<NotAuthorizedException>(() => this.leaseReportsService.GetAggregatedLeaseReport(2022));
        }

        [Fact]
        public void GetAggregatedLeases_Success()
        {
            // Arrange
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            lease.OrigExpiryDate = new DateTime(2022, 4, 1);
            this.helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, TermExpiryDate = DateTime.Now.AddDays(10) };

            this.MockCommonServices();
            this.leaseRepository.Setup(x => x.Get(It.IsAny<LeaseFilter>(), true)).Returns(new List<PimsLease>() { lease });

            // Act
            var leases = this.leaseReportsService.GetAggregatedLeaseReport(2022);

            // Assert
            leases.Should().HaveCount(1);
            leases.FirstOrDefault().Should().Be(lease);
            this.leaseRepository.Verify(x => x.Get(It.IsAny<LeaseFilter>(), true));
        }
        #endregion
        #endregion
    }
}
