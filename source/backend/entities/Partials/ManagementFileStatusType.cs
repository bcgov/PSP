using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsManagementFileStatusType class, provides an entity for the datamodel to manage Management file status types.
    /// </summary>
    public partial class PimsManagementFileStatusType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify management file status type.
        /// </summary>
        [NotMapped]
        public string Id { get => ManagementFileStatusTypeCode; set => ManagementFileStatusTypeCode = value; }

        #endregion

        #region Constructors

        public PimsManagementFileStatusType()
        {
        }

        /// <summary>
        /// Create a new instance of a PimsManagementFileStatusType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsManagementFileStatusType(string id)
        {
            Id = id;
        }

        #endregion
    }
}
