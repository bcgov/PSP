
using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface IConsultationRepository : IRepository
    {
        List<PimsLeaseConsultation> GetConsultationsByLease(long leaseId);

        PimsLeaseConsultation GetConsultationById(long consultationId);

        PimsLeaseConsultation AddConsultation(PimsLeaseConsultation consultation);

        PimsLeaseConsultation UpdateConsultation(PimsLeaseConsultation consultation);

        bool TryDeleteConsultation(long consultationId);
    }
}
