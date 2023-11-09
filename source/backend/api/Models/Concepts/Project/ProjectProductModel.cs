
namespace Pims.Api.Models.Concepts
{
    /*
    * Frontend model
    * LINK @frontend/src\models\api\Project.ts
    */
    public class ProjectProductModel : BaseAppModel
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
