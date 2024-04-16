using System.Collections.Generic;
using Pims.Api.Models.Concepts.File;

namespace Pims.Api.Models.Concepts.ResearchFile
{
    public class ResearchFilePropertyModel : FilePropertyModel
    {
        #region Properties

        /// <summary>
        /// get/set - Flag to mark if legal option is required.
        /// </summary>
        public bool? IsLegalOpinionRequired { get; set; }

        /// <summary>
        /// get/set - Flag to mark if legal option was obtained.
        /// </summary>
        public bool? IsLegalOpinionObtained { get; set; }

        /// <summary>
        /// get/set - Reference to the property document.
        /// </summary>
        public string DocumentReference { get; set; }

        /// <summary>
        /// get/set - Research summary text.
        /// </summary>
        public string ResearchSummary { get; set; }

        /// <summary>
        /// get/set - The relationship's research file.
        /// </summary>
        public new ResearchFileModel File { get; set; }

        /// <summary>
        /// get/set - The property's purpose types.
        /// </summary>
        public IList<PropertyPurposeModel> PurposeTypes { get; set; }
        #endregion
    }
}
