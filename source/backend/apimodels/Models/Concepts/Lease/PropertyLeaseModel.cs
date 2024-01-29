using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.Property;

namespace Pims.Api.Models.Concepts.Lease
{
    public class PropertyLeaseModel : BaseAuditModel
    {
        #region Properties

        public long? Id { get; set; }

        public long? LeaseId { get; set; }

        public long? PropertyId { get; set; }

        public PropertyModel Property { get; set; }

        public LeaseModel Lease { get; set; }

        public string PropertyName { get; set; }

        public double? LeaseArea { get; set; }

        public CodeTypeModel<string> AreaUnitType { get; set; }

        #endregion
    }
}
