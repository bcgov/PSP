using Pims.Api.Models.Base;

namespace Pims.Api.Areas.Lease.Models.Lease
{
    /// <summary>
    /// Provides a lease-oriented insurance model.
    /// </summary>
    public class RegionModel : BaseConcurrentModel
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
