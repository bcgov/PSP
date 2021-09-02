using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PropertyEvaluation class, provides an entity for the datamodel to manage property evaluations.
    /// </summary>
    [MotiTable("PIMS_PROPERTY_EVALUATION", "PRPEVL")]
    public class PropertyEvaluation : BaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify property evaluation.
        /// </summary>
        [Column("PROPERTY_EVALUATION_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - Foreign key to the property.
        /// </summary>
        [Column("PROPERTY_ID")]
        public long PropertyId { get; set; }

        /// <summary>
        /// get/set - The property the evaluation belongs to.
        /// </summary>
        public Property Property { get; set; }

        /// <summary>
        /// get/set - When the evaluation was taken.
        /// </summary>
        [Column("EVALUATION_DATE")]
        public DateTime EvaluatedOn { get; set; }

        /// <summary>
        /// get/set - A key to identify the type of evaluation.
        /// </summary>
        [Column("KEY")]
        public int Key { get; set; }

        /// <summary>
        /// get/set - The evaluation value.
        /// </summary>
        [Column("VALUE")]
        public decimal Value { get; set; }

        /// <summary>
        /// get/set - A note to describe the evaluation.
        /// </summary>
        [Column("NOTE")]
        public string Note { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a PropertyEvaluation class.
        /// </summary>
        public PropertyEvaluation() { }

        /// <summary>
        /// Create a new instance of a PropertyEvaluation class.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="evaluatedOn"></param>
        public PropertyEvaluation(Property property, int key, decimal value, DateTime evaluatedOn)
        {
            this.Property = property ?? throw new ArgumentNullException(nameof(property));
            this.PropertyId = property.Id;
            this.Key = key;
            this.Value = value;
            this.EvaluatedOn = evaluatedOn;
        }
        #endregion
    }
}
