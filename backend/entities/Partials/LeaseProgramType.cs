using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// LeaseProgramType class, provides an entity for the datamodel to manage a list of lease program types.
    /// </summary>
    public partial class PimsLeaseProgramType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify lease program type.
        /// </summary>
        [NotMapped]
        public string Id { get => LeaseProgramTypeCode; set => LeaseProgramTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a LeaseProgramType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsLeaseProgramType(string id)
            : this()
        {
            Id = id;
        }
        #endregion
    }
}
