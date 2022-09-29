using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// SecurityDepositType class, provides an entity for the datamodel to manage a list of security deposit types.
    /// </summary>
    public partial class PimsSecurityDepositType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify security deposit type.
        /// </summary>
        [NotMapped]
        public string Id { get => SecurityDepositTypeCode; set => SecurityDepositTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a SecurityDepositType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsSecurityDepositType(string id)
            : this()
        {
            Id = id;
        }
        #endregion
    }
}
