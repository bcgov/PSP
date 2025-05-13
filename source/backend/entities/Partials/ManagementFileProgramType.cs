using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsManagementFileProgramType class, provides an entity for the datamodel to manage Management file program types.
    /// </summary>
    public partial class PimsManagementFileProgramType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify management file program type.
        /// </summary>
        [NotMapped]
        public string Id { get => ManagementFileProgramTypeCode; set => ManagementFileProgramTypeCode = value; }

        #endregion

        #region Constructors

        public PimsManagementFileProgramType()
        {
        }

        /// <summary>
        /// Create a new instance of a PimsManagementFileProgramType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsManagementFileProgramType(string id)
        {
            Id = id;
        }

        #endregion
    }
}
