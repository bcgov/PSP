using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface ILeasePaymentRepository : IRepository
    {
        void Delete(long leasePaymentId);
        PimsLeasePayment Update(PimsLeasePayment pimsLeasePayment);
        PimsLeasePayment Add(PimsLeasePayment pimsLeasePayment);
        PimsLeasePayment GetById(long leasePaymentId);
    }
}
