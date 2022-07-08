using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// District class, provides an entity for the datamodel to manage districts.
    /// </summary>
    public partial class PimsDistrict : ICodeEntity<short>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify district.
        /// </summary>
        [NotMapped]
        public short Id { get => DistrictCode; set => DistrictCode = value; }

        [NotMapped]
        public short Code { get => DistrictCode; set => DistrictCode = value; }

        [NotMapped]
        public string Description { get => DistrictName; set => DistrictName = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a District class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="region"></param>
        public PimsDistrict(string name, PimsRegion region)
            : this()
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"Argument '{nameof(name)}' is required.", nameof(name));
            }

            this.DistrictName = name;
            this.RegionCodeNavigation = region ?? throw new ArgumentNullException(nameof(region));
            this.RegionCode = region.RegionCode;
        }
        #endregion
    }
}
