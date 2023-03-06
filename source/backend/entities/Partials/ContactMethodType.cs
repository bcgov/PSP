using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// ContactMethodType class, provides an entity for the datamodel to manage a list of contact method types.
    /// </summary>
    public partial class PimsContactMethodType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify contact method type.
        /// </summary>
        [NotMapped]
        public string Id { get => ContactMethodTypeCode; set => ContactMethodTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a ContactMethodType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsContactMethodType(string id)
            : this()
        {
            Id = id;
        }
        #endregion
    }
}
