using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// Building class, provides an entity for the datamodel to manage buildings.
    /// </summary>
    [MotiTable("PIMS_BUILDING", "BUILDG")]
    public class Building : Property
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify building.
        /// </summary>
        [Column("BUILDING_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - The foreign key to the property building construction type.
        /// </summary>
        [Column("BUILDING_CONSTRUCTION_TYPE_ID")]
        public long BuildingConstructionTypeId { get; set; }

        /// <summary>
        /// get/set - The building construction type for this property.
        /// </summary>
        public BuildingConstructionType BuildingConstructionType { get; set; }

        /// <summary>
        /// get/set - The number of floors in the building.
        /// </summary>
        [Column("BUILDING_FLOOR_COUNT")]
        public int BuildingFloorCount { get; set; }

        /// <summary>
        /// get/set - The foreign key to the building predominant use.
        /// </summary>
        [Column("BUILDING_PREDOMINATE_USE_ID")]
        public long BuildingPredominateUseId { get; set; }

        /// <summary>
        /// get/set - The building predominant use for this building.
        /// </summary>
        public BuildingPredominateUse BuildingPredominateUse { get; set; }

        /// <summary>
        /// get/set - The type of tenancy for this building.
        /// </summary>
        [Column("BUILDING_TENANCY")]
        public string BuildingTenancy { get; set; }

        /// <summary>
        /// get/set - The date the building tenancy was last updated.
        /// </summary>
        [Column("BUILDING_TENANCY_UPDATED_ON")]
        public DateTime? BuildingTenancyUpdatedOn { get; set; }

        /// <summary>
        /// get/set - The building rentable area.
        /// </summary>
        [Column("RENTABLE_AREA")]
        public float RentableArea { get; set; }

        /// <summary>
        /// get/set - The building total area.
        /// </summary>
        [Column("TOTAL_AREA")]
        public float TotalArea { get; set; }

        /// <summary>
        /// get/set - The foreign key to the building occupant type.
        /// </summary>
        [Column("BUILDING_OCCUPANT_TYPE_ID")]
        public long BuildingOccupantTypeId { get; set; }

        /// <summary>
        /// get/set - The type of occupant for this building.
        /// </summary>
        public BuildingOccupantType BuildingOccupantType { get; set; }

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
        public bool TransferLeaseOnSale { get; set; } = false;

        /// <summary>
        /// get/set - Metadata related to the buildings leased status.
        /// </summary>
        [Column("LEASED_LAND_METADATA")]
        public string LeasedLandMetadata { get; set; }

        /// <summary>
        /// get - A collection of parcels this building is located on.
        /// </summary>
        public ICollection<ParcelBuilding> Parcels { get; } = new List<ParcelBuilding>();

        /// <summary>
        /// get - A collection of evaluations for this building.
        /// </summary>
        /// <typeparam name="BuildingEvaluation"></typeparam>
        public ICollection<BuildingEvaluation> Evaluations { get; } = new List<BuildingEvaluation>();

        /// <summary>
        /// get - A collection of fiscal values for this building.
        /// </summary>
        /// <typeparam name="BuildingFiscals"></typeparam>
        public ICollection<BuildingFiscal> Fiscals { get; } = new List<BuildingFiscal>();

        /// <summary>
        /// get - A collection of projects this building is assocated to.
        /// </summary>
        public ICollection<ProjectProperty> Projects { get; } = new List<ProjectProperty>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a Building class.
        /// </summary>
        public Building() { }

        /// <summary>
        /// Create a new instance of a Building class.
        /// </summary>
        /// <param name="parcel"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        public Building(Parcel parcel, double latitude, double longitude) : base(latitude, longitude)
        {
            if (parcel != null)
            {
                var pb = new ParcelBuilding(parcel, this);
                this.Parcels.Add(pb);
            }
        }
        #endregion
    }
}
