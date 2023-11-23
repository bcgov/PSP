using System.Collections.Generic;
using Pims.Api.Concepts.Models.Base;
using Pims.Api.Concepts.Models.Concepts.Property;

namespace Pims.Api.Concepts.Models.Concepts.ResearchFile
{
    public class ResearchFilePropertyModel : BaseConcurrentModel
    {
        #region Properties

        /// <summary>
        /// get/set - The relationship id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The name of the property for this research file.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// get/set - The order to display the relationship.
        /// </summary>
        public int? DisplayOrder { get; set; }

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
        /// get/set - The relationship's property.
        /// </summary>
        public PropertyModel Property { get; set; }

        /// <summary>
        /// get/set - The relationship's research file.
        /// </summary>
        public ResearchFileModel File { get; set; }

        /// <summary>
        /// get/set - The property's purpose types.
        /// </summary>
        public IList<PropertyPurposeModel> PurposeTypes { get; set; }
        #endregion
    }
}
