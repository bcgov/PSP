using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.Project;

namespace Pims.Api.Models.Concepts.ResearchFile
{
    public class ResearchFileProjectModel : BaseConcurrentModel
    {
        #region Properties

        /// <summary>
        /// get/set - The relationship id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The relationship's project.
        /// </summary>
        public ProjectModel Project { get; set; }

        /// <summary>
        /// get/set - The research project's id.
        /// </summary>
        public long? ProjectId { get; set; }

        /// <summary>
        /// get/set - The relationship's research file.
        /// </summary>
        public ResearchFileModel File { get; set; }

        /// <summary>
        /// get/set - The relationship's research file id.
        /// </summary>
        public long FileId { get; set; }

        #endregion
    }
}
