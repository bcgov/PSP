using Pims.Api.Models.Base;

namespace Pims.Api.Models.Concepts.Lease
{
    public class LeasePurposeModel : BaseAuditModel
    {
        /// <summary>
        /// get/set - Lease Purpose id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - Parent Lease id.
        /// </summary>
        public long LeaseId { get; set; }

        /// <summary>
        /// get/set - LeasePurpose type code.
        /// </summary>
        public virtual CodeTypeModel<string> LeasePurposeTypeCode { get; set; }

        /// <summary>
        /// get/set - LeasePurpose Other description.
        /// </summary>
        public string PurposeOtherDescription { get; set; }
    }
}
