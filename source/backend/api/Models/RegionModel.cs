namespace Pims.Api.Models
{
    /// <summary>
    /// Provides a lease-oriented insurance model.
    /// </summary>
    public class RegionModel : BaseModel
    {
        /// <summary>
        /// get/set - The region code.
        /// </summary>
        public short Id { get; set; }

        /// <summary>
        /// get/set - The region Name.
        /// </summary>
        public string Description { get; set; }
    }
}
