using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsAcqFlTeamProfileType class, provides an entity for the datamodel to manage a list of Team Profile types.
    /// </summary>
    public partial class PimsAcqFlTeamProfileType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify Team Profile type.
        /// </summary>
        [NotMapped]
        public string Id { get => AcqFlTeamProfileTypeCode; set => AcqFlTeamProfileTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a PimsAcqFlTeamProfileType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsAcqFlTeamProfileType(string id)
            : this()
        {
            Id = id;
        }
        #endregion
    }
}
