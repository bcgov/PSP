using System;
using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.Property;

namespace Pims.Api.Models.Concepts.Lease
{
    public class PropertyImprovementModel : BaseAuditModel
    {
        public long? Id { get; set; }

        public long? PropertyId { get; set; }

        public PropertyModel Property { get; set; }

        public CodeTypeModel<string> ImprovementTypeCode { get; set; }

        public string ImprovementName { get; set; }

        public DateOnly? ImprovementDate { get; set; }

        public CodeTypeModel<string> ImprovementStatusCode { get; set; }

        public string ImprovementDescription { get; set; }
    }
}
