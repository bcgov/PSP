using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface ICompensationRequisitionRepository : IRepository<PimsCompensationRequisition>
    {
        IList<PimsCompensationRequisition> GetAllByAcquisitionFileId(long acquisitionFileId);

        PimsCompensationRequisition GetById(long compensationRequisitionId);

        PimsCompensationRequisition Add(PimsCompensationRequisition compensationRequisition);

        PimsCompensationRequisition Update(PimsCompensationRequisition compensationRequisition);

        PimsAcquisitionPayee UpdatePayee(PimsAcquisitionPayee compensationPayee);

        PimsAcqPayeeCheque UpdatePayeeCheque(PimsAcqPayeeCheque payeeCheque);

        bool TryDelete(long compensationId);

        PimsAcquisitionPayee GetPayee(long payeeId);
    }
}
