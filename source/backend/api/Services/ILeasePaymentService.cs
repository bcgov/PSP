using System;
using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public interface ILeasePaymentService
    {
        PimsLeasePayment AddPayment(long leaseId, PimsLeasePayment payment);

        PimsLeasePayment UpdatePayment(long leaseId, long paymentId, PimsLeasePayment payment);

        bool DeletePayment(long leaseId, PimsLeasePayment payment);

        IEnumerable<PimsLeasePayment> GetAllByDateRange(DateTime startDate, DateTime endDate);
    }
}
