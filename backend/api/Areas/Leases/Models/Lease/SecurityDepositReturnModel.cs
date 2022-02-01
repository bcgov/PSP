using System;
using Pims.Api.Models;

namespace Pims.Api.Areas.Lease.Models.Lease
{
    public class SecurityDepositReturnModel : BaseModel
    {
        /// <summary>
        /// get/set - Primary key to identify Security Deposit Return.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - Foreign key to identify the parent Security Deposit.
        /// </summary>
        public long ParentDepositId { get; set; }

        /// <summary>
        /// get/set - Security deposit type.
        /// </summary>
        public TypeModel<string> DepositType { get; set; }

        /// <summary>
        /// get/set - The termination date of the deposit.
        /// </summary>
        public DateTime TerminationDate { get; set; }

        /// <summary>
        /// get/set - any claims made against the deposit total, reducing the returned amount.
        /// </summary>
        public decimal? ClaimsAgainst { get; set; }

        /// <summary>
        /// get/set - the deposit total, less any claims against.
        /// </summary>
        public decimal ReturnAmount { get; set; }

        /// <summary>
        /// get/set - The date when the deposit was returned.
        /// </summary>
        public DateTime ReturnDate { get; set; }

        /// <summary>
        /// get/set - The payee name.
        /// </summary>
        public string PayeeName { get; set; }

        /// <summary>
        /// get/set - The payee address.
        /// </summary>
        public string PayeeAddress { get; set; }

        /// <summary>
        /// get/set - Person deposit holder return holder.
        /// </summary>
        public PersonModel PersonDepositReturnHolder { get; set; }

        /// <summary>
        /// get/set - Person deposit holder return id.
        /// </summary>
        public long? PersonDepositReturnHolderId { get; set; }

        /// <summary>
        /// get/set - Organization deposit return holder.
        /// </summary>
        public OrganizationModel OrganizationDepositReturnHolder { get; set; }

        /// <summary>
        /// get/set - Organization deposit return holder id.
        /// </summary>
        public long? OrganizationDepositReturnHolderId { get; set; }
    }
}
