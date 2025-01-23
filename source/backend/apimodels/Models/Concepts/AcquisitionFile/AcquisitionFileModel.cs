using System;
using System.Collections.Generic;
using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.CompensationRequisition;
using Pims.Api.Models.Concepts.File;
using Pims.Api.Models.Concepts.InterestHolder;
using Pims.Api.Models.Concepts.Product;
using Pims.Api.Models.Concepts.Project;

namespace Pims.Api.Models.Concepts.AcquisitionFile
{
    public class AcquisitionFileModel : FileWithChecklistModel
    {
        #region Properties

        /// <summary>
        ///  get/set - Link to the parent acquisition file. Only used for sub-files.
        /// </summary>
        public long? ParentAcquisitionFileId { get; set; }

        /// <summary>
        /// get/set - The auto-generated portion of the acquisition file number.
        /// </summary>
        public long FileNo { get; set; }

        /// <summary>
        /// get/set - The File number Suffix.
        /// </summary>
        public short FileNumberSuffix { get; set; }

        /// <summary>
        /// get/set - A historical reference number of this file in a legacy system (likely PAIMS).
        /// </summary>
        public string LegacyFileNumber { get; set; }

        /// <summary>
        /// The assigned date.
        /// </summary>
        public DateOnly? AssignedDate { get; set; }

        /// <summary>
        /// The date for delivery of the property to the project.
        /// </summary>
        public DateOnly? DeliveryDate { get; set; }

        /// <summary>
        /// Estimated date by which the acquisition would be completed.
        /// </summary>
        public DateOnly? EstimatedCompletionDate { get; set; }

        /// <summary>
        /// Date of possession following acquisition completion.
        /// </summary>
        public DateOnly? PossessionDate { get; set; }

        /// <summary>
        /// get/set - The list of progress statuses for this file.
        /// </summary>
        public IList<AcquisitionFileProgressStatusesModel> AcquisitionFileProgressStatuses { get; set; }

        /// <summary>
        /// get/set - The acquisition file appraisal status type.
        /// </summary>
        public CodeTypeModel<string> AcquisitionFileAppraisalStatusTypeCode { get; set; }

        /// <summary>
        /// get/set - The acquisition file Legal Survey status type.
        /// </summary>
        public CodeTypeModel<string> AcquisitionFileLegalSurveyStatusTypeCode { get; set; }

        /// <summary>
        /// get/set - The list of Taking types statuses for this file.
        /// </summary>
        public IList<AcquisitionFileTakingStatusesModel> AcquisitionFileTakingStatuses { get; set; }

        /// <summary>
        /// get/set - The acquisition file Risk level status type.
        /// </summary>
        public CodeTypeModel<string> AcquisitionFileExpropiationRiskStatusTypeCode { get; set; }

        /// <summary>
        /// get/set - The acquisition physical file status type.
        /// </summary>
        public CodeTypeModel<string> AcquisitionPhysFileStatusTypeCode { get; set; }

        /// <summary>
        /// get/set - The acquisition type.
        /// </summary>
        public CodeTypeModel<string> AcquisitionTypeCode { get; set; }

        /// <summary>
        /// get/set - The acquisition product's id.
        /// </summary>
        public long? ProductId { get; set; }

        /// <summary>
        /// get/set - The acquisition product.
        /// </summary>
        public ProductModel Product { get; set; }

        /// <summary>
        /// get/set - The funding type.
        /// </summary>
        public CodeTypeModel<string> FundingTypeCode { get; set; }

        /// <summary>
        /// get/set - Description of funding type if Other.
        /// </summary>
        public string FundingOther { get; set; }

        /// <summary>
        /// get/set - The acquisition project's id.
        /// </summary>
        public long? ProjectId { get; set; }

        /// <summary>
        /// get/set - The acquisition project.
        /// </summary>
        public ProjectModel Project { get; set; }

        /// <summary>
        /// get/set - The MOTI region that this acquisition file falls under.
        /// </summary>
        public CodeTypeModel<short> RegionCode { get; set; }

        /// <summary>
        /// get/set - The SubfileInterestTypeCode that this acquisition file falls under.
        /// </summary>
        public CodeTypeModel<string> SubfileInterestTypeCode { get; set; }

        /// <summary>
        /// get/set - Description wheb 'Other' SubfileInterestTypeCode.
        /// </summary>
        public string OtherSubfileInterestType { get; set; }

        /// <summary>
        /// get/set - List of Legacy Stakeholders.
        /// </summary>
        public IList<string> LegacyStakeholders { get; set; }

        /// <summary>
        /// get/set - A list of research property relationships.
        /// </summary>
        public new IList<AcquisitionFilePropertyModel> FileProperties { get; set; }

        /// <summary>
        /// get/set - A list of acquisition file team relationships.
        /// </summary>
        public IList<AcquisitionFileTeamModel> AcquisitionTeam { get; set; }

        /// <summary>
        /// get/set - A list of acquisition file person relationships.
        /// </summary>
        public IList<AcquisitionFileOwnerModel> AcquisitionFileOwners { get; set; }

        /// <summary>
        /// get/set - A list of acquisition file interest holder relationships.
        /// </summary>
        public IList<InterestHolderModel> AcquisitionFileInterestHolders { get; set; }

        /// <summary>
        /// get/set - A list of compensation requisitions related to the acquisition file.
        /// </summary>
        public IList<CompensationRequisitionModel> CompensationRequisitions { get; set; }

        #endregion
    }
}
