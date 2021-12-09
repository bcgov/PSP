using System;

#nullable disable

namespace Pims.Dal.Entities
{
    public partial class PimsTask
    {
        public long TaskId { get; set; }
        public long TaskTemplateId { get; set; }
        public long? ActivityId { get; set; }
        public long UserId { get; set; }
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

        public virtual PimsActivity Activity { get; set; }
        public virtual PimsTaskTemplate TaskTemplate { get; set; }
        public virtual PimsUser User { get; set; }
    }
}
