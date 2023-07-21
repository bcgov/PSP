using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public interface ILeaseTermService
    {
        IEnumerable<PimsLeaseTerm> GetTerms(long leaseId);

        PimsLeaseTerm AddTerm(long leaseId, PimsLeaseTerm term);

        PimsLeaseTerm UpdateTerm(long leaseId, long termId, PimsLeaseTerm term);

        bool DeleteTerm(long leaseId, PimsLeaseTerm term);
    }
}
