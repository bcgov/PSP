using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.Organization;
using Pims.Api.Models.Concepts.Person;

namespace Pims.Api.Models.Concepts.DispositionFile
{
    public class DispositionSalePurchaserAgentModel : BaseConcurrentModel
    {

        /// <summary>
        /// get/set - The relationship id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Parent Disposition Sale.
        /// </summary>
        public long? DispositionSaleId { get; set; }

        /// <summary>
        /// get/set - The Id of the person(s) associated with a disposition Sale as Purchaser Agent.
        /// </summary>
        public long? PersonId { get; set; }

        /// <summary>
        /// get/set - The Person associated with a disposition Sale as Purchaser Agent.
        /// </summary>
        public PersonModel Person { get; set; }

        /// <summary>
        /// get/set - The Id of the organization associated with a disposition Sale as Purchaser Agent.
        /// </summary>
        public long? OrganizationId { get; set; }

        /// <summary>
        /// get/set - The organization associated with a disposition Sale as Purchaser Agent.
        /// </summary>
        public OrganizationModel Organization { get; set; }

        /// <summary>
        /// get/set - The Id of the primary contact associated with a disposition Sale as Purchaser Agent.
        /// </summary>
        public long? PrimaryContactId { get; set; }

        /// <summary>
        /// get/set - The primary contact associated with a disposition Sale as Purchaser Agent..
        /// </summary>
        public PersonModel PrimaryContact { get; set; }
    }
}
