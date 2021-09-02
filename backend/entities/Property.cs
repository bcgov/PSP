using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// Property class, provides an entity for the datamodel to manage properties.
    /// </summary>
    [MotiTable("PIMS_PROPERTY", "PRPRTY")]
    public class Property : BaseAppEntity
    {
        #region Properties
        #region Identity
        /// <summary>
        /// get/set - Primary key to uniquely identify the property.
        /// </summary>
        [Column("PROPERTY_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - A unique identifier of titled property.
        /// </summary>
        [Column("PID")]
        public int PID { get; set; }

        /// <summary>
        /// get - The friendly formated Parcel Id.
        /// </summary>
        [NotMapped]
        public string ParcelIdentity { get { return this.PID > 0 ? $"{this.PID:000-000-000}" : null; } }

        /// <summary>
        /// get/set - A unique identifier of untitled property.
        /// </summary>
        [Column("PIN")]
        public int? PIN { get; set; }

        /// <summary>
        /// get/set - Foreign key to the property type.
        /// </summary>
        [Column("PROPERTY_TYPE_CODE")]
        public string PropertyTypeId { get; set; }

        /// <summary>
        /// get/set - The type of the property.
        /// </summary>
        public PropertyType PropertyType { get; set; }

        /// <summary>
        /// get/set - Foreign key to the property status type.
        /// </summary>
        [Column("PROPERTY_STATUS_TYPE_CODE")]
        public string StatusId { get; set; }

        /// <summary>
        /// get/set - The status type of the property.
        /// </summary>
        public PropertyStatusType Status { get; set; }

        /// <summary>
        /// get/set - Foreign key to the property data source type.
        /// </summary>
        [Column("PROPERTY_DATA_SOURCE_TYPE_CODE")]
        public string DataSourceId { get; set; }

        /// <summary>
        /// get/set - The data source type of the property.
        /// </summary>
        public PropertyDataSourceType DataSource { get; set; }

        /// <summary>
        /// get/set - The effective date of the data source.
        /// </summary>
        [Column("PROPERTY_DATA_SOURCE_EFFECTIVE_DATE")]
        public DateTime DataSourceEffectiveDate { get; set; }

        /// <summary>
        /// get/set - Foreign key to the property classification type.
        /// </summary>
        [Column("PROPERTY_CLASSIFICATION_TYPE_CODE")]
        public string ClassificationId { get; set; }

        /// <summary>
        /// get/set - The classification type.
        /// </summary>
        public PropertyClassificationType Classification { get; set; }

        /// <summary>
        /// get/set - Foreign key to the property tenure type.
        /// </summary>
        [Column("PROPERTY_TENURE_TYPE_CODE")]
        public string TenureId { get; set; }

        /// <summary>
        /// get/set - The tenure type.
        /// </summary>
        public PropertyTenureType Tenure { get; set; }


        /// <summary>
        /// get/set - A friendly name to identify the property.
        /// </summary>
        [Column("NAME")]
        public string Name { get; set; }

        /// <summary>
        /// get/set - A description of the property.
        /// </summary>
        [Column("DESCRIPTION")]
        public string Description { get; set; }
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
        /// get/set - The foreign key to the address region.
        /// </summary>
        [Column("REGION_CODE")]
        public int RegionId { get; set; }

        /// <summary>
        /// get/set - The address for this address region.
        /// </summary>
        public Region Region { get; set; }

        /// <summary>
        /// get/set - The foreign key to the address district.
        /// </summary>
        [Column("DISTRICT_CODE")]
        public int DistrictId { get; set; }

        /// <summary>
        /// get/set - The address for this address district.
        /// </summary>
        public District District { get; set; }

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
        /// get/set - Foreign key to the property area unit type.
        /// </summary>
        [Column("PROPERTY_AREA_UNIT_TYPE_CODE")]
        public string AreaUnitId { get; set; }

        /// <summary>
        /// get/set - The area unit type.
        /// </summary>
        public PropertyAreaUnitType AreaUnit { get; set; }

        /// <summary>
        /// get/set - The land area of the property.
        /// </summary>
        [Column("LAND_AREA")]
        public Single LandArea { get; set; }

        /// <summary>
        /// get/set - The land legal description of the property.
        /// </summary>
        [Column("LAND_LEGAL_DESCRIPTION")]
        public string LandLegalDescription { get; set; }

        /// <summary>
        /// get/set - The encumbrance reason.
        /// </summary>
        [Column("ENCUMBRANCE_REASON")]
        public string EncumbranceReason { get; set; }

        /// <summary>
        /// get/set - Whether this property is considered sensitive and should only be visible to users who are part of the owning organization.
        /// </summary>
        [Column("IS_SENSITIVE")]
        public bool IsSensitive { get; set; }

        /// <summary>
        /// get/set - Whether the property is currently owned by the ministry.
        /// </summary>
        [Column("IS_OWNED")]
        public bool IsOwned { get; set; }

        /// <summary>
        /// get/set - Whether this property is a property of interest.
        /// </summary>
        [Column("IS_PROPERTY_OF_INTEREST")]
        public bool IsPropertyOfInterest { get; set; }

        /// <summary>
        /// get/set - Whether this property is visible to other agencies.
        /// </summary>
        [Column("IS_VISIBLE_TO_OTHER_AGENCIES")]
        public bool IsVisibleToOtherAgencies { get; set; }

        /// <summary>
        /// get/set - The current zoning.
        /// </summary>
        [Column("ZONING")]
        public string Zoning { get; set; }

        /// <summary>
        /// get/set - The potential zoning.
        /// </summary>
        [Column("ZONING_POTENTIAL")]
        public string ZoningPotential { get; set; }

        /// <summary>
        /// get - A collection of property service files.
        /// </summary>
        public ICollection<PropertyServiceFile> ServiceFiles { get; } = new List<PropertyServiceFile>();

        /// <summary>
        /// get - A collection of many-to-many property service files.
        /// </summary>
        public ICollection<PropertyPropertyServiceFile> ServiceFilesManyToMany { get; } = new List<PropertyPropertyServiceFile>();

        /// <summary>
        /// get - A collection of organizations.
        /// </summary>
        public ICollection<Organization> Organizations { get; } = new List<Organization>();

        /// <summary>
        /// get - A collection of many-to-many organizations.
        /// </summary>
        public ICollection<PropertyOrganization> OrganizationsManyToMany { get; } = new List<PropertyOrganization>();

        /// <summary>
        /// get - A collection of projects.
        /// </summary>
        public ICollection<Project> Projects { get; } = new List<Project>();

        /// <summary>
        /// get - A collection of many-to-many organizations.
        /// </summary>
        public ICollection<ProjectProperty> ProjectsManyToMany { get; } = new List<ProjectProperty>();

        /// <summary>
        /// get - A collection of property project activities.
        /// </summary>
        public ICollection<ProjectActivity> ProjectActivities { get; } = new List<ProjectActivity>();

        /// <summary>
        /// get - A collection of many-to-many property project activities.
        /// </summary>
        public ICollection<PropertyProjectActivity> ProjectActivitiesManyToMany { get; } = new List<PropertyProjectActivity>();

        /// <summary>
        /// get - A collection of property evaluations.
        /// </summary>
        public ICollection<PropertyEvaluation> Evaluations { get; } = new List<PropertyEvaluation>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a Property class.
        /// </summary>
        public Property()
        {
        }

        /// <summary>
        /// Create a new instance of a Property class.
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="type"></param>
        /// <param name="classification"></param>
        /// <param name="address"></param>
        /// <param name="tenure"></param>
        /// <param name="areaUnit"></param>
        /// <param name="dataSource"></param>
        /// <param name="dataSourceEffectiveDate"></param>
        public Property(int pid, PropertyType type, PropertyClassificationType classification, Address address, PropertyTenureType tenure, PropertyAreaUnitType areaUnit, PropertyDataSourceType dataSource, DateTime dataSourceEffectiveDate) : this()
        {
            this.PID = pid;
            this.PropertyType = type ?? throw new ArgumentNullException(nameof(type));
            this.PropertyTypeId = type.Id;
            this.Classification = classification ?? throw new ArgumentNullException(nameof(classification));
            this.ClassificationId = classification.Id;
            this.Address = address ?? throw new ArgumentNullException(nameof(address));
            this.AddressId = address.Id;
            this.Region = address.Region ?? throw new ArgumentException($"Argument '{nameof(address)}.{nameof(address.Region)}' is required.", nameof(address));
            this.RegionId = address.RegionId.Value;
            this.District = address.District ?? throw new ArgumentException($"Argument '{nameof(address)}.{nameof(address.District)}' is required.", nameof(address));
            this.DistrictId = address.DistrictId.Value;
            this.Tenure = tenure ?? throw new ArgumentNullException(nameof(tenure));
            this.TenureId = tenure.Id;
            this.AreaUnit = areaUnit ?? throw new ArgumentNullException(nameof(areaUnit));
            this.AreaUnitId = areaUnit.Id;
            if (address.Longitude.HasValue && address.Latitude.HasValue)
                this.Location = new Point(address.Longitude.Value, address.Latitude.Value) { SRID = 4326 };
            this.DataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
            this.DataSourceId = dataSource.Id;
            this.DataSourceEffectiveDate = dataSourceEffectiveDate;
        }
        #endregion
    }
}
