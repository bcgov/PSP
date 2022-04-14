using System.Collections.Generic;

namespace Pims.Api.Models.Concepts
{
    public class ResearchFileModel : BaseAppModel
    {
        #region Properties
        /// <summary>
        /// get/set - The model id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The research file name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// get/set - The R-File number for this research file.
        /// </summary>
        public string RfileNumber { get; set; }

        /// <summary>
        /// get/set - The research file status type.
        /// </summary>
        public TypeModel<string> StatusType { get; set; }

        /// <summary>
        /// get/set - A list of research property relationships.
        /// </summary>
        public IList<ResearchFilePropertyModel> ResearchProperties { get; set; }
        #endregion
    }
}
