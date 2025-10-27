using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsAcqFileAppraisalType class, provides an entity for the datamodel to manage acquisition status types.
    /// </summary>
    public partial class PimsFileAppraisalType : ITypeEntity<string>
    {
        /// <summary>
        /// get/set - Primary key to identify acquisition file appraisal status type.
        /// </summary>
        [NotMapped]
        public string Id { get => FileAppraisalTypeCode; set => FileAppraisalTypeCode = value; }

        /// <summary>
        /// Create a new instance of a PimsAcqFileAppraisalType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsFileAppraisalType(string id)
            : this()
        {
            Id = id;
        }

        public PimsFileAppraisalType()
        {
        }
    }
}
