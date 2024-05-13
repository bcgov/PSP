using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsHistoricalFileNumberType class, provides an entity for the datamodel to manage Historical File Number types.
    /// </summary>
    public partial class PimsHistoricalFileNumberType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify File Number type.
        /// </summary>
        [NotMapped]
        public string Id { get => HistoricalFileNumberTypeCode; set => HistoricalFileNumberTypeCode = value; }
        #endregion
    }
}
