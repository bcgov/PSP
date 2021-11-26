using System.Collections.Generic;

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
        /// <value></value>
        public string LFileNo { get; set; }

        /// <summary>
        /// get/set - The list of tenants for this lease
        /// </summary>
        /// <value></value>
        public IList<string> TenantNames { get; set; } = new List<string>();

        /// <summary>
        /// get/set - The value of the program name.
        /// </summary>
        /// <value></value>
        public string ProgramName { get; set; }

        /// <summary>
        /// get/set - The list of programs associated to the lease.
        /// </summary>
        /// <value></value>
        public IList<PropertyModel> Properties { get; set; } = new List<PropertyModel>();
        #endregion
    }
}
