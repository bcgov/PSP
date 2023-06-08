using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface IAcquisitionPayeeRepository : IRepository<PimsAcquisitionPayee>
    {

        PimsAcquisitionPayee GetById(long payeeId);
    }
}
