using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsDspPhysFileStatusType class, provides an entity for the datamodel to manage Disposition physical file status types.
    /// </summary>
    public partial class PimsDspPhysFileStatusType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify disposition physical file status type.
        /// </summary>
        [NotMapped]
        public string Id { get => DspPhysFileStatusTypeCode; set => DspPhysFileStatusTypeCode = value; }
        #endregion

        #region Constructors
        public PimsDspPhysFileStatusType()
        {
        }

        /// <summary>
        /// Create a new instance of a PimsDspPhysFileStatusType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsDspPhysFileStatusType(string id)
        {
            Id = id;
        }
        #endregion
    }
}
