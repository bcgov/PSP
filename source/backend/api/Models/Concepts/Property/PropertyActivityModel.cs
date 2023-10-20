using System;

namespace Pims.Api.Models.Concepts
{
    public class PropertyActivityModel : BaseAppModel
    {
        public long Id { get; set; }

        public string ActivityTypeCode { get; set; }

        public TypeModel<string> ActivityType { get; set; }

        public string ActivitySubTypeCode { get; set; }

        public TypeModel<string> ActivitySubType { get; set; }

        public string ActivityStatusTypeCode { get; set; }

        public TypeModel<string> ActivityStatusType { get; set; }

        public DateTime RequestedAddedDate { get; set; }
    }
}
