using System.ComponentModel;

namespace Pims.Dal.Entities.Models
{
    public class AcquisitionFileExportModel
    {
        [DisplayName("Acquisition File #")]
        public string FileNumber { get; set; }

        [DisplayName("Historical File #")]
        public string LegacyFileNumber { get; set; }

        [DisplayName("Acquisition File Name")]
        public string FileName { get; set; }

        [DisplayName("MOTI Region")]
        public string MotiRegion { get; set; }

        [DisplayName("Ministry Project")]
        public string MinistryProject { get; set; }

        [DisplayName("Civic Address")]
        public string CivicAddress { get; set; }

        [DisplayName("General Location")]
        public string GeneralLocation { get; set; }

        [DisplayName("PID")]
        public string Pid { get; set; }

        [DisplayName("PIN")]
        public string Pin { get; set; }

        [DisplayName("Status (File)")]
        public string AcquisitionFileStatusTypeCode { get; set; }

        [DisplayName("Funding (File)")]
        public string FileFunding { get; set; }

        [DisplayName("Assigned Date")]
        public string FileAssignedDate { get; set; }

        [DisplayName("Delivery Date")]
        public string FileDeliveryDate { get; set; }

        [DisplayName("Acquisition Completed")]
        public string FileAcquisitionCompleted { get; set; }

        [DisplayName("Physical File Status")]
        public string FilePhysicalStatus { get; set; }

        [DisplayName("Acquisition Type")]
        public string FileAcquisitionType { get; set; }

        [DisplayName("Acquisition Team")]
        public string FileAcquisitionTeam { get; set; }

        [DisplayName("Owners")]
        public string FileAcquisitionOwners { get; set; }
    }
}
