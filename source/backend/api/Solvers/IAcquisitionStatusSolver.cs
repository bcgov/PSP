namespace Pims.Api.Services
{
    public interface IAcquisitionStatusSolver
    {
        bool CanEditDetails();

        bool CanEditTakes();

        bool CanEditOrDeleteCompensation(bool? isDraftCompensation);

        bool CanEditOrDeleteAgreement(string agreementStatusCode);

        bool CanEditChecklists();

        bool CanEditStakeholders();
    }
}