using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// Parcel class, provides an entity for the datamodel to manage parcels.
    /// </summary>
    [MotiTable("PIMS_PARCEL", "PARCEL")]
    public class Parcel : Property
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify parcel.
        /// </summary>
        [Column("PARCEL_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - The property identification number for Titled land.
        /// </summary>
        [Column("PID")]
        public int PID { get; set; }

        /// <summary>
        /// get - The friendly formated Parcel Id.
        /// </summary>
        [NotMapped]
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
        public float LandArea { get; set; }

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

        /// <summary>
        /// get/set - Provides a way to identify parcels that are not owned by the agency.
        /// </summary>
        [Column("NOT_OWNED")]
        public bool NotOwned { get; set; }

        /// <summary>
        /// get/set - A collection of buildings on this parcel.
        /// </summary>
        public ICollection<Building> Buildings { get; } = new List<Building>();

        /// <summary>
        /// get - Collection of many-to-many relational entities to support the relationship to buildings.
        /// </summary>
        public ICollection<ParcelBuilding> BuildingsManyToMany { get; } = new List<ParcelBuilding>();

        /// <summary>
        /// get - A collection of evaluations for this parcel.
        /// </summary>
        public ICollection<ParcelEvaluation> Evaluations { get; } = new List<ParcelEvaluation>();

        /// <summary>
        /// get - A collection of fiscal values for this parcel.
        /// </summary>
        public ICollection<ParcelFiscal> Fiscals { get; } = new List<ParcelFiscal>();

        /// <summary>
        /// get/set - A collection of parcels associated to this subdivision (empty if this parcel is not a subdivision).
        /// </summary>
        public ICollection<Parcel> Parcels { get; } = new List<Parcel>();

        /// <summary>
        /// get - Collection of many-to-many relational entities to support the relationship to parcels.
        /// </summary>
        public ICollection<ParcelParcel> ParcelsManyToMany { get; } = new List<ParcelParcel>();

        /// <summary>
        /// get/set - A collection of subdivisions associated to this parcel (empty if this parcel is not subdivided).
        /// </summary>
        public ICollection<Parcel> Subdivisions { get; } = new List<Parcel>();

        /// <summary>
        /// get - Collection of many-to-many relational entities to support the relationship to subdivisions.
        /// </summary>
        public ICollection<ParcelParcel> SubdivisionsManyToMany { get; } = new List<ParcelParcel>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a Parcel class.
        /// </summary>
        public Parcel()
        {
            this.PropertyTypeId = (long)PropertyTypes.Land;
        }

        /// <summary>
        /// Create a new instance of a Parcel class.
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        public Parcel(int pid, double latitude, double longitude) : base(latitude, longitude)
        {
            this.PID = pid;
            this.PropertyTypeId = (long)PropertyTypes.Land;
        }
        #endregion
    }
}
