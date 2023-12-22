using System;
using Pims.Api.Models.Base;

namespace Pims.Api.Models.Models.Concepts.DispositionFile
{
    public class DispositionFileOfferModel : BaseConcurrentModel
    {
        /// <summary>
        /// get/set - The relationship id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Parent Disposition File.
        /// </summary>
        public long DispositionFileId { get; set; }

        /// <summary>
        /// Disposition Offer Status Code.
        /// </summary>
        public string DispositionOfferStatusTypeCode { get; set; }

        /// <summary>
        /// Disposition Offer Status Object.
        /// </summary>
        public TypeModel<string> DispositionOfferStatusType { get; set; }

        /// <summary>
        /// Disposition Offer Name.
        /// </summary>
        public string OfferName { get; set; }

        /// <summary>
        /// Disposition Offer Date.
        /// </summary>
        public DateOnly OfferDate { get; set; }

        /// <summary>
        /// Disposition Offer Expiration Date.
        /// </summary>
        public DateOnly? OfferExpiryDate { get; set; }

        /// <summary>
        /// Disposition Offer Amount.
        /// </summary>
        public decimal OfferAmount { get; set; }

        /// <summary>
        /// Disposition Offer Note.
        /// </summary>
        public string OfferNote { get; set; }
    }
}
