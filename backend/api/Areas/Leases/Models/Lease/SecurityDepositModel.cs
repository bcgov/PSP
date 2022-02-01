using System;
using Pims.Api.Models;

namespace Pims.Api.Areas.Lease.Models.Lease
{
    public class SecurityDepositModel : BaseModel
    {
        /// <summary>
        /// get/set - Security deposit id.
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// get/set - Security deposit description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// get/set - Security deposit amount paid.
        /// </summary>
        public decimal AmountPaid { get; set; }

        /// <summary>
        /// get/set - Security deposit date.
        /// </summary>
        public DateTime? DepositDate { get; set; }

        /// <summary>
        /// get/set - Security deposit type.
        /// </summary>
        public TypeModel<string> DepositType { get; set; }

        /// <summary>
        /// get/set - Other type description.
        /// </summary>
        public string OtherTypeDescription { get; set; }

        /// <summary>
        /// get/set - Person deposit holder.
        /// </summary>
        public PersonModel PersonDepositHolder { get; set; }

        /// <summary>
        /// get/set - Person deposit holder id.
        /// </summary>
        public long? PersonDepositHolderId { get; set; }

        /// <summary>
        /// get/set - Organization deposit holder.
        /// </summary>
        public OrganizationModel OrganizationDepositHolder { get; set; }

        /// <summary>
        /// get/set - Organization deposit holder id.
        /// </summary>
        public long? OrganizationDepositHolderId { get; set; }
    }
}
