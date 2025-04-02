using System.Collections.Generic;
using System.IO;

namespace Pims.Api.Services
{
    public interface IBctfaOwnershipService
    {
        int[] ParseCsvFileToIntArray(Stream fileStream);

        void UpdateBctfaOwnership(IEnumerable<int> allPids);
    }
}
