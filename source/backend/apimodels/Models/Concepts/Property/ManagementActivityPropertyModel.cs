using Pims.Api.Models.Base;

namespace Pims.Api.Models.Concepts.Property
{
    public class ManagementActivityPropertyModel : BaseAuditModel
    {
        #region Properties

        public long Id { get; set; }

        public long ManagementActivityId { get; set; }

        public ManagementActivityModel ManagementActivity { get; set; }

        public long PropertyId { get; set; }

        public PropertyModel Property { get; set; }

        #endregion
    }
}
