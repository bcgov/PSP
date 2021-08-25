using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PropertyServiceFileType class, provides an entity for the datamodel to manage a list of property service file types.
    /// </summary>
    [MotiTable("PIMS_PROPERTY_SERVICE_FILE_TYPE", "PRSVFT")]
    public class PropertyServiceFileType : TypeEntity<string>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify property service file type.
        /// </summary>
        [Column("PROPERTY_SERVICE_FILE_TYPE_CODE")]
        public override string Id { get; set; }

        /// <summary>
        /// get - Collection of property service files.
        /// </summary>
        public ICollection<PropertyServiceFile> ServiceFiles { get; } = new List<PropertyServiceFile>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a PropertyServiceFileType class.
        /// </summary>
        public PropertyServiceFileType() { }

        /// <summary>
        /// Create a new instance of a PropertyServiceFileType class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        public PropertyServiceFileType(string id, string description) : base(id, description)
        {
        }
        #endregion
    }
}
