using System.ComponentModel.DataAnnotations.Schema;
namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsPropertyAdjacentLandType class, provides an entity for the datamodel to manage a list of property adjacent land types.
    /// </summary>
    public partial class PimsPropertyAdjacentLandType : ITypeEntity<string>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify adjacent land type.
        /// </summary>
        [NotMapped]
        public string Id { get => PropertyAdjacentLandTypeCode; set => PropertyAdjacentLandTypeCode = value; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a PimsPropertyAdjacentLandType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsPropertyAdjacentLandType(string id) : this()
        {
            Id = id;
        }
        #endregion
    }
}
