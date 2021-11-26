using System;
using System.Collections.Generic;

#nullable disable

namespace Pims.Dal.Entities
{
    public partial class PimsPropertyServiceFile
    {
        public PimsPropertyServiceFile()
        {
            PimsPropertyPropertyServiceFiles = new HashSet<PimsPropertyPropertyServiceFile>();
        }

        public long PropertyServiceFileId { get; set; }
        public string PropertyServiceFileTypeCode { get; set; }
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

        public virtual PimsPropertyServiceFileType PropertyServiceFileTypeCodeNavigation { get; set; }
        public virtual ICollection<PimsPropertyPropertyServiceFile> PimsPropertyPropertyServiceFiles { get; set; }
    }
}
