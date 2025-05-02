using System;
using System.Collections.Generic;
using Pims.Api.Models.Concepts.CompensationRequisition;
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

        IEnumerable<PimsCompReqAcqPayee> GetCompensationRequisitionAcquisitionPayees(long compReqId);

        IEnumerable<PimsCompReqLeasePayee> GetCompensationRequisitionLeasePayees(long compReqId);

        PimsCompensationRequisition GetCompensationRequisitionAtTime(long compReqId, DateTime time);

        IEnumerable<PimsPropertyAcquisitionFile> GetCompensationRequisitionAcqPropertiesAtTime(long compReqId, DateTime time);

        IEnumerable<PimsPropertyLease> GetCompensationRequisitionLeasePropertiesAtTime(long compReqId, DateTime time);

        IEnumerable<PimsCompReqAcqPayee> GetCompensationRequisitionAcquisitionPayeesAtTime(long compReqId, DateTime time);

        IEnumerable<PimsCompReqLeasePayee> GetCompensationRequisitionLeasePayeesAtTime(long compReqId, DateTime time);
    }
}
