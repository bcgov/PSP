using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// ActivityInstanceStatusType class, provides an entity for the datamodel to manage a list of lease types.
    /// </summary>
    public partial class PimsActivityInstanceStatusType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify activity status type.
        /// </summary>
        [NotMapped]
        public string Id { get => ActivityInstanceStatusTypeCode; set => ActivityInstanceStatusTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a ActivityInstanceStatusType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsActivityInstanceStatusType(string id)
            : this()
        {
        }
        #endregion
    }
}
