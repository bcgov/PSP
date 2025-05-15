using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// Provides an interface for interacting with BCTFA ownership list in the PSP database.
    /// </summary>
    public interface IPmbcBctfaPidRepository : IRepository
    {
        /// <summary>
        /// Get all BCTFA PIDs.
        /// </summary>
        /// <returns>An array of integers representing the PIDs.</returns>
        IEnumerable<PmbcBctfaPid> GetAll();

        /// <summary>
        /// Add BCTFA PIDs.
        /// </summary>
        /// <param name="pids">An array of PmbcBctfaPid representing the PIDs to add.</param>
        void AddRange(IEnumerable<PmbcBctfaPid> pids);

        /// <summary>
        /// Update BCTFA PIDs.
        /// </summary>
        /// <param name="pids">An array of PmbcBctfaPid representing the PIDs to update.</param>
        void UpdateRange(IEnumerable<PmbcBctfaPid> pids);
    }
}
