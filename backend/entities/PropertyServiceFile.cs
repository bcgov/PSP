using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PropertyServiceFile class, provides an entity for the datamodel to manage property service files.
    /// </summary>
    [MotiTable("PIMS_PROPERTY_SERVICE_FILE", "PRPSVC")]
    public class PropertyServiceFile : BaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify entity.
        /// </summary>
        [Column("PROPERTY_SERVICE_FILE_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - Foreign key to service file type.
        /// </summary>
        [Column("PROPERTY_SERVICE_FILE_TYPE_CODE")]
        public string FileTypeId { get; set; }

        /// <summary>
        /// get/set - The service file type.
        /// </summary>
        public PropertyServiceFileType FileType { get; set; }

        /// <summary>
        /// get - A collection of properties.
        /// </summary>
        public ICollection<Property> Properties { get; } = new List<Property>();

        /// <summary>
        /// get - A collection of properties many-to-many references.
        /// </summary>
        public ICollection<PropertyPropertyServiceFile> PropertiesManyToMany { get; } = new List<PropertyPropertyServiceFile>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a PropertyServiceFile class.
        /// </summary>
        public PropertyServiceFile() { }

        /// <summary>
        /// Create a new instance of a PropertyServiceFile class.
        /// </summary>
        /// <param name="type"></param>
        public PropertyServiceFile(PropertyServiceFileType type)
        {
            this.FileType = type ?? throw new ArgumentNullException(nameof(type));
            this.FileTypeId = type.Id;
        }
        #endregion
    }
}
