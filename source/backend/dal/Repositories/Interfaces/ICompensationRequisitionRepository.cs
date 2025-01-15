using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface ICompensationRequisitionRepository : IRepository<PimsCompensationRequisition>
    {
        IList<PimsCompensationRequisition> GetAllByAcquisitionFileId(long acquisitionFileId);

        IList<PimsCompensationRequisition> GetAllByLeaseFileId(long leaseFileId);

        PimsCompensationRequisition GetById(long compensationRequisitionId);

        PimsCompensationRequisition Add(PimsCompensationRequisition compensationRequisition);

        PimsCompensationRequisition Update(PimsCompensationRequisition compensationRequisition);

        bool TryDelete(long compensationId);

        List<PimsPropertyAcquisitionFile> GetAcquisitionCompReqPropertiesById(long compensationRequisitionId);

        List<PimsPropertyLease> GetLeaseCompReqPropertiesById(long compensationRequisitionId);

        IEnumerable<PimsCompReqFinancial> GetCompensationRequisitionFinancials(long compReqId);

        IEnumerable<PimsCompReqPayee> GetCompensationRequisitionPayees(long compReqId);
    }
}
