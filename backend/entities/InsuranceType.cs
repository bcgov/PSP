using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// InsuranceType class, provides an entity for the datamodel to manage a list of insurance types.
    /// </summary>
    [MotiTable("PIMS_INSURANCE_TYPE", "INSPYT")]
    public class InsuranceType : TypeEntity<string>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify insurance type.
        /// </summary>
        [Column("INSURANCE_TYPE_CODE")]
        public override string Id { get; set; }

        /// <summary>
        /// get - Collection of insurance.
        /// </summary>
        public ICollection<Insurance> Insurances { get; } = new List<Insurance>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a InsuranceType class.
        /// </summary>
        public InsuranceType() { }

        /// <summary>
        /// Create a new instance of a InsuranceType class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        public InsuranceType(string id, string description) : base(id, description)
        {
        }
        #endregion
    }
}
