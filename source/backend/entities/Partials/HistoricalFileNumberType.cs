using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsHistoricalFileNumberType class, provides an entity for the datamodel to manage Historical File Number types.
    /// </summary>
    public partial class PimsHistoricalFileNumberType : ITypeEntity<string>
    {
        /// <summary>
        /// get/set - Primary key to identify the File Number type.
        /// </summary>
        [NotMapped]
        public string Id { get => HistoricalFileNumberTypeCode; set => HistoricalFileNumberTypeCode = value; }

        /// <summary>
        /// Create a new instance of a PimsFileNumberType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsHistoricalFileNumberType(string id)
            : this()
        {
            Id = id;
        }

        public PimsHistoricalFileNumberType()
        {
        }
    }
}
