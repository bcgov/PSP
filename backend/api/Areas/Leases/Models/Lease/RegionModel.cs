using Pims.Api.Models;

namespace Pims.Api.Areas.Lease.Models.Lease
{
    /// <summary>
    /// Provides a lease-oriented insurance model.
    /// </summary>
    public class RegionModel : BaseModel
    {
        /// <summary>
        /// get/set - The region code.
        /// </summary>
        public short RegionCode { get; set; }

        /// <summary>
        /// get/set - The region Name.
        /// </summary>
        public string RegionName { get; set; }
    }
}
