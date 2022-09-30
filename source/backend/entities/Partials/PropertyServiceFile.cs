using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PropertyServiceFile class, provides an entity for the datamodel to manage property service files.
    /// </summary>
    public partial class PimsPropertyServiceFile : IBaseAppEntity
    {
        #region Properties

        /// <summary>
        /// get - A collection of properties.
        /// </summary>
        [NotMapped]
        public ICollection<PimsProperty> Properties { get; } = new List<PimsProperty>();
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a PropertyServiceFile class.
        /// </summary>
        /// <param name="type"></param>
        public PimsPropertyServiceFile(PimsPropertyServiceFileType type)
            : this()
        {
            this.PropertyServiceFileTypeCodeNavigation = type ?? throw new ArgumentNullException(nameof(type));
            this.PropertyServiceFileTypeCode = type.Id;
        }
        #endregion
    }
}
