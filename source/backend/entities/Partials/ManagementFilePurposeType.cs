using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    public partial class PimsManagementFilePurposeType : ITypeEntity<string>
    {
        /// <summary>
        /// get/set - Primary key to identify management file status type.
        /// </summary>
        [NotMapped]
        public string Id { get => ManagementFilePurposeTypeCode; set => ManagementFilePurposeTypeCode = value; }

        public PimsManagementFilePurposeType()
        {
        }

        public PimsManagementFilePurposeType(string id)
        {
            Id = id;
        }
    }
}
