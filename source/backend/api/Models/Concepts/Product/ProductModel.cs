using System;
using System.Collections.Generic;

namespace Pims.Api.Models.Concepts
{
    /*
    * Front end model
    * LINK @frontend/src\models\api\Project.ts
    */
    public class ProductModel : BaseAppModel
    {
        #region Properties

        /// <summary>
        /// get/set - The product id.
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// get/set - The project associations to this product.
        /// </summary>
        public List<ProjectProductModel> ProjectProducts { get; set; }

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
        public DateOnly? StartDate { get; set; }

        /// <summary>
        /// get/set - The product cost estimate.
        /// </summary>
        public decimal? CostEstimate { get; set; }

        /// <summary>
        /// get/set - The product cost estimate date.
        /// </summary>
        public DateOnly? CostEstimateDate { get; set; }

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
