using System.ComponentModel.DataAnnotations.Schema;
namespace Pims.Dal.Entities
{
    /// <summary>
    /// SecurityDepositHolderType class, provides an entity for the datamodel to manage a list of security deposit holder types.
    /// </summary>
    public partial class PimsSecDepHolderType : ITypeEntity<string>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify security deposit holder type.
        /// </summary>
        [NotMapped]
        public string Id { get => SecDepHolderTypeCode; set => SecDepHolderTypeCode = value; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a SecurityDepositHolderType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsSecDepHolderType(string id):this()
        {
            Id = id;
        }
        #endregion
    }
}
