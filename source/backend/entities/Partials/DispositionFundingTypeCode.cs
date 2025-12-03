using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsDispositionFundingType class, provides an entity for the datamodel to manage Disposition funding types.
    /// </summary>
    public partial class PimsDispositionFundingType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify disposition funding type.
        /// </summary>
        [NotMapped]
        public string Id { get => DispositionFundingTypeCode; set => DispositionFundingTypeCode = value; }
        #endregion

        #region Constructors

        public PimsDispositionFundingType()
        {
        }

        /// <summary>
        /// Create a new instance of a PimsDispositionFundingType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsDispositionFundingType(string id)
        {
            Id = id;
        }
        #endregion
    }
}
