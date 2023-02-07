using System;
using System.Collections.Generic;

namespace Pims.Api.Models.Concepts
{
    public class ResearchFileModel : FileModel
    {
        #region Properties

        public string RoadName { get; set; }

        public string RoadAlias { get; set; }

        /// <summary>
        /// get/set - A list of research property relationships.
        /// </summary>
        public IList<ResearchFilePropertyModel> FileProperties { get; set; }

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

        public IList<ResearchFileProjectModel> ResearchFileProjects { get; set; }
        #endregion
    }
}
