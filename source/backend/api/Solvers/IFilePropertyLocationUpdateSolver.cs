using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public interface IFilePropertyLocationUpdateSolver
    {
        bool CanEditFilePropertyLocation<T>(T incomingFileProperty, T existingFileProperty)
            where T : IFilePropertyEntity;
    }
}
