using Pims.Api.Concepts.Models.Base;

namespace Pims.Api.Concepts.Models.Concepts.ResearchFile
{
    public class ResearchFilePurposeModel : BaseAuditModel
    {
        #region Properties

        /// <summary>
        /// get/set - Research file purpose id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// get/set - Purpose type code.
        /// </summary>
        public virtual TypeModel<string> ResearchPurposeTypeCode { get; set; }

        #endregion
    }
}
