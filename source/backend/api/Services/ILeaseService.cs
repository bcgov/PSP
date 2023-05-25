using System.Collections.Generic;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;

namespace Pims.Api.Services
{
    public interface ILeaseService
    {
        bool IsRowVersionEqual(long leaseId, long rowVersion);

        PimsLease GetById(long leaseId);

        PimsLease Add(PimsLease lease, IEnumerable<UserOverrideCode> userOverrides);

        PimsLease Update(PimsLease lease, IEnumerable<UserOverrideCode> userOverrides);
    }
}
