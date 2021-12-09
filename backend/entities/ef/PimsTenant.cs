using System;

#nullable disable

namespace Pims.Dal.Entities
{
    public partial class PimsTenant
    {
        public long TenantId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Settings { get; set; }
        public long ConcurrencyControlNumber { get; set; }
        public DateTime DbCreateTimestamp { get; set; }
        public string DbCreateUserid { get; set; }
        public DateTime DbLastUpdateTimestamp { get; set; }
        public string DbLastUpdateUserid { get; set; }
    }
}
