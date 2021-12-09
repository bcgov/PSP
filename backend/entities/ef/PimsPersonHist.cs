using System;

#nullable disable

namespace Pims.Dal.Entities
{
    public partial class PimsPersonHist
    {
        public long PersonHistId { get; set; }
        public DateTime EffectiveDateHist { get; set; }
        public DateTime? EndDateHist { get; set; }
        public long PersonId { get; set; }
        public string Surname { get; set; }
        public string FirstName { get; set; }
        public string MiddleNames { get; set; }
        public string NameSuffix { get; set; }
        public string PreferredName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Comment { get; set; }
        public bool IsDisabled { get; set; }
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
