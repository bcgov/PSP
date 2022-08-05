using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PropertyServiceFileType class, provides an entity for the datamodel to manage a list of property service file types.
    /// </summary>
    public partial class PimsPropertyServiceFileType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify property service file type.
        /// </summary>
        [NotMapped]
        public string Id { get => PropertyServiceFileTypeCode; set => PropertyServiceFileTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a PropertyServiceFileType class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        public PimsPropertyServiceFileType(string id)
            : this()
        {
            Id = id;
        }
        #endregion
    }
}
