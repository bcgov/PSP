using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PropertyClassificationType class, provides an entity for the datamodel to manage a list of property classification types.
    /// </summary>
    public partial class PimsPropertyClassificationType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to property classification types.
        /// </summary>
        [NotMapped]
        public string Id { get => PropertyClassificationTypeCode; set => PropertyClassificationTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a PropertyClassificationType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsPropertyClassificationType(string id)
            : this()
        {
            Id = id;
        }
        #endregion
    }
}
