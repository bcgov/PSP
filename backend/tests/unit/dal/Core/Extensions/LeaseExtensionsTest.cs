using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;

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
            Lease lease = new Lease() { OrigExpiryDate = null, TermExpiryDate = null };
            Assert.Null(lease.GetExpiryDate());
        }

        [Fact]
        public void GetExpiryDate_OrigExpiry()
        {
            DateTime now = DateTime.Now;
            Lease lease = new Lease() { OrigExpiryDate = now, TermExpiryDate = null };
            Assert.Equal(now, lease.GetExpiryDate());
        }

        [Fact]
        public void GetExpiryDate_TermExpiry()
        {
            DateTime now = DateTime.Now;
            Lease lease = new Lease() { OrigExpiryDate = null, TermExpiryDate = now };
            Assert.Equal(now, lease.GetExpiryDate());
        }

        [Fact]
        public void GetExpiryDate_OrigExpiryLater()
        {
            DateTime now = DateTime.Now;
            DateTime later = now.AddDays(1);
            Lease lease = new Lease() { OrigExpiryDate = later, TermExpiryDate = now };
            Assert.Equal(later, lease.GetExpiryDate());
        }

        [Fact]
        public void GetExpiryDate_TermExpiryLater()
        {
            DateTime now = DateTime.Now;
            DateTime later = now.AddDays(1);
            Lease lease = new Lease() { OrigExpiryDate = now, TermExpiryDate = later };
            Assert.Equal(later, lease.GetExpiryDate());
        }
        #endregion
    }
}
