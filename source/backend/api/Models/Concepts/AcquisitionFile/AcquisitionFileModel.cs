using System;
using System.Collections.Generic;

namespace Pims.Api.Models.Concepts
{
    public class AcquisitionFileModel : FileModel
    {
        #region Properties

        /// <summary>
        /// get/set - The auto-generated portion of the acquisition file number.
        /// </summary>
        public long FileNo { get; set; }

        /// <summary>
        /// get/set - A historical reference number of this file in a legacy system (likely PAIMS).
        /// </summary>
        public string LegacyFileNumber { get; set; }

        /// <summary>
        /// get/set - The ministry project number.
        /// </summary>
        public string MinistryProjectNumber { get; set; }

        /// <summary>
        /// get/set - The ministry project name.
        /// </summary>
        public string MinistryProjectName { get; set; }

        /// <summary>
        /// The assigned date.
        /// </summary>
        public DateTime? AssignedDate { get; set; }

        /// <summary>
        /// The date for delivery of the property to the project.
        /// </summary>
        public DateTime? DeliveryDate { get; set; }

        /// <summary>
        /// get/set - The acquisition physical file status type.
        /// </summary>
        public TypeModel<string> AcquisitionPhysFileStatusTypeCode { get; set; }

        /// <summary>
        /// get/set - The acquisition type.
        /// </summary>
        public TypeModel<string> AcquisitionTypeCode { get; set; }

        /// <summary>
        /// get/set - The acquisition product.
        /// </summary>
        public ProductModel Product { get; set; }

        /// <summary>
        /// get/set - The funding type.
        /// </summary>
        public TypeModel<string> FundingTypeCode { get; set; }

        /// <summary>
        /// get/set - Description of funding type if Other.
        /// </summary>
        public string FundingOther { get; set; }

        /// <summary>
        /// get/set - The acquisition project.
        /// </summary>
        public ProjectModel Project { get; set; }

        /// <summary>
        /// get/set - The MOTI region that this acquisition file falls under.
        /// </summary>
        public TypeModel<short> RegionCode { get; set; }

        /// <summary>
        /// get/set - A list of research property relationships.
        /// </summary>
        public IList<AcquisitionFilePropertyModel> FileProperties { get; set; }

        /// <summary>
        /// get/set - A list of acquisition file person relationships.
        /// </summary>
        public IList<AcquisitionFilePersonModel> AcquisitionTeam { get; set; }

        /// <summary>
        /// get/set - A list of acquisition file person relationships.
        /// </summary>
        public IList<AcquisitionFileOwnerModel> AcquisitionFileOwners { get; set; }

        /// <summary>
        /// get/set - A list of acquisition file checklist items.
        /// </summary>
        public IList<AcquisitionFileChecklistItemModel> AcquisitionFileChecklist { get; set; }
        #endregion
    }
}
