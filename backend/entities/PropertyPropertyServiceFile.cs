using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PropertyPropertyServiceFile class, provides an entity for the datamodel to manage the many-to-many relationship between properties and property service files.
    /// </summary>
    [MotiTable("PIMS_PROPERTY_PROPERTY_SERVICE_FILE", "PRPPSF")]
    public class PropertyPropertyServiceFile : BaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify property and property service file type.
        /// </summary>
        [Column("PROPERTY_PROPERTY_SERVICE_FILE_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - Foreign key to the property.
        /// </summary>
        [Column("PROPERTY_ID")]
        public long PropertyId { get; set; }

        /// <summary>
        /// get/set - The property.
        /// </summary>
        public Property Property { get; set; }

        /// <summary>
        /// get/set - Foreign key to the service file.
        /// </summary>
        [Column("PROPERTY_SERVICE_FILE_ID")]
        public long ServiceFileId { get; set; }

        /// <summary>
        /// get/set - The service file.
        /// </summary>
        public PropertyServiceFile ServiceFile { get; set; }

        /// <summary>
        /// get/set - Whether this relationship to the service file has been disabled.
        /// </summary>
        [Column("IS_DISABLED")]
        public bool? IsDisabled { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a PropertyPropertyServiceFile class.
        /// </summary>
        public PropertyPropertyServiceFile() { }

        /// <summary>
        /// Create a new instance of a PropertyPropertyServiceFile class.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="serviceFile"></param>
        public PropertyPropertyServiceFile(Property property, PropertyServiceFile serviceFile)
        {
            this.Property = property ?? throw new ArgumentNullException(nameof(property));
            this.PropertyId = property.Id;
            this.ServiceFile = serviceFile ?? throw new ArgumentNullException(nameof(serviceFile));
            this.ServiceFileId = serviceFile.Id;
        }
        #endregion
    }
}
