using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// LeaseCategoryType class, provides an entity for the datamodel to manage a list of lease category types.
    /// </summary>
    public partial class PimsLeaseCategoryType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify lease category type.
        /// </summary>
        [NotMapped]
        public string Id { get => LeaseCategoryTypeCode; set => LeaseCategoryTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a LeaseType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsLeaseCategoryType(string id)
            : this()
        {
            Id = id;
        }
        #endregion
    }
}
