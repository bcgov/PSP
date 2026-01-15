using Pims.Api.Models.Base;

namespace Pims.Api.Models.Concepts.Property
{
    public class PropertyTenureCleanupModel : BaseAuditModel
    {
        #region Properties

        /// <summary>
        /// get/set - Property tenure cleanup id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - Parent property id.
        /// </summary>
        public long PropertyId { get; set; }

        /// <summary>
        /// get/set - Tenure cleanup type code.
        /// </summary>
        public virtual CodeTypeModel<string> TenureCleanupTypeCode { get; set; }

        #endregion
    }
}
