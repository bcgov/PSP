using System.Threading.Tasks;
using Pims.Api.Models.Concepts.Property;

namespace Pims.Api.Services
{
    /// <summary>
    /// Provides orchestrated property lookup operations across PIMS and PMBC.
    /// </summary>
    public interface IPropertyLookupService
    {
        /// <summary>
        /// Lookup a PID in PIMS first, then PMBC when not found in PIMS.
        /// </summary>
        /// <param name="pid">The PID value to lookup.</param>
        /// <returns>A lookup result indicating where the PID was found.</returns>
        Task<PropertyPidLookupResultModel> LookupByPidAsync(string pid);
    }
}
