using System;
using System.Collections.Generic;
using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.Address;

namespace Pims.Api.Models.Concepts.Property
{
    /// <summary>
    /// PropertyModel class, provides a model to represent the property whether Land or Building.
    /// </summary>
    public class PropertyModel : BaseConcurrentModel
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
        public CodeTypeModel<string> PropertyType { get; set; }

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
        /// get/set - The status description.
        /// </summary>
        public CodeTypeModel<string> Status { get; set; }

        /// <summary>
        /// get/set - The data source description.
        /// </summary>
        public CodeTypeModel<string> DataSource { get; set; }

        /// <summary>
        /// get/set - The MOTI region that this property falls under.
        /// </summary>
        public CodeTypeModel<short> Region { get; set; }

        /// <summary>
        /// get/set - The property's district.
        /// </summary>
        public CodeTypeModel<short> District { get; set; }

        /// <summary>
        /// get/set - The data source effective date.
        /// </summary>
        public DateOnly DataSourceEffectiveDateOnly { get; set; }

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
        /// get/set - Whether the property is retired.
        /// </summary>
        public bool IsRetired { get; set; }

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
        public int? Pid { get; set; }

        /// <summary>
        /// get/set - A unique identifier for an untitled parcel.
        /// </summary>
        public int? Pin { get; set; }

        /// <summary>
        /// get/set - The survey plan number.
        /// </summary>
        public string PlanNumber { get; set; }

        /// <summary>
        /// get/set - Whether this parcel is owned by the ministry.
        /// </summary>
        public bool IsOwned { get; set; }

        /// <summary>
        /// get/set - Whether or not other agencies can view this property.
        /// </summary>
        public bool IsVisibleToOtherAgencies { get; set; }

        /// <summary>
        /// get/set - Area Unit name.
        /// </summary>
        public CodeTypeModel<string> AreaUnit { get; set; }

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
        public float? VolumetricMeasurement { get; set; }

        /// <summary>
        /// get/set - Volumetric Unit name.
        /// </summary>
        public CodeTypeModel<string> VolumetricUnit { get; set; }

        /// <summary>
        /// get/set - Volumetric parcel type. e.g. airspace / sub-surface.
        /// </summary>
        public CodeTypeModel<string> VolumetricType { get; set; }

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
        /// get/set - The boundary of the property.
        /// </summary>
        public PolygonModel Boundary { get; set; }

        /// <summary>
        /// get/set - General location of the property.
        /// </summary>
        public string GeneralLocation { get; set; }

        /// <summary>
        /// get/set - Property contacts.
        /// </summary>
        public IList<PropertyContactModel> PropertyContacts { get; set; }

        /// <summary>
        /// get/set - The property notes.
        /// </summary>
        public string Notes { get; set; }
        #endregion

        #region Surplus
        public CodeTypeModel<string> SurplusDeclarationType { get; set; }

        public string SurplusDeclarationComment { get; set; }

        public DateOnly SurplusDeclarationDate { get; set; }
        #endregion

        #endregion
    }
}
