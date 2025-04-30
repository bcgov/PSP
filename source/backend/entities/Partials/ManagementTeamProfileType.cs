using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsManagementFileProfileType class, provides an entity for the datamodel to manage a list of Team Profile types.
    /// </summary>
    public partial class PimsManagementFileProfileType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify Team Profile type.
        /// </summary>
        [NotMapped]
        public string Id { get => ManagementFileProfileTypeCode; set => ManagementFileProfileTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a PimsManagementFileProfileType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsManagementFileProfileType(string id)
            : this()
        {
            Id = id;
        }

        public PimsManagementFileProfileType()
        {
        }
        #endregion
    }
}
