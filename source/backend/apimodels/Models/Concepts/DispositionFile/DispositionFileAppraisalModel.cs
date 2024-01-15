using System;
using Pims.Api.Models.Base;

namespace Pims.Api.Models.Concepts.DispositionFile
{
    public class DispositionFileAppraisalModel : BaseConcurrentModel
    {
        /// <summary>
        /// get/set - The relationship id.
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// Parent Disposition File.
        /// </summary>
        public long DispositionFileId { get; set; }

        /// <summary>
        /// PIMS_DISPOSITION_APPRAISAL => get/set - The Disposition Apprasided Value amount.
        /// </summary>
        public decimal? AppraisedAmount { get; set; }

        /// <summary>
        /// PIMS_DISPOSITION_APPRAISAL => get/set - The Date the disposition was appraised.
        /// </summary>
        public DateOnly? AppraisalDate { get; set; }

        /// <summary>
        /// PIMS_DISPOSITION_APPRAISAL => get/set - BCA value amount.
        /// </summary>
        public decimal? BcaValueAmount { get; set; }

        /// <summary>
        /// PIMS_DISPOSITION_APPRAISAL => get/set - BCA roll year.
        /// </summary>
        public string BcaRollYear { get; set; }

        /// <summary>
        /// PIMS_DISPOSITION_APPRAISAL => get/set - BCA list price amount.
        /// </summary>
        public decimal? ListPriceAmount { get; set; }
    }
}
