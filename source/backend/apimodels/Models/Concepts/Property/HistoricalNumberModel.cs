using Pims.Api.Models.Base;

namespace Pims.Api.Models.Concepts.Property
{
    public class HistoricalNumberModel : BaseConcurrentModel
    {
        public long Id { get; set; }

        public long PropertyId { get; set; }

        public PropertyModel Property { get; set; }

        public string HistoricalNumber { get; set; }

        public CodeTypeModel<string> HistoricalNumberType { get; set; }

        public string OtherHistoricalNumberType { get; set; }

        public bool? IsDisabled { get; set; }
    }
}
