using System;

namespace Pims.Api.Models.Concepts
{
    public class SecurityDepositReturnModel : BaseModel
    {
        /// <summary>
        /// get/set - Primary key to identify Security Deposit Return.
        /// </summary>
        public long? Id { get; set; }

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
        /// get/set - the interest paid.
        /// </summary>
        public decimal InterestPaid { get; set; }

        /// <summary>
        /// get/set - The date when the deposit was returned.
        /// </summary>
        public DateTime ReturnDate { get; set; }

        /// <summary>
        /// get/set - Contact Holder.
        /// </summary>
        public ContactModel ContactHolder { get; set; }
    }
}
