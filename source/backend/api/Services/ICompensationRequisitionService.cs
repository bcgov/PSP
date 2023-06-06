using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public interface ICompensationRequisitionService
    {
        PimsCompensationRequisition GetById(long compensationRequisitionId);

        PimsAcquisitionPayee GetPayeeById(long payeeId);

        PimsCompensationRequisition Update(PimsCompensationRequisition compensationRequisition);

        bool DeleteCompensation(long compensationId);

        PimsAcquisitionPayee GetPayee(long compensationRequisitionId);
    }
}
