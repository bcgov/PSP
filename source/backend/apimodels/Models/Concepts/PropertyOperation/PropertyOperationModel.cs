using System;
using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.Property;

namespace Pims.Api.Models.Concepts.PropertyOperation
{
    public class PropertyOperationModel : BaseAuditModel
    {
        public long Id { get; set; }

        public long SourcePropertyId { get; set; }

        public PropertyModel SourceProperty { get; set; }

        public long DestinationPropertyId { get; set; }

        public PropertyModel DestinationProperty { get; set; }

        public long PropertyOperationNo { get; set; }

        public CodeTypeModel<string> PropertyOperationTypeCode { get; set; }

        public DateTime? OperationDt { get; set; }

        public bool? IsDisabled { get; set; }
    }
}
