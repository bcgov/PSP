using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// TakeSiteContamType class, provides an entity for the datamodel to manage a list of take site contamination types.
    /// </summary>
    public partial class PimsTakeSiteContamType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify take site contamination type.
        /// </summary>
        [NotMapped]
        public string Id { get => TakeSiteContamTypeCode; set => TakeSiteContamTypeCode = value; }
        [NotMapped]
        public bool? IsDisabled { get; set; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a PimsTakeSiteContamType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsTakeSiteContamType(string id)
            : this()
        {
            Id = id;
        }
        #endregion
    }
}
