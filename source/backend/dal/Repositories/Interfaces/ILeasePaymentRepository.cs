using System;
using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface ILeasePaymentRepository : IRepository
    {
        void Delete(long leasePaymentId);

        PimsLeasePayment Update(PimsLeasePayment pimsLeasePayment);

        PimsLeasePayment Add(PimsLeasePayment pimsLeasePayment);

        IEnumerable<PimsLeasePayment> GetAllTracking(DateTime startDate, DateTime endDate);
    }
}
