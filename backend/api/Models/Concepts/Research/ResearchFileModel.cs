namespace Pims.Api.Models.Concepts
{
    using System;
    using System.Collections.Generic;

    public class ResearchFileModel : BaseAppModel
    {
        /// <summary>
        /// get/set - The model id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The research file status type.
        /// </summary>
        public TypeModel<string> ResearchFileStatusTypeCode { get; set; }

        /// <summary>
        /// get/set - The research file name.
        /// </summary>
        public string Name { get; set; }

        public string RoadName { get; set; }

        public string RoadAlias { get; set; }

        /// <summary>
        /// get/set - The R-File number for this research file.
        /// </summary>
        public string RfileNumber { get; set; }

        /// <summary>
        /// get/set - A list of research property relationships.
        /// </summary>
        public IList<ResearchFilePropertyModel> ResearchProperties { get; set; }
    }
}
