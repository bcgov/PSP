using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.Property;

namespace Pims.Api.Models.Concepts.Lease
{
    public class PropertyImprovementModel : BaseAuditModel
    {
        public long? Id { get; set; }

        public long? PropertyId { get; set; }

        public PropertyModel Property { get; set; }

        public string ImprovementDescription { get; set; }

        public CodeTypeModel<string> PropertyImprovementTypeCode { get; set; }
    }
}
