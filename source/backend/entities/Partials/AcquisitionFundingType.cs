using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsAcquisitionFundingType class, provides an entity for the datamodel to manage Acquisition funding types.
    /// </summary>
    public partial class PimsAcquisitionFundingType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify Person Profile type.
        /// </summary>
        [NotMapped]
        public string Id { get => AcquisitionFundingTypeCode; set => AcquisitionFundingTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a PimsAcquisitionFundingType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsAcquisitionFundingType(string id)
            : this()
        {
            Id = id;
        }
        #endregion
    }
}
