using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// SecurityDepositHolderType class, provides an entity for the datamodel to manage a list of security deposit holder types.
    /// </summary>
    [MotiTable("PIMS_SEC_DEP_HOLDER_TYPE", "SCHLDT")]
    public class SecurityDepositHolderType : TypeEntity<string>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify security deposit holder type.
        /// </summary>
        [Column("SEC_DEP_HOLDER_TYPE_CODE")]
        public override string Id { get; set; }

        /// <summary>
        /// get - Collection of Security Deposits.
        /// </summary>
        public ICollection<SecurityDeposit> SecurityDeposits { get; } = new List<SecurityDeposit>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a SecurityDepositHolderType class.
        /// </summary>
        public SecurityDepositHolderType() { }

        /// <summary>
        /// Create a new instance of a SecurityDepositHolderType class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        public SecurityDepositHolderType(string id, string description) : base(id, description)
        {
        }
        #endregion
    }
}
