using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// District class, provides an entity for the datamodel to manage districts.
    /// </summary>
    [MotiTable("PIMS_DISTRICT", "DSTRCT")]
    public class District : BaseEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify district.
        /// </summary>
        [Column("DISTRICT_CODE")]
        public int Id { get; set; }

        /// <summary>
        /// get/set - The parent district this district belongs to.
        /// </summary>
        [Column("REGION_CODE")]
        public int RegionId { get; set; }

        /// <summary>
        /// get/set - The region.
        /// </summary>
        public Region Region { get; set; }

        /// <summary>
        /// get/set - The name of the district.
        /// </summary>
        [Column("DISTRICT_NAME")]
        public string Name { get; set; }

        /// <summary>
        /// get/set - Whether this district is disabled.
        /// </summary>
        [Column("IS_DISABLED")]
        public int IsDisabled { get; set; }

        /// <summary>
        /// get/set - The parent district this district belongs to.
        /// </summary>
        [Column("DISPLAY_ORDER")]
        public int? DisplayOrder { get; set; }

        /// <summary>
        /// get - Collection of addresses.
        /// </summary>
        public ICollection<Address> Addresses { get; } = new List<Address>();

        /// <summary>
        /// get - Collection of organization.
        /// </summary>
        public ICollection<Organization> Organizations { get; } = new List<Organization>();

        /// <summary>
        /// get - Collection of properties.
        /// </summary>
        public ICollection<Property> Properties { get; } = new List<Property>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a District class.
        /// </summary>
        public District() { }

        /// <summary>
        /// Create a new instance of a District class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="region"></param>
        public District(string name, Region region)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException($"Argument '{nameof(name)}' is required.", nameof(name));

            this.Name = name;
            this.Region = region ?? throw new ArgumentNullException(nameof(region));
            this.RegionId = region.Id;
        }
        #endregion
    }
}
