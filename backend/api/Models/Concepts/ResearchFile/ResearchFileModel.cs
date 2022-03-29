using System.Collections.Generic;

namespace Pims.Api.Models.Concepts
{
    public class ResearchFileModel : BaseModel
    {
        #region Properties
        /// <summary>
        /// get/set - The model id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - True if the model is disabled.
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// get/set - The name of the research file.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// get/set - The research file properties.
        /// </summary>
        public IList<PropertyModel> Properties { get; set; }
        #endregion
    }
}
