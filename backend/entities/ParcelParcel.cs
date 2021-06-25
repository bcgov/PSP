using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// ParcelParcels class, provides the many-to-many relationship between parcels and parcels (generally for subdivisions).
    /// </summary>
    [MotiTable("PIMS_PARCEL_PARCEL", "PRCPRC")]
    public class ParcelParcel : BaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify the parcel parcel.
        /// </summary>
        [Column("PARCEL_PARCEL_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - Primary key: The foreign key to the parcel.
        /// </summary>
        [Column("PARCEL_ID")]
        [ForeignKey("PIMS_PARCEL_PIMS_PRCPRC_PARCEL_FK")]
        public long ParcelId { get; set; }

        /// <summary>
        /// get/set - The parcel that the subdivision is located on.
        /// </summary>
        public virtual Parcel Parcel { get; set; }

        /// <summary>
        /// get/set - Primary key: The foreign key to the subdivision parcel.
        /// </summary>
        [Column("SUBDIVISION_PARCEL_ID")]
        [ForeignKey("PIMS_PARCEL_PIMS_PRCPRC_SUBDIVISION_FK")]
        public long SubdivisionId { get; set; }

        /// <summary>
        /// get/set - The subdivision located on the parcel.
        /// </summary>
        public virtual Parcel Subdivision { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ParcelParcels object.
        /// </summary>
        public ParcelParcel() { }

        /// <summary>
        /// Creates a new instance of a ParcelParcels object, initializes with specified arguments.
        /// </summary>
        /// <param name="parcel"></param>
        /// <param name="building"></param>
        public ParcelParcel(Parcel parcel, Parcel subdivision)
        {
            this.ParcelId = parcel?.Id ?? throw new ArgumentNullException(nameof(parcel));
            this.Parcel = parcel;
            this.SubdivisionId = subdivision?.Id ?? throw new ArgumentNullException(nameof(subdivision));
            this.Subdivision = subdivision;
        }
        #endregion
    }
}
