using System;
using System.Collections.Generic;
using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.File;
using Pims.Api.Models.Concepts.Organization;
using Pims.Api.Models.Concepts.Person;

namespace Pims.Api.Models.Concepts.ResearchFile
{
    public class ResearchFileModel : FileModel
    {
        #region Properties

        public string RoadName { get; set; }

        public string RoadAlias { get; set; }

        /// <summary>
        /// get/set - A list of research property relationships.
        /// </summary>
        public new IList<ResearchFilePropertyModel> FileProperties { get; set; }

        public DateOnly? RequestDate { get; set; }

        public string RequestDescription { get; set; }

        public string RequestSourceDescription { get; set; }

        public string ResearchResult { get; set; }

        public DateOnly? ResearchCompletionDate { get; set; }

        public bool? IsExpropriation { get; set; }

        public string ExpropriationNotes { get; set; }

        public CodeTypeModel<string> RequestSourceType { get; set; }

        public PersonModel RequestorPerson { get; set; }

        public OrganizationModel RequestorOrganization { get; set; }

        public IList<ResearchFilePurposeModel> ResearchFilePurposes { get; set; }

        public IList<ResearchFileProjectModel> ResearchFileProjects { get; set; }
        #endregion
    }
}
