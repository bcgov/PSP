using Pims.Api.Models.CodeTypes;

namespace Pims.Api.Services
{
    public interface ILeaseStatusSolver
    {
        bool CanEditDetails(LeaseStatusTypes? leaseStatus);

        bool CanEditProperties(LeaseStatusTypes? leaseStatus);

        bool CanEditOrDeleteCompensation(LeaseStatusTypes? leaseStatus, bool? isDraftCompensation, bool? isAdmin);

        bool CanEditOrDeleteConsultation(LeaseStatusTypes? leaseStatus);

        bool CanEditChecklists(LeaseStatusTypes? leaseStatus);

        bool CanEditStakeholders(LeaseStatusTypes? leaseStatus);

        bool CanEditImprovements(LeaseStatusTypes? leaseStatus);

        bool CanEditInsurance(LeaseStatusTypes? leaseStatus);

        bool CanEditDeposits(LeaseStatusTypes? leaseStatus);

        bool CanEditPayments(LeaseStatusTypes? leaseStatus);

        LeaseStatusTypes? GetCurrentLeaseStatus(string pimsLeaseStatusType);
    }
}
