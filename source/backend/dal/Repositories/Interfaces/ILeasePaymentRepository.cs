using Pims.Dal.Entities;
using System;
using System.Collections.Generic;

namespace Pims.Dal.Repositories
{
    public interface ILeasePaymentRepository : IRepository
    {
        void Delete(long leasePaymentId);

        PimsLeasePayment Update(PimsLeasePayment pimsLeasePayment);

        PimsLeasePayment Add(PimsLeasePayment pimsLeasePayment);

        PimsLeasePayment GetById(long leasePaymentId);

        IEnumerable<PimsLeasePayment> GetAll(DateTime startDate, DateTime endDate);
    }
}
