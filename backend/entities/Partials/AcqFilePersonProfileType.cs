using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsAcqFlPersonProfileType class, provides an entity for the datamodel to manage a list of Person Profile types.
    /// </summary>
    public partial class PimsAcqFlPersonProfileType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify Person Profile type.
        /// </summary>
        [NotMapped]
        public string Id { get => AcqFlPersonProfileTypeCode; set => AcqFlPersonProfileTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a PimsAcqFlPersonProfileType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsAcqFlPersonProfileType(string id)
            : this()
        {
            Id = id;
        }
        #endregion
    }
}
