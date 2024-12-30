using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsAcqFileTakeType class, provides an entity for the datamodel to manage acquisition file progress status types.
    /// </summary>
    public partial class PimsAcqFileTakeType : ITypeEntity<string>
    {
        /// <summary>
        /// get/set - Primary key to identify acquisition file take type status type.
        /// </summary>
        [NotMapped]
        public string Id { get => AcqFileTakeTypeCode; set => AcqFileTakeTypeCode = value; }

        /// <summary>
        /// Create a new instance of a PimsAcqFileProgessType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsAcqFileTakeType(string id)
            : this()
        {
            Id = id;
        }

        public PimsAcqFileTakeType()
        {
        }
    }
}
