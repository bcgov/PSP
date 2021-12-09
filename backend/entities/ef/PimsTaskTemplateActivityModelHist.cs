using System;

#nullable disable

namespace Pims.Dal.Entities
{
    public partial class PimsTaskTemplateActivityModelHist
    {
        public long TaskTemplateActivityModelHistId { get; set; }
        public DateTime EffectiveDateHist { get; set; }
        public DateTime? EndDateHist { get; set; }
        public long TaskTemplateActivityModelId { get; set; }
        public long TaskTemplateId { get; set; }
        public long ActivityModelId { get; set; }
        public bool IsMandatory { get; set; }
        public short ImplementationOrder { get; set; }
        public bool? IsDisabled { get; set; }
        public long ConcurrencyControlNumber { get; set; }
        public DateTime AppCreateTimestamp { get; set; }
        public string AppCreateUserid { get; set; }
        public Guid? AppCreateUserGuid { get; set; }
        public string AppCreateUserDirectory { get; set; }
        public DateTime AppLastUpdateTimestamp { get; set; }
        public string AppLastUpdateUserid { get; set; }
        public Guid? AppLastUpdateUserGuid { get; set; }
        public string AppLastUpdateUserDirectory { get; set; }
        public DateTime DbCreateTimestamp { get; set; }
        public string DbCreateUserid { get; set; }
        public DateTime DbLastUpdateTimestamp { get; set; }
        public string DbLastUpdateUserid { get; set; }
    }
}
