using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// BuildingEvaluation class, provides an entity to map building evaluation values to a date.
    /// </summary>
    [MotiTable("PIMS_BUILDING_EVALUATION", "BLDEVL")]
    public class BuildingEvaluation : BaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify the building evaluation.
        /// </summary>
        [Column("BUILDING_EVALUATION_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - The primary key and the foreign key to the building.
        /// </summary>
        [Column("BUILDING_ID")]
        public long BuildingId { get; set; }

        /// <summary>
        /// get/set - The building.
        /// </summary>
        public Building Building { get; set; }

        /// <summary>
        /// get/set - The primary key and the date the evaluation is for.
        /// </summary>
        [Column("DATE")]
        public DateTime Date { get; set; }

        /// <summary>
        /// get/set - The key for this fiscal value.
        /// </summary>
        [Column("KEY")]
        public EvaluationKeys Key { get; set; }

        /// <summary>
        /// get/set - The value of the fiscal key for this building.
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
        /// Creates a new instance of a BuildingEvaluation class.
        /// </summary>
        public BuildingEvaluation() { }

        /// <summary>
        /// Creates a new instance of a BuildingEvaluation class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="building"></param>
        /// <param name="date"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public BuildingEvaluation(Building building, DateTime date, EvaluationKeys key, decimal value)
        {
            this.BuildingId = building?.Id ??
                throw new ArgumentNullException(nameof(building));
            this.Building = building;
            this.Date = date;
            this.Key = key;
            this.Value = value;
        }
        #endregion
    }
}
