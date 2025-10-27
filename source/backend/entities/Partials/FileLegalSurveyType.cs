using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsAcqFileProgessType class, provides an entity for the datamodel to manage acquisition file legal survey status types.
    /// </summary>
    public partial class PimsFileLglSrvyType : ITypeEntity<string>
    {
        /// <summary>
        /// get/set - Primary key to identify acquisition file legal survey status type.
        /// </summary>
        [NotMapped]
        public string Id { get => FileLglSrvyTypeCode; set => FileLglSrvyTypeCode = value; }

        /// <summary>
        /// Create a new instance of a PimsAcqFileProgessType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsFileLglSrvyType(string id)
            : this()
        {
            Id = id;
        }

        public PimsFileLglSrvyType()
        {
        }
    }
}
