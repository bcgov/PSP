using System.Collections.Generic;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;

namespace Pims.Api.Services
{
    public interface ILeaseService
    {
        PimsLease GetById(long leaseId);

        IEnumerable<PimsLease> GetAllByIds(IEnumerable<long> leaseIds);

        LastUpdatedByModel GetLastUpdateInformation(long leaseId);

        Paged<PimsLease> GetPage(LeaseFilter filter, bool? all = false);

        PimsLease Add(PimsLease lease, IEnumerable<UserOverrideCode> userOverrides);

        PimsLease Update(PimsLease lease, IEnumerable<UserOverrideCode> userOverrides);

        IEnumerable<PimsPropertyLease> GetPropertiesByLeaseId(long leaseId);

        IEnumerable<PimsInsurance> GetInsuranceByLeaseId(long leaseId);

        IEnumerable<PimsInsurance> UpdateInsuranceByLeaseId(long leaseId, IEnumerable<PimsInsurance> pimsInsurances);

        IEnumerable<PimsPropertyImprovement> GetImprovementsByLeaseId(long leaseId);

        IEnumerable<PimsPropertyImprovement> UpdateImprovementsByLeaseId(long leaseId, IEnumerable<PimsPropertyImprovement> pimsPropertyImprovements);

        IEnumerable<PimsLeaseStakeholder> GetStakeholdersByLeaseId(long leaseId);

        IEnumerable<PimsLeaseStakeholder> UpdateStakeholdersByLeaseId(long leaseId, IEnumerable<PimsLeaseStakeholder> pimsLeaseStakeholders);

        IEnumerable<PimsLeaseRenewal> GetRenewalsByLeaseId(long leaseId);

        IEnumerable<PimsLeaseChecklistItem> GetChecklistItems(long id);

        PimsLease UpdateChecklistItems(long leaseId, IList<PimsLeaseChecklistItem> checklistItems);

        IEnumerable<PimsLeaseStakeholderType> GetAllStakeholderTypes();

        IEnumerable<PimsLeaseConsultation> GetConsultations(long leaseId);

        PimsLeaseConsultation GetConsultationById(long consultationId);

        PimsLeaseConsultation AddConsultation(PimsLeaseConsultation consultation);

        PimsLeaseConsultation UpdateConsultation(PimsLeaseConsultation consultation);

        bool DeleteConsultation(long consultationId);
    }
}
