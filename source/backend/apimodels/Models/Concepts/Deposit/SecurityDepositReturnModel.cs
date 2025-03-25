using System;
using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.Contact;

namespace Pims.Api.Models.Concepts.Deposit
{
    public class SecurityDepositReturnModel : BaseConcurrentModel
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
        public CodeTypeModel<string> DepositType { get; set; }

        /// <summary>
        /// get/set - The termination date of the deposit.
        /// </summary>
        public DateOnly TerminationDate { get; set; }

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
        public decimal? InterestPaid { get; set; }

        /// <summary>
        /// get/set - The date when the deposit was returned.
        /// </summary>
        public DateOnly ReturnDate { get; set; }

        /// <summary>
        /// get/set - Contact Holder.
        /// </summary>
        public ContactModel ContactHolder { get; set; }
    }
}
