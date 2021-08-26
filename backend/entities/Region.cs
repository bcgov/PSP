using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// Region class, provides an entity for the datamodel to manage regions.
    /// </summary>
    [MotiTable("PIMS_REGION", "REGION")]
    public class Region : BaseEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify region.
        /// </summary>
        [Column("REGION_CODE")]
        public int Id { get; set; }

        /// <summary>
        /// get/set - The name of the region.
        /// </summary>
        [Column("REGION_NAME")]
        public string Name { get; set; }

        /// <summary>
        /// get/set - Whether this region is disabled.
        /// </summary>
        [Column("IS_DISABLED")]
        public int IsDisabled { get; set; }

        /// <summary>
        /// get/set - The parent region this region belongs to.
        /// </summary>
        [Column("DISPLAY_ORDER")]
        public int? DisplayOrder { get; set; }

        /// <summary>
        /// get - Collection of addresses.
        /// </summary>
        public ICollection<Address> Addresses { get; } = new List<Address>();

        /// <summary>
        /// get - Collection of districts.
        /// </summary>
        public ICollection<District> Districts { get; } = new List<District>();

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
        /// Create a new instance of a Region class.
        /// </summary>
        public Region() { }

        /// <summary>
        /// Create a new instance of a Region class.
        /// </summary>
        /// <param name="name"></param>
        public Region(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException($"Argument '{nameof(name)}' is required.", nameof(name));

            this.Name = name;
        }
        #endregion
    }
}
