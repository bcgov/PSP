using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pims.Api.Areas.Property.Models.Property
{
    public class LeaseModel
    {
        public long Id { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string TenantName { get; set; }
    }
}
