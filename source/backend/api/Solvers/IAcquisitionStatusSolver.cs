using Pims.Api.Models.CodeTypes;

namespace Pims.Api.Services
{
    public interface IAcquisitionStatusSolver
    {
        bool CanEditDetails(AcquisitionStatusTypes? acquisitionStatus);

        bool CanEditProperties(AcquisitionStatusTypes? acquisitionStatus);

        bool CanEditTakes(AcquisitionStatusTypes? acquisitionStatus);

        bool CanEditOrDeleteCompensation(AcquisitionStatusTypes? acquisitionStatus, bool? isDraftCompensation, bool? isAdmin);

        bool CanEditOrDeleteAgreement(AcquisitionStatusTypes? acquisitionStatus);

        bool CanEditChecklists(AcquisitionStatusTypes? acquisitionStatus);

        bool CanEditStakeholders(AcquisitionStatusTypes? acquisitionStatus);

        bool CanEditExpropriation(AcquisitionStatusTypes? acquisitionStatus);
    }
}
