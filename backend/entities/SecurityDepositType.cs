using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// SecurityDepositType class, provides an entity for the datamodel to manage a list of security deposit types.
    /// </summary>
    [MotiTable("PIMS_SECURITY_DEPOSIT_TYPE", "SECDPT")]
    public class SecurityDepositType : TypeEntity<string>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify security deposit type.
        /// </summary>
        [Column("SECURITY_DEPOSIT_TYPE_CODE")]
        public override string Id { get; set; }

        /// <summary>
        /// get - Collection of Security Deposits.
        /// </summary>
        public ICollection<SecurityDeposit> SecurityDeposits { get; } = new List<SecurityDeposit>();

        /// <summary>
        /// get - Collection of Security Deposit Returns.
        /// </summary>
        public ICollection<SecurityDepositReturn> SecurityDepositReturns { get; } = new List<SecurityDepositReturn>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a SecurityDepositType class.
        /// </summary>
        public SecurityDepositType() { }

        /// <summary>
        /// Create a new instance of a SecurityDepositType class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        public SecurityDepositType(string id, string description) : base(id, description)
        {
        }
        #endregion
    }
}
