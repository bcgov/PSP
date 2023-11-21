using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public class AcquisitionStatusSolverFactory : IAcquisitionStatusSolverFactory
    {
        public IAcquisitionStatusSolver CreateSolver(PimsAcquisitionFile acquisitionFile)
        {
            return new AcquisitionStatusSolver(acquisitionFile);
        }
    }
}