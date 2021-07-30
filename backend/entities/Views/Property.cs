using NetTopologySuite.Geometries;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Pims.Dal.Entities.Views
{
    /// <summary>
    /// Property class, provides a model that represents a view in the database.
    /// </summary>
    public class Property
    {
        #region Properties
        /// <summary>
        /// get/set - The primary key IDENTITY SEED.
        /// </summary>
        [Column("ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - The concurrency row version.
        /// </summary>
        [Column("CONCURRENCY_CONTROL_NUMBER")]
        public long RowVersion { get; set; }

        /// <summary>
        /// get/set - The property type [0=Parcel, 1=Building].
        /// </summary>
        [Column("PROPERTY_TYPE_ID")]
        public PropertyTypes PropertyTypeId { get; set; }

        /// <summary>
        /// get/set - The RAEG/SPP project number.
        /// </summary>
        [Column("PROJECT_NUMBERS")]
        public string ProjectNumbers { get; set; }

        /// <summary>
        /// get/set - The foreign key to the property classification.
        /// </summary>
        [Column("CLASSIFICATION_ID")]
        public long ClassificationId { get; set; }

        /// <summary>
        /// get/set - The classification for this property.
        /// </summary>
        [Column("CLASSIFICATION")]
        public string Classification { get; set; }

        /// <summary>
        /// get/set - The foreign key to the agency that owns this property.
        /// </summary>
        [Column("AGENCY_ID")]
        public long? AgencyId { get; set; }

        /// <summary>
        /// get/set - The parent agency this property belongs to.
        /// /summary>
        [Column("AGENCY")]
        public string Agency { get; set; }

        /// <summary>
        /// get/set - The parent agency code this property belongs to.
        /// /summary>
        [Column("AGENCY_CODE")]
        public string AgencyCode { get; set; }

        /// <summary>
        /// get/set - The sub agency this property belongs to.
        /// /summary>
        [Column("SUB_AGENCY")]
        public string SubAgency { get; set; }

        /// <summary>
        /// get/set - The sub agency code this property belongs to.
        /// /summary>
        [Column("SUB_AGENCY_CODE")]
        public string SubAgencyCode { get; set; }

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
        /// get/set - The foreign key to the property address.
        /// </summary>
        [Column("ADDRESS_ID")]
        public long AddressId { get; set; }

        /// <summary>
        /// get/set - The address for this property.
        /// </summary>
        [Column("ADDRESS")]
        public string Address { get; set; }

        /// <summary>
        /// get/set - The administrative area (city, municipality, district, etc) for this property.
        /// </summary>
        [Column("ADMINISTRATIVE_AREA")]
        public string AdministrativeArea { get; set; }

        /// <summary>
        /// get/set - The address for this property.
        /// </summary>
        [Column("PROVINCE")]
        public string Province { get; set; }

        /// <summary>
        /// get/set - The address for this property.
        /// </summary>
        [Column("POSTAL")]
        public string Postal { get; set; }

        /// <summary>
        /// get/set - The location of the property.
        /// </summary>
        [Column("LOCATION")]
        public Point Location { get; set; }

        /// <summary>
        /// get/set - The property boundary polygon.
        /// </summary>
        [Column("BOUNDARY")]
        public Geometry Boundary { get; set; }

        /// <summary>
        /// get/set - Whether this property is considered sensitive and should only be visible to users who are part of the owning agency.
        /// </summary>
        [Column("IS_SENSITIVE")]
        public bool IsSensitive { get; set; }

        /// <summary>
        /// get/set - Whether the property is visible to other agencies.  This is used to make properties visible during ERP, but can be used at other times too.
        /// </summary>
        [Column("IS_VISIBLE_TO_OTHER_AGENCIES")]
        public bool IsVisibleToOtherAgencies { get; set; }

        #region Financials
        /// <summary>
        /// get/set - The most recent market value.
        /// </summary>
        [Column("MARKET", TypeName = "MONEY")]
        public decimal? Market { get; set; }

        /// <summary>
        /// get/set - The fiscal year for the market value.
        /// </summary>
        [Column("MARKET_FISCAL_YEAR")]
        public int? MarketFiscalYear { get; set; }

        /// <summary>
        /// get/set - The most recent netbook value.
        /// </summary>
        [Column("NET_BOOK", TypeName = "MONEY")]
        public decimal? NetBook { get; set; }

        /// <summary>
        /// get/set - The fiscal year netbook value.
        /// </summary>
        [Column("NET_BOOK_FISCAL_YEAR")]
        public int? NetBookFiscalYear { get; set; }

        /// <summary>
        /// get/set - The most recent assessment for the land.
        /// </summary>
        [Column("ASSESSED_LAND", TypeName = "MONEY")]
        public decimal? AssessedLand { get; set; }

        /// <summary>
        /// get/set - When the most recent assessment was taken.
        /// </summary>
        [Column("ASSESSED_LAND_DATE")]
        public DateTime? AssessedLandDate { get; set; }

        /// <summary>
        /// get/set - The most recent assessment for the building and improvements.
        /// </summary>
        [Column("ASSESSED_BUILDING", TypeName = "MONEY")]
        public decimal? AssessedBuilding { get; set; }

        /// <summary>
        /// get/set - When the most recent assessment was taken.
        /// </summary>
        [Column("ASSESSED_BUILDING_DATE")]
        public DateTime? AssessedBuildingDate { get; set; }
        #endregion

        #region Parcel Properties
        /// <summary>
        /// get/set - The property identification number for Titled land.
        /// </summary>
        [Column("PID")]
        public int? PID { get; set; }

        /// <summary>
        /// get - The friendly formated Parcel Id.
        /// </summary>
        public string ParcelIdentity { get { return this.PID > 0 ? $"{this.PID:000-000-000}" : null; } }

        /// <summary>
        /// get/set - The property identification number of Crown Lands Registry that are not Titled.
        /// </summary>
        [Column("PIN")]
        public int? PIN { get; set; }

        /// <summary>
        /// get/set - The land area.
        /// </summary>
        [Column("LAND_AREA")]
        public float? LandArea { get; set; }

        /// <summary>
        /// get/set - The land legal description.
        /// </summary>
        [Column("LAND_LEGAL_DESCRIPTION")]
        public string LandLegalDescription { get; set; }

        /// <summary>
        /// get/set - Current Parcel zoning information
        /// </summary>
        [Column("ZONING")]
        public string Zoning { get; set; }

        /// <summary>
        /// get/set - Potential future Parcel zoning information
        /// </summary>
        [Column("ZONING_POTENTIAL")]
        public string ZoningPotential { get; set; }
        #endregion

        #region Building Properties
        /// <summary>
        /// get/set - The parent parcel Id.
        /// </summary>
        [Column("PARCEL_ID")]
        public long? ParcelId { get; set; }

        /// <summary>
        /// get/set - The foreign key to the property building construction type.
        /// </summary>
        [Column("BUILDING_CONSTRUCTION_TYPE_ID")]
        public long? BuildingConstructionTypeId { get; set; }

        /// <summary>
        /// get/set - The building construction type for this property.
        /// </summary>
        [Column("BUILDING_CONSTRUCTION_TYPE")]
        public string BuildingConstructionType { get; set; }

        /// <summary>
        /// get/set - The number of floors in the building.
        /// </summary>
        [Column("BUILDING_FLOOR_COUNT")]
        public int? BuildingFloorCount { get; set; }

        /// <summary>
        /// get/set - The foreign key to the building predominant use.
        /// </summary>
        [Column("BUILDING_PREDOMINATE_USE_ID")]
        public long? BuildingPredominateUseId { get; set; }

        /// <summary>
        /// get/set - The building predominant use for this building.
        /// </summary>
        [Column("BUILDING_PREDOMINATE_USE")]
        public string BuildingPredominateUse { get; set; }

        /// <summary>
        /// get/set - The type of tenancy for this building.
        /// </summary>
        [Column("BUILDING_TENANCY")]
        public string BuildingTenancy { get; set; }

        /// <summary>
        /// get/set - The building rentable area.
        /// </summary>
        [Column("RENTABLE_AREA")]
        public float? RentableArea { get; set; }

        /// <summary>
        /// get/set - The foreign key to the building occupant type.
        /// </summary>
        [Column("BUILDING_OCCUPANT_TYPE_ID")]
        public long? BuildingOccupantTypeId { get; set; }

        /// <summary>
        /// get/set - The type of occupant for this building.
        /// </summary>
        [Column("BUILDING_OCCUPANT_TYPE")]
        public string BuildingOccupantType { get; set; }

        /// <summary>
        /// get/set - The expiry date of the currently active lease
        /// </summary>
        [Column("LEASE_EXPIRY")]
        public DateTime? LeaseExpiry { get; set; }

        /// <summary>
        /// get/set - The name of the occupant/organization
        /// </summary>
        [Column("OCCUPANT_NAME")]
        public string OccupantName { get; set; }

        /// <summary>
        /// get/set - Whether the lease on this building would be transferred if the building is sold.
        /// </summary>
        [Column("TRANSFER_LEASE_ON_SALE")]
        public bool? TransferLeaseOnSale { get; set; }
        #endregion
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a Property object.
        /// </summary>
        public Property()
        {

        }

        /// <summary>
        /// Creates a new instance of a Property object, initializes it with the specified arguments.
        /// </summary>
        /// <param name="property"></param>
        public Property(Entities.Property property)
        {
            this.Id = typeof(Parcel).IsAssignableFrom(property.GetType()) ? ((Parcel)property).Id : ((Building)property).Id;
            this.ProjectNumbers = property.ProjectNumbers;
            this.ClassificationId = property.ClassificationId;
            this.Classification = property.Classification?.Name;

            this.AgencyId = property.AgencyId;
            this.Agency = property.Agency?.ParentId.HasValue ?? false ? property.Agency.Parent?.Name : property.Agency?.Name;
            this.AgencyCode = property.Agency?.ParentId.HasValue ?? false ? property.Agency.Parent?.Code : property.Agency?.Code;
            this.SubAgency = property.Agency?.ParentId.HasValue ?? false ? property.Agency?.Name : null;
            this.SubAgencyCode = property.Agency?.ParentId.HasValue ?? false ? property.Agency?.Code : null;

            this.Name = property.Name;
            this.Description = property.Description;
            this.AddressId = property.AddressId;
            this.Address = property.Address != null ? $"{property.Address?.Address1} {property.Address?.Address2}".Trim() : null;
            this.AdministrativeArea = property.Address?.AdministrativeArea;
            this.Province = property.Address?.Province?.Name;
            this.Postal = property.Address?.Postal;
            this.Location = property.Location;
            this.Boundary = property.Boundary;
            this.IsSensitive = property.IsSensitive;
        }

        /// <summary>
        /// Creates a new instance of a Property object, initializes it with the specified arguments.
        /// </summary>
        /// <param name="parcel"></param>
        public Property(Parcel parcel) : this((Entities.Property)parcel)
        {
            this.PropertyTypeId = (PropertyTypes)parcel.PropertyTypeId;
            this.PID = parcel.PID;
            this.PIN = parcel.PIN;
            this.LandArea = parcel.LandArea;
            this.LandLegalDescription = parcel.LandLegalDescription;
            this.Zoning = parcel.Zoning;
            this.ZoningPotential = parcel.ZoningPotential;

            var assessed = parcel.Evaluations.OrderByDescending(e => e.Date).FirstOrDefault(e => e.Key == EvaluationKeys.Assessed);
            this.AssessedLand = assessed?.Value;
            this.AssessedLandDate = assessed?.Date;

            var improvements = parcel.Evaluations.OrderByDescending(e => e.Date).FirstOrDefault(e => e.Key == EvaluationKeys.Improvements);
            this.AssessedBuilding = improvements?.Value;
            this.AssessedBuildingDate = improvements?.Date;
        }

        /// <summary>
        /// Creates a new instance of a Property object, initializes it with the specified arguments.
        /// </summary>
        /// <param name="building"></param>
        public Property(Building building) : this((Entities.Property)building)
        {
            this.PropertyTypeId = (PropertyTypes)building.PropertyTypeId;
            this.PID = building.Parcels.FirstOrDefault()?.PID ?? 0;
            this.PIN = building.Parcels.FirstOrDefault()?.PIN;
            this.ParcelId = building.Parcels.FirstOrDefault()?.Id;
            this.BuildingConstructionTypeId = building.BuildingConstructionTypeId;
            this.BuildingConstructionType = building.BuildingConstructionType?.Name;
            this.BuildingFloorCount = building.BuildingFloorCount;
            this.BuildingPredominateUseId = building.BuildingPredominateUseId;
            this.BuildingPredominateUse = building.BuildingPredominateUse?.Name;
            this.BuildingTenancy = building.BuildingTenancy;
            this.RentableArea = building.RentableArea;
            this.BuildingOccupantTypeId = building.BuildingOccupantTypeId;
            this.BuildingOccupantType = building.BuildingOccupantType?.Name;
            this.LeaseExpiry = building.LeaseExpiry;
            this.OccupantName = building.OccupantName;
            this.TransferLeaseOnSale = building.TransferLeaseOnSale;

            var improvements = building.Evaluations.OrderByDescending(e => e.Date).FirstOrDefault(e => e.Key == EvaluationKeys.Assessed);
            this.AssessedBuilding = improvements?.Value;
            this.AssessedBuildingDate = improvements?.Date;
        }
        #endregion
    }
}
