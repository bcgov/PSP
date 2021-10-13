using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pims.Api.Areas.Lease.Models.Lease
{
    public class OrganizationModel
    {
        #region Properties
        /// <summary>
        /// get/set - The primary key to identify the property.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The organization's company name.
        /// </summary>
        public string Name { get; set; }
        #endregion
    }
}
