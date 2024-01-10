using Pims.Api.Models.Base;

namespace Pims.Api.Models.Concepts.Property
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
        public virtual CodeTypeModel<string> PropertyRoadTypeCode { get; set; }

        #endregion
    }
}
