using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public interface IAcquisitionStatusSolverFactory
    {
        IAcquisitionStatusSolver CreateSolver(PimsAcquisitionFile acquisitionFile);
    }
}