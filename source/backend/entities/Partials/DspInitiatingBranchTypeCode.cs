using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsDspInitiatingBranchType class, provides an entity for the datamodel to manage Disposition initiating branch types.
    /// </summary>
    public partial class PimsDspInitiatingBranchType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify disposition initiating branch type.
        /// </summary>
        [NotMapped]
        public string Id { get => DspInitiatingBranchTypeCode; set => DspInitiatingBranchTypeCode = value; }
        #endregion

        #region Constructors
        public PimsDspInitiatingBranchType()
        {
        }

        /// <summary>
        /// Create a new instance of a PimsDspInitiatingBranchType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsDspInitiatingBranchType(string id)
        {
            Id = id;
        }
        #endregion
    }
}
