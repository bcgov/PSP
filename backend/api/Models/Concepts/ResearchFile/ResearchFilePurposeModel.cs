namespace Pims.Api.Models.Concepts
{
    public class ResearchFilePurposeModel : BaseAppModel
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
