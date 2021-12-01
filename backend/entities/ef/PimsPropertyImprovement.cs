﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Pims.Dal.Entities
{
    public partial class PimsPropertyImprovement
    {
        public long PropertyImprovementId { get; set; }
        public long LeaseId { get; set; }
        public string PropertyImprovementTypeCode { get; set; }
        public string ImprovementDescription { get; set; }
        public string StructureSize { get; set; }
        public string Unit { get; set; }
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

        public virtual PimsLease Lease { get; set; }
        public virtual PimsPropertyImprovementType PropertyImprovementTypeCodeNavigation { get; set; }
    }
}
