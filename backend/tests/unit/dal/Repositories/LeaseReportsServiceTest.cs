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
using static Pims.Dal.Entities.PimsLeasePaymentStatusType;
using Pims.Dal.Repositories;

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
            helper = new TestHelper();
        }

        public void Dispose()
        {
            leaseRepository = null;
        }

        private void MockCommonServices()
        {
            leaseReportsService = helper.Create<LeaseReportsService>();
            leaseRepository = helper.GetService<Mock<ILeaseRepository>>();
        }

        #region Tests
        #region GetAggregated
        [Fact]
        public void GetAggregatedLeases_NotAuthorized()
        {
            // Arrange
            var user = PrincipalHelper.CreateForPermission();

            var lease = EntityHelper.CreateLease(1);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, TermExpiryDate = DateTime.Now.AddDays(10) };

            MockCommonServices();
            leaseRepository.Setup(x => x.Get(It.IsAny<LeaseFilter>(), true)).Returns(new List<PimsLease>() { lease });

            // Act
            // Assert
            Assert.Throws<NotAuthorizedException>(() => leaseReportsService.GetAggregatedLeaseReport(2022));
        }

        [Fact]
        public void GetAggregatedLeases_Success()
        {
            // Arrange
            var user = PrincipalHelper.CreateForPermission(Permissions.LeaseView);

            var lease = EntityHelper.CreateLease(1);
            lease.OrigExpiryDate = new DateTime(2022, 4, 1);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(lease);
            var term = new PimsLeaseTerm() { TermStartDate = DateTime.Now, TermExpiryDate = DateTime.Now.AddDays(10) };

            MockCommonServices();
            leaseRepository.Setup(x => x.Get(It.IsAny<LeaseFilter>(), true)).Returns(new List<PimsLease>() { lease });

            // Act
            var leases = leaseReportsService.GetAggregatedLeaseReport(2022);

            // Assert
            leases.Should().HaveCount(1);
            leases.FirstOrDefault().Should().Be(lease);
            leaseRepository.Verify(x => x.Get(It.IsAny<LeaseFilter>(), true));
        }
        #endregion
        #endregion
    }
}
