using System.Collections.Generic;
using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.File;
using Pims.Api.Models.Concepts.Product;
using Pims.Api.Models.Concepts.Project;

namespace Pims.Api.Models.Concepts.ManagementFile
{
    public class ManagementFileModel : FileModel
    {
        #region Properties

        /// <summary>
        /// get/set - The file's purpose.
        /// </summary>
        public string FilePurpose { get; set; }

        /// <summary>
        /// get/set - The file's details.
        /// </summary>
        public string AdditionalDetails { get; set; }

        /// <summary>
        /// get/set - The file's legacy file number.
        /// </summary>
        public string LegacyFileNum { get; set; }

        /// <summary>
        /// get/set - The project's id.
        /// </summary>
        public long? ProjectId { get; set; }

        /// <summary>
        /// get/set - The management project.
        /// </summary>
        public ProjectModel Project { get; set; }

        /// <summary>
        /// get/set - The product's id.
        /// </summary>
        public long? ProductId { get; set; }

        /// <summary>
        /// get/set - The product.
        /// </summary>
        public ProductModel Product { get; set; }

        /// <summary>
        /// get/set - The funding management file falls under.
        /// </summary>
        public CodeTypeModel<string> FundingTypeCode { get; set; }

        /// <summary>
        /// get/set - The program management file falls under.
        /// </summary>
        public CodeTypeModel<string> ProgramTypeCode { get; set; }

        /// <summary>
        /// get/set - A list of management file properties.
        /// </summary>
        public new IList<ManagementFilePropertyModel> FileProperties { get; set; }

        /// <summary>
        /// get/set - A list of management file team relationships.
        /// </summary>
        public IList<ManagementFileTeamModel> ManagementTeam { get; set; }

        #endregion
    }
}
