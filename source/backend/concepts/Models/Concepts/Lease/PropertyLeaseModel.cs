using Pims.Api.Concepts.Models.Base;
using Pims.Api.Concepts.Models.Concepts.Property;

namespace Pims.Api.Concepts.Models.Concepts.Lease
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

        public TypeModel<string> AreaUnitType { get; set; }

        #endregion
    }
}
