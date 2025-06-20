using Pims.Api.Models.Base;

namespace Pims.Api.Models.Concepts.ManagementActivity
{
    public class ManagementActivitySubTypeModel : BaseAuditModel
    {
        /// <summary>
        /// get/set - Management Activity Subtype Id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - Management Activity Parent Id.
        /// </summary>
        public long ManagementActivityId { get; set; }

        /// <summary>
        /// get/set - Sub-Type Code Definition.
        /// </summary>
        public CodeTypeModel<string> ManagementActivitySubtypeCode { get; set; }
    }
}
