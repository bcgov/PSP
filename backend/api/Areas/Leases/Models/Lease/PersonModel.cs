using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pims.Api.Areas.Lease.Models.Lease
{
    public class PersonModel
    {
        #region Properties
        /// <summary>
        /// get/set - The primary key to identify the property.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The person's first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// get/set - The person's middle name(s).
        /// </summary>
        public string MiddleNames { get; set; }

        /// <summary>
        /// get/set - The person's last name.
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// get/set - The person's concatenated full name.
        /// </summary>
        public string FullName { get; set; }
        #endregion
    }
}
