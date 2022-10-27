using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// Region class, provides an entity for the datamodel to manage regions.
    /// </summary>
    public partial class PimsRegion : ICodeEntity<short>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify region.
        /// </summary>
        [NotMapped]
        public short Id { get => RegionCode; set => RegionCode = value; }

        [NotMapped]
        public short Code { get => RegionCode; set => RegionCode = value; }

        [NotMapped]
        public string Description { get => RegionName; set => RegionName = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a Region class.
        /// </summary>
        /// <param name="name"></param>
        public PimsRegion(string name)
            : this()
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"Argument '{nameof(name)}' is required.", nameof(name));
            }

            this.RegionName = name;
        }
        #endregion
    }
}
