using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsFileNumberType class, provides an entity for the datamodel to manage File Number types.
    /// </summary>
    public partial class PimsFileNumberType : ITypeEntity<string>
    {
        /// <summary>
        /// get/set - Primary key to identify the File Number type.
        /// </summary>
        [NotMapped]
        public string Id { get => FileNumberTypeCode; set => FileNumberTypeCode = value; }

        /// <summary>
        /// Create a new instance of a PimsFileNumberType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsFileNumberType(string id)
            : this()
        {
            Id = id;
        }

        public PimsFileNumberType()
        {
        }
    }
}
