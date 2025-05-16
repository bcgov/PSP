using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsPropertyOperationType class, provides an entity for the datamodel to manage a property operation types.
    /// </summary>
    public partial class PimsPropertyOperationType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to the property operation type.
        /// </summary>
        [NotMapped]
        public string Id { get => PropertyOperationTypeCode; set => PropertyOperationTypeCode = value; }
        #endregion
    }
}
