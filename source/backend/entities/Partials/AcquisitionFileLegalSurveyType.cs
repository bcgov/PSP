using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsAcqFileProgessType class, provides an entity for the datamodel to manage acquisition file legal survey status types.
    /// </summary>
    public partial class PimsAcqFileLglSrvyType : ITypeEntity<string>
    {
        /// <summary>
        /// get/set - Primary key to identify acquisition file legal survey status type.
        /// </summary>
        [NotMapped]
        public string Id { get => AcqFileLglSrvyTypeCode; set => AcqFileLglSrvyTypeCode = value; }

        /// <summary>
        /// Create a new instance of a PimsAcqFileProgessType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsAcqFileLglSrvyType(string id)
            : this()
        {
            Id = id;
        }

        public PimsAcqFileLglSrvyType()
        {
        }
    }
}
