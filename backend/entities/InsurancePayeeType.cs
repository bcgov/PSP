using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// InsurancePayeeType class, provides an entity for the datamodel to manage a list of insurance payee types.
    /// </summary>
    [MotiTable("PIMS_INSURANCE_PAYEE_TYPE", "INSPAY")]
    public class InsurancePayeeType : TypeEntity<string>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify insurance payee type.
        /// </summary>
        [Column("INSURANCE_PAYEE_TYPE_CODE")]
        public override string Id { get; set; }

        /// <summary>
        /// get - Collection of insurance.
        /// </summary>
        public ICollection<Insurance> Insurances { get; } = new List<Insurance>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a InsurancePayeeType class.
        /// </summary>
        public InsurancePayeeType() { }

        /// <summary>
        /// Create a new instance of a InsurancePayeeType class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        public InsurancePayeeType(string id, string description) : base(id, description)
        {
        }
        #endregion
    }
}
