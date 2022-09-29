using System;

namespace Pims.Api.Areas.Property.Models.Property
{
    public class LeaseModel
    {
        public long Id { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public string TenantName { get; set; }
    }
}
