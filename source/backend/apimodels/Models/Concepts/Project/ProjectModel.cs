using System.Collections.Generic;
using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.FinancialCode;

namespace Pims.Api.Models.Concepts.Project
{
    /*
    * Frontend model
    * LINK @frontend/src\models\api\Project.ts:10
    */
    public class ProjectModel : BaseAuditModel
    {
        #region Properties

        /// <summary>
        /// get/set - The project id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The status type code.
        /// </summary>
        public CodeTypeModel<string> ProjectStatusTypeCode { get; set; }

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
        public CodeTypeModel<short> RegionCode { get; set; }

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
        public List<ProjectProductModel> ProjectProducts { get; set; }
        #endregion
    }
}
