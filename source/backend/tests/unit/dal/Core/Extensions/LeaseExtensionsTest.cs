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
            PimsLease lease = new PimsLease() { OrigExpiryDate = null, PimsLeaseTerms = null };
            Assert.Null(lease.GetExpiryDate());
        }

        [Fact]
        public void GetExpiryDate_OrigExpiry()
        {
            DateTime now = DateTime.Now;
            PimsLease lease = new PimsLease() { OrigExpiryDate = now, PimsLeaseTerms = null };
            Assert.Equal(now, lease.GetExpiryDate());
        }

        [Fact]
        public void GetExpiryDate_TermExpiry()
        {
            DateTime now = DateTime.Now;
            PimsLease lease = new PimsLease()
            {
                OrigExpiryDate = null,
                PimsLeaseTerms = new List<PimsLeaseTerm>() {
                new PimsLeaseTerm() { TermExpiryDate = now }, },
            };
            Assert.Equal(now, lease.GetExpiryDate());
        }

        [Fact]
        public void GetExpiryDate_OrigExpiryLater()
        {
            DateTime now = DateTime.Now;
            DateTime later = now.AddDays(1);
            PimsLease lease = new PimsLease()
            {
                OrigExpiryDate = later,
                PimsLeaseTerms = new List<PimsLeaseTerm>() {
                new PimsLeaseTerm() { TermExpiryDate = now }, },
            };
            Assert.Equal(later, lease.GetExpiryDate());
        }

        [Fact]
        public void GetExpiryDate_TermExpiryLater()
        {
            DateTime now = DateTime.Now;
            DateTime later = now.AddDays(1);
            PimsLease lease = new PimsLease()
            {
                OrigExpiryDate = now,
                PimsLeaseTerms =
                new List<PimsLeaseTerm>() { new PimsLeaseTerm() { TermExpiryDate = later } },
            };
            Assert.Equal(later, lease.GetExpiryDate());
        }

        [Fact]
        public void GetExpiryDate_MultipleTermExpiryLater()
        {
            DateTime now = DateTime.Now;
            DateTime later = now.AddDays(1);
            DateTime before = now.AddDays(-1);
            PimsLease lease = new PimsLease()
            {
                OrigExpiryDate = now,
                PimsLeaseTerms =
                new List<PimsLeaseTerm>() { new PimsLeaseTerm() { TermExpiryDate = later },
                    new PimsLeaseTerm() { TermExpiryDate = before }, },
            };
            Assert.Equal(later, lease.GetExpiryDate());
        }

        [Fact]
        public void GetExpiryDate_TermNoExpiry()
        {
            DateTime now = DateTime.Now;
            DateTime later = now.AddDays(1);
            DateTime before = now.AddDays(-1);
            PimsLease lease = new PimsLease()
            {
                OrigExpiryDate = now,
                PimsLeaseTerms =
                new List<PimsLeaseTerm>() { new PimsLeaseTerm() { TermExpiryDate = null },
                    new PimsLeaseTerm() { TermExpiryDate = before }, },
            };
            Assert.Equal(null, lease.GetExpiryDate());
        }
        #endregion
    }
}
