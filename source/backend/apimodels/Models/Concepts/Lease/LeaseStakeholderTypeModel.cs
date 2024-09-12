using Pims.Api.Models.Concepts.Address;

namespace Pims.Api.Models.Concepts.Lease
{
    public class LeaseStakeholderTypeModel : CodeTypeModel
    {
        /// <summary>
        /// get/set - If the stakeholder type belongs to a payable or receivable lease.
        /// </summary>
        public bool IsPayableRelated { get; set; }

        /// <summary>
        /// get/set - Whether this starkeholder type item is disabled.
        /// </summary>
        public bool IsDisabled { get; set; }
    }
}
