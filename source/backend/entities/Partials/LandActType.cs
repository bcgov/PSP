using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// LandActType class, provides an entity for the datamodel to manage a list of take land act types.
    /// </summary>
    public partial class PimsLandActType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify take status type.
        /// </summary>
        [NotMapped]
        public string Internal_Id { get => this.LandActTypeCode; set => this.LandActTypeCode = value; }

        [NotMapped]
        public string Id { get => this.LandActTypeCode; set => this.LandActTypeCode = value; }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a PimsTakeStatusType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsLandActType(string id)
            : this()
        {
            Id = id;
        }
        #endregion
    }
}
