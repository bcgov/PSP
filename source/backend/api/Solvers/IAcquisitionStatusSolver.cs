using Pims.Api.Constants;

namespace Pims.Api.Services
{
    public interface IAcquisitionStatusSolver
    {
        bool CanEditDetails(AcqusitionStatusTypes? acquisitionStatus);

        bool CanEditTakes(AcqusitionStatusTypes? acquisitionStatus);

        bool CanEditOrDeleteCompensation(AcqusitionStatusTypes? acquisitionStatus, bool? isDraftCompensation);

        bool CanEditOrDeleteAgreement(AcqusitionStatusTypes? acquisitionStatus, AgreementStatusTypes? agreementStatus);

        bool CanEditChecklists(AcqusitionStatusTypes? acquisitionStatus);

        bool CanEditStakeholders(AcqusitionStatusTypes? acquisitionStatus);
    }
}