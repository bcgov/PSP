using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// Property class, provides an entity for the datamodel to manage properties.
    /// </summary>
    public abstract class Property : BaseAppEntity
    {
        #region Properties
        #region Identity
        /// <summary>
        /// get/set - The id referencing the type of the property. Type is managed by business logic.
        /// </summary>
        [Column("PROPERTY_TYPE_ID")]
        public long PropertyTypeId { get; set; }

        /// <summary>
        /// get/set - The type of the property. Type is managed by business logic.
        /// </summary>
        public PropertyType PropertyType { get; set; }

        /// <summary>
        /// get/set - The RAEG/SPP project numbers.
        /// </summary>
        [Column("PROJECT_NUMBERS")]
        public string ProjectNumbers { get; set; }

        /// <summary>
        /// get/set - The property name.
        /// </summary>
        [Column("NAME")]
        public string Name { get; set; }

        /// <summary>
        /// get/set - The property description.
        /// </summary>
        [Column("DESCRIPTION")]
        public string Description { get; set; }

        /// <summary>
        /// get/set - The foreign key to the property classification.
        /// </summary>
        [Column("PROPERTY_CLASSIFICATION_ID")]
        public long ClassificationId { get; set; }

        /// <summary>
        /// get/set - The classification for this property.
        /// </summary>
        public PropertyClassification Classification { get; set; }

        /// <summary>
        /// get/set - The encumbrance reason for this property.
        /// </summary>
        [Column("ENCUMBRANCE_REASON")]
        public string EncumbranceReason { get; set; }

        /// <summary>
        /// get/set - The foreign key to the agency that owns this property.
        /// </summary>
        [Column("AGENCY_ID")]
        public long? AgencyId { get; set; }

        /// <summary>
        /// get/set - The agency this property belongs to.
        /// /summary>
        public Agency Agency { get; set; }
        #endregion

        #region Location
        /// <summary>
        /// get/set - The foreign key to the property address.
        /// </summary>
        [Column("ADDRESS_ID")]
        public long AddressId { get; set; }

        /// <summary>
        /// get/set - The address for this property.
        /// </summary>
        public Address Address { get; set; }

        /// <summary>
        /// get/set - The longitude (x), latitude (y) location of the property.
        /// </summary>
        [Column("LOCATION")]
        public Point Location { get; set; }

        /// <summary>
        /// get/set - The property boundary polygon.
        /// </summary>
        [Column("BOUNDARY")]
        public Geometry Boundary { get; set; }
        #endregion

        /// <summary>
        /// get/set - Whether this property is considered sensitive and should only be visible to users who are part of the owning agency.
        /// </summary>
        [Column("IS_SENSITIVE")]
        public bool IsSensitive { get; set; }

        /// <summary>
        /// get/set - Whether the property is visible to other agencies.  This is used to make properties visible during ERP, but can be used at other times too.
        /// </summary>
        [Column("IS_VISIBLE_TO_OTHER_AGENCIES")]
        public bool IsVisibleToOtherAgencies { get; set; } // TODO: This might be removable at this point.
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a Property class.
        /// </summary>
        public Property()
        {
            this.ClassificationId = 1L;
        }

        /// <summary>
        /// Create a new instance of a Property class.
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        public Property(double lat, double lng) : this()
        {
            this.Location = new Point(lng, lat) { SRID = 4326 };
        }
        #endregion
    }
}
