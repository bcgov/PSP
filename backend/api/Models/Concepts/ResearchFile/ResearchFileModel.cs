using System;
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

        public string RoadName { get; set; }

        public string RoadAlias { get; set; }

        /// <summary>
        /// get/set - The R-File number for this research file.
        /// </summary>
        public string RfileNumber { get; set; }

        /// <summary>
        /// get/set - The research file status type.
        /// </summary>
        public TypeModel<string> ResearchFileStatusTypeCode { get; set; }

        /// <summary>
        /// get/set - A list of research property relationships.
        /// </summary>
        public IList<ResearchFilePropertyModel> ResearchProperties { get; set; }


        public DateTime? RequestDate { get; set; }
        public string RequestDescription { get; set; }
        public string RequestSourceDescription { get; set; }
        public string ResearchResult { get; set; }

        public DateTime? ResearchCompletionDate { get; set; }

        public bool? IsExpropriation { get; set; }
        public string ExpropriationNotes { get; set; }

        public TypeModel<string> RequestSourceType { get; set; }
        public PersonModel RequestorPerson { get; set; }
        public OrganizationModel RequestorOrganization { get; set; }
        public IList<ResearchFilePurposeModel> ResearchFilePurposes { get; set; }
        #endregion
    }
}
