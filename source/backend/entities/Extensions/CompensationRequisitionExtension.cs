using System;
using Pims.Core.Exceptions;

namespace Pims.Dal.Entities.Extensions
{
    public static class CompensationRequisitionExtension
    {
        public static void ThrowInvalidParentId(this PimsCompensationRequisition compReq)
        {
            ArgumentNullException.ThrowIfNull(compReq);

            if(compReq.AcquisitionFileId is null && compReq.LeaseId is null)
            {
                throw new BusinessRuleViolationException("Compensation requisition missing AcquisitionFileId or LeaseId");
            }

            if (compReq.AcquisitionFileId is not null && compReq.LeaseId is not null)
            {
                throw new BusinessRuleViolationException("Compensation requiestion must have only one ParentId");
            }
        }
    }
}
