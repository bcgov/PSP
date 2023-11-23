using Pims.Api.Concepts.Models.Base;

namespace Pims.Api.Concepts.Models.Concepts.Property
{
    public class PropertyRoadModel : BaseAuditModel
    {
        #region Properties

        /// <summary>
        /// get/set - Property road id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - Parent property id.
        /// </summary>
        public long PropertyId { get; set; }

        /// <summary>
        /// get/set - Road type code.
        /// </summary>
        public virtual TypeModel<string> PropertyRoadTypeCode { get; set; }

        #endregion
    }
}
