using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsPropertyRoadType class, provides an entity for the datamodel to manage a list of property road types.
    /// </summary>
    public partial class PimsPropertyRoadType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify a road type.
        /// </summary>
        [NotMapped]
        public string Id { get => PropertyRoadTypeCode; set => PropertyRoadTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a PimsPropertyRoadType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsPropertyRoadType(string id)
            : this()
        {
            Id = id;
        }
        #endregion
    }
}
