using System;
using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public class FilePropertyLocationUpdateSolver : IFilePropertyLocationUpdateSolver
    {
        public bool CanEditFilePropertyLocation<T>(T incomingFileProperty, T existingFileProperty)
            where T : IFilePropertyEntity
        {
            ArgumentNullException.ThrowIfNull(incomingFileProperty, nameof(incomingFileProperty));
            ArgumentNullException.ThrowIfNull(existingFileProperty, nameof(existingFileProperty));

            if (existingFileProperty.Location is null)
            {
                // Can add a new location
                return incomingFileProperty.Location is not null;
            }
            else
            {
                // Cannot delete an existing location
                return incomingFileProperty.Location is not null && !incomingFileProperty.Location.EqualsExact(existingFileProperty.Location);
            }
        }

        public bool CanEditFilePropertyBoundary<T>(T incomingFileProperty, T existingFileProperty)
            where T : IFilePropertyEntity
        {
            ArgumentNullException.ThrowIfNull(incomingFileProperty, nameof(incomingFileProperty));
            ArgumentNullException.ThrowIfNull(existingFileProperty, nameof(existingFileProperty));

            if (existingFileProperty.Boundary is null && incomingFileProperty.Boundary is null)
            {
                // No change
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
