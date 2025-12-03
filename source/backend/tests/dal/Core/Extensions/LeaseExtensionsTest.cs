using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Xunit;

namespace Pims.Dal.Test.Core.Extensions
{
    [Trait("category", "unit")]
    [Trait("category", "core")]
    [Trait("category", "function")]
    [ExcludeFromCodeCoverage]
    public class LeaseExtensionsTest
    {
        #region GetExpiryDate
        [Fact]
        public void GetExpiryDate_null()
        {
            PimsLease lease = new PimsLease() { OrigExpiryDate = null, PimsLeasePeriods = null };
            Assert.Null(lease.GetExpiryDate());
        }

        [Fact]
        public void GetExpiryDate_OrigExpiry()
        {
            DateTime now = DateTime.Now;
            PimsLease lease = new PimsLease() { OrigExpiryDate = now, PimsLeasePeriods = null };
            Assert.Equal(now, lease.GetExpiryDate());
        }

        [Fact]
        public void GetExpiryDate_RenewalExpiry()
        {
            DateTime now = DateTime.Now;
            PimsLease lease = new PimsLease()
            {
                OrigExpiryDate = null,
                PimsLeaseRenewals = new List<PimsLeaseRenewal>() {
                new PimsLeaseRenewal() { IsExercised = true, ExpiryDt = now }, },
            };
            Assert.Equal(now, lease.GetExpiryDate());
        }

        [Fact]
        public void GetExpiryDate_RenewalExpiry_NotExercised()
        {
            DateTime now = DateTime.Now;
            PimsLease lease = new PimsLease()
            {
                OrigExpiryDate = null,
                PimsLeaseRenewals = new List<PimsLeaseRenewal>() {
                new PimsLeaseRenewal() { IsExercised = false, ExpiryDt = now }, },
            };
            Assert.Equal(null, lease.GetExpiryDate());
        }

        [Fact]
        public void GetExpiryDate_OrigExpiryLater()
        {
            DateTime now = DateTime.Now;
            DateTime later = now.AddDays(1);
            PimsLease lease = new PimsLease()
            {
                OrigExpiryDate = later,
                PimsLeaseRenewals = new List<PimsLeaseRenewal>() {
                new PimsLeaseRenewal() { IsExercised=true, ExpiryDt = now }, },
            };
            Assert.Equal(later, lease.GetExpiryDate());
        }

        [Fact]
        public void GetExpiryDate_RenewalExpiryLater()
        {
            DateTime now = DateTime.Now;
            DateTime later = now.AddDays(1);
            PimsLease lease = new PimsLease()
            {
                OrigExpiryDate = now,
                PimsLeaseRenewals =
                new List<PimsLeaseRenewal>() { new PimsLeaseRenewal() { IsExercised = true, ExpiryDt = later } },
            };
            Assert.Equal(later, lease.GetExpiryDate());
        }

        [Fact]
        public void GetExpiryDate_MultipleRenewalExpiryLater()
        {
            DateTime now = DateTime.Now;
            DateTime later = now.AddDays(1);
            DateTime before = now.AddDays(-1);
            PimsLease lease = new PimsLease()
            {
                OrigExpiryDate = now,
                PimsLeaseRenewals =
                new List<PimsLeaseRenewal>() { new PimsLeaseRenewal() { IsExercised=true, ExpiryDt = later },
                    new PimsLeaseRenewal() { IsExercised=true, ExpiryDt = before }, },
            };
            Assert.Equal(later, lease.GetExpiryDate());
        }

        [Fact]
        public void GetExpiryDate_RenewalNoExpiry()
        {
            DateTime now = DateTime.Now;
            DateTime later = now.AddDays(1);
            DateTime before = now.AddDays(-1);
            PimsLease lease = new PimsLease()
            {
                OrigExpiryDate = now,
                PimsLeaseRenewals =
                new List<PimsLeaseRenewal>() { new PimsLeaseRenewal() { IsExercised=true, ExpiryDt = null },
                    new PimsLeaseRenewal() { IsExercised = false, ExpiryDt = before }, },
            };
            Assert.Equal(now, lease.GetExpiryDate());
        }
        #endregion
    }
}
