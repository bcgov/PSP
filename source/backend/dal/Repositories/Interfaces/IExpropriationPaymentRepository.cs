using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface IExpropriationPaymentRepository : IRepository<PimsExpropriationPayment>
    {
        PimsExpropriationPayment Add(PimsExpropriationPayment expropriationPayment);

        IList<PimsExpropriationPayment> GetAllByAcquisitionFileId(long acquisitionFileId);

        PimsExpropriationPayment GetById(long expropriationPaymentId);

        PimsExpropriationPayment Update(PimsExpropriationPayment expropriationPayment);

        bool TryDelete(long id);
    }
}
