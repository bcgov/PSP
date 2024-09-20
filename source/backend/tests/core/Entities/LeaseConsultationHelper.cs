using System;
using System.Linq;
using Pims.Dal;
using Pims.Dal.Entities;
using Entity = Pims.Dal.Entities;

namespace Pims.Core.Test
{
    /// <summary>
    /// EntityHelper static class, provides helper methods to create test entities.
    /// </summary>
    public static partial class EntityHelper
    {
        /// <summary>
        /// Create a new instance of a lease consultation.
        /// </summary>
        /// <returns></returns>
        public static Entity.PimsLeaseConsultation CreateLeaseConsultationItem(long? leaseConsultationId = null, long? leaseId = null, long? personId = null, long? organizationId = null, PimsConsultationType type = null, PimsConsultationStatusType statusType = null, PimsConsultationOutcomeType outcomeType = null)
        {
            var consultationItem = new Entity.PimsLeaseConsultation()
            {
                Internal_Id = leaseConsultationId ?? 1,
                LeaseId = leaseId ?? 1,
                PersonId = personId,
                OrganizationId = organizationId,
                IsResponseReceived = false,
                RequestedOn = DateTime.UtcNow,
                ResponseReceivedDate = DateTime.UtcNow,
            };
            consultationItem.ConsultationOutcomeTypeCodeNavigation = outcomeType ?? new Entity.PimsConsultationOutcomeType() { Id = "APPRDENIED", Description = "Denied", DbCreateUserid = "test", DbLastUpdateUserid = "test", DbLastUpdateTimestamp = System.DateTime.Now };
            consultationItem.ConsultationOutcomeTypeCode = consultationItem.ConsultationOutcomeTypeCodeNavigation.Id;
            consultationItem.ConsultationStatusTypeCodeNavigation = statusType ?? new Entity.PimsConsultationStatusType() { Id = "REQCOMP", Description = "Required", DbCreateUserid = "test", DbLastUpdateUserid = "test", DbLastUpdateTimestamp = System.DateTime.Now };
            consultationItem.ConsultationStatusTypeCode = consultationItem.ConsultationStatusTypeCodeNavigation.Id;
            consultationItem.ConsultationTypeCodeNavigation = type ?? new Entity.PimsConsultationType() { Id = "ACTIVE", Description = "Active", DbCreateUserid = "test", DbLastUpdateUserid = "test", DbLastUpdateTimestamp = System.DateTime.Now };
            consultationItem.ConsultationTypeCode = consultationItem.ConsultationTypeCodeNavigation.Id;

            return consultationItem;
        }

        /// <summary>
        /// Create a new instance of a lease consultation.
        /// </summary>
        /// <returns></returns>
        public static Entity.PimsLeaseConsultation CreateLeaseConsultationItem(this PimsContext context, long? leaseId = null, long? leaseConsultationId = null)
        {
            var statusType = context.PimsConsultationStatusTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find consultation status type.");
            var itemType = context.PimsConsultationTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find consultation type.");
            var outcomeType = context.PimsConsultationOutcomeTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find consultation outcome type.");
            var consultation = EntityHelper.CreateLeaseConsultationItem(leaseId: leaseId, leaseConsultationId: leaseConsultationId, statusType: statusType, type: itemType, outcomeType: outcomeType);
            context.PimsLeaseConsultations.Add(consultation);

            return consultation;
        }
    }
}
