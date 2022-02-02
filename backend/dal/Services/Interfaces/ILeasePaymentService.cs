using Pims.Dal.Entities;

namespace Pims.Dal.Services
{
    public interface ILeasePaymentService
    {
        PimsLease AddPayment(long leaseId, long leaseRowVersion, PimsLeasePayment payment);
        PimsLease UpdatePayment(long leaseId, long paymentId, long leaseRowVersion, PimsLeasePayment payment);
        PimsLease DeletePayment(long leaseId, long leaseRowVersion, PimsLeasePayment payment);
    }
}
