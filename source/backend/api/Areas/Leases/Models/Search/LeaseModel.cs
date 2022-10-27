using System;
using System.Collections.Generic;
using Pims.Api.Models;

namespace Pims.Api.Areas.Lease.Models.Search
{
    public class LeaseModel
    {
        #region Properties

        /// <summary>
        /// get/set - The primary key to identify the lease.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The LIS L File #.
        /// </summary>
        public string LFileNo { get; set; }

        /// <summary>
        /// get/set - The expiry date time.
        /// </summary>
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// get/set - The value of the program name.
        /// </summary>
        public string ProgramName { get; set; }

        /// <summary>
        /// get/set - The list of tenants for this lease.
        /// </summary>
        public IList<string> TenantNames { get; set; } = new List<string>();

        /// <summary>
        /// get/set - The list of programs associated to the lease.
        /// </summary>
        public IList<PropertyModel> Properties { get; set; } = new List<PropertyModel>();

        /// <summary>
        /// get/set - The status of this lease.
        /// </summary>
        public TypeModel<string> StatusType { get; set; }
        #endregion
    }
}
