using Pims.Api.Models.Base;

using Pims.Api.Models.Concepts.Product;

namespace Pims.Api.Models.Concepts.Project
{
    /*
    * Frontend model
    * LINK @frontend/src\models\api\Project.ts
    */
    public class ProjectProductModel : BaseAuditModel
    {
        #region Properties

        public long Id { get; set; }

        public long ProjectId { get; set; }

        public ProductModel Product { get; set; }

        public long ProductId { get; set; }

        public ProjectModel Project { get; set; }

        #endregion
    }
}
