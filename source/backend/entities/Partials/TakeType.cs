using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsTakeType class, provides an entity for the datamodel to manage a list of take types.
    /// </summary>
    public partial class PimsTakeType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify take type.
        /// </summary>
        [NotMapped]
        public string Id { get => TakeTypeCode; set => TakeTypeCode = value; }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a PimsTakeType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsTakeType(string id)
            : this()
        {
            Id = id;
        }
        #endregion
    }
}
