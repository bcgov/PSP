using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// ParcelEvaluation class, provides an entity to map parcel evaluation values to a date.
    /// </summary>
    [MotiTable("PIMS_PARCEL_EVALUATION", "PREVAL")]
    public class ParcelEvaluation : BaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify parcel evaluation.
        /// </summary>
        [Column("PARCEL_EVALUATION_ID")]
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
        /// get/set - The primary key and the date the evaluation is for.
        /// </summary>
        [Column("DATE")]
        public DateTime Date { get; set; }

        /// <summary>
        /// get/set - the firm that performed this evaluation.
        /// </summary>
        [Column("FIRM")]
        public string Firm { get; set; }

        /// <summary>
        /// get/set - The key for this fiscal value.
        /// </summary>
        [Column("KEY")]
        public EvaluationKeys Key { get; set; }

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
        /// Creates a new instance of a ParcelEvaluation class.
        /// </summary>
        public ParcelEvaluation() { }

        /// <summary>
        /// Creates a new instance of a ParcelEvaluation class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="parcel"></param>
        /// <param name="date"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public ParcelEvaluation(Parcel parcel, DateTime date, EvaluationKeys key, decimal value)
        {
            this.ParcelId = parcel?.Id ??
                throw new ArgumentNullException(nameof(parcel));
            this.Parcel = parcel;
            this.Date = date;
            this.Key = key;
            this.Value = value;
        }
        #endregion
    }
}
