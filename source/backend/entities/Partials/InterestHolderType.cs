using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsInterestHolderType class, provides an entity for the datamodel to manage interest holding types.
    /// </summary>
    public partial class PimsInterestHolderType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify the interest holder type.
        /// </summary>
        [NotMapped]
        public string Id { get => InterestHolderTypeCode; set => InterestHolderTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a PimsInterestHolderType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsInterestHolderType(string id)
            : this()
        {
            Id = id;
        }
        #endregion
    }
}
