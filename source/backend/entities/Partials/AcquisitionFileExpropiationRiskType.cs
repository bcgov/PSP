using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsAcqFileExpropRiskType class, provides an entity for the datamodel to manage acquisition file progress status types.
    /// </summary>
    public partial class PimsAcqFileExpropRiskType : ITypeEntity<string>
    {
        /// <summary>
        /// get/set - Primary key to identify acquisition file risk type status type.
        /// </summary>
        [NotMapped]
        public string Id { get => AcqFileExpropRiskTypeCode; set => AcqFileExpropRiskTypeCode = value; }

        /// <summary>
        /// Create a new instance of a PimsAcqFileExpropRiskType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsAcqFileExpropRiskType(string id)
            : this()
        {
            Id = id;
        }

        public PimsAcqFileExpropRiskType()
        {
        }
    }
}
