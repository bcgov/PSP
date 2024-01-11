using Pims.Api.Models.Base;

namespace Pims.Api.Models.Concepts.Property
{
    public class PropertyAnomalyModel : BaseAuditModel
    {
        #region Properties

        /// <summary>
        /// get/set - Property anomaly id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - Parent property id.
        /// </summary>
        public long PropertyId { get; set; }

        /// <summary>
        /// get/set - Anomaly type code.
        /// </summary>
        public virtual CodeTypeModel<string> PropertyAnomalyTypeCode { get; set; }

        #endregion
    }
}
