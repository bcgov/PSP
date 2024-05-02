
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsFileNumberType class, provides an entity for the datamodel to manage File Number types.
    /// </summary>
    public partial class PimsFileNumberType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify File Number type.
        /// </summary>
        [NotMapped]
        public string Id { get => FileNumberTypeCode; set => FileNumberTypeCode = value; }
        #endregion
    }
}
