using Pims.Api.Concepts.Models.Base;

namespace Pims.Api.Concepts.Models.Concepts.Property
{
    public class PropertyManagementPurposeModel : BaseAuditModel
    {
        #region Properties

        /// <summary>
        /// get/set - The relationship id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - Parent property id.
        /// </summary>
        public long PropertyId { get; set; }

        /// <summary>
        /// get/set - The property purpose type code.
        /// </summary>
        public TypeModel<string> PropertyPurposeTypeCode { get; set; }

        #endregion
    }
}
