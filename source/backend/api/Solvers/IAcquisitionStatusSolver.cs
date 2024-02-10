using Pims.Api.Constants;

namespace Pims.Api.Services
{
    public interface IAcquisitionStatusSolver
    {
        bool CanEditDetails(AcquisitionStatusTypes? acquisitionStatus);

        bool CanEditProperties(AcquisitionStatusTypes? acquisitionStatus);

        bool CanEditTakes(AcquisitionStatusTypes? acquisitionStatus);

        bool CanEditOrDeleteCompensation(AcquisitionStatusTypes? acquisitionStatus, bool? isDraftCompensation);

        bool CanEditOrDeleteAgreement(AcquisitionStatusTypes? acquisitionStatus, AgreementStatusTypes? agreementStatus);

        bool CanEditChecklists(AcquisitionStatusTypes? acquisitionStatus);

        bool CanEditStakeholders(AcquisitionStatusTypes? acquisitionStatus);
    }
}
