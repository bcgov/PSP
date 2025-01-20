using System.Collections.Generic;
using Pims.Api.Models.CodeTypes;
using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public interface ICompensationRequisitionService
    {
        PimsCompensationRequisition GetById(long compensationRequisitionId);

        PimsCompensationRequisition Update(FileTypes fileType, PimsCompensationRequisition compensationRequisition);

        bool DeleteCompensation(long compensationId);

        IEnumerable<PimsPropertyAcquisitionFile> GetAcquisitionProperties(long id);

        IEnumerable<PimsPropertyLease> GetLeaseProperties(long id);

        IEnumerable<PimsCompensationRequisition> GetFileCompensationRequisitions(FileTypes fileType, long fileId);

        PimsCompensationRequisition AddCompensationRequisition(FileTypes fileType, PimsCompensationRequisition compensationRequisition);

        IEnumerable<PimsCompReqFinancial> GetCompensationRequisitionFinancials(long compReqId);

        IEnumerable<PimsCompReqPayee> GetCompensationRequisitionPayees(long compReqId);
    }
}
