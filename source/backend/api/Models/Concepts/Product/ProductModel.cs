using System;
using System.Collections.Generic;

namespace Pims.Api.Models.Concepts
{
    public class ProductModel : BaseAppModel
    {
        #region Properties

        /// <summary>
        /// get/set - The product id.
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// get/set - The project associated to this product.
        /// </summary>
        public virtual ProjectModel ParentProject { get; set; }

        /// <summary>
        /// get/set - The project's id.
        /// </summary>
        public long? ParentProjectId { get; set; }

        /// <summary>
        /// get/set - The product associated files.
        /// </summary>
        public List<AcquisitionFileModel> AcquisitionFiles { get; set; }

        /// <summary>
        /// get/set - Product's code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// get/set - The product description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// get/set - The product start date.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// get/set - The product cost estimate.
        /// </summary>
        public decimal? CostEstimate { get; set; }

        /// <summary>
        /// get/set - The product cost estimate date.
        /// </summary>
        public DateTime? CostEstimateDate { get; set; }

        /// <summary>
        /// get/set - The product objective.
        /// </summary>
        public string Objective { get; set; }

        /// <summary>
        /// get/set - The product's scope.
        /// </summary>
        public string Scope { get; set; }
        #endregion
    }
}
