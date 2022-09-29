using System;
using System.Collections.Generic;
using Pims.Api.Models;
using Pims.Api.Models.Concepts;

namespace Pims.Api.Areas.Property.Models.Property
{
    /// <summary>
    /// PropertyModel class, provides a model to represent the property whether Land or Building.
    /// </summary>
    public class PropertyModel : BaseModel
    {
        #region Properties
        #region Identification

        /// <summary>
        /// get/set - The primary key to identify the property.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The property type description.
        /// </summary>
        public TypeModel<string> PropertyType { get; set; }

        /// <summary>
        /// get/set - The property anomalies.
        /// </summary>
        public IList<TypeModel<string>> Anomalies { get; set; }

        /// <summary>
        /// get/set - The tenure description.
        /// </summary>
        public IList<TypeModel<string>> Tenure { get; set; }

        /// <summary>
        /// get/set - The road type description.
        /// </summary>
        public IList<TypeModel<string>> RoadType { get; set; }

        /// <summary>
        /// get/set - The adjacent land description.
        /// </summary>
        public IList<TypeModel<string>> AdjacentLand { get; set; }

        /// <summary>
        /// get/set - The status description.
        /// </summary>
        public TypeModel<string> Status { get; set; }

        /// <summary>
        /// get/set - The data source description.
        /// </summary>
        public TypeModel<string> DataSource { get; set; }

        /// <summary>
        /// get/set - The data source effective date.
        /// </summary>
        public DateTime DataSourceEffectiveDate { get; set; }

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

        /// <summary>
        /// get/set - Whether the property is a POI.
        /// </summary>
        public bool IsPropertyOfInterest { get; set; }

        /// <summary>
        /// get/set - Whether the property is a provincial highway.
        /// </summary>
        public bool? IsProvincialPublicHwy { get; set; }

        /// <summary>
        /// get/set - Updated by user id.
        /// </summary>
        public string PphStatusUpdateUserid { get; set; }

        /// <summary>
        /// get/set - Updated on date time stamp.
        /// </summary>
        public DateTime? PphStatusUpdateTimestamp { get; set; }

        /// <summary>
        /// get/set - Updated by user guid.
        /// </summary>
        public Guid? PphStatusUpdateUserGuid { get; set; }

        /// <summary>
        /// get/set - Whether the property is a Rwy Belt Dom Patent.
        /// </summary>
        public bool? IsRwyBeltDomPatent { get; set; }

        /// <summary>
        /// get/set - Provincial Public Hwy Status.
        /// </summary>
        public string PphStatusTypeCode { get; set; }
        #endregion

        #region Address

        /// <summary>
        /// get/set - The address of the property.
        /// </summary>
        public AddressModel Address { get; set; }
        #endregion

        #region Parcel Properties

        /// <summary>
        /// get/set - A unique identifier for the titled parcel.
        /// </summary>
        public string PID { get; set; }

        /// <summary>
        /// get/set - A unique identifier for an untitled parcel.
        /// </summary>
        public int PIN { get; set; }

        /// <summary>
        /// get/set - Area Unit name.
        /// </summary>
        public TypeModel<string> AreaUnit { get; set; }

        /// <summary>
        /// get/set - District type.
        /// </summary>
        public TypeModel<short> DistrictType { get; set; }

        /// <summary>
        /// get/set - Region type.
        /// </summary>
        public TypeModel<short> RegionType { get; set; }

        /// <summary>
        /// get/set - The land area of the parcel.
        /// </summary>
        public float LandArea { get; set; }

        /// <summary>
        /// get/set - Whether the property is a volumetric parcel.
        /// </summary>
        public bool? IsVolumetricParcel { get; set; }

        /// <summary>
        /// get/set - The volumetric measurement of the parcel. Only applies if IsVolumetricParcel is true.
        /// </summary>
        public float VolumetricMeasurement { get; set; }

        /// <summary>
        /// get/set - Volumetric Unit name.
        /// </summary>
        public TypeModel<string> VolumetricUnit { get; set; }

        /// <summary>
        /// get/set - Volumetric parcel type. e.g. airspace / sub-surface.
        /// </summary>
        public TypeModel<string> VolumetricType { get; set; }

        /// <summary>
        /// get/set - The land legal description of the parcel.
        /// </summary>
        public string LandLegalDescription { get; set; }

        /// <summary>
        /// get/set - The property municipal zoning name.
        /// </summary>
        public string MunicipalZoning { get; set; }

        /// <summary>
        /// get/set - The property zoning name.
        /// </summary>
        public string Zoning { get; set; }

        /// <summary>
        /// get/set - The property zoning potential.
        /// </summary>
        public string ZoningPotential { get; set; }

        /// <summary>
        /// get/set - The property notes.
        /// </summary>
        public string Notes { get; set; }
        #endregion

        public IEnumerable<LeaseModel> Leases { get; set; } = new List<LeaseModel>();
        #endregion
    }
}
