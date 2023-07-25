using System.Collections.Generic;

namespace Pims.Api.Models.Concepts
{
    public class ProjectModel : BaseAppModel
    {
        #region Properties

        /// <summary>
        /// get/set - The project id.
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// get/set - The status type code.
        /// </summary>
        public TypeModel<string> ProjectStatusTypeCode { get; set; }

        /// <summary>
        /// get/set - Business function code.
        /// </summary>
        public FinancialCodeModel BusinessFunctionCode { get; set; }

        /// <summary>
        /// get/set - Cost type code.
        /// </summary>
        public FinancialCodeModel CostTypeCode { get; set; }

        /// <summary>
        /// get/set - Work activity code.
        /// </summary>
        public FinancialCodeModel WorkActivityCode { get; set; }

        /// <summary>
        /// get/set - The region code.
        /// </summary>
        public TypeModel<short> RegionCode { get; set; }

        /// <summary>
        /// get/set - The project code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// get/set - Project description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// get/set - Project notes.
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// get/set - Project products.
        /// </summary>
        public List<ProductModel> Products { get; set; }
        #endregion
    }
}
