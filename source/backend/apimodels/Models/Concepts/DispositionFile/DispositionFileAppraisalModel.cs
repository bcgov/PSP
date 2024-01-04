using System;
using System.Collections.Generic;
using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.File;
using Pims.Api.Models.Models.Concepts.DispositionFile;

/*
* Frontend model
* LINK @frontend/src\models\api\DispositionFile.ts:14
*/
namespace Pims.Api.Models.Concepts.DispositionFile
{
    public class DispositionFileAppraisalModel : FileModel
    {
        #region Properties

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

        #endregion
    }
}
