using System;
using System.Collections.Generic;
using Pims.Api.Concepts.Models.Base;
using Pims.Api.Concepts.Models.Concepts.Contact;

namespace Pims.Api.Concepts.Models.Concepts.Deposit
{
    public class SecurityDepositModel : BaseConcurrentModel
    {
        /// <summary>
        /// get/set - Security deposit id.
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// get/set - Security deposit parent lease id.
        /// </summary>
        public long LeaseId { get; set; }

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
        /// get/set - Child return security deposits.
        /// </summary>
        public IList<SecurityDepositReturnModel> DepositReturns { get; set; }

        /// <summary>
        /// get/set - Contact Holder.
        /// </summary>
        public ContactModel ContactHolder { get; set; }
    }
}
