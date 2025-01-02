using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsAcqFileProgessType class, provides an entity for the datamodel to manage acquisition file progress status types.
    /// </summary>
    public partial class PimsAcqFileProgessType : ITypeEntity<string>
    {
        /// <summary>
        /// get/set - Primary key to identify acquisition file progress status type.
        /// </summary>
        [NotMapped]
        public string Id { get => AcqFileProgessTypeCode; set => AcqFileProgessTypeCode = value; }

        /// <summary>
        /// Create a new instance of a PimsAcqFileProgessType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsAcqFileProgessType(string id)
            : this()
        {
            Id = id;
        }

        public PimsAcqFileProgessType()
        {
        }
    }
}
