using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsDspFlTeamProfileType class, provides an entity for the datamodel to manage Disposition fl profile types.
    /// </summary>
    public partial class PimsDspFlTeamProfileType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify disposition fl profile type.
        /// </summary>
        [NotMapped]
        public string Id { get => DspFlTeamProfileTypeCode; set => DspFlTeamProfileTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a PimsDspFlTeamProfileType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsDspFlTeamProfileType(string id)
            : this()
        {
            Id = id;
        }
        #endregion
    }
}
