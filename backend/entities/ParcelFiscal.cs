using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// ParcelFiscal class, provides an entity to map values to a fiscal year.
    /// </summary>
    [MotiTable("PIMS_PARCEL_FISCAL", "PRFSCL")]
    public class ParcelFiscal : BaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key of the parcel fiscal.
        /// </summary>
        [Column("PARCEL_FISCAL_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - The primary key and the foreign key to the parcel.
        /// </summary>
        [Column("PARCEL_ID")]
        public long ParcelId { get; set; }

        /// <summary>
        /// get/set - The parcel.
        /// </summary>
        public Parcel Parcel { get; set; }

        /// <summary>
        /// get/set - The primary key and the fiscal year the evaluation is for.
        /// </summary>
        [Column("FISCAL_YEAR")]
        public int FiscalYear { get; set; }

        /// <summary>
        /// get/set - The effective date of this fiscal value
        /// </summary>
        [Column("EFFECTIVE_DATE")]
        public DateTime? EffectiveDate { get; set; }

        /// <summary>
        /// get/set - The key for this fiscal value.
        /// </summary>
        [Column("KEY")]
        public FiscalKeys Key { get; set; }

        /// <summary>
        /// get/set - The value of the fiscal key for this parcel.
        /// </summary>
        [Column("VALUE")]
        public decimal Value { get; set; }

        /// <summary>
        /// get/set - A note related to this fiscal value.
        /// </summary>
        [Column("NOTE")]
        public string Note { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ParcelFiscal class.
        /// </summary>
        public ParcelFiscal() { }

        /// <summary>
        /// Creates a new instance of a ParcelFiscal class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="parcel"></param>
        /// <param name="fiscalYear"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public ParcelFiscal(Parcel parcel, int fiscalYear, FiscalKeys key, decimal value)
        {
            this.ParcelId = parcel?.Id ??
                throw new ArgumentNullException(nameof(parcel));
            this.Parcel = parcel;
            this.FiscalYear = fiscalYear;
            this.Key = key;
            this.Value = value;
        }
        #endregion
    }
}
