using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pims.Api.Areas.Lease.Models.Lease
{
    public class LeaseModel
    {
        #region Properties
        /// <summary>
        /// get/set - The primary key to identify the property.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The concurrency row version.
        /// </summary>
        public long RowVersion { get; set; }

        /// <summary>
        /// get/set - The value of the tenant name.
        /// </summary>
        /// <value></value>
        public string TenantName { get; set; }

        /// <summary>
        /// get/set - The value of the program name.
        /// </summary>
        /// <value></value>
        public string ProgramName { get; set; }

        /// <summary>
        /// get/set - The string value of the street address.
        /// </summary>
        /// <value></value>
        public string Address { get; set; }

        /// <summary>
        /// get/set - The LIS L File #.
        /// </summary>
        /// <value></value>
        public string LFileNo { get; set; }

        /// <summary>
        /// get/set - The calculated expiry date of the lease
        /// </summary>
        /// <value></value>
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// get/set - The receivable payment type code identifier
        /// </summary>
        /// <value></value>
        public string PaymentReceivableTypeId { get; set; }

        /// <summary>
        /// get/set - A list of persons tenants associated with this lease
        /// </summary>
        public IEnumerable<PersonModel> Persons { get; set; }

        /// <summary>
        /// get/set - A list of organization tenants associated with this lease
        /// </summary>
        public IEnumerable<OrganizationModel> Organizations { get; set; }

        /// <summary>
        /// get/set - A list of properties associated with this lease
        /// </summary>
        public IEnumerable<PropertyModel> Properties { get; set; }
        #endregion
    }
}
