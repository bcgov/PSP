using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsTakeStatusType class, provides an entity for the datamodel to manage a list of take status types.
    /// </summary>
    public partial class PimsTakeStatusType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify take status type.
        /// </summary>
        [NotMapped]
        public string Id { get => TakeStatusTypeCode; set => TakeStatusTypeCode = value; }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a PimsTakeStatusType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsTakeStatusType(string id)
            : this()
        {
            Id = id;
        }
        #endregion
    }
}
