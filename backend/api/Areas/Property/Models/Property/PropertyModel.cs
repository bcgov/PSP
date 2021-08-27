using System;

namespace Pims.Api.Areas.Property.Models.Property
{
    /// <summary>
    /// PropertyModel class, provides a model to represent the property whether Land or Building.
    /// </summary>
    public class PropertyModel
    {
        #region Properties
        #region Identification
        /// <summary>
        /// get/set - The primary key to identify the property.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The concurrency row version.
        /// </summary>
        /// <value></value>
        public long RowVersion { get; set; }

        /// <summary>
        /// get/set - The foreign key to the property type.
        /// </summary>
        public string PropertyType { get; set; }

        /// <summary>
        /// get/set - The foreign key to the status type.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// get/set - The foreign key to the data source type.
        /// </summary>
        public string DataSource { get; set; }

        /// <summary>
        /// get/set - The data source effective date
        /// </summary>
        public DateTime DataSourceEffectiveDate { get; set; }

        /// <summary>
        /// get/set - The foreign key to the classification type.
        /// </summary>
        public string Classification { get; set; }

        /// <summary>
        /// get/set - The foreign key to the tenure type.
        /// </summary>
        public string Tenure { get; set; }

        /// <summary>
        /// get/set - The GIS latitude location of the property.
        /// </summary>
        public double? Latitude { get; set; }

        /// <summary>
        /// get/set - The GIS latitude location of the property.
        /// </summary>
        public double? Longitude { get; set; }

        /// <summary>
        /// get/set - The property name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// get/set - The property description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// get/set - Whether the property is sensitive data.
        /// </summary>
        public bool IsSensitive { get; set; }
        #endregion

        #region Address
        /// <summary>
        /// get/set - The foreign key to the address.
        /// </summary>
        public long AddressId { get; set; }

        /// <summary>
        /// get/set - The address of the property.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// get/set - Foreign key to the region.
        /// </summary>
        public int RegionId { get; set; }

        /// <summary>
        /// get/set - The name of the region.
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// get/set - Foreign key to the district.
        /// </summary>
        public string DistrictId { get; set; }

        /// <summary>
        /// get/set - The name of the district.
        /// </summary>
        public string District { get; set; }

        /// <summary>
        /// get/set - The name of the municipality name.
        /// </summary>
        public string Municipality { get; set; }

        /// <summary>
        /// get/set - The name of the province.
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// get/set - The postal code.
        /// </summary>
        public string Postal { get; set; }
        #endregion

        #region Parcel Properties
        /// <summary>
        /// get/set - A unique identifier for the titled parcel.
        /// </summary>
        public string PID { get; set; }

        /// <summary>
        /// get/set - A unique identifier for an untitled parcel.
        /// </summary>
        public string PIN { get; set; }

        /// <summary>
        /// get/set - Foreign key to the area unit type.
        /// </summary>
        public string AreaUnit { get; set; }

        /// <summary>
        /// get/set - The land area of the parcel.
        /// </summary>
        public Single LandArea { get; set; }

        /// <summary>
        /// get/set - The land legal description of the parcel.
        /// </summary>
        public string LandLegalDescription { get; set; }

        /// <summary>
        /// get/set - The property zoning name.
        /// </summary>
        public string Zoning { get; set; }

        /// <summary>
        /// get/set - The property zoning potential.
        /// </summary>
        public string ZoningPotential { get; set; }
        #endregion
        #endregion
    }
}
