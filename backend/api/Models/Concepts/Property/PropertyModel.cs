using System;
using System.Collections.Generic;
using Pims.Api.Models;
using Pims.Api.Models.Concepts;

namespace Pims.Api.Models.Concepts
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
        public IList<PropertyAnomalyModel> Anomalies { get; set; }

        /// <summary>
        /// get/set - The property tenures.
        /// </summary>
        public IList<PropertyTenureModel> Tenures { get; set; }

        /// <summary>
        /// get/set - The road type description.
        /// </summary>
        public IList<PropertyRoadModel> RoadTypes { get; set; }

        /// <summary>
        /// get/set - The adjacent land description.
        /// </summary>
        public IList<PropertyAdjacentLandModel> AdjacentLands { get; set; }

        /// <summary>
        /// get/set - The status description.
        /// </summary>
        public TypeModel<string> Status { get; set; }

        /// <summary>
        /// get/set - The data source description.
        /// </summary>
        public TypeModel<string> DataSource { get; set; }

        /// <summary>
        /// get/set - The MOTI region that this property falls under.
        /// </summary>
        public TypeModel<short> Region { get; set; }

        /// <summary>
        /// get/set - The data source effective date
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
        /// get/set - Whether the property is a provincial highway.
        /// </summary>
        public bool? IsProvincialPublicHwy { get; set; }
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
        public int? Pid { get; set; }

        /// <summary>
        /// get/set - A unique identifier for an untitled parcel.
        /// </summary>
        public int? Pin { get; set; }

        /// <summary>
        /// get/set - Area Unit name.
        /// </summary>
        public TypeModel<string> AreaUnit { get; set; }

        /// <summary>
        /// get/set - The land area of the parcel.
        /// </summary>
        public float? LandArea { get; set; }

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
        /// get/set - Volumetric parcel type. e.g. airspace / sub-surface
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
        /// get/set - The location of the property.
        /// </summary>
        public GeometryModel Location { get; set; }

        /// <summary>
        /// get/set - The property's district.
        /// </summary>
        public CodeTypeModel District { get; set; }

        /// <summary>
        /// get/set - The property notes.
        /// </summary>
        public string Notes { get; set; }
        #endregion
        #endregion
    }
}
