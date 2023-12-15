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
    public class DispositionFileModel : FileModel
    {
        #region Properties

        /// <summary>
        /// get/set - reference number for historic program or file number (e.g.  RAEG, Acquisition File, etc.).
        /// </summary>
        public string FileReference { get; set; }

        /// <summary>
        /// The initiating document date.
        /// </summary>
        public DateTime? InitiatingDocumentDate { get; set; }

        /// <summary>
        /// The date of disposition file assignment.
        /// </summary>
        public DateTime? AssignedDate { get; set; }

        /// <summary>
        /// The date of disposition file completion.
        /// </summary>
        public DateTime? CompletionDate { get; set; }

        /// <summary>
        /// get/set - The disposition type.
        /// </summary>
        public TypeModel<string> DispositionTypeCode { get; set; }

        /// <summary>
        /// get/set - The disposition status type.
        /// </summary>
        public TypeModel<string> DispositionStatusTypeCode { get; set; }

        /// <summary>
        /// get/set - The initiating branch type.
        /// </summary>
        public TypeModel<string> InitiatingBranchTypeCode { get; set; }

        /// <summary>
        /// get/set - The initiating document type.
        /// </summary>
        public TypeModel<string> PhysicalFileStatusTypeCode { get; set; }

        /// <summary>
        /// get/set - The funding type.
        /// </summary>
        public TypeModel<string> FundingTypeCode { get; set; }

        /// <summary>
        /// get/set - The initiating document type.
        /// </summary>
        public TypeModel<string> InitiatingDocumentTypeCode { get; set; }

        /// <summary>
        /// get/set - The disposition other text.
        /// </summary>
        public string DispositionTypeOther { get; set; }

        /// <summary>
        /// get/set - The initiating document other text.
        /// </summary>
        public string InitiatingDocumentTypeOther { get; set; }

        /// <summary>
        /// get/set - The MOTI region that this disposition file falls under.
        /// </summary>
        public TypeModel<short> RegionCode { get; set; }

        /// <summary>
        /// PIMS_DISPOSITION_SALE => get/set - The Disposition Apprasided Value amount.
        /// </summary>
        public decimal? AppraisedValueAmount { get; set; }

        /// <summary>
        /// PIMS_DISPOSITION_SALE => get/set - The Date the disposition was appraised.
        /// </summary>
        public DateTime? AppraisalDate { get; set; }

        /// <summary>
        /// PIMS_DISPOSITION_SALE => get/set - BCA value amount.
        /// </summary>
        public decimal? BcaValueAmount { get; set; }

        /// <summary>
        /// PIMS_DISPOSITION_SALE => get/set - BCA roll year.
        /// </summary>
        public string BcaRollYear { get; set; }

        /// <summary>
        /// PIMS_DISPOSITION_SALE => get/set - BCA list price amount.
        /// </summary>
        public decimal? ListPriceAmount { get; set; }

        /// <summary>
        /// get/set - A list of disposition property relationships.
        /// </summary>
        public IList<DispositionFilePropertyModel> FileProperties { get; set; }

        /// <summary>
        /// get/set - A list of disposition file team relationships.
        /// </summary>
        public IList<DispositionFileTeamModel> DispositionTeam { get; set; }

        /// <summary>
        /// get/set - A list of disposition file offers.
        /// </summary>
        public IList<DispositionFileOfferModel> DispositionOffers { get; set; }

        /// <summary>
        /// get/set - A list of disposition file sales.
        /// </summary>
        public IList<DispositionFileSaleModel> DispositionSales { get; set; }
        #endregion
    }
}
