using NetTopologySuite.Geometries;
using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public class FilePropertyLocationUpdateSolver : IFilePropertyLocationUpdateSolver
    {
        public bool CanEditFilePropertyLocation<T>(T incomingFileProperty, T existingFileProperty)
            where T : IFilePropertyEntity
        {
            if (HasLocationChanged(incomingFileProperty.Location, existingFileProperty.Location))
            {
                return true;
            }

            if (HasBoundaryChanged(incomingFileProperty.Boundary, existingFileProperty.Boundary))
            {
                return true;
            }

            return false;
        }

        private static bool HasLocationChanged(Geometry incomingLocation, Geometry existingLocation)
        {
            if (existingLocation is null || (incomingLocation is not null && !existingLocation.EqualsExact(incomingLocation)))
            {
                return true;
            }
            return false;
        }

        private static bool HasBoundaryChanged(Geometry incomingBoundary, Geometry existingBoundary)
        {
            if (existingBoundary is null || (incomingBoundary is not null && !existingBoundary.EqualsExact(incomingBoundary)))
            {
                return true;
            }
            return false;
        }
    }
}
