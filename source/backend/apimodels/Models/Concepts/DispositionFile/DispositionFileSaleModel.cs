using System;
using System.Collections.Generic;
using Pims.Api.Models.Base;

namespace Pims.Api.Models.Concepts.DispositionFile
{
    public class DispositionFileSaleModel : BaseConcurrentModel
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
        /// Disposition Sale Removal Date.
        /// </summary>
        public DateOnly? FinalConditionRemovalDate { get; set; }

        /// <summary>
        /// Disposition Sale Completion Date.
        /// </summary>
        public DateOnly? SaleCompletionDate { get; set; }

        /// <summary>
        /// Disposition Sale Fiscal Year.
        /// </summary>
        public string SaleFiscalYear { get; set; }

        /// <summary>
        /// Disposition Sale Amount.
        /// </summary>
        public decimal? FinalSaleAmount { get; set; }

        /// <summary>
        /// Disposition Sale Realtor Commission Amount.
        /// </summary>
        public decimal? RealtorCommissionAmount { get; set; }

        /// <summary>
        /// Disposition Sale Requires GST.
        /// </summary>
        public bool? IsGstRequired { get; set; }

        /// <summary>
        /// Disposition Sale GST collected.
        /// </summary>
        public decimal? GstCollectedAmount { get; set; }

        /// <summary>
        /// Disposition Sale Net Book Amount.
        /// </summary>
        public decimal? NetBookAmount { get; set; }

        /// <summary>
        /// Disposition Sale Total Cost Amount.
        /// </summary>
        public decimal? TotalCostAmount { get; set; }

        /// <summary>
        /// Disposition Sale Net Proceeds Before Spp Amount.
        /// </summary>
        public decimal? NetProceedsBeforeSppAmount { get; set; }

        /// <summary>
        /// Disposition Sale Net Proceeds After Spp Amount.
        /// </summary>
        public decimal? NetProceedsAfterSppAmount { get; set; }

        /// <summary>
        /// Disposition Sale Spp Amount.
        /// </summary>
        public decimal? SppAmount { get; set; }

        /// <summary>
        /// Disposition Sale Remediation Amount.
        /// </summary>
        public decimal? RemediationAmount { get; set; }

        /// <summary>
        /// get/set - A list of disposition Sale Purchaser(s).
        /// </summary>
        public IList<DispositionSalePurchaserModel> DispositionPurchasers { get; set; }

        /// <summary>
        /// get/set - A list of disposition Sale Purchaser(s) Agents.
        /// </summary>
        public IList<DispositionSalePurchaserAgentModel> DispositionPurchaserAgents { get; set; }

        /// <summary>
        /// get/set - A list of disposition Sale Purchaser(s) Solicitors.
        /// </summary>
        public IList<DispositionSalePurchaserSolicitorModel> DispositionPurchaserSolicitors { get; set; }
    }
}
