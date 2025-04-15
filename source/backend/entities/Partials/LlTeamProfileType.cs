using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsLlTeamProfileType class, provides an entity for the datamodel to manage a list of Team Profile types.
    /// </summary>
    public partial class PimsLlTeamProfileType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify Team Profile type.
        /// </summary>
        [NotMapped]
        public string Id { get => LlTeamProfileTypeCode; set => LlTeamProfileTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a PimsLlTeamProfileType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsLlTeamProfileType(string id)
            : this()
        {
            Id = id;
        }

        public PimsLlTeamProfileType()
        {
        }
        #endregion
    }
}
