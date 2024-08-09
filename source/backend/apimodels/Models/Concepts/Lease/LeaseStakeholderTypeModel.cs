using Pims.Api.Models.Base;

namespace Pims.Api.Models.Models.Concepts.Lease
{
    public class LeaseStakeholderTypeModel : BaseConcurrentModel
    {
        /// <summary>
        /// get/set - Stakeholder type item code value.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// get/set - Stakeholder type description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// get/set - If the stakeholder type belongs to a payable or receivable lease.
        /// </summary>
        public bool IsPayableRelated { get; set; }

        /// <summary>
        /// get/set - Whether this starkeholder type item is disabled.
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// get/set - The display order.
        /// </summary>
        public int? DisplayOrder { get; set; }
    }
}
